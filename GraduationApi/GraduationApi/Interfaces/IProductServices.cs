using GraduationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationApi.Interfaces
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<List<ProductImageDto>> GetProductsWithImages();

        Task<Product> GetProductById(int id);

        Task<ProductImageDto> GetProductWithImage(int id);

        Task<Product> GetProductByName(string productName);

        Task<IEnumerable<Product>> GetProductsByFarmerId(int id);

        Task<IEnumerable<Product>> GetProductsByFarmerName(string farmerName);

        Task<string> AddProduct(Product Product);

        string UpdateProduct(Product Product);

        string DeleteProduct(Product Product);

        Task<bool> IsValidProduct(int id);

    }
}
