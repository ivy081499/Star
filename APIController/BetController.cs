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

            bool result = ResolveStringHelper.CheckBetContent(request.BetContent, out BetContent betContent);

            if (!result)
            {
                return new BaseResponseModel() { IsSuccess = false, Message = "數入文字格式不正確", };
            }

            CustomerBetInfo customerBetInfo = await GetCustomerBetInfoFromRedis(request.Date, $"{request.CustomerName}");

            switch (betContent.BetContentType)
            {
                case BetContentType.Serial:
                case BetContentType.Column:
                    customerBetInfo.ColumnBetList.Add(new ColumnBet()
                    {
                        BookieType = request.Bookie,
                        CustomerName = request.CustomerName,
                        Date = request.Date.ToDateFormat(),
                        Id = request.Date.ToDateFormat(),
                        RawContent = request.BetContent,
                        PaperNumber = request.PaperNumber,
                        Content = (List<List<string>>)betContent.ParsedContent,
                        OddsInfo = new OddsInfo()
                        {
                            TwoStar = request.TwoStarOdds,
                            ThreeStar = request.ThreeStarOdds,
                            FourStar = request.FourStarOdds,
                        },
                    });
                    break;
                case BetContentType.Car:
                    customerBetInfo.CarBetList.Add(new CarBet()
                    {
                        BookieType = request.Bookie,
                        CustomerName = request.CustomerName,
                        Date = request.Date.ToDateFormat(),
                        Id = request.Date.ToDateFormat(),
                        PaperNumber = request.PaperNumber,
                        Content = (string)betContent.ParsedContent,
                        RawContent = request.BetContent,
                        OddsInfo = new OddsInfo()
                        {
                            TwoStar = request.TwoStarOdds,
                        },
                    });
                    break;
                default:
                    break;
            }

            CustomerInfo customer = _customerSettings.CustomerList.First(o => o.Name == request.CustomerName);

            CountingHelper.SetReport(customerBetInfo.SerialBetList, customerBetInfo.ColumnBetList, customerBetInfo.CarBetList, customerBetInfo.Report, customer.Cost539);

            await SetCustomerBetInfoToRedis(request.Date, customerBetInfo);


            return new BaseResponseModel() { IsSuccess = true, };

        }

        [HttpDelete]
        public async Task<bool> Delete(DeleteRecordRequestModel request)
        {
            bool isSuccess = false;

            CustomerBetInfo CustomerBetInfo = await GetCustomerBetInfoFromRedis(request.Date, request.CustomerName);

            if (CustomerBetInfo.SerialBetList.Any(o => o.Id == request.Id))
            {
                int count = CustomerBetInfo.SerialBetList.RemoveAll(o => o.Id == request.Id);
                isSuccess = count > 0;
            }
            else if (CustomerBetInfo.ColumnBetList.Any(o => o.Id == request.Id))
            {
                int count = CustomerBetInfo.ColumnBetList.RemoveAll(o => o.Id == request.Id);
                isSuccess = count > 0;
            }
            else if (CustomerBetInfo.CarBetList.Any(o => o.Id == request.Id))
            {
                int count = CustomerBetInfo.CarBetList.RemoveAll(o => o.Id == request.Id);
                isSuccess = count > 0;
            }

            if (isSuccess)
            {
                CustomerInfo customerInfo = _customerSettings.CustomerList.Single(o => o.Name == request.CustomerName);

                CountingHelper.SetReport(CustomerBetInfo.SerialBetList, CustomerBetInfo.ColumnBetList, CustomerBetInfo.CarBetList,
                    CustomerBetInfo.Report, customerInfo.Cost539);

                await SetCustomerBetInfoToRedis(request.Date, CustomerBetInfo);
            }

            return isSuccess;
        }

        [HttpPost]
        [Route("GetCustomerBet")]
        public async Task<CustomerBetInfo> GetCustomerBet(GetCustomerBetInfoModel model)
        {
            CustomerBetInfo result = await GetCustomerBetInfoFromRedis(model.Date, model.CustomerName);
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

            CustomerBetInfo CustomerBetInfo = await GetCustomerBetInfoFromRedis(request.Date, request.CustomerName);

            CustomerBetInfo.Report.TwoStarBonus = Convert.ToDouble(request.TwoStarBonus);
            CustomerBetInfo.Report.ThreeStarBonus = Convert.ToDouble(request.ThreeStarBonus);
            CustomerBetInfo.Report.FourStarBonus = Convert.ToDouble(request.FourStarBonus);

            CountingHelper.SetReport(CustomerBetInfo.SerialBetList, CustomerBetInfo.ColumnBetList, CustomerBetInfo.CarBetList,
               CustomerBetInfo.Report, customerInfo.Cost539);

            await SetCustomerBetInfoToRedis(request.Date, CustomerBetInfo);

            return true;
        }

        [HttpPost]
        [Route("GetBookieBet")]
        public async Task<BookieBetInfo> GetBookieBet(GetBookieBetRequestModel request)
        {
            BookieInfo bookieInfo = _bookieSettings.BookieSettingList.First(o => o.BookieType == request.BookieType);

            List<CustomerBetInfo> customerBetList = await GetCustomerBetInfoListFromRedis(request.Date);

            IEnumerable<ColumnBet> totalSerialBetList = new List<ColumnBet>();
            IEnumerable<ColumnBet> totalColumnBetList = new List<ColumnBet>();
            IEnumerable<CarBet> totalCarBetList = new List<CarBet>();

            IEnumerable<int> paperNumberList = new List<int>();

            foreach (CustomerBetInfo CustomerBetInfo in customerBetList)
            {
                totalSerialBetList = totalColumnBetList.Concat(CustomerBetInfo.SerialBetList.Where(o => o.BookieType == request.BookieType));
                totalColumnBetList = totalColumnBetList.Concat(CustomerBetInfo.ColumnBetList.Where(o => o.BookieType == request.BookieType));
                totalCarBetList = totalCarBetList.Concat(CustomerBetInfo.CarBetList.Where(o => o.BookieType == request.BookieType));
            }

            paperNumberList = totalSerialBetList.Select(o => o.PaperNumber)
                .Concat(totalColumnBetList.Select(o => o.PaperNumber))
                .Concat(totalCarBetList.Select(o => o.PaperNumber))
                .Distinct().Order();

            List<PaperBet> paperBetList = new List<PaperBet>();

            foreach (int pagerNumber in paperNumberList)
            {
                List<ColumnBet> serialBetList = totalColumnBetList.Where(o => o.PaperNumber == pagerNumber).ToList();
                List<ColumnBet> columnBetList = totalColumnBetList.Where(o => o.PaperNumber == pagerNumber).ToList();
                List<CarBet> carBetList = totalCarBetList.Where(o => o.PaperNumber == pagerNumber).ToList();
                Report report = new Report();

                CountingHelper.SetReport(serialBetList, columnBetList, carBetList, report, bookieInfo.Cost539);

                paperBetList.Add(new PaperBet()
                {
                    CarBetList = carBetList,
                    Number = pagerNumber,
                    Report = report,
                    ColumnBetList = columnBetList,
                    SerialBetList = serialBetList,
                });
            }

            Report totalPaperReport = new Report();

            CountingHelper.SetReport(totalPaperReport, bookieInfo.Cost539, paperBetList);

            BookieBetInfo result = new BookieBetInfo()
            {
                Date = request.Date.ToDateFormat(),
                PaperBetList = paperBetList,
                Report = totalPaperReport,
            };

            return result;

        }

        private async Task<List<CustomerBetInfo>> GetCustomerBetInfoListFromRedis(DateTime date)
        {
            List<CustomerBetInfo> result = new List<CustomerBetInfo>();

            HashEntry[] list = await _database.HashGetAllAsync(date.ToDateFormat());

            foreach (HashEntry data in list)
            {
                CustomerBetInfo CustomerBetInfo = JsonConvert.DeserializeObject<CustomerBetInfo>(data.Value.ToString());
                CustomerBetInfo.SerialBetList ??= new List<ColumnBet>();
                CustomerBetInfo.ColumnBetList ??= new List<ColumnBet>();
                CustomerBetInfo.CarBetList ??= new List<CarBet>();

                result.Add(CustomerBetInfo);
            }

            return result;
        }

        private async Task<CustomerBetInfo> GetCustomerBetInfoFromRedis(DateTime date, string customerName)
        {
            RedisValue data = await _database.HashGetAsync(date.ToDateFormat(), customerName);

            CustomerBetInfo CustomerBetInfo;

            if (data.HasValue)
            {
                CustomerBetInfo = JsonConvert.DeserializeObject<CustomerBetInfo>(data.ToString());
            }
            else
            {
                CustomerInfo customer = _customerSettings.CustomerList.First(o => o.Name == customerName);

                CustomerBetInfo = new CustomerBetInfo()
                {
                    Date = date.ToDateFormat(),
                    SerialBetList = new List<ColumnBet>(),
                    ColumnBetList = new List<ColumnBet>(),
                    CarBetList = new List<CarBet>(),
                    CustomerName = customerName,
                    Report = new Report(),
                };
            }

            return CustomerBetInfo;
        }

        private async Task SetCustomerBetInfoToRedis(DateTime date, CustomerBetInfo customerBetInfo)
        {
            HashEntry[] array = new HashEntry[] {
                new HashEntry(customerBetInfo.CustomerName, JsonConvert.SerializeObject(customerBetInfo))
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

