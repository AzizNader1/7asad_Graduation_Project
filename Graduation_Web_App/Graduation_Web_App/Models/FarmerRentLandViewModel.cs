﻿namespace Graduation_Web_App.Models
{
    public class FarmerRentLandViewModel
    {
        public LandImageDto LandImageDto { get; set; }
        public int FarmerId { get; set; }
        public int BuyerFarmerId { get; set; }
        public int LandId { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public double ServicePrice { get; set; }
        public double LandSize { get; set; }
    }
}