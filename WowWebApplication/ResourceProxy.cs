using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WowWebApplication
{
    public class ResourceProxy : IResourceProxy
    {
        private readonly HttpClient httpClient;
        ILogger logger;

        public ResourceProxy(HttpClient httpClient, ILogger<ResourceProxy> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<decimal> CalculateTrolleyTotal(Trolley trolley)
        {
            var json = JsonConvert.SerializeObject(trolley);

            try
            {
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("trolleyCalculator?token=03e690e0-7f10-4d25-8b0b-e62ff2e1ad1e", stringContent);

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    return decimal.Parse(result);
                }
            }
            catch (Exception)
            {

                logger.LogInformation(json);
                throw;
            }
        }

        public async Task<IEnumerable<ProductHistory>> GetProductHistory()
        {
            var responseString = await httpClient.GetStringAsync("shopperHistory?token=03e690e0-7f10-4d25-8b0b-e62ff2e1ad1e");

            return JsonConvert.DeserializeObject<ProductHistory[]>(responseString);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var responseString = await httpClient.GetStringAsync("products?token=03e690e0-7f10-4d25-8b0b-e62ff2e1ad1e");

            return JsonConvert.DeserializeObject<Product[]>(responseString);
        }

    }


    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ProductHistory
    {
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
    }

    public class TrolleyProduct
    {
        public string name { get; set; }
        public decimal price { get; set; }
    }

    public class Quantity
    {
        public string name { get; set; }
        public int quantity { get; set; }
    }

    public class Special
    {
        public List<Quantity> quantities { get; set; }
        public decimal total { get; set; }
    }

    public class Trolley
    {
        public List<TrolleyProduct> products { get; set; }
        public List<Special> specials { get; set; }
        public List<Quantity> quantities { get; set; }
    }
}
