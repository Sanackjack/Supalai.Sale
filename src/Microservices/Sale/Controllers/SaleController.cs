using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Supalai.Sale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        [HttpGet("get/{id}", Name = "GetSaleById")]
        public IActionResult GetSaleById([FromRoute][Required] string id)
        {

            //Example to use custom Exception
            //throw new AuthenticateException(ResponseDatas.AUTHENTICATION_FAIL);
            //throw new AuthenticateException(ResponseDatas.AUTHENTICATION_FAIL.Code, ResponseDatas.AUTHENTICATION_FAIL.Message, ResponseDatas.AUTHENTICATION_FAIL.HttpStatus);

            return new OkObjectResult("Hello world");

        }
    }
}

