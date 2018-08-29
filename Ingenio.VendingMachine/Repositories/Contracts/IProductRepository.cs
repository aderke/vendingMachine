using Ingenio.VendingMachine.Models;
using System.Collections.Generic;

namespace Ingenio.VendingMachine.Repositories.Contracts
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();

        List<Product> GetAdditionalProducts();

        Product GetProduct(int id);
    }
}
