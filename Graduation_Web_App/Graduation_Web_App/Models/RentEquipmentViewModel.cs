namespace Graduation_Web_App.Models
{
    public class RentEquipmentViewModel
    {
        public EquipmentImageDto EquipmentImageDto { get; set; }
        public int EquipmentId { get; set; }
        public int FarmerId { get; set; }
        public int BuyerFarmerId { get; set; }
        public DateTime RentEndDate { get; set; }
        public DateTime RentStartDate { get; set; }
        public double RentPrice { get; set; }
        public double EquipmentPrice { get; set; }
    }
}
