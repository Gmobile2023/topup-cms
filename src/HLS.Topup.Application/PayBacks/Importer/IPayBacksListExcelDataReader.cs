using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.PayBacks.Dtos;

namespace HLS.Topup.PayBacks.Importer
{
    public interface IPayBacksListExcelDataReader : ITransientDependency
    {
        List<PayBacksImportDto> GetPayBacksFromExcel(byte[] fileBytes);
    }
}