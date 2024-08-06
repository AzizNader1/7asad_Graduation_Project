using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Web_App.Models
{
    // Represents a file information entry in the database
    public class FileInformation
    {
        public int Id { get; set; }
        public string OriginalFileName { get; set; }
        public string TransformedFileName { get; set; }
        public string FilePath { get; set; }
        public string ModelType { get; set; } /*
                                               "Farmer", "Engineer", 
                                                "Company", "Represintor",
                                                "Land", "Equipment",
                                                 and "Product"
                                                */

        [ForeignKey("Farmer")]
        public int? FarmerId { get; set; }
        public virtual Farmer? Farmer { get; set; }
        [ForeignKey("BuyerFarmer")]
        public int? BuyerFarmerId { get; set; }
        public virtual BuyerFarmer? BuyerFarmer { get; set; }

        [ForeignKey("Engineer")]
        public int? EngineerId { get; set; }
        public virtual Engineer? Engineer { get; set; }
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        [ForeignKey("Represintor")]
        public int? RepresintorId { get; set; }
        public virtual Represintor? Represintor { get; set; }
        [ForeignKey("Land")]
        public int? LandId { get; set; }
        public virtual Land? Land { get; set; }
        [ForeignKey("Equipment")]
        public int? EquipmentId { get; set; }
        public virtual Equipment? Equipment { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }

}
