using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Newtonsoft.Json;
using Graduation_Web_App.Models;

namespace Graduation_Web_App.Services
{
    public class FileService : IFileService
    {
        Uri address = new Uri("https://localhost:44398/api/");
        private readonly HttpClient _httpClient;

        public FileService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = address;
        }

        public async Task UploadFile(IFormFile file, string modelType, int modelId)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
            content.Add(new StringContent(modelType), "modelType");
            content.Add(new StringContent(modelId.ToString()), "modelId");

            var response = await _httpClient.PostAsync("files/upload", content);
            response.EnsureSuccessStatusCode();
        }

        //public async Task<IActionResult> DownloadFile(string modelType, int modelId, string fileName)
        //{
        //    var response = await _httpClient.GetAsync($"files/download/{modelType.ToLower()}/{modelId}/{fileName}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var fileStream = await response.Content.ReadAsStreamAsync();

        //        // Determine the content type of the file based on its extension
        //        var contentType = GetContentType(fileName);

        //        // Return the file as a FileStreamResult to initiate download
        //        return new FileStreamResult(fileStream, contentType)
        //        {
        //            FileDownloadName = fileName // Set the suggested download file name
        //        };
        //    }
        //    else
        //    {
        //        var errorMessage = "Error downloading the file."; // Customize the error message
        //        return new ContentResult
        //        {
        //            StatusCode = (int)response.StatusCode,
        //            Content = errorMessage,
        //            ContentType = "text/plain"
        //        };
        //    }
        //}


        // Helper method to get the content type based on the file extension
        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


        //public async Task<string> GetUserProfileImageFileName(string modelType, int modelId)
        //{
        //    // Call the API endpoint to get files related to the user's ID
        //    var files = await _httpClient.GetFromJsonAsync<List<FileInformation>>($"files/GetFilesByIdAndModel/{modelType.ToLower()}/{modelId}");

        //    // Filter the files to find the latest uploaded image
        //    var latestImageFile = files
        //        .Where(file => IsImageFile(file)) // Filter only image files
        //        .OrderByDescending(file => file.Id) // Order by ID (assuming higher ID means newer file)
        //        .FirstOrDefault();

        //    return latestImageFile?.OriginalFileName;
        //}


        //public async Task<(string latestPdfFileName, string latestWordFileName)> GetUserProfileLatestDocumentFileNames(string modelType, int modelId)
        //{
        //    // Call the API endpoint to get files related to the user's ID
        //    var files = await _httpClient.GetFromJsonAsync<List<FileInformation>>($"files/GetFilesByIdAndModel/{modelType.ToLower()}/{modelId}");

        //    // Filter the files to find the latest uploaded PDF and Word files
        //    var latestPdfFile = files
        //        .Where(file => IsPdfFile(file)) // Filter PDF files
        //        .OrderByDescending(file => file.Id) // Order by ID (assuming higher ID means newer file)
        //        .FirstOrDefault();

        //    var latestWordFile = files
        //        .Where(file => IsWordFile(file)) // Filter Word files
        //        .OrderByDescending(file => file.Id) // Order by ID (assuming higher ID means newer file)
        //        .FirstOrDefault();

        //    return (latestPdfFile?.OriginalFileName, latestWordFile?.OriginalFileName);
        //}





        public async Task<ViewFilesViewModel> GetUserProfileLatestFileNames(string modelType, int modelId)
        {
            // Call the API endpoint to get files related to the user's ID
            var files = await _httpClient.GetFromJsonAsync<List<FileInformation>>($"files/GetFilesByIdAndModel/{modelType.ToLower()}/{modelId}");

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


        //public async Task<List<GrantViewModel>> GetGrantsWithLatestFiles()
        //{
        //    //var grants = await _httpClient.GetFromJsonAsync<List<Grant>>("grants");

        //    List<Grant> grants = new List<Grant>();

        //    HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Grant/GetAllGrants");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        grants = JsonConvert.DeserializeObject<List<Grant>>(data);
        //    }

        //    var grantViewModels = new List<GrantViewModel>();

        //    foreach (var grant in grants)
        //    {
        //        var latestFiles = await GetUserProfileLatestFileNames("grant" , grant.GraId);

        //        var grantViewModel = new GrantViewModel
        //        {
        //            Grant = grant,
        //            LatestImageFileName = latestFiles.latestImageFileName,
        //            LatestPdfFileName = latestFiles.latestPdfFileName,
        //            LatestWordFileName = latestFiles.latestWordFileName
        //        };

        //        grantViewModels.Add(grantViewModel);
        //    }

        //    return grantViewModels;
        //}


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


        public async Task<List<(string OriginalFileName, int ForeignKeyId)>> GetImagesByModelType(string modelType)
        {
            // Call the API endpoint to get files related to the specified ModelType
            var files = await _httpClient.GetFromJsonAsync<List<FileInformation>>($"files/GetFilesByModelType/{modelType.ToLower()}");

            // Filter the files to find image files
            var imageFiles = files
                .Where(file => IsImageFile(file)) // Filter only image files
                .Select(file => (file.OriginalFileName, GetForeignKeyId(file))) // Select original file name and foreign key ID
                .ToList();

            return imageFiles;
        }

        private int GetForeignKeyId(FileInformation file)
        {
            // Implement logic to get the foreign key ID from the FileInformation object
            // For example, check FarmerId, EngineerId, CompanyId, RepresintorId, LandId, ProductId, or EquipmentId properties
            if (file.ModelType.ToLower() == "farmer")
            {
                return file.FarmerId ?? 0; // Return FarmerId if not null, otherwise 0
            }
            else if (file.ModelType.ToLower() == "engineer")
            {
                return file.EngineerId ?? 0; // Return EngineerId if not null, otherwise 0
            }
            else if (file.ModelType.ToLower() == "company")
            {
                return file.CompanyId ?? 0; // Return CompanyId if not null, otherwise 0
            }
            else if (file.ModelType.ToLower() == "represintor")
            {
                return file.RepresintorId ?? 0; // Return RepresintorId if not null, otherwise 0
            }
            else if (file.ModelType.ToLower() == "land")
            {
                return file.LandId ?? 0; // Return LandId if not null, otherwise 0
            }
            else if (file.ModelType.ToLower() == "product")
            {
                return file.ProductId ?? 0; // Return ProductId if not null, otherwise 0
            }
            else if (file.ModelType.ToLower() == "equipment")
            {
                return file.EquipmentId ?? 0; // Return EquipmentId if not null, otherwise 0
            }
            else if(file.ModelType.ToLower() == "buyerfarmer")
            {
                return file.BuyerFarmerId ?? 0;
            }

            return 0; // Default value if ModelType is not recognized
        }

       
    }

}
