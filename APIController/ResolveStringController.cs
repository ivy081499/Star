using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Star.Helper;
using Star.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Star.Controllers
{
    [Route("api/[controller]")]
    public class ResolveStringController : Controller
    {
        [HttpPost]
        public ResponseModel Resolve(string value)
        {
            bool result = ResolveStringHelper.ResolveStringToColumnList(value, out List<Column> columns);

            if (result)
            {
                return new ResponseModel() { IsSuccess = result };
            }

            return new ResponseModel() { IsSuccess = false, Message = "數入文字格式不正確", };
        }
    }
}

