using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WowWebApplication
{
    public interface IResourceProxy
    {
        Task<IEnumerable<ProductHistory>> GetProductHistory();
        Task<IEnumerable<Product>> GetProducts();
        Task<decimal> CalculateTrolleyTotal(Trolley trolley);
    }
}