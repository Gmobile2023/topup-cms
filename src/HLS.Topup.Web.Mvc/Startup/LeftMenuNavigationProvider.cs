using Abp.Application.Navigation;
using Abp.Localization;

namespace HLS.Topup.Web.Startup
{
    public class LeftMenuNavigationProvider : NavigationProvider
    {
        public const string MenuName = "LeftMenu";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var leftMenu = new MenuDefinition(MenuName, new FixedLocalizableString("Left menu"));
            context.Manager.Menus[MenuName] = leftMenu;

            leftMenu
                .AddItem(new MenuItemDefinition(
                        FrontEndPageNames.Profile,
                        L("AccountManger"),
                        icon: "fa fa-user")
                    .AddItem(new MenuItemDefinition(
                        FrontEndPageNames.Profile,
                        L("Profile"),
                        url: "/Profile",
                        icon: "fas fa-id-badge"))
                    .AddItem(new MenuItemDefinition(
                        FrontEndPageNames.Deposit,
                        L("Deposit"),
                        url: "/Transactions/Deposit",
                        icon: "fas fa-comments-dollar"))
                    .AddItem(new MenuItemDefinition(
                        FrontEndPageNames.TrasferMoney,
                        L("TransferMoney"),
                        url: "/Transactions/TransferMoney",
                        icon: "fas fa-comments-dollar"))
                    .AddItem(new MenuItemDefinition(
                        FrontEndPageNames.BalanceHistory,
                        L("TotalDay"),
                        url: "/Report/TotalDay",
                        icon: "fas fa-file-medical-alt"))
                )
                // .AddItem(new MenuItemDefinition(
                //     FrontEndPageNames.AgentManagement,
                //     L("AgentManagement"),
                //     icon: "fas fa-users-cog"
                //     )
                //     .AddItem(new MenuItemDefinition(
                //         FrontEndPageNames.AgentManagement,
                //         L("AgentManagement"),
                //         url: "/AgentManagement",
                //         icon: "fas fa-user-plus"))
                //
                //     .AddItem(new MenuItemDefinition(
                //         FrontEndPageNames.DiscountManagement,
                //         L("DiscountManagementAgent"),
                //         icon: "fas fa-user-tag",
                //         url: "/DiscountManagement"))
                //     .AddItem(new MenuItemDefinition(
                //         FrontEndPageNames.CollectDiscount,
                //         L("CollectDiscount"),
                //         icon: "fas fa-user-tag",
                //         url: "/DiscountManagement/CollectDiscount"))
                // )
                .AddItem(new MenuItemDefinition(
                            FrontEndPageNames.Topup,
                            L("Payments"),
                            icon: "fas fa-shopping-cart")
                        .AddItem(new MenuItemDefinition(
                            FrontEndPageNames.Topup,
                            L("TopupPhone"),
                            icon: "fas fa-phone",
                            url: "/Topup"))
                        // .AddItem(new MenuItemDefinition(
                        //      FrontEndPageNames.TopupList,
                        //      L("TopupList"),
                        //      icon: "fas fa-clipboard-list",
                        //      url: "/Topup/TopupList"))
                        .AddItem(new MenuItemDefinition(
                            FrontEndPageNames.PinCode,
                            L("PinCode"),
                            icon: "fas fa-sd-card",
                            url: "/Topup/PinCode"))
                        .AddItem(new MenuItemDefinition(
                            FrontEndPageNames.BillPayment,
                            L("BillPayment"),
                            icon: "fas fa-file-text",
                            url: "/BillPayment"))
                        .AddItem(new MenuItemDefinition(
                            FrontEndPageNames.BatchLotPayment,
                            L("Nạp lô"),
                            icon: "fas fa-file-text",
                            url: "/batchtopup"))
                // .AddItem(new MenuItemDefinition(
                //     FrontEndPageNames.Tkc,
                //     L("Tkc"),
                //     icon: "flaticon-line-graph",
                //     url: "/Topup/Tkc"))
                // .AddItem(new MenuItemDefinition(
                //     FrontEndPageNames.SmsGame,
                //     L("SmsGame"),
                //     icon: "fas fa-gamepad",
                //     url: "/Topup/SmsGame"))
                )
                .AddItem(new MenuItemDefinition(
                    FrontEndPageNames.TopupHistory,
                    L("TopupHistory"),
                    icon: "fas fa-history",
                    url: "/Transactions/TransactionHistory"
                ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TopupConsts.LocalizationSourceName);
        }
    }
}