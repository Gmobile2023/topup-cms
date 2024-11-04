using System.Threading.Tasks;
using Abp.Dependency;

namespace HLS.Topup.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}