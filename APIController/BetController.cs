using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Star.Enums;
using Star.Settings;
using Star.ViewModels;

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
    }
}

