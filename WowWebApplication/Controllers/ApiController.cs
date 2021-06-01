using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowWebApplication.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> logger;

        public ApiController(ILogger<ApiController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("productHistory")]
        public async Task<IEnumerable<ProductHistory>> GetResource([FromServices] IResourceProxy proxy)
        {
            return await proxy.GetProductHistory();
        }

        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetProducts([FromServices] IResourceProxy proxy)
        {
            return await proxy.GetProducts();
        }

        [HttpGet("user")]
        public ActionResult GetUserResource()
        {
            return Ok(new { name = "Karthikumar Subramanian", token = "1234-455662-22233333-3333" });
        }

        [HttpPost("trolleyTotal")]
        public async Task<ActionResult> TrolleyTotal([FromServices] IResourceProxy proxy, Trolley trolley)
        {
            var products = await proxy.GetProducts();
            if (products.Any() == false)
                throw new InvalidOperationException("No products");

            var result = await proxy.CalculateTrolleyTotal( trolley);

            return Ok(result);
        }

        

        [HttpGet("sort")]
        public async Task<ActionResult> SortResource([FromServices] IResourceProxy proxy, string sortOption)
        {
            //TODO if the backend is not load bearing cache the responses
            var products = await proxy.GetProducts();
            if (products.Any() == false)
                throw new InvalidOperationException("No products");

            var sorter = new ProductSorter();
            switch (sortOption)
            {
                case "Low":
                    return Ok(sorter.SortLowToHighPrice(products));
                case "High":
                    return Ok(sorter.SortHighToLowPrice(products));
                case "Ascending":
                    return Ok(sorter.NameAscending(products));
                case "Descending":
                    return Ok(sorter.NameDescending(products));
                case "Recommended":
                    var productHistory = await proxy.GetProductHistory();
                    return Ok(sorter.Recommended(products, productHistory));                
                default:
                    break;
            };
            return BadRequest($"Invalid Sort Option {sortOption}");
        }
    }
}
