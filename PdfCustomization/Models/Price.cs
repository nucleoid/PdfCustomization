
namespace PdfCustomization.Models
{
    public class Price
    {
        public string DeliveryLocation { get; set; }
        public string State { get; set; }
        public string PackageSizeAndType { get; set; }
        public Mode Mode { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public enum Mode
    {
        Online,
        Offline,
        Magic
    }
}