using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WowTests")]


namespace WowWebApplication
{
    internal class ProductSorter
    {
        public ProductSorter()
        {
        }

        internal IEnumerable<Product> SortLowToHighPrice(IEnumerable<Product> products)
        {
            return products.OrderBy(x => x.Price);
        }

        internal IEnumerable<Product> SortHighToLowPrice(IEnumerable<Product> products)
        {
            return products.OrderByDescending(x => x.Price);
        }

        internal IEnumerable<Product> NameAscending(IEnumerable<Product> products)
        {
            return products.OrderBy(x => x.Name);
        }

        internal IEnumerable<Product> NameDescending(IEnumerable<Product> products)
        {
            return products.OrderByDescending(x => x.Name);
        }

        internal IEnumerable<Product> Recommended(IEnumerable<Product> products, IEnumerable<ProductHistory> productHistory)
        {
            if (productHistory.Any() == false)
                throw new InvalidOperationException("no productHistory");

            var popularity = productHistory.SelectMany(x => x.Products)
                .GroupBy(x => x.Name)
                .OrderByDescending(x => x.Sum(s => s.Quantity))
                .Select(x => x.Key);

            return ProductsByPopularity(products, popularity);
        }

        //TODO this can be done better
        internal List<Product> ProductsByPopularity(IEnumerable<Product> products, IEnumerable<string> popularity)
        {
            var hashOfProducts = products.ToDictionary(k => k.Name, v => v);

            var result = new List<Product>();
            foreach (var name in popularity)
                if (hashOfProducts.ContainsKey(name))
                {
                    result.Add(hashOfProducts[name]);
                    hashOfProducts.Remove(name);
                }

            // add products not in history
            foreach (var product in hashOfProducts)
                result.Add(product.Value);
            return result;
        }
    }
}