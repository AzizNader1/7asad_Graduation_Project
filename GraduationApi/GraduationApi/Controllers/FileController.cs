using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using GraduationApi.Data;
using GraduationApi.Models;
using GraduationApi.Interfaces;

namespace GraduationApi.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath = "C:\\Users\\ZeeZo\\Desktop\\Graduation_Project\\Graduation_Web_App\\Graduation_Web_App\\wwwroot\\";
        private readonly IFileServices _fileServices;
        public FilesController(ApplicationDbContext context , IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;   
        }


        [HttpGet("GetFilesByIdAndModel/{modelType}/{modelId}")]
        public async Task<List<FileInformation>> GetFilesByIdAndModel(int modelId, string modelType)
        {
            var files = await _fileServices.GetFilesByIdAndModel(modelId , modelType);
            return files;
        }


        [HttpGet("GetFilesByModelType/{modelType}")]
        public async Task<List<FileInformation>> GetFilesByModelType(string modelType)
        {
            var files = await _fileServices.GetFilesByModelType(modelType);

            return files;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var modelType = Request.Form["modelType"].ToString().ToLower();
                var modelId = Convert.ToInt32(Request.Form["modelId"]);
                var file = Request.Form.Files[0];

                if (file == null || file.Length == 0)
                    return BadRequest("No file found in the request.");

                var maxFileSizeBytes = 15 * 1024 * 1024; // 15 megabytes

                if (file.Length > maxFileSizeBytes)
                {
                    return BadRequest("File size exceeds the allowed limit.");
                }

                var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png",};
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    return BadRequest("Unsupported file type.");

                string modelFolder;

                switch (modelType)
                {
                    case "farmer":
                        modelFolder = "FarmerImages";
                        break;
                    case "engineer":
                        modelFolder = "EngineerImages";
                        break;
                    case "company":
                        modelFolder = "CompanyImages";
                        break;
                    case "land":
                        modelFolder = "LandImages";
                        break;
                    case "product":
                        modelFolder = "ProductImages";
                        break;
                    case "equipment":
                        modelFolder = "EquipmentImages";
                        break;
                    case "represintor":
                        modelFolder = "RepresintorImages";
                        break;
                    case "buyerfarmer":
                        modelFolder = "BuyerFarmerImages";
                        break;
                    default:
                        return BadRequest("Invalid model type.");
                }

                var folderPath = Path.Combine(_uploadPath, modelFolder, modelId.ToString());
                Directory.CreateDirectory(folderPath);

                var originalFileName = file.FileName; // Store the original file name
                var transformedFileName = Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);

                var filePath = Path.Combine(folderPath, originalFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Additional logic to save the file information and its association with the model in the database
                // For example, using Entity Framework Core:
                var fileInformation = new FileInformation
                {
                    OriginalFileName = originalFileName, // Store the original file name
                    TransformedFileName = transformedFileName, // Store the transformed file name
                    FilePath = filePath,
                    ModelType = modelType,
                    FarmerId = modelType == "farmer" ? modelId : (int?)null,
                    EngineerId = modelType == "engineer" ? modelId : (int?)null,
                    ProductId = modelType == "product" ? modelId : (int?)null,
                    CompanyId = modelType == "company" ? modelId : (int?)null,
                    LandId = modelType == "land" ? modelId : (int?)null,
                    EquipmentId = modelType == "equipment" ? modelId : (int?)null,
                    RepresintorId = modelType == "represintor" ? modelId : (int?)null,
                    BuyerFarmerId = modelType == "buyerfarmer" ? modelId : (int?)null

                };
                _context.FileInformations.Add(fileInformation);
                _context.SaveChanges();

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        //[HttpGet("download/{modelType}/{modelId}/{fileName}")]
        //public IActionResult DownloadFile(string modelType, int modelId, string fileName)
        //{
        //    try
        //    {
        //        var modelFolder = GetModelFolder(modelType.ToLower());

        //        if (modelFolder == null)
        //            return BadRequest("Invalid model type.");

        //        var fileInformation = _context.fileInformation
        //            .OrderBy(fi => fi.ModelType == modelType &&
        //                ((modelType == "user" && fi.UserUId == modelId) ||
        //                (modelType == "grant" && fi.GrantGraId == modelId) ||
        //                (modelType == "project" && fi.ProjectProId == modelId) ||
        //                (modelType == "report" && fi.ReportId == modelId)) &&
        //                fi.OriginalFileName == fileName).LastOrDefault();

        //        if (fileInformation == null)
        //            return NotFound();

        //        var filePath = Path.Combine(_uploadPath, modelFolder, modelId.ToString(), fileInformation.TransformedFileName);
        //        var contentType = GetContentType(filePath);

        //        return PhysicalFile(filePath, contentType, fileInformation.OriginalFileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        return StatusCode(500, "An error occurred while processing the request.");
        //    }
        //}


        [HttpGet("GetLatestFileNames/{modelType}/{modelId}")]
        public async Task<ViewFilesViewModel> GetLatestFileNames(string modelType, int modelId)
        {


            var viewModel = await _fileServices.GetLatestFileNames(modelType, modelId);

            return (viewModel);
        }

        private string GetModelFolder(string modelType)
        {
            switch (modelType)
            {
                case "farmer":
                    return "FarmerImages";
                case "engineer":
                    return "EngineerImages";
                case "company":
                    return "CompanyImages";
                case "represintor":
                    return "RepresintorImages";
                case "land":
                    return "LandImages";
                case "product":
                    return "ProductImages";
                case "equipment":
                    return "EquipmentImages";
                case "buyerfarmer":
                    return "BuyerFarmerImages";
                default:
                    return null;
            }
        }

        private string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
