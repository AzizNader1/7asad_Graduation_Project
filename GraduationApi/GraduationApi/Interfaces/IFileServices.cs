using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IFileServices
    {
        Task<List<FileInformation>> GetFilesByIdAndModel(int modelId, string modelType);

        Task<List<FileInformation>> GetFilesByModelType(string modelType);

        Task<ViewFilesViewModel> GetLatestFileNames(string modelType, int modelId);

    }
}
