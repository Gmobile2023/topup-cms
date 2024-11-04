using HLS.Topup.Notifications.Dtos;
using HLS.Topup.Notifications;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.BalanceManager;
using HLS.Topup.LimitationManager.Dtos;
using HLS.Topup.LimitationManager;
using HLS.Topup.FeeManager.Dtos;
using HLS.Topup.FeeManager;
using HLS.Topup.Sale.Dtos;
using HLS.Topup.Sale;
using HLS.Topup.Vendors.Dtos;
using HLS.Topup.Vendors;
using HLS.Topup.Address.Dtos;
using HLS.Topup.Address;
using HLS.Topup.Configuration.Dtos;
using HLS.Topup.Configuration;
using HLS.Topup.StockManagement.Dtos;
using HLS.Topup.StockManagement;
using HLS.Topup.DiscountManager.Dtos;
using HLS.Topup.DiscountManager;
using HLS.Topup.Deposits.Dtos;
using HLS.Topup.Deposits;
using HLS.Topup.Banks.Dtos;
using HLS.Topup.Banks;
using HLS.Topup.Providers.Dtos;
using HLS.Topup.Providers;
using HLS.Topup.Products.Dtos;
using HLS.Topup.Products;
using HLS.Topup.Categories.Dtos;
using HLS.Topup.Categories;
using HLS.Topup.Services.Dtos;
using HLS.Topup.Services;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using HLS.Topup.Auditing.Dto;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Authorization.Delegation;
using HLS.Topup.Authorization.Permissions.Dto;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Roles.Dto;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Delegation.Dto;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Authorization.Users.Importing.Dto;
using HLS.Topup.Authorization.Users.Profile.Dto;
using HLS.Topup.Chat;
using HLS.Topup.Chat.Dto;
using HLS.Topup.DynamicEntityProperties.Dto;
using HLS.Topup.Editions;
using HLS.Topup.Editions.Dto;
using HLS.Topup.Friendships;
using HLS.Topup.Friendships.Cache;
using HLS.Topup.Friendships.Dto;
using HLS.Topup.Localization.Dto;
using HLS.Topup.MultiTenancy;
using HLS.Topup.MultiTenancy.Dto;
using HLS.Topup.MultiTenancy.HostDashboard.Dto;
using HLS.Topup.MultiTenancy.Payments;
using HLS.Topup.MultiTenancy.Payments.Dto;
using HLS.Topup.Notifications.Dto;
using HLS.Topup.Organizations.Dto;
using HLS.Topup.Paybacks;
using HLS.Topup.PayBacks.Dtos;
using HLS.Topup.Sessions.Dto;
using HLS.Topup.WebHooks.Dto;
using HLS.Topup.Configuration.PartnerServiceConfigurationDtos;

namespace HLS.Topup
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditNotificationScheduleDto, NotificationSchedule>().ReverseMap();
            configuration.CreateMap<NotificationScheduleDto, NotificationSchedule>().ReverseMap();
            configuration.CreateMap<CreateOrEditSystemAccountTransferDto, SystemAccountTransfer>().ReverseMap();
            configuration.CreateMap<SystemAccountTransferDto, SystemAccountTransfer>().ReverseMap();
            configuration.CreateMap<CreateOrEditPayBatchBillDto, PayBatchBill>().ReverseMap();
            configuration.CreateMap<PayBatchBillDto, PayBatchBill>().ReverseMap();
            configuration.CreateMap<CreateOrEditAccountBlockBalanceDto, AccountBlockBalance>().ReverseMap();
            configuration.CreateMap<AccountBlockBalanceDto, AccountBlockBalance>().ReverseMap();
            //configuration.CreateMap<CreateOrEditLimitProductDto, LimitProduct>().ReverseMap();
            //configuration.CreateMap<LimitProductDto, LimitProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditPayBacksDto, PayBack>().ReverseMap();
            configuration.CreateMap<CreateOrEditFeeDto, Fee>().ReverseMap();
            configuration.CreateMap<FeeDto, Fee>().ReverseMap();
            configuration.CreateMap<CreateOrEditSaleClearDebtDto, SaleClearDebt>().ReverseMap();
            configuration.CreateMap<SaleClearDebtDto, SaleClearDebt>().ReverseMap();
            configuration.CreateMap<CreateOrEditSaleLimitDebtDto, SaleLimitDebt>().ReverseMap();
            configuration.CreateMap<SaleLimitDebtDto, SaleLimitDebt>().ReverseMap();
            //configuration.CreateMap<CreateOrEditSaleManDto, SaleMan>().ReverseMap();
            //configuration.CreateMap<SaleManDto, SaleMan>().ReverseMap();
            configuration.CreateMap<CreateOrEditVendorDto, Vendor>().ReverseMap();
            configuration.CreateMap<VendorDto, Vendor>().ReverseMap();
            configuration.CreateMap<CreateOrEditWardDto, Ward>().ReverseMap();
            configuration.CreateMap<WardDto, Ward>().ReverseMap();
            configuration.CreateMap<CreateOrEditDistrictDto, District>().ReverseMap();
            configuration.CreateMap<DistrictDto, District>().ReverseMap();
            configuration.CreateMap<CreateOrEditCityDto, City>().ReverseMap();
            configuration.CreateMap<CityDto, City>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<CreateOrEditServiceConfigurationDto, ServiceConfiguration>().ReverseMap();
            configuration.CreateMap<ServiceConfigurationDto, ServiceConfiguration>().ReverseMap();
            configuration.CreateMap<CreateOrEditDiscountDto, Discount>().ReverseMap();
            configuration.CreateMap<DiscountDto, Discount>().ReverseMap();
            // configuration.CreateMap<Discount, DiscountDto>()
            //     .ForMember(dto => dto.StatusName, options => options.Ignore())
            //     .ReverseMap();
            configuration.CreateMap<CreateOrEditDepositDto, Deposit>().ReverseMap();
            configuration.CreateMap<DepositDto, Deposit>().ReverseMap();
            configuration.CreateMap<CreateOrEditBankDto, Bank>().ReverseMap();
            configuration.CreateMap<BankDto, Bank>().ReverseMap();
            configuration.CreateMap<CreateOrEditProviderDto, Provider>().ReverseMap();
            configuration.CreateMap<ProviderDto, Provider>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<CreateOrEditCategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CreateOrEditServiceDto, Service>().ReverseMap();
            configuration.CreateMap<ServiceDto, Service>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());



            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.PropertyName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            configuration.CreateMap<CreateOrEditPartnerServiceConfigurationDto, PartnerServiceConfiguration>().ReverseMap();
            configuration.CreateMap<PartnerServiceConfigurationDto, PartnerServiceConfiguration>().ReverseMap();
        }
    }
}
