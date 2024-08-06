using GraduationApi.Models;

namespace GraduationApi.Interfaces
{
    public interface IProductOrderServices
    {
        Task<IEnumerable<ProductOrder>> GetAllProductOrders();

        Task<ProductOrder> GetProductOrderById(int id);

        Task<string> AddProductOrder(ProductOrder productOrder);

        string UpdateProductOrder(ProductOrder productOrder);

        string DeleteProductOrder(ProductOrder productOrder);

        Task<IEnumerable<ProductOrder>> GetProductsByFarmerId(int farmerId);

        Task<IEnumerable<ProductOrder>> GetProductsByCompanyId(int companyId);

        Task<IEnumerable<ProductOrder>> GetProductsByCompanyName(string companyName);

        Task<IEnumerable<ProductOrder>> GetProductsByFarmerName(string farmerName);

    }
}
