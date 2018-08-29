using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ingenio.VendingMachine.Repositories;
using Ingenio.VendingMachine.Services;

namespace Ingenio.VendingMachine.Tests
{
    [TestClass]
    public class ReportingServiceTests
    {
        private ProductRepository productRepository;

        private VendingService vendingService;

        private ReportingService reportingService;

        [TestInitialize]
        public void Init()
        {
            // Arrange
            productRepository = new ProductRepository();

            vendingService = new VendingService();

            reportingService = new ReportingService(productRepository, vendingService);
        }

        [TestMethod]
        public void ReportingService_GetBestseller_RerurnedProduct()
        {
            // Act 
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(3);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            var bestseller = reportingService.GetBestsellerProduct();

            // Assert
            Assert.IsNotNull(bestseller);
            Assert.AreEqual(1, bestseller.Id);
            Assert.AreEqual(1.99m, bestseller.Price);
        }

        [TestMethod]
        public void ReportingService_GetBadseller_RerurnedProduct()
        {
            // Act 
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(3);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            var badseller = reportingService.GetBadSellingProduct();

            // Assert
            Assert.IsNotNull(badseller);
            Assert.AreEqual(3, badseller.Id);
            Assert.AreEqual(1m, badseller.Price);
        }


        [TestMethod]
        public void ReportingService_GetProductsTodayCount_RerurnedCount()
        {
            // Act 
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(3);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            var report = reportingService.GetPurchasesCount(1, DateTime.Now);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(3, report);
        }


        [TestMethod]
        public void ReportingService_GetProductsTodayCount_RerurnedCount1()
        {
            // Act 
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(3);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            var report = reportingService.GetPurchasesCount(2, DateTime.Now);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(2, report);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ReportingService_GetNotBoughtProductsTodayCount_Exception()
        {
            // Act 
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(2);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(3);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            vendingService.ChooseProduct(1);
            vendingService.DepositMoney(10m);

            var report = reportingService.GetPurchasesCount(999, DateTime.Now);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(0, report);
        }

        [TestMethod]
        public void ReportingService_GetNotSoldProductsTodayCount_Exception1()
        {
            // Act 
            var products = productRepository.GetAllProducts();
            vendingService.Load(products);
           
            var report = reportingService.GetPurchasesCount(1, DateTime.Now);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(0, report);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ReportingService_GetNotBoughtProductsTodayCount_Exception2()
        {
            // Act            
            var report = reportingService.GetPurchasesCount(1, DateTime.Now);

            // Assert
            Assert.IsNotNull(report);
            Assert.AreEqual(0, report);
        }
    }
}