using Ingenio.VendingMachine.Models;
using System.Collections.Generic;
namespace Ingenio.VendingMachine.Services.Contracts
{
    public interface IVendingService
    {
        bool Load(List<Product> products);

        VendingResult ChooseProduct(int productId);

        VendingResult DepositMoney(decimal money);

        Dictionary<int, ProductType> GetProductHistory();
    }
}
