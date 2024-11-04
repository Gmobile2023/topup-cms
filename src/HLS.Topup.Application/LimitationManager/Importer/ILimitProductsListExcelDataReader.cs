using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.LimitationManager.Dtos;

namespace HLS.Topup.LimitationManager.Importer
{
    public interface ILimitProductsListExcelDataReader : ITransientDependency
    {
        List<LimitProductImportDto> GetLimitProductFromExcel(byte[] fileBytes);
    }
}