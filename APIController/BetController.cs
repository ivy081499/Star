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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star.APIController
{
    [Route("api/[controller]")]
    public class BetController : Controller
    {
        private readonly CustomerSettings _customerSettings;

        public BetController(CustomerSettings customerSettings)
        {
            _customerSettings = customerSettings;
        }

        [HttpGet]
        public BetViewModel Get()
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
        public ResponseModel<CustomerBet> Bet(BetRequestModel request)
        {

            bool result = ResolveStringHelper.CheckBetContent(request.BetContent, out List<Column> columns);

            if (!result)
            {
                return new ResponseModel<CustomerBet>() { IsSuccess = false, Message = "數入文字格式不正確", };
            }

            //todo
            List<BetInfo> betInfoList = new List<BetInfo>();
            List<Column> columnList = new List<Column>();
            columnList.Add(new Column() { Numbers = new int[] { 1, 2 }.ToList() });
            columnList.Add(new Column() { Numbers = new int[] { 3, 4 }.ToList() });
            betInfoList.Add(new BetInfo()
            {
                ColumnList = columnList,
                TwoStarOdds = 0.5f,
                ThreeStarOdds = 0.2f,
                FourStarOdds = 0.1f,
            });

            List<CarSetInfo> carSetInfoList = new List<CarSetInfo>();
            carSetInfoList.Add(new CarSetInfo()
            {

                BallNumber = 5,
                Odds = 0.3f,
            });

            CustomerBet customerBet = new CustomerBet()
            {
                Date = DateTime.Now,
                BookieType = BookieType.小惠.ToString(),
                Customer = _customerSettings.CustomerList[4],
                TotalCarSet = 20,
                TotalTwoStar = 15,
                TotalThreeStar = 34,
                TotalFourStar = 5,
                BetInfoList = betInfoList,
                CarSetInfoList = carSetInfoList,
            };

            return new ResponseModel<CustomerBet>()
            {
                IsSuccess = true,
                Data = customerBet,
            };

        }
    }
}

