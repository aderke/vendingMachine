namespace Ingenio.VendingMachine.Models
{
    public class VendingResult
    {
        public string Message { get; set; }

        public int ProductIdInTray { get; set; }

        public decimal Change { get; set; }
    }
}
