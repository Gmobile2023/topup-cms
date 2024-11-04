namespace HLS.Topup.Web.Areas.App.Startup
{
    public class AppPageNames
    {
        public static class Common
        {                  
            public const string LowBalanceAlerts = "Alarm.LowBalanceAlerts";
            
            public const string NotificationSchedules = "Notifications.NotificationSchedules";
            public const string SystemAccountTransfers = "BalanceManager.SystemAccountTransfers";
            public const string PayBacks = "PayBacks.PayBacks";
            public const string PayBatchBills = "BalanceManager.PayBatchBills";
            public const string AccountBlockBalances = "BalanceManager.AccountBlockBalances";
            public const string LimitProducts = "LimitationManager.LimitProducts";
            public const string Fees = "FeeManager.Fees";
            public const string SaleClearDebts = "Sale.SaleClearDebts";
            public const string SaleLimitDebts = "Sale.SaleLimitDebts";
            public const string SaleMans = "Sale.SaleMans";

            public const string TopupGateResponseMessage ="Administration.TopupGateResponseMessage";
            
            public const string AgentsManage = "Agents.AgentsManage";
            public const string AgentsSupper = "Agents.AgentsSupper";

            public const string AuditActivities = "AuditActivities.AuditActivities";

            public const string Vendors = "Vendors.Vendors";
            public const string Wards = "Address.Wards";
            public const string Districts = "Address.Districts";
            public const string Cities = "Address.Cities";
            public const string Countries = "Address.Countries";
            public const string ServiceConfigurations = "Configuration.ServiceConfigurations";
            public const string Cards = "StockManagement.Cards";
            public const string CardStocks = "StockManagement.CardStocks";
            public const string CardBatchs = "StockManagement.CardBatchs";
            public const string ListTransCard = "StockManagement.ListTransCard";

            public const string BatchAirtimes = "StockManagement.BatchAirtimes";
            public const string StocksAirtimes = "StockManagement.StocksAirtimes";

            public const string Discounts = "DiscountManager.Discounts";
            public const string Deposits = "Deposits.Deposits";
            public const string Banks = "Banks.Banks";
            public const string Providers = "Providers.Providers";
            public const string Products = "Products.Products";
            public const string Categories = "Categories.Categories";
            public const string Services = "Services.Services";
            public const string Administration = "Administration";
            public const string Roles = "Administration.Roles";
            public const string Users = "Administration.Users";
            public const string UserAccounts = "Administration.UserAccounts";
            public const string AuditLogs = "Administration.AuditLogs";
            public const string OrganizationUnits = "Administration.OrganizationUnits";
            public const string Languages = "Administration.Languages";
            public const string DemoUiComponents = "Administration.DemoUiComponents";
            public const string UiCustomization = "Administration.UiCustomization";
            public const string WebhookSubscriptions = "Administration.WebhookSubscriptions";
            public const string DynamicProperties = "Administration.DynamicProperties";
            public const string DynamicEntityProperties = "Administration.DynamicEntityProperties";
            public const string CategoriesManagerment = "Administration.CategoriesManagerment";

            public const string ReportSumTotalBalanceAccount = "Report.ReportSumTotalBalanceAccount";
            public const string ReportDetailBalanceAccount = "Report.ReportDetailBalanceAccount";
            public const string ReportSumBalanceAccount = "Report.ReportSumBalanceAccount";
            public const string ReportTopupItemsAccount = "Report.ReportTopupItemsAccount";
            public const string ReportCardStockHistories = "Report.ReportCardStockHistories";
            public const string ReportCardStockImExProvider = "Report.ReportCardStockImExProvider";
            public const string ReportCardStocImExPort = "Report.ReportCardStocImExPort";
            public const string ReportCardStockInventory = "Report.ReportCardStockInventory";
            public const string ReportTotalDebt = "Report.ReportTotalDebt";
            public const string ReportDebtDetail = "Report.ReportDebtDetail";
            public const string ReportRefundDetail = "Report.ReportRefundDetail";
            public const string ReportTransferDetail = "Report.ReportTransferDetail";
            public const string ReportServiceDetail = "Report.ReportServiceDetail";
            public const string ReportServiceTotal = "Report.ReportServiceTotal";
            public const string ReportServiceProvider = "Report.ReportServiceProvider";
            public const string ReportAgentBalance = "Report.ReportAgentBalance";
            public const string ReportRevenueAgent = "Report.ReportRevenueAgent";
            public const string ReportRevenueCity = "Report.ReportRevenueCity";
            public const string ReportTotalSaleAgent = "Report.ReportTotalSaleAgent";
            public const string ReportRevenueActive = "Report.ReportRevenueActive";
            public const string ProviderReconcile = "Report.ProviderReconcile";
            public const string RefundReconcile = "Report.RefundReconcile";
            public const string ReportComparePartner = "Report.ReportComparePartner";

            public const string ReportCommissionDetail = "Report.ReportCommissionDetail";
            public const string ReportCommissionTotal = "Report.ReportCommissionTotal";

            public const string PolicyManagement = "Administration.PolicyManagement";
            public const string ReportManagement = "Administration.ReportManagement";
            public const string StockManagement = "Administration.StockManagement";
            public const string AirtimeManagement = "Administration.AirtimeManagement";
            public const string FinancialManagement = "Administration.FinancialManagement";
            public const string TransactionMangerment = "Administration.TransactionMangerment";
            public const string SaleMangerment = "Administration.SaleMangerment";
            public const string AgentsMangerment = "Administration.AgentsMangerment";
            public const string OffsetTopupMangerment = "Administration.OffsetTopupMangerment";
            public const string ReportTopupRequestLogs = "Report.ReportTopupRequestLogs";
            public const string PartnerServiceConfigurations = "Configuration.PartnerServiceConfigurations";
        }

        public static class Host
        {
            public const string Tenants = "Tenants";
            public const string Editions = "Editions";
            public const string Maintenance = "Administration.Maintenance";
            public const string Settings = "Administration.Settings.Host";
            public const string Dashboard = "Dashboard";
        }

        public static class Tenant
        {
            public const string Dashboard = "Dashboard.Tenant";
            public const string Settings = "Administration.Settings.Tenant";
            public const string SubscriptionManagement = "Administration.SubscriptionManagement.Tenant";
        }
    }
}
