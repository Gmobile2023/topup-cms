using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using HLS.Topup.Dto;

namespace HLS.Topup.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
