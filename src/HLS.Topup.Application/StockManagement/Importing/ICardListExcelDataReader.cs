using System.Collections.Generic;
using Abp.Dependency;
using HLS.Topup.StockManagement.Dtos;
using TW.CardMapping.Authorization.Users.Importing.Dto;

namespace HLS.Topup.StockManagement.Importing
{
    public interface ICardListExcelDataReader:ITransientDependency
    {
        List<CardImportItem> GetCardsFromExcel(byte[] fileBytes);
    }
}