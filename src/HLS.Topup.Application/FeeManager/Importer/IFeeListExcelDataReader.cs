using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.FeeManager.Dtos;

namespace HLS.Topup.FeeManager.Importer
{
    public interface IFeeListExcelDataReader : ITransientDependency
    {
        List<FeeImportDto> GetFeeImportFromExcel(byte[] fileBytes);
    }
}