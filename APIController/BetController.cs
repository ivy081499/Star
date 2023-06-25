using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Star.Enums;
using Star.Helper;
using Star.RequestModel;
using Star.Settings;
using Star.ViewModels;
using Star.Response;
using Star.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Net;
using Star.ResponseModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star.APIController
{
    [Route("api/[controller]")]
    public class BetController : Controller
    {
        private readonly CustomerSettings _customerSettings;
        private readonly BookieSettings _bookieSettings;
        private readonly IDistributedCache _redisCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public BetController(
            CustomerSettings customerSettings,
            BookieSettings bookieSettings,
            IDistributedCache redisCache,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _redisCache = redisCache; //不好用，提供的方法沒有這麼多
            _customerSettings = customerSettings;
            _bookieSettings = bookieSettings;
        }

        [HttpGet]
        public BetViewModel GetSelectList()
        {
            List<BookieViewModel> bookieList = new List<BookieViewModel>();

            foreach (Enum item in Enum.GetValues(typeof(BookieType)))
            {
                bookieList.Add(new BookieViewModel()
                {
                    Value = (int)(BookieType)item,
                    Name = item.ToString(),
                });
            }

            return new BetViewModel()
            {
                BookieList = bookieList,
                CustomerSettings = _customerSettings,
            };
        }

        [HttpPost]
        public async Task<BaseResponseModel> Bet(BetRequestModel request)
        {

            bool result = ResolveStringHelper.CheckBetContent(request.BetContent, out List<Column> columns);

            if (!result)
            {
                return new BaseResponseModel() { IsSuccess = false, Message = "數入文字格式不正確", };
            }

            CustomerDailyBet customerDailyBet = await GetCustomerDailyBetFromRedis(request.Date, $"{request.CustomerName}");

            customerDailyBet.BetInfoList.Add(new BetInfo()
            {
                CustomerName = request.CustomerName,
                ColumnList = columns,
                FourStarOdds = request.FourStarOdds,
                ThreeStarOdds = request.ThreeStarOdds,
                TwoStarOdds = request.TwoStarOdds,
                Id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                BetContent = FormatBetContent(request.BetContent),
                BookieType = request.Bookie,
                PaperNumber = request.PaperNumber,
            });

            CustomerInfo customer = _customerSettings.CustomerList.First(o => o.Name == request.CustomerName);

            customerDailyBet.DailyReport = CountingHelper.GetDailyReport(customerDailyBet.BetInfoList, customerDailyBet.CarSetInfoList, customer.Cost539);

            await SetCustomerDailyBetToRedis(request.Date, customerDailyBet);


            return new BaseResponseModel() { IsSuccess = true, };

        }

        [HttpPost]
        [Route("CarSetBet")]
        public async Task<BaseResponseModel> CarSetBet(CarSetRequestModel request)
        {
            CustomerDailyBet customerDailyBet = await GetCustomerDailyBetFromRedis(request.Date, request.CustomerName);

            customerDailyBet.CarSetInfoList.Add(new CarSetInfo()
            {
                Id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                CarSetNumber = request.CarSetNumber,
                Odds = request.Odds,
                BookieType = request.Bookie,
                PaperNumber = request.PaperNumber,
                CustomerName = request.CustomerName,                
            });

            CustomerInfo customer = _customerSettings.CustomerList.First(o => o.Name == request.CustomerName);

            customerDailyBet.DailyReport = CountingHelper.GetDailyReport(customerDailyBet.BetInfoList, customerDailyBet.CarSetInfoList, customer.Cost539);

            await SetCustomerDailyBetToRedis(request.Date, customerDailyBet);

            return new BaseResponseModel() { IsSuccess = true, };
        }


        [HttpDelete]
        public async Task<bool> Delete(DeleteRecordRequestModel request)
        {
            bool isSuccess = false;

            CustomerDailyBet customerDailyBet = await GetCustomerDailyBetFromRedis(request.Date, request.CustomerName);

            BetInfo? betInfo = customerDailyBet.BetInfoList.SingleOrDefault(o => o.Id == request.Id);

            if (betInfo != null)
            {
                int count = customerDailyBet.BetInfoList.RemoveAll(o => o.Id == request.Id);
                isSuccess = count > 0;
            }

            CarSetInfo? carSetInfo = customerDailyBet.CarSetInfoList.FirstOrDefault(o => o.Id == request.Id);

            if (carSetInfo != null)
            {
                int count = customerDailyBet.CarSetInfoList.RemoveAll(o => o.Id == request.Id);

                isSuccess = count > 0;
            }

            if (isSuccess)
            {
                customerDailyBet.DailyReport = CountingHelper.GetDailyReport(customerDailyBet.BetInfoList, customerDailyBet.CarSetInfoList, customerDailyBet.Customer.Cost539);

                await SetCustomerDailyBetToRedis(request.Date, customerDailyBet);
            }

            return isSuccess;
        }

        [HttpPost]
        [Route("GetCustomerBet")]
        public async Task<CustomerDailyBet> GetCustomerBet(GetCustomerBetInfoModel model)
        {
            CustomerDailyBet result = await GetCustomerDailyBetFromRedis(model.Date, model.CustomerName);
            return result;
        }

        [HttpPost]
        [Route("Redis")]
        public bool Redis(string data)
        {
            _database.StringSetAsync("yowko", "test", TimeSpan.FromSeconds(60), When.NotExists);

            return true;
        }

        [HttpPut]
        [Route("ModifyBonus")]
        public async Task<bool> ModifyBonus(ModifyBonusRequestModel request)
        {
            CustomerInfo customerInfo = _customerSettings.CustomerList.Single(o => o.Name == request.CustomerName);

            CustomerDailyBet customerDailyBet = await GetCustomerDailyBetFromRedis(request.Date, request.CustomerName);

            customerDailyBet.DailyReport.TwoStarBonus = Convert.ToDouble(request.TwoStarBonus);
            customerDailyBet.DailyReport.ThreeStarBonus = Convert.ToDouble(request.ThreeStarBonus);
            customerDailyBet.DailyReport.FourStarBonus = Convert.ToDouble(request.FourStarBonus);

            CountingHelper.ReCalulateReport(customerDailyBet.DailyReport, customerInfo.Cost539);

            await SetCustomerDailyBetToRedis(request.Date, customerDailyBet);

            return true;
        }

        [HttpPost]
        [Route("GetBookieBet")]
        public async Task<BookieBetModel> GetBookieBet(GetBookieBetRequestModel request)
        {

            BookieInfo bookieInfo = _bookieSettings.BookieSettingList.First(o => o.BookieType == request.BookieType);

            List<CustomerDailyBet> customerBetList = await GetCustomerDailyBetListFromRedis(request.Date);

            IEnumerable<BetInfo> totalBetInfoList = new List<BetInfo>();
            IEnumerable<CarSetInfo> totalCarSetInfoList = new List<CarSetInfo>();
            IEnumerable<int> paperNumberList = new List<int>();

            foreach (CustomerDailyBet customerDailyBet in customerBetList)
            {
                totalBetInfoList = totalBetInfoList.Concat(customerDailyBet.BetInfoList.Where(o => o.BookieType == request.BookieType));
                totalCarSetInfoList = totalCarSetInfoList.Concat(customerDailyBet.CarSetInfoList.Where(o => o.BookieType == request.BookieType));
            }

            paperNumberList = totalBetInfoList.Select(o => o.PaperNumber).Concat(totalCarSetInfoList.Select(o => o.PaperNumber)).Distinct().Order();

            List<BookiePaper> BookiePapaerList = new List<BookiePaper>();

            foreach (int pagerNumber in paperNumberList)
            {
                IEnumerable<BetInfo> betInfoList = totalBetInfoList.Where(o => o.PaperNumber == pagerNumber);
                IEnumerable<CarSetInfo> carSetInfoList = totalCarSetInfoList.Where(o => o.PaperNumber == pagerNumber);

                BookiePapaerList.Add(new BookiePaper()
                {
                    PaperNumber = pagerNumber,
                    BetInfoList = betInfoList.ToList(),
                    CarSetInfoList = carSetInfoList.ToList(),
                    PaperReport = CountingHelper.GetDailyReport(betInfoList, carSetInfoList, bookieInfo.Cost539),
                });
            }

            BookieBetModel result = new BookieBetModel()
            {
                Date = request.Date.ToString("yyyy-MM-dd"),
                BookiePapaerList = BookiePapaerList,
                DailyReport = CountingHelper.GetDailyReport(totalBetInfoList, totalCarSetInfoList, bookieInfo.Cost539),
            };

            return result;

        }

        private async Task<List<CustomerDailyBet>> GetCustomerDailyBetListFromRedis(DateTime date)
        {
            List<CustomerDailyBet> result = new List<CustomerDailyBet>();

            HashEntry[] list = await _database.HashGetAllAsync(date.ToString("yyyy-MM-dd"));

            foreach (HashEntry data in list)
            {
                CustomerDailyBet customerDailyBet = JsonConvert.DeserializeObject<CustomerDailyBet>(data.Value.ToString());
                customerDailyBet.BetInfoList ??= new List<BetInfo>();
                customerDailyBet.CarSetInfoList ??= new List<CarSetInfo>();

                result.Add(customerDailyBet);
            }

            return result;
        }

        private async Task<CustomerDailyBet> GetCustomerDailyBetFromRedis(DateTime date, string customerName)
        {
            RedisValue data = await _database.HashGetAsync(date.ToString("yyyy-MM-dd"), customerName);
            CustomerDailyBet customerDailyBet;

            if (data.HasValue)
            {
                customerDailyBet = JsonConvert.DeserializeObject<CustomerDailyBet>(data.ToString());
            }
            else
            {
                CustomerInfo customer = _customerSettings.CustomerList.First(o => o.Name == customerName);

                customerDailyBet = new CustomerDailyBet()
                {
                    BetInfoList = new List<BetInfo>(),
                    CarSetInfoList = new List<CarSetInfo>(),
                    Customer = customer,
                    DailyReport = new Report(),
                    Date = date.ToString("yyyy-MM-dd"),
                };
            }

            return customerDailyBet;
        }

        private async Task SetCustomerDailyBetToRedis(DateTime date, CustomerDailyBet customerDailyBet)
        {
            HashEntry[] array = new HashEntry[] {
                new HashEntry(customerDailyBet.Customer.Name, JsonConvert.SerializeObject(customerDailyBet))
            };

            await _database.HashSetAsync(date.ToString("yyyy-MM-dd"), array);
        }

        private string FormatBetContent(string value)
        {
            return value
                .Replace("*", "x")
                .Replace("X", "x")
                .Replace("x", " x ");
        }
    }
}

