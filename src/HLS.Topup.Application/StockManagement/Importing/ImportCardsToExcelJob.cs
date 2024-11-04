using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Threading;
using Abp.UI;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Notifications;
using HLS.Topup.RequestDtos;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.Storage;
using TW.CardMapping.Authorization.Users.Importing.Dto;

namespace HLS.Topup.StockManagement.Importing
{
    public class ImportCardsToExcelJob : BackgroundJob<ImportCardsFromExcelJobArgs>, ITransientDependency
    {
        private readonly ICardListExcelDataReader _cardListExcelDataReader;
        private readonly IInvalidCardExporter _invalidUserExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly ICardManager _cardManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserManager UserManager { get; set; }

        public ImportCardsToExcelJob(
            ICardListExcelDataReader cardListExcelDataReader,
            IInvalidCardExporter invalidCardExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            ICardManager cardManager, IUnitOfWorkManager unitOfWorkManager)
        {
            _cardManager = cardManager;
            _unitOfWorkManager = unitOfWorkManager;
            _cardListExcelDataReader = cardListExcelDataReader;
            _invalidUserExporter = invalidCardExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _localizationSource = localizationManager.GetSource(TopupConsts.LocalizationSourceName);
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public override void Execute(ImportCardsFromExcelJobArgs args)
        {
            using var uow = _unitOfWorkManager.Begin();
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var cards = GetCardListFromExcelOrNull(args);
                if (cards == null || !cards.Any() || cards.Any(x => !x.CanBeImported()))
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateCards(args, cards);
            }
            uow.Complete();

            // using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            // {
            //     var cards = GetCardListFromExcelOrNull(args);
            //     if (cards == null || !cards.Any() || cards.Any(x => !x.CanBeImported()))
            //     {
            //         SendInvalidExcelNotification(args);
            //         return;
            //     }
            //
            //     CreateCards(args, cards);
            // }
        }

        private List<CardImportItem> GetCardListFromExcelOrNull(ImportCardsFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _cardListExcelDataReader.GetCardsFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateCards(ImportCardsFromExcelJobArgs args, List<CardImportItem> cards)
        {
            var invalidUsers = new List<CardImportItem>();
            var lstCard = new List<CardItem>();
            foreach (var card in cards)
            {
                if (card.CanBeImported())
                {
                    try
                    {
                        lstCard.Add(new CardItem
                        {
                            CardCode = card.CardCode,
                            ExpiredDate = card.ExpiredDate.Value,
                            Serial = card.Serial,
                            BatchCode = args.BatchCode,
                            CardValue = args.CardValue,
                            //StockType = args.StockType,
                            Status = 0
                        });
                    }
                    catch (UserFriendlyException exception)
                    {
                        card.Exception = exception.Message;
                        invalidUsers.Add(card);
                    }
                    catch (Exception exception)
                    {
                        card.Exception = exception.ToString();
                        invalidUsers.Add(card);
                    }
                }
                else
                {
                    invalidUsers.Add(card);
                }
            }

            AsyncHelper.RunSync(() => CreateCardAsync(lstCard, args.StockType, args.BatchCode));
            //AsyncHelper.RunSync(() => ProcessImportCardsResultAsync(args, invalidUsers));
        }

        private async Task CreateCardAsync(List<CardItem> input, string telco, string batchcode)
        {
            try
            {
                await _cardManager.CardImportListRequest(new CardImportListRequest
                {
                    CardItems = input,
                    //BatchCode = batchcode,
                    //StockType = telco
                });
            }
            catch (Exception ex)
            {
            }
        }

        private void SendInvalidExcelNotification(ImportCardsFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }

        private string GetRoleNameFromDisplayName(string displayName, List<Role> roleList)
        {
            return roleList.FirstOrDefault(
                r => r.DisplayName?.ToLowerInvariant() == displayName?.ToLowerInvariant()
            )?.Name;
        }
    }
}