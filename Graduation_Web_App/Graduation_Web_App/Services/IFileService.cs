using Microsoft.AspNetCore.Mvc;
using Graduation_Web_App.Models;

namespace Graduation_Web_App.Services
{
    public interface IFileService
    {
        Task UploadFile(IFormFile file, string modelType, int modelId);
        Task<List<(string OriginalFileName, int ForeignKeyId)>> GetImagesByModelType(string modelType);

        Task<ViewFilesViewModel> GetUserProfileLatestFileNames(string modelType, int modelId);


        //Task<IActionResult> DownloadFile(string modelType, int modelId, string fileName);
        //Task<ViewFilesViewModel> ViewFiles(string modelType, int modelId);
        //Task<string> GetUserProfileImageFileName(string modelType, int modelId);
        //Task<(string latestPdfFileName, string latestWordFileName)> GetUserProfileLatestDocumentFileNames(string modelType, int modelId);

        //Task<List<GrantViewModel>> GetGrantsWithLatestFiles();

        //Task<(string latestImageFileName, string latestPdfFileName, string latestWordFileName)> GetUserProfileLatestFileNames(string modelType, int modelId);
    }

}
