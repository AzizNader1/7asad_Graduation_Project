namespace Graduation_Web_App.Models
{
    public class BuyProductViewModel
    {
        public ProductImageDto ProductImageDto { get; set; }
        public int CompanyId { get; set; }
        public int FarmerId { get; set; }
        public string ProductName { get; set; }
        public double OrderWeight { get; set; }
        public double OrderPrice { get; set; }
        public double AvalibleWeight { get; set; }
        public double CurrentPrice { get; set; }
    }
}
