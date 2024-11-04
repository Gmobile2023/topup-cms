using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using HLS.Topup.Authorization;

namespace HLS.Topup.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] =
                new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.Dashboard,
                //         L("Dashboard"),
                //         url: "App/HostDashboard",
                //         icon: "flaticon-line-graph",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions
                //             .Pages_Administration_Host_Dashboard)
                //     )
                // )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.StockManagement,
                        L("StockManagement"),
                        icon: "la-warehouse"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.CardBatchs,
                            L("CardBatchs"),
                            url: "App/CardBatchs",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CardBatchs)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.CardStocks,
                            L("CardStocks"),
                            url: "App/CardStocks",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CardStocks)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Cards,
                            L("Cards"),
                            url: "App/Cards",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Cards)
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ListTransCard,
                            L("ListTransCard"),
                            url: "App/Cards/ListTransCard",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Cards)
                        )
                    )
                )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Common.AirtimeManagement,
                //         L("AirtimeManagement"),
                //         icon: "la-archive"
                //     )
                //     .AddItem(new MenuItemDefinition(
                //             AppPageNames.Common.StocksAirtimes,
                //             L("StocksAirtimes"),
                //             url: "App/StocksAirtimes",
                //             icon: "la-braille",
                //             permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_StocksAirtimes)
                //         )
                //     ).AddItem(new MenuItemDefinition(
                //             AppPageNames.Common.BatchAirtimes,
                //             L("BatchAirtimes"),
                //             url: "App/BatchAirtimes",
                //             icon: "la-braille",
                //             permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_BatchAirtimes)
                //         )
                //     )
                // )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.PolicyManagement,
                        L("PolicyManagement"),
                        icon: "la-file-alt"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Discounts,
                            L("Discounts"),
                            url: "App/Discounts",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Discounts)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Fees,
                            L("Fees"),
                            url: "App/Fees",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Fees)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.LimitProducts,
                            L("LimitProducts"),
                            url: "App/LimitProducts",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LimitProducts)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.SaleMangerment,
                        L("SaleMangerment"),
                        icon: "la-users-cog"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.SaleMans,
                            L("SaleMans"),
                            url: "App/SaleMans",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SaleMans)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.SaleLimitDebts,
                            L("SaleLimitDebts"),
                            url: "App/SaleLimitDebts",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SaleLimitDebts)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.SaleClearDebts,
                            L("SaleClearDebts"),
                            url: "App/SaleClearDebts",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_SaleClearDebts)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.AgentsMangerment,
                        L("AgentsMangerment"),
                        icon: "la-user-check"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AgentsManage,
                            L("AgentsManage"),
                            url: "App/AgentsManage",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AgentsManage)
                        )
                    )
                     .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AgentsSupper,
                            L("AgentsSupperTotal"),
                            url: "App/AgentsSupper",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AgentsSupper)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditActivities,
                            L("AuditActivities"),
                            url: "App/AuditActivities",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_AuditActivities)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.LowBalanceAlerts,
                            L("LowBalanceAlerts"),
                            url: "App/LowBalanceAlerts",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LowBalanceAlerts)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.TransactionMangerment,
                        L("TransactionManagements"),
                        icon: "la-exchange-alt"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Deposits,
                            L("Deposits"),
                            url: "App/Deposits",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Deposits)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.TransactionMangerment,
                            L("TransactionManagements"),
                            url: "App/TransactionManagement",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_TransactionManagements)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OffsetTopupMangerment,
                            L("OffsetTopupManagements"),
                            url: "App/TransactionManagement/OffsetTopup",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_TransactionManagements)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.PayBacks,
                            L("PayBacks"),
                            url: "App/PayBacks",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PayBacks)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AccountBlockBalances,
                            L("AccountBlockBalances"),
                            url: "App/AccountBlockBalances",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_AccountBlockBalances)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.PayBatchBills,
                            L("PayBatchBills"),
                            url: "App/PayBatchBills",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PayBatchBills)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.SystemAccountTransfers,
                            L("SystemAccountTransfers"),
                            url: "App/SystemAccountTransfers",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_SystemAccountTransfers)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ProviderReconcile,
                            L("ProviderReconcile"),
                            url: "App/ProviderReconcile",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_ProviderReconcile)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.RefundReconcile,
                            L("ProviderRefund"),
                            url: "App/ProviderReconcile/refund",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_RefundsReconcile)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.CategoriesManagerment,
                        L("CategoriesManagerment"),
                        icon: "la-folder-plus"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Providers,
                            L("Providers"),
                            url: "App/Providers",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Providers)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Products,
                            L("Products"),
                            url: "App/Products",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Products)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Categories,
                            L("Categories"),
                            url: "App/Categories",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Categories)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Services,
                            L("Services"),
                            url: "App/Services",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Services)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Vendors,
                            L("Vendors"),
                            url: "App/Vendors",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Vendors)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Banks,
                            L("Banks"),
                            url: "App/Banks",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Banks)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Countries,
                            L("Countries"),
                            url: "App/Countries",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Countries)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Cities,
                            L("Cities"),
                            url: "App/Cities",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Cities)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Districts,
                            L("Districts"),
                            url: "App/Districts",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Districts)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Wards,
                            L("Wards"),
                            url: "App/Wards",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Wards)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.NotificationSchedules,
                            L("NotificationSchedules"),
                            url: "App/NotificationSchedules",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_NotificationSchedules)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.TopupGateResponseMessage,
                        L("TopupGateResponseMessage"),
                        icon: "la-folder-plus"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.TopupGateResponseMessage,
                            L("TopupGateResponseMessage"),
                            url: "App/TopupGateResponseMessage",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_TopupGateResponseMessage)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ReportManagement,
                            L("ReportManagement"),
                            icon: "la-chart-bar"
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportDetailBalanceAccount,
                                L("ReportDetailBalanceAccount"),
                                url: "App/Reports/BalanceAccount",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportDetailBalanceAccount)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportSumBalanceAccount,
                                L("ReportSumBalanceAccount"),
                                url: "App/Reports/BalanceAccounts",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportSumBalanceAccount)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportSumTotalBalanceAccount,
                                L("ReportSumTotalBalanceAccount"),
                                url: "App/Reports/TotalBalance",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportSumTotalBalanceAccount)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportCardStockHistories,
                                L("ReportCardStockHistories"),
                                url: "App/Reports/CardStockHistories",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_CardStockHistories)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportCardStocImExPort,
                                L("ReportCardStockImExPort"),
                                url: "App/Reports/CardStockImExPort",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_CardStockImExPort)
                            )
                        )
                         .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportCardStockImExProvider,
                                L("ReportCardStockImExProvider"),
                                url: "App/Reports/CardStockImExProvider",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_CardStockImExProvider)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportCardStockInventory,
                                L("ReportCardStockInventory"),
                                url: "App/Reports/CardStockInventory",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_CardStockInventory)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportAgentBalance,
                                L("ReportAgentBalance"),
                                url: "App/Reports/ReportAgentBalance",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportAgentBalance)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportDebtDetail,
                                L("ReportDebtDetail"),
                                url: "App/Reports/AccountDebtDetail",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportDebtDetail)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportTotalDebt,
                                L("ReportTotalDebt"),
                                url: "App/Reports/TotalDebtBalance",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportTotalDebt)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportTransferDetail,
                                L("ReportTransferDetail"),
                                url: "App/Reports/ReportTransferDetail",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportTransferDetail)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportServiceDetail,
                                L("ReportServiceDetail"),
                                url: "App/Reports/ReportServiceDetail",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportServiceDetail)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportServiceTotal,
                                L("ReportServiceTotal"),
                                url: "App/Reports/ReportServiceTotal",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportServiceTotal)
                            )
                        )
                       .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportServiceProvider,
                                L("ReportServiceProvider"),
                                url: "App/Reports/ReportServiceProvider",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportServiceProvider)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportRefundDetail,
                                L("ReportRefundDetail"),
                                url: "App/Reports/ReportRefundDetail",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportRefundDetail)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportRevenueAgent,
                                L("ReportRevenueAgent"),
                                url: "App/Reports/ReportRevenueAgent",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportRevenueAgent)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportRevenueCity,
                                L("ReportRevenueCity"),
                                url: "App/Reports/ReportRevenueCity",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportRevenueCity)
                            )
                        ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ReportTotalSaleAgent,
                            L("ReportTotalSaleAgent"),
                            url: "App/Reports/ReportTotalSaleAgent",
                            icon: "la-braille",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Report_ReportTotalSaleAgent)
                        ))
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportRevenueActive,
                                L("ReportRevenueActive"),
                                url: "App/Reports/ReportRevenueActive",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportRevenueActive)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportComparePartner,
                                L("ReportComparePartner"),
                                url: "App/Reports/ReportComparePartner",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportComparePartner)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportCommissionDetail,
                                L("ReportCommissionDetail"),
                                url: "App/ReportCommission/CommissionDetail",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportCommissionDetail)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportCommissionTotal,
                                L("ReportCommissionTotal"),
                                url: "App/ReportCommission/CommissionTotal",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportCommissionTotal)
                            )
                        ).AddItem(new MenuItemDefinition(
                                AppPageNames.Common.ReportTopupRequestLogs,
                                L("ReportTopupRequestLogs"),
                                url: "App/Reports/ReportTopupRequestLogs",
                                icon: "la-braille",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Report_ReportTopupRequestLogs)
                            )
                        )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.Tenants,
                //         L("Tenants"),
                //         url: "App/Tenants",
                //         icon: "flaticon-list-3",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                //     )
                // )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.Editions,
                //         L("Editions"),
                //         url: "App/Editions",
                //         icon: "flaticon-app",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                //     )
                // )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Tenant.Dashboard,
                //         L("Dashboard"),
                //         url: "App/TenantDashboard",
                //         icon: "flaticon-line-graph",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                //     )
                // )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "la-cog"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "la-sitemap",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "la-user-shield",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "la-users-cog",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UserAccounts,
                            L("UserAccounts"),
                            url: "App/UserAccounts",
                            icon: "la-users-cog",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UserAccounts)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "la-globe-europe",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "la-history",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "la-power-off",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.ServiceConfigurations,
                            L("ServiceConfigurations"),
                            url: "App/ServiceConfigurations",
                            icon: "la-chalkboard",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_ServiceConfigurations)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.PartnerServiceConfigurations,
                            L("PartnerServiceConfigurations"),
                            url: "App/PartnerServiceConfigurations",
                            icon: "la-chalkboard",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_PartnerServiceConfigurations)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "la-palette",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UiCustomization)
                        )
                    )
                    // .AddItem(new MenuItemDefinition(
                    //         AppPageNames.Common.WebhookSubscriptions,
                    //         L("WebhookSubscriptions"),
                    //         url: "App/WebhookSubscription",
                    //         icon: "flaticon2-world",
                    //         permissionDependency: new SimplePermissionDependency(AppPermissions
                    //             .Pages_Administration_WebhookSubscription)
                    //     )
                    // )
                    // .AddItem(new MenuItemDefinition(
                    //         AppPageNames.Common.DynamicProperties,
                    //         L("DynamicProperties"),
                    //         url: "App/DynamicProperty",
                    //         icon: "flaticon-interface-8",
                    //         permissionDependency: new SimplePermissionDependency(AppPermissions
                    //             .Pages_Administration_DynamicProperties)
                    //     )
                    // )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "la-cog",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "la-cog",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_Settings)
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TopupConsts.LocalizationSourceName);
        }
    }
}
