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
            return new OkObjectResult("Hello world");
        }
    }
}

