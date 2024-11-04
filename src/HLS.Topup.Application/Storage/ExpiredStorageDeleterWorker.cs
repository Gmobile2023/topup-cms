using System;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.EFPlus;
using Abp.Threading;
using Microsoft.Extensions.Logging;

namespace HLS.Topup.Storage
{
    public class ExpiredStorageDeleterWorker : ITransientDependency
    {
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;
        private readonly ILogger<ExpiredStorageDeleterWorker> _logger;
        private const int MaxDeletionCount = 1;

        public ExpiredStorageDeleterWorker(IRepository<BinaryObject, Guid> binaryObjectRepository,
            ILogger<ExpiredStorageDeleterWorker> logger)
        {
            _binaryObjectRepository = binaryObjectRepository;
            _logger = logger;
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual void DeleteBinaryObject()
        {
            try
            {
                DoWork();
            }
            catch (Exception e)
            {
                _logger.LogError($"DeleteBinaryObject error:{e}");
            }
        }

        private void DoWork()
        {
            _logger.LogInformation($"Begin check items");
            var items = _binaryObjectRepository.GetAll();
            if (!items.Any()) return;
            _logger.LogInformation($"Delete:{items.Count()} items");
            foreach (var item in items)
            {
                _logger.LogInformation($"Begin Delete item:{item.Id}");
                AsyncHelper.RunSync(() => _binaryObjectRepository.DeleteAsync(item));
                _logger.LogInformation($"Delete item:{item.Id} success");
            }
        }
    }
}