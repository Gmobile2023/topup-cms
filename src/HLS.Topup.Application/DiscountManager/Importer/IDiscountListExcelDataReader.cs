using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.DiscountManager.Dtos;

namespace HLS.Topup.DiscountManager.Importer
{
    public interface IDiscountListExcelDataReader : ITransientDependency
    {
        List<DiscountImportDto> GetDiscountFromExcel(byte[] fileBytes);
    }
}