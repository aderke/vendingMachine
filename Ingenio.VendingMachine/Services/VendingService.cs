using Ingenio.VendingMachine.Infrastructure;
using Ingenio.VendingMachine.Models;
using Ingenio.VendingMachine.Services.Contracts;
using System;
using System.Collections.Generic;

namespace Ingenio.VendingMachine.Services
{
    public class VendingService : IVendingService
    {   
        #region Fields and Properties

        private int? choosedProductId;

        private decimal insertedMoney;
        
        // Key: ProductId, Value: ProductType(Count, SoldCount, Price, History)
        private Dictionary<int, ProductType> stock = new Dictionary<int, ProductType>();
             
        #endregion

        #region Public Methods

        public bool Load(List<Product> products)
        {
            foreach(var product in products)
            {
                if(stock.ContainsKey(product.Id))
                {
                    var productType = stock[product.Id];
                    productType.Count++;
                    productType.Price = product.Price;
                }
                else
                {
                    stock.Add(product.Id, new ProductType
                    {
                        Count = 1,
                        Price = product.Price
                    });                   
                }
            }

            return true;
        }

        public VendingResult ChooseProduct(int productId)
        {
            if (choosedProductId.HasValue)
            {
                return new VendingResult { Message = Resources.AlreadyChoosedProductMessage };
            }

            if (!stock.ContainsKey(productId))
            {
                return new VendingResult { Message = Resources.NoProductInStockMessage };
            }

            choosedProductId = productId;

            return new VendingResult { Message = string.Format(Resources.PurchaseProcessMessage, choosedProductId, 0, stock[productId].Price) };
        }

        public VendingResult DepositMoney(decimal money)
        {
            if (!choosedProductId.HasValue)
            {
                return new VendingResult { Message = Resources.ChooseProductBeforeMessage };
            }

            insertedMoney += money;
            var price = stock[choosedProductId.Value].Price;

            if (insertedMoney < price)
            {
                var moneyToAdd = price - insertedMoney;
                return new VendingResult { Message = string.Format(Resources.PurchaseProcessMessage, choosedProductId, insertedMoney, moneyToAdd) };
            }

            var output = WithdrawProduct();

            Reset();
            return output;
        }

        public Dictionary<int, ProductType> GetProductHistory()
        {
            return stock;
        }

        #endregion

        private VendingResult WithdrawProduct()
        {
            var productType = stock[choosedProductId.Value];
            var change = insertedMoney - productType.Price;

            productType.Count--;
            productType.SoldCount++;
            productType.History.SoldDate.Add(DateTime.UtcNow);

            return new VendingResult { Message = Resources.ThanksPurchaseMessage, Change = change, ProductIdInTray = choosedProductId.Value };
        }

        private void Reset()
        {         
            insertedMoney = 0;
            choosedProductId = null;
        }
    }
}