using Ingenio.VendingMachine.Infrastructure;
using Ingenio.VendingMachine.Models;
using Ingenio.VendingMachine.Repositories.Contracts;
using Ingenio.VendingMachine.Services.Contracts;
using System;
using System.Linq;

namespace Ingenio.VendingMachine.Services
{
    public class ReportingService : IReportingService
    {
        private IProductRepository productRepository;

        private IVendingService vendingService;

        public ReportingService(IProductRepository productRepository, IVendingService vendingService)
        {
            this.vendingService = vendingService;
            this.productRepository = productRepository;
        }

        public int GetPurchasesCount(int productId, DateTime date)
        {
            var history = vendingService.GetProductHistory();

            if (history == null || history[productId] == null)
                throw new Exception(Resources.NoSuchProductExceptionMessage);

            var prodHistory = history[productId];

            return prodHistory.History.SoldDate.Where(h => h.Day == date.Day
                && h.Year == date.Year
                && h.Month == date.Month).Count();
        }

        public Product GetBestsellerProduct()
        {
            var history = vendingService.GetProductHistory();

            if (history == null)
                throw new Exception(Resources.NoProductsExceptionMessage);

            var id = history
                .OrderByDescending(p => p.Value.SoldCount)
                .First().Key;

            return productRepository.GetProduct(id);
        }

        public Product GetBadSellingProduct()
        {
            var history = vendingService.GetProductHistory();

            if (history == null)
                throw new Exception(Resources.NoProductsExceptionMessage);

            var id = history
                .Where(p => p.Value.SoldCount != 0)
                .OrderBy(p => p.Value.SoldCount)
                .First().Key;

            return productRepository.GetProduct(id); 
        }
    }
}
