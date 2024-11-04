using Abp.Application.Navigation;
using Abp.Localization;

namespace HLS.Topup.Web.Startup
{
    public class MenuNavigationProvider : NavigationProvider
    {
        public const string MenuName = "MenuFrontEnd";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var leftMenu = new MenuDefinition(MenuName, new FixedLocalizableString("Menu"));
            context.Manager.Menus[MenuName] = leftMenu;

            leftMenu
                .AddItem(new MenuItemDefinition(
                    FrontEndPageNames.Home,
                    L("Home_Page"),
                    //icon: "fas fa-history",
                    url: "/"
                ))
                .AddItem(new MenuItemDefinition(
                    FrontEndPageNames.News_Discount,
                    L("News_Page"),
                    //icon: "fas fa-history",
                    url: "/"
                ))
                .AddItem(new MenuItemDefinition(
                    FrontEndPageNames.Introduces,
                    L("Introduces_Page"),
                    //icon: "fas fa-history",
                    url: "/"
                ))
                .AddItem(new MenuItemDefinition(
                    FrontEndPageNames.Contact,
                    L("Contact_Page"),
                    //icon: "fas fa-history",
                    url: "/"
                ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TopupConsts.LocalizationSourceName);
        }
    }
}