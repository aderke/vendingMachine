using Ingenio.VendingMachine.History;

namespace Ingenio.VendingMachine.Models
{
    public class ProductType
    {
        public int Count { get; set; }

        public int SoldCount { get; set; }

        public decimal Price { get; set; }

        public StockHistory History { get; set; } = new StockHistory();
    }
}
