using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLS.Topup.MultiTenancy.Accounting.Dto;

namespace HLS.Topup.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
