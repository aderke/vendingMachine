using Ingenio.VendingMachine.Repositories;
using Ingenio.VendingMachine.Services;
using System;

namespace Ingenio.VendingMachine
{
    public class Program
    {
        #region Services

        private static ProductRepository productRepository = new ProductRepository();

        private static VendingService vendingService = new VendingService();

        private static ReportingService reportingService = null;
        private static ReportingService GetReportingService()
        {
            if (reportingService == null)
            {
                reportingService = new ReportingService(productRepository, vendingService);
            }

            return reportingService;
        }

        #endregion

        public static void Main(string[] args)
        {
            // Get all products from repository. Should be 50
            var products = productRepository.GetAllProducts();
            Console.WriteLine("All products count: " + products.Count);

            // Initiallize beverage vending machine with products
            var result = vendingService.Load(products);
            Console.WriteLine("Loaded products: " + result);

            // Load additional products (one existing type and one new type)
            var addProducts = productRepository.GetAdditionalProducts();
            Console.WriteLine("All additional products count: " + addProducts.Count);
            var loadAdditional = vendingService.Load(addProducts);
            Console.WriteLine("Loaded additional products: " + loadAdditional);

            // Customer choose product #1
            var output1 = vendingService.ChooseProduct(1);
            // Output should be: Message = "Product #1 choosed. Cash inserted: 0, cash to add: 1.99"
            Console.WriteLine("Message: " + output1.Message);

            // Customer choose product #1 once again
            var output2 = vendingService.ChooseProduct(1);
            // Output should be: Message = "Product already choosed. Please complete purchase process"
            Console.WriteLine("Message: " + output2.Message);

            // Deposit cash $1
            var output3 = vendingService.DepositMoney(1);
            // Output should be: Message = "Product #1 choosed. Cash inserted: 1, cash to add: 0.99"
            Console.WriteLine("Message: " + output3.Message);

            // Deposit cash $1 once again
            var output4 = vendingService.DepositMoney(1);
            // Output should be: Message = "Your product in the tray. Thanks"
            Console.WriteLine("Message: " + output4.Message);
            Console.WriteLine("ProductId in tray: " + output4.ProductIdInTray);
            Console.WriteLine("Change: " + output4.Change);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(5);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10);

            // Report methods
            var reporting = GetReportingService();
            // Get bestseller product. Should be #1
            var bestseller = reporting.GetBestsellerProduct();
            Console.WriteLine("Bestseller: #" + bestseller.Id + ", Name: " + bestseller.Name);

            // Get bestseller product. Should be #1
            var badtseller = reporting.GetBadSellingProduct();
            Console.WriteLine("Badseller: #" + badtseller.Id + ", Name: " + badtseller.Name);

            // Get product purchased count for today. Should be 2 items
            var count = reporting.GetPurchasesCount(1, DateTime.Now);
            Console.WriteLine("Count: " + count);

            Console.ReadLine();
        }
    }
}    