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
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Net;
using Star.ResponseModel;

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

            string redisKey = $"{request.Date.ToString("yyyy-MM-dd")}:{request.CustomerName}:{request.Bookie}";

            Customer customer = _customerSettings.CustomerList.First(o => o.Name == request.CustomerName);

            CustomerBet customerBet = await GetCustomerBetFromRedis(redisKey);
            customerBet.BookieText = request.Bookie.ToString();
            customerBet.Bookie = request.Bookie;
            customerBet.Customer = customer;
            customerBet.Date = request.Date;
            customerBet.PaperNumber = request.PaperNumber;
            customerBet.BetInfoList.Add(new BetInfo()
            {
                ColumnList = columns,
                FourStarOdds = request.FourStarOdds,
                ThreeStarOdds = request.ThreeStarOdds,
                TwoStarOdds = request.TwoStarOdds,
                Id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                BetContent = request.BetContent.Replace("X", "x").Replace("x", " x "),
            });

            customerBet.BetStatistics = CountingHelper.GetBetStatistics(customerBet.BetInfoList, customerBet.CarSetInfoList, customer);

            await _database.StringSetAsync(redisKey, JsonConvert.SerializeObject(customerBet), CountingHelper.Range);

            return new BaseResponseModel() { IsSuccess = true, };

        }

        [HttpPost]
        [Route("CarSetBet")]
        public async Task<BaseResponseModel> CarSetBet(CarSetRequestModel request)
        {
            string redisKey = $"{request.Date.ToString("yyyy-MM-dd")}:{request.CustomerName}:{request.Bookie}";

            Customer customer = _customerSettings.CustomerList.First(o => o.Name == request.CustomerName);

            CustomerBet customerBet = await GetCustomerBetFromRedis(redisKey);
            customerBet.BookieText = request.Bookie.ToString();
            customerBet.Bookie = request.Bookie;
            customerBet.Customer = customer;
            customerBet.Date = request.Date;
            customerBet.PaperNumber = request.PaperNumber;
            customerBet.CarSetInfoList.Add(new CarSetInfo()
            {
                Id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                carSetNumber = request.CarSetNumber,
                Odds = request.Odds,
            });

            customerBet.BetStatistics = CountingHelper.GetBetStatistics(customerBet.BetInfoList, customerBet.CarSetInfoList, customer);

            await _database.StringSetAsync(redisKey, JsonConvert.SerializeObject(customerBet), CountingHelper.Range);

            return new BaseResponseModel() { IsSuccess = true, };

        }


        [HttpDelete]
        public async Task<bool> Delete(DeleteRecordRequestModel request)
        {
            bool isSuccess = false;

            BookieType bookie = (BookieType)request.Bookie;

            string redisKey = $"{request.Date.ToString("yyyy-MM-dd")}:{request.CustomerName}:{bookie.ToString()}";

            CustomerBet customerBet = await GetCustomerBetFromRedis(redisKey);

            BetInfo? betInfo = customerBet.BetInfoList.SingleOrDefault(o => o.Id == request.Id);

            if (betInfo != null)
            {
                int count = customerBet.BetInfoList.RemoveAll(o => o.Id == request.Id);
                isSuccess = count > 0;
            }

            CarSetInfo? carSetInfo = customerBet.CarSetInfoList.FirstOrDefault(o => o.Id == request.Id);

            if (carSetInfo != null)
            {
                int count = customerBet.CarSetInfoList.RemoveAll(o => o.Id == request.Id);

                isSuccess = count > 0;
            }

            if (isSuccess)
            {
                await _database.StringSetAsync(redisKey, JsonConvert.SerializeObject(customerBet), CountingHelper.Range);
            }

            return isSuccess;
        }

        [HttpPost]
        [Route("GetCustomerBet")]
        public async Task<List<CustomerBet>> GetCustomerBet(GetCustomerBetInfoModel model)
        {
            List<CustomerBet> result = new List<CustomerBet>();

            string redisKey1 = $"{model.Date.ToString("yyyy-MM-dd")}:{model.CustomerName}:{BookieType.小惠}";

            CustomerBet customerBet1 = await GetCustomerBetFromRedis(redisKey1);


            if (customerBet1.BetInfoList.Any() || customerBet1.CarSetInfoList.Any())
            {
                result.Add(customerBet1);
            }

            string redisKey2 = $"{model.Date.ToString("yyyy-MM-dd")}:{model.CustomerName}:{BookieType.楊董}";

            CustomerBet customerBet2 = await GetCustomerBetFromRedis(redisKey2);

            if (customerBet2.BetInfoList.Any() || customerBet2.CarSetInfoList.Any())
            {
                result.Add(customerBet2);
            }


            return result;

            ////todo
            //List<BetInfo> betInfoList = new List<BetInfo>();
            //List<Column> columnList = new List<Column>();
            //columnList.Add(new Column() { Numbers = new int[] { 1, 2 }.ToList() });
            //columnList.Add(new Column() { Numbers = new int[] { 3, 4 }.ToList() });
            //betInfoList.Add(new BetInfo()
            //{
            //    ColumnList = columnList,
            //    TwoStarOdds = 0.5f,
            //    ThreeStarOdds = 0.2f,
            //    FourStarOdds = 0.1f,
            //});

            //List<CarSetInfo> carSetInfoList = new List<CarSetInfo>();
            //carSetInfoList.Add(new CarSetInfo()
            //{

            //    carSetNumber = 5,
            //    Odds = 0.3f,
            //});

            //CustomerBet customerBet = new CustomerBet()
            //{
            //    Date = DateTime.Now,
            //    BookieType = BookieType.小惠.ToString(),
            //    Customer = _customerSettings.CustomerList[4],
            //    BetStatistics = new BetStatistics()
            //    {
            //        TotalCarSet = 20,
            //        TotalTwoStar = 15,
            //        TotalThreeStar = 34,
            //        TotalFourStar = 5,
            //    },
            //    BetInfoList = betInfoList,
            //    CarSetInfoList = carSetInfoList,
            //};

            //return customerBet;
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
            Calulate(request.Date, BookieType.小惠, request.CustomerName, request.TwoStarBonus1, request.ThreeStarBonus1, request.FourStarBonus1);
            Calulate(request.Date, BookieType.楊董, request.CustomerName, request.TwoStarBonus2, request.ThreeStarBonus2, request.FourStarBonus2);
            return true;
        }

        [HttpGet]
        public async Task<BookieBetModel> GetBookieBet(GetBookieBetRequestModel request)
        {
            BookieBetModel result = new BookieBetModel()
            {
                Date = request.Date,
                BookiePapaerList = new List<BookiePaper>(),
            };

            BookieSetting bookieSetting = _bookieSettings.BookieSettingList.First(o => o.BookieType == (BookieType)request.Bookie);

            string redisKey = $"{request.Date.ToString("yyyy-MM-dd")}";

            List<CustomerBet> customerBetList = await GetCustomerBetListFromRedis(redisKey);

            var paperNumbers = customerBetList
                .Where(o => o.Bookie == (BookieType)request.Bookie)
                .GroupBy(o => o.PaperNumber);

            foreach (var paperNumberData in paperNumbers)
            {
                IEnumerable<BetInfo> BetInfoList = new List<BetInfo>();
                paperNumberData
                    .Select(o => o.BetInfoList).ToList()
                    .ForEach(o => BetInfoList = BetInfoList.Concat(o));

                IEnumerable<CarSetInfo> CarSetInfoList = new List<CarSetInfo>();
                paperNumberData
                    .Select(o => o.CarSetInfoList).ToList()
                    .ForEach(o => CarSetInfoList = CarSetInfoList.Concat(o));

                BookiePaper bookiePaper = new BookiePaper()
                {
                    PaperNumber = paperNumberData.Key,
                    BetInfoList = BetInfoList.ToList(),
                    CarSetInfoList = CarSetInfoList.ToList(),
                    BetStatistics = new BetStatistics()
                    {
                        TotalTwoStar = paperNumberData.Sum(o => float.Parse(o.BetStatistics.TotalTwoStar)).ToString("0.##"),
                        TotalThreeStar = paperNumberData.Sum(o => float.Parse(o.BetStatistics.TotalThreeStar)).ToString("0.##"),
                        TotalFourStar = paperNumberData.Sum(o => float.Parse(o.BetStatistics.TotalFourStar)).ToString("0.##"),
                        TotalCarSet = paperNumberData.Sum(o => float.Parse(o.BetStatistics.TotalCarSet)).ToString("0.##"),
                        FourStarBonus = 0,
                        TwoStarBonus =0,
                        ThreeStarBonus =0,
                        TotalBonusDollars = 0,
                    },
                };

                int TotalBetDollars = Convert.ToInt32(
                    bookiePaper.BetStatistics.TwoStarBonus +
                    bookiePaper.BetStatistics.ThreeStarBonus +
                    bookiePaper.BetStatistics.FourStarBonus +
                    bookiePaper.BetStatistics.TotalCarSet);

                bookiePaper.BetStatistics.TotalBetDollars = TotalBetDollars;
                bookiePaper.BetStatistics.WinLoseDollars = TotalBetDollars;

                result.BookiePapaerList.Add(bookiePaper);
            }


            result.WinLoseDollars = result.BookiePapaerList.Sum(o => o.BetStatistics.WinLoseDollars);

            return result;

        }

        private async void Calulate(DateTime date, BookieType bookie, string CustomerName,
            string TwoStarBonus1, string ThreeStarBonus1, string FourStarBonus1)
        {
            string redisKey = $"{date.ToString("yyyy-MM-dd")}:{CustomerName}:{bookie.ToString()}";
            CustomerBet customerBet = await GetCustomerBetFromRedis(redisKey);

            customerBet.BetStatistics.TwoStarBonus = TwoStarBonus1 == null ? 0 : float.Parse(TwoStarBonus1);
            customerBet.BetStatistics.ThreeStarBonus = ThreeStarBonus1 == null ? 0 : float.Parse(ThreeStarBonus1);
            customerBet.BetStatistics.FourStarBonus = FourStarBonus1 == null ? 0 : float.Parse(FourStarBonus1);

            CountingHelper.Calculate(customerBet);

            await _database.StringSetAsync(redisKey, JsonConvert.SerializeObject(customerBet), CountingHelper.Range);
        }

        private async Task<CustomerBet> GetCustomerBetFromRedis(string redisKey)
        {
            RedisValue data = await _database.StringGetAsync(redisKey);
            CustomerBet customerBet;

            if (data.HasValue)
            {
                customerBet = JsonConvert.DeserializeObject<CustomerBet>(data.ToString()) ?? new CustomerBet();
            }
            else
            {
                customerBet = new CustomerBet();
            }

            customerBet.BetInfoList ??= new List<BetInfo>();
            customerBet.CarSetInfoList ??= new List<CarSetInfo>();

            return customerBet;
        }

        private async Task<List<CustomerBet>> GetCustomerBetListFromRedis(string redisKey)
        {
            RedisValue data = await _database.StringGetAsync(redisKey);
            List<CustomerBet> customerBetList;

            if (data.HasValue)
            {
                customerBetList = JsonConvert.DeserializeObject<List<CustomerBet>>(data.ToString()) ?? new List<CustomerBet>();
            }
            else
            {
                customerBetList = new List<CustomerBet>();
            }

            return customerBetList;
        }
    }
}

