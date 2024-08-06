namespace Graduation_Web_App.Models
{
    public class FileUploadModel
    {
        public string ModelType { get; set; } /*
                                               "Farmer", "Engineer", 
                                                "Company", "Represintor",
                                                "Land", "Equipment",
                                                 and "Product"
                                                */
        public int ModelId { get; set; } /*
                                           ID of the associated model 
                                           "FarmerId", "EngineerId", 
                                            "CompanyId", "RepresintorId",
                                            "LandId", "EquipmentId",
                                            and "ProductId"
                                          */
        public IFormFile File { get; set; } // The uploaded file
    }
}
