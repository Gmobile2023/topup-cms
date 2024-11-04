using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.PayBacks.Dtos;
using Microsoft.Extensions.Logging;
using System.Linq;
using Abp.Collections.Extensions;
using HLS.Topup.Dtos.Stock;

namespace HLS.Topup.Paybacks
{
    public class PayBacksManager : TopupDomainServiceBase, IPayBacksManager
    {
        private readonly IRepository<PayBack> _payBacksRepository;
        private readonly IRepository<PayBackDetail> _payBacksDetailsRepository;
        private readonly ILogger<PayBacksManager> _logger;
        private readonly ICommonManger _commonManger;
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _lookup_userRepository;
        
        public PayBacksManager(IRepository<PayBack> payBacksRepository,
            IRepository<PayBackDetail> payBacksDetailsRepository,
            ILogger<PayBacksManager> logger,
            ICommonManger commonManger,
            UserManager userManager,
            IRepository<User, long> lookup_userRepository)
        {
            _payBacksRepository = payBacksRepository;
            _payBacksDetailsRepository = payBacksDetailsRepository;
            _commonManger = commonManger;
            _userManager = userManager;
            _logger = logger;
            _lookup_userRepository = lookup_userRepository;
        }
        
        public async Task<List<PayBacksDetailDto>> GetPayBacksDetails(int payBacksId)
        {
            var query = from p in _lookup_userRepository.GetAll()
                join d in _payBacksDetailsRepository.GetAllIncluding(x => x.PayBackFk)
                        .Where(x => x.PayBackId == payBacksId) on p.Id equals d.UserId
            select new PayBacksDetailDto
                {
                     UserId = d.UserId,
                     AgentCode = p.AccountCode,
                     FullName = p.FullName,
                     PhoneNumber = p.PhoneNumber,
                     Amount = d.Amount,
                     TransCode = d.TransCode
                };
            return query.ToList();
        }

       
    }
}