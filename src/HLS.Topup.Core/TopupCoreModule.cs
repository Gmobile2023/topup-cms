using System;
using Abp.AspNetZeroCore;
using Abp.AspNetZeroCore.Timing;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Sources;
using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Configuration;
using Castle.MicroKernel.Registration;
using MailKit.Security;
using HLS.Topup.Authorization.Delegation;
using HLS.Topup.Authorization.Ldap;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Chat;
using HLS.Topup.Common;
using HLS.Topup.Configuration;
using HLS.Topup.DashboardCustomization.Definitions;
using HLS.Topup.Debugging;
using HLS.Topup.DynamicEntityProperties;
using HLS.Topup.Features;
using HLS.Topup.Friendships;
using HLS.Topup.Friendships.Cache;
using HLS.Topup.Localization;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Net.Emailing;
using HLS.Topup.Net.Sms;
using HLS.Topup.Notifications;
using HLS.Topup.WebHooks;

namespace HLS.Topup
{
    [DependsOn(
        typeof(TopupCoreSharedModule),
        typeof(AbpZeroCoreModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetZeroCoreModule),
        typeof(AbpMailKitModule))]
    public class TopupCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //workaround for issue: https://github.com/aspnet/EntityFrameworkCore/issues/9825
            //related github issue: https://github.com/aspnet/EntityFrameworkCore/issues/10407
            AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);
            //Gunner fix How to say Datetime - timestamp without time zone in EF Core 6.0
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Configuration.Auditing.IsEnabled = true;
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            TopupLocalizationConfigurer.Configure(Configuration.Localization);

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            //Adding webhook definition providers
            Configuration.Webhooks.Providers.Add<AppWebhookDefinitionProvider>();
            Configuration.Webhooks.TimeoutDuration = TimeSpan.FromMinutes(1);
            Configuration.Webhooks.IsAutomaticSubscriptionDeactivationEnabled = false;

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = TopupConsts.MultiTenancyEnabled;

            //Enable LDAP authentication
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            //Twilio - Enable this line to activate Twilio SMS integration
            //Configuration.ReplaceService<ISmsSender,TwilioSmsSender>();

            //MobileNet - Enable this line to activate Mobilenet integration
            Configuration.ReplaceService<ISmsSender, MobileNetSender>();

            //Adding DynamicEntityParameters definition providers
            Configuration.DynamicEntityProperties.Providers.Add<AppDynamicEntityPropertyDefinitionProvider>();

            // MailKit configuration
            Configuration.Modules.AbpMailKit().SecureSocketOption = SecureSocketOptions.Auto;
            Configuration.ReplaceService<IMailKitSmtpBuilder, TopupMailKitSmtpBuilder>(DependencyLifeStyle.Transient);

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            //if (DebugHelper.IsDebug)
            //{
            //Disabling email sending in debug mode
            //Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            //}

            Configuration.ReplaceService(typeof(IEmailSenderConfiguration), () =>
            {
                Configuration.IocManager.IocContainer.Register(
                    Component.For<IEmailSenderConfiguration, ISmtpEmailSenderConfiguration>()
                        .ImplementedBy<TopupSmtpEmailSenderConfiguration>()
                        .LifestyleTransient()
                );
            });

            Configuration.Caching.Configure(FriendCacheItem.CacheName,
                cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30); });

            Configuration.Caching.Configure("AbpOtpCache", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(15);
            });
            Configuration.Caching.Configure("SystemAddress", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(365);
            });
            Configuration.Caching.Configure("Cms", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
            Configuration.Caching.Configure("ServiceConfiguations", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
            Configuration.Caching.Configure("PartnerInfo", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
            Configuration.Caching.Configure("ProviderAirtime", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(360);
            });
            Configuration.Caching.Configure(CacheConst.DiscountCache, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
            Configuration.Caching.Configure(CacheConst.Users, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
            Configuration.Caching.Configure("PartnerServiceConfiguations", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });

            IocManager.Register<DashboardConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IChatCommunicator, NullChatCommunicator>();
            IocManager.Register<IUserDelegationConfiguration, UserDelegationConfiguration>();

            IocManager.Resolve<ChatUserStateWatcher>().Initialize();
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
