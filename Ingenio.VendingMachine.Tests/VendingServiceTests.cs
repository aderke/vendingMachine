using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ingenio.VendingMachine.Repositories;
using Ingenio.VendingMachine.Services;
using System.Linq;

namespace Ingenio.VendingMachine.Tests
{
    [TestClass]
    public class VendingServiceTests
    {
        private ProductRepository productRepository;

        private VendingService vendingService;

        [TestInitialize]
        public void Init()
        {
            // Arrange
            productRepository = new ProductRepository();

            vendingService = new VendingService();
        }

        [TestMethod]
        public void ProductRepository_GetMainProducts_RerurnedProducts()
        {
            // Act 
            var products = productRepository.GetAllProducts();            

            // Assert
            Assert.AreEqual(50, products.Count);
            Assert.AreEqual(true, products.Any(p => p.Id == 1));
            Assert.AreEqual(true, products.Any(p => p.Id == 2));
            Assert.AreEqual(true, products.Any(p => p.Id == 3));
            Assert.AreEqual(15, products.Count(p => p.Id == 1));
            Assert.AreEqual(10, products.Count(p => p.Id == 2));
            Assert.AreEqual(25, products.Count(p => p.Id == 3));
        }

        [TestMethod]
        public void ProductRepository_GetAdditionalProducts_RerurnedProducts()
        {
            // Act 
            var products = productRepository.GetAdditionalProducts();

            // Assert
            Assert.AreEqual(25, products.Count);
            Assert.AreEqual(true, products.Any(p => p.Id == 1));
            Assert.AreEqual(true, products.Any(p => p.Id == 4));
            Assert.AreEqual(5, products.Count(p => p.Id == 1));
            Assert.AreEqual(20, products.Count(p => p.Id == 4));
        }

        [TestMethod]
        public void ProductRepository_GetExistingProduct_Product()
        {
            // Act 
            var product = productRepository.GetProduct(1);

            // Assert
            Assert.AreEqual(1, product.Id);
            Assert.AreEqual(1.99m, product.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ProductRepository_GetNonExistingProduct_Exception()
        {
            // Act 
            var product = productRepository.GetProduct(999);
        }


        [TestMethod]
        public void VendingService_ChooseProduct_ProductChoosed()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            var result = vendingService.Load(products);

            // Act 
            var output = vendingService.ChooseProduct(1);          

            // Assert
            Assert.AreEqual(true, result);
            Assert.IsNotNull(output);
            Assert.AreEqual(0, output.ProductIdInTray);
            Assert.AreEqual(0, output.Change);           
        }

        [TestMethod]
        public void VendingService_ChooseProductOneMoreTime_WarningMessage()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            // Act 
            vendingService.ChooseProduct(1);
            var output = vendingService.ChooseProduct(1);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(0, output.ProductIdInTray);
            Assert.AreEqual(0, output.Change);
            Assert.AreEqual("Product already choosed. Please complete purchase process", output.Message);
        }

        [TestMethod]
        public void VendingService_AddMoneyFirst_WarningMessage()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            // Act 
            var output = vendingService.DepositMoney(1);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(0, output.ProductIdInTray);
            Assert.AreEqual(0, output.Change);
            Assert.AreEqual("Please choose your product before inserting money", output.Message);
        }

        [TestMethod]
        public void VendingService_BuyProductInsufficientMoney_Message()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            // Act 
            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(0.01m);
            vendingService.DepositMoney(0.02m);
            var output = vendingService.DepositMoney(0.01m);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(0, output.ProductIdInTray);
            Assert.AreEqual(0, output.Change);
        }

        [TestMethod]
        public void VendingService_BuyProductExactMoney_Product()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            // Act 
            vendingService.ChooseProduct(1);
            var output =  vendingService.DepositMoney(1.99m);
           
            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(1, output.ProductIdInTray);
            Assert.AreEqual(0, output.Change);
            Assert.AreEqual("Your product in the tray. Thanks", output.Message);
        }

        [TestMethod]
        public void VendingService_BuyProductWithChange_Change()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            // Act 
            vendingService.ChooseProduct(1);
            var output = vendingService.DepositMoney(10.99m);

            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(1, output.ProductIdInTray);
            Assert.AreEqual(9, output.Change);
            Assert.AreEqual("Your product in the tray. Thanks", output.Message);
        }

        [TestMethod]
        public void VendingService_BuyNotExistingProduct_WarningMessage()
        {
            // Assert
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            // Act 
            var output = vendingService.ChooseProduct(999);
            
            // Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(0, output.ProductIdInTray);
            Assert.AreEqual(0, output.Change);
            Assert.AreEqual("No such product. Choose another one", output.Message);
        }
    }
}