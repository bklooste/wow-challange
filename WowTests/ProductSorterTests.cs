using Xunit;
using WowWebApplication;
using System.Linq;
using System.Collections.Generic;
using System;

namespace WowTests
{

    public class ProductSorterTests
    {

        [Fact]
        public void NameAscending()
        {
            var data = GetNameData();
            var result = new ProductSorter().NameAscending(data);
            Assert.Equal("a", result.First().Name);
            Assert.Equal("c", result.Last().Name);
        }

        [Fact]
        public void NameDescending()
        {
            var data = GetNameData();
            var result = new ProductSorter().NameDescending(data);
            Assert.Equal("c", result.First().Name);
        }

        [Fact]
        public void PriceAscending()
        {
            var data = GetPriceData();
            var result = new ProductSorter().SortLowToHighPrice(data);
            Assert.Equal(4, result.First().Price);
            Assert.Equal(6, result.Last().Price);
        }

        [Fact]
        public void PriceDescending()
        {
            var data = GetPriceData();
            var result = new ProductSorter().SortHighToLowPrice(data);
            Assert.Equal(6, result.First().Price);
            Assert.Equal(4, result.Last().Price);
        }

        [Fact]
        public void Recommended()
        {
            var data = GetNameData();
            var result = new ProductSorter().Recommended(data, GetHistory());
            Assert.Equal("c", result.First().Name);
        }

        //TODO more testing on recommended esp where products are not in history

        private IEnumerable<ProductHistory> GetHistory()
        {
            return new ProductHistory[]
            {
                new ProductHistory(){ Products = GetNameData().ToList() , CustomerId =1 },
                new ProductHistory(){ Products = GetNameData().ToList() , CustomerId =2 },
            };
        }

        private Product[] GetNameData()
        {
            return new Product[]
            {
                new Product(){ Name = "b" , Price = 1 , Quantity = 1},
                new Product(){ Name = "a" , Price = 1 , Quantity = 2},
                new Product(){ Name = "c" , Price = 1 , Quantity = 3}
            };
        }

        private Product[] GetPriceData()
        {
            return new Product[]
            {
                new Product(){ Name = "a" , Price = 5 , Quantity = 1},
                new Product(){ Name = "a" , Price = 4 , Quantity = 1},
                new Product(){ Name = "a" , Price = 6 , Quantity = 1}
            };
        }
    }
}
