using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Common;
using HLS.Topup.Products;
using HLS.Topup.Products.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace HLS.Topup.Categories
{
    public class CategoryManager : TopupDomainServiceBase, ICategoryManager
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        //private readonly Logger _logger = LogManager.GetLogger("CategoryManager");
        private readonly ILogger<CategoryManager> _logger;

        public CategoryManager(ICommonManger commonManger,
            IRepository<Category> categoryRepository,
            IRepository<Product> productRepository, ILogger<CategoryManager> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetProductByCategory(string categoryCode)
        {
            var data = await _productRepository
                .GetAllIncluding(x => x.CategoryFk)
                .Where(x => x.CategoryFk.CategoryCode == categoryCode && x.Status == CommonConst.ProductStatus.Active).ToListAsync();
            return data?.ConvertTo<List<ProductDto>>();
        }

        public async Task<List<CategoryDto>> GetCategoryByServiceCode(string serviceCode)
        {
            var data = await _categoryRepository
                .GetAllIncluding(x => x.ServiceFk)
                .Where(x => x.ServiceFk.ServiceCode == serviceCode && x.Status == CommonConst.CategoryStatus.Active).ToListAsync();
            return data?.ConvertTo<List<CategoryDto>>();
        }

        public async Task<List<CategoryDto>> GetCategoryByServiceCodeMuti(List<string> serviceCode)
        {            
            var serviceCodes = serviceCode.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).ToList();
            var data = await _categoryRepository
                .GetAllIncluding(x => x.ServiceFk)
                .Where(x => serviceCodes.Contains(x.ServiceFk.ServiceCode) && x.Status == CommonConst.CategoryStatus.Active).ToListAsync();
            return data?.ConvertTo<List<CategoryDto>>();
        }
    }
}
