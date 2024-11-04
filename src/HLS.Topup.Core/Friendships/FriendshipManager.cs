using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;

namespace HLS.Topup.Friendships
{
    public class FriendshipManager : TopupDomainServiceBase, IFriendshipManager
    {
        private readonly IRepository<Friendship, long> _friendshipRepository;

        public FriendshipManager(IRepository<Friendship, long> friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task CreateFriendshipAsync(Friendship friendship)
        {
            if (friendship.TenantId == friendship.FriendTenantId &&
                friendship.UserId == friendship.FriendUserId)
            {
                throw new UserFriendlyException(L("YouCannotBeFriendWithYourself"));
            }

            using (CurrentUnitOfWork.SetTenantId(friendship.TenantId))
            {
                _friendshipRepository.Insert(friendship);
               await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task UpdateFriendshipAsync(Friendship friendship)
        {
            using (CurrentUnitOfWork.SetTenantId(friendship.TenantId))
            {
                _friendshipRepository.Update(friendship);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task<Friendship> GetFriendshipOrNullAsync(UserIdentifier user, UserIdentifier probableFriend)
        {
            using (CurrentUnitOfWork.SetTenantId(user.TenantId))
            {
                return await _friendshipRepository.FirstOrDefaultAsync(friendship =>
                                    friendship.UserId == user.UserId &&
                                    friendship.TenantId == user.TenantId &&
                                    friendship.FriendUserId == probableFriend.UserId &&
                                    friendship.FriendTenantId == probableFriend.TenantId);
            }
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task BanFriendAsync(UserIdentifier userIdentifier, UserIdentifier probableFriend)
        {
            var friendship = (await GetFriendshipOrNullAsync(userIdentifier, probableFriend));
            if (friendship == null)
            {
                throw new Exception("Friendship does not exist between " + userIdentifier + " and " + probableFriend);
            }

            friendship.State = FriendshipState.Blocked;
            await UpdateFriendshipAsync(friendship);
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public async Task AcceptFriendshipRequestAsync(UserIdentifier userIdentifier, UserIdentifier probableFriend)
        {
            var friendship = (await GetFriendshipOrNullAsync(userIdentifier, probableFriend));
            if (friendship == null)
            {
                throw new Exception("Friendship does not exist between " + userIdentifier + " and " + probableFriend);
            }

            friendship.State = FriendshipState.Accepted;
            await UpdateFriendshipAsync(friendship);
        }
    }
}