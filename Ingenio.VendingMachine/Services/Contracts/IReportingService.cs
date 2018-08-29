using Ingenio.VendingMachine.Models;
using System;

namespace Ingenio.VendingMachine.Services.Contracts
{
    public interface IReportingService
    {
        int GetPurchasesCount(int productId, DateTime date);

        Product GetBestsellerProduct();

        Product GetBadSellingProduct();
    }
}
