using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduationApi.Data;
using GraduationApi.Models;
using GraduationApi.Interfaces;

namespace GraduationApi.Services
{
    public class FileServices:IFileServices
    {

        private readonly ApplicationDbContext _context;

        public FileServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FileInformation>> GetFilesByIdAndModel(int modelId, string modelType)
        {
            var files = await _context.FileInformations
                .Where(file => (file.FarmerId == modelId && file.ModelType == modelType) ||
                                (file.BuyerFarmerId == modelId && file.ModelType == modelType) ||
                             (file.EngineerId == modelId && file.ModelType == modelType) ||
                             (file.CompanyId == modelId && file.ModelType == modelType) ||
                             (file.LandId == modelId && file.ModelType == modelType) ||
                             (file.ProductId == modelId && file.ModelType == modelType) ||
                             (file.EquipmentId == modelId && file.ModelType == modelType) ||
                             (file.RepresintorId == modelId && file.ModelType == modelType))
                .ToListAsync();

            return files;
        }

        public async Task<List<FileInformation>> GetFilesByModelType(string modelType)
        {
            var files = await _context.FileInformations
                .Where(file => file.ModelType == modelType).ToListAsync();

            return files;
        }

        public async Task<ViewFilesViewModel> GetLatestFileNames(string modelType, int modelId)
        {
            // Call the API endpoint to get files related to the user's ID
            var files = await GetFilesByIdAndModel(modelId, modelType);

            // Filter the files to find the latest uploaded image, PDF, and Word files
            var latestImageFile = files
                .Where(file => IsImageFile(file)) // Filter only image files
                .OrderByDescending(file => file.Id) // Order by ID (assuming higher ID means newer file)
                .FirstOrDefault();

            //var latestPdfFile = files
            //    .Where(file => IsPdfFile(file)) // Filter PDF files
            //    .OrderByDescending(file => file.Id) // Order by ID (assuming higher ID means newer file)
            //    .FirstOrDefault();

            //var latestWordFile = files
            //    .Where(file => IsWordFile(file)) // Filter Word files
            //    .OrderByDescending(file => file.Id) // Order by ID (assuming higher ID means newer file)
            //    .FirstOrDefault();

            var viewModel = new ViewFilesViewModel { ModelType = modelType, ModelId = modelId, latestImageFileName = latestImageFile?.OriginalFileName};

            return (viewModel);
        }

        private bool IsImageFile(FileInformation file)
        {
            // Implement logic to determine if a file is an image
            // For example, check the file extension or other properties
            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.OriginalFileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        //private bool IsPdfFile(FileInformation file)
        //{
        //    // Implement logic to determine if a file is a PDF
        //    var pdfExtensions = new List<string> { ".pdf" };
        //    var fileExtension = Path.GetExtension(file.OriginalFileName).ToLower();
        //    return pdfExtensions.Contains(fileExtension);
        //}

        //private bool IsWordFile(FileInformation file)
        //{
        //    // Implement logic to determine if a file is a Word document
        //    var wordExtensions = new List<string> { ".doc", ".docx" };
        //    var fileExtension = Path.GetExtension(file.OriginalFileName).ToLower();
        //    return wordExtensions.Contains(fileExtension);
        //}

    }
}
