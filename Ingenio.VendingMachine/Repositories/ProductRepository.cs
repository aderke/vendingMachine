using Ingenio.VendingMachine.Infrastructure;
using Ingenio.VendingMachine.Models;
using Ingenio.VendingMachine.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ingenio.VendingMachine.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> allProductTypes = new List<Product>
        {
            new Product { Id = 1, Name = "Sugar-Free Soda", Price = 1.99m },
            new Product { Id = 2, Name = "Healthy Soda", Price = 2.99m },
            new Product { Id = 3, Name = "Unhealthy Soda", Price = 1m },
            new Product { Id = 4, Name = "Craft Beer", Price = 4m }
        };

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            products.AddRange(Enumerable.Repeat(allProductTypes.First(p => p.Id == 1), 15));
            products.AddRange(Enumerable.Repeat(allProductTypes.First(p => p.Id == 2), 10));
            products.AddRange(Enumerable.Repeat(allProductTypes.First(p => p.Id == 3), 25));

            return products;
        }

        public List<Product> GetAdditionalProducts()
        {         
            var products = new List<Product>();

            products.AddRange(Enumerable.Repeat(allProductTypes.First(p => p.Id == 1), 5));
            products.AddRange(Enumerable.Repeat(allProductTypes.First(p => p.Id == 4), 20));

            return products;
        }

        public Product GetProduct(int id)
        {
            var product = allProductTypes.FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new Exception(Resources.NoSuchProductExceptionMessage);

            return product;
        }
    }
}
