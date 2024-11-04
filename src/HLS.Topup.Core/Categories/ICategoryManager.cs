using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Products.Dtos;

namespace HLS.Topup.Categories
{
    public interface ICategoryManager
    {
        Task<List<ProductDto>> GetProductByCategory(string categoryCode);
        Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode);

        Task<List<CategoryDto>> GetCategoryByServiceCodeMuti(List<string> serviceCode);

    }
}