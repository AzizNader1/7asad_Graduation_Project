namespace Graduation_Web_App.Models
{
    public class FarmerBuyProductViewModel
    {
        public ProductImageDto ProductImageDto { get; set; }
        public int BuyerFarmerId { get; set; }
        public int FarmerId { get; set; }
        public string ProductName { get; set; }
        public double OrderWeight { get; set; }
        public double OrderPrice { get; set; }
        public double CurrentWeight { get; set; }
        public double CurrentPrice { get; set; }
    }
}
