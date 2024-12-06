using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace HLS.Topup.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ??
                        context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var pagesFrontEnd = pages.CreateChildPermission(AppPermissions.PagesFrontEnd, L("FrontEnd"));
            var pagesBackend = pages.CreateChildPermission(AppPermissions.PagesBackEnd, L("BackEnd"));

            var lowBalanceAlerts =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_LowBalanceAlerts, L("LowBalanceAlerts"));
            lowBalanceAlerts.CreateChildPermission(AppPermissions.Pages_LowBalanceAlerts_Create,
                L("LowBalanceAlertsCreate"));
            lowBalanceAlerts.CreateChildPermission(AppPermissions.Pages_LowBalanceAlerts_Edit,
                L("LowBalanceAlertsEdit"));

            var providerReconcile =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_ProviderReconcile, L("ProviderReconcile"));
            providerReconcile.CreateChildPermission(AppPermissions.Pages_ProviderReconcile_Comapre,
                L("ProviderReconcile_Comapre"));

            var refundsReconcile =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_RefundsReconcile, L("RefundsReconcile"));
            refundsReconcile.CreateChildPermission(AppPermissions.Pages_RefundsReconcile_Approval,
                L("RefundsReconcile_Approval"));

            var auditActivities =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_AuditActivities, L("AuditActivities"));

            var payment =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_CreatePayment, L("Pages_CreatePayment"));
            var paymentTopup =
                payment.CreateChildPermission(AppPermissions.Pages_CreatePayment_Topup, L("Pages_CreatePayment_Topup"));
            var paymentPinCode =
                payment.CreateChildPermission(AppPermissions.Pages_CreatePayment_PinCode,
                    L("Pages_CreatePayment_PinCode"));
            var paymentPayBill =
                payment.CreateChildPermission(AppPermissions.Pages_CreatePayment_PayBill,
                    L("Pages_CreatePayment_PayBill"));
            var requestDeposit =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_RequestDeposit, L("Pages_RequestDeposit"));
            var transfer =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_TransferMoney, L("Pages_TransferMoney"));
            var balanceHistory =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_BalanceHistory, L("Pages_BalanceHistory"));
            var batchLotHistory =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_BatchLotHistory, L("Pages_BatchLotHistory"));
            batchLotHistory.CreateChildPermission(AppPermissions.Pages_BatchLotStop, L("BatchLotStop"));
            var batchLotPayment = pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_CreateBatchLotPayment,
                L("Pages_CreateBatchLotPayment"));
            batchLotPayment.CreateChildPermission(AppPermissions.Pages_BatchLotTopup, L("BatchLotTopup"));
            batchLotPayment.CreateChildPermission(AppPermissions.Pages_BatchLotPinCode, L("BatchLotPinCode"));
            batchLotPayment.CreateChildPermission(AppPermissions.Pages_BatchLotPayBill, L("BatchLotPayBill"));
            var transactionHistory = pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_TransactionHistory,
                L("Pages_TransactionHistory"));
            var agentParentFontend =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_Report_CommissionAgentGeneral,
                    L("ReportCommissionAgentGeneral"));
            agentParentFontend.CreateChildPermission(AppPermissions.Pages_Report_CommissionAgentDetail,
                L("ReportCommissionAgentDetail"));
            agentParentFontend.CreateChildPermission(AppPermissions.Pages_Report_CommissionAgentTotal,
                L("ReportCommissionAgentTotal"));
            agentParentFontend.CreateChildPermission(AppPermissions.Pages_Report_CommissionAgentDash,
                L("ReportCommissionAgentDash"));


            var staffManager =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_StaffManager, L("Pages_StaffManager"));
            staffManager.CreateChildPermission(AppPermissions.Pages_StaffManager_Create,
                L("Pages_StaffManager_Create"));
            staffManager.CreateChildPermission(AppPermissions.Pages_StaffManager_Update,
                L("Pages_StaffManager_Update"));
            staffManager.CreateChildPermission(AppPermissions.Pages_StaffManager_Delete,
                L("Pages_StaffManager_Delete"));
            var agentManager =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_AgentManager, L("Pages_AgentManager"));
            agentManager.CreateChildPermission(AppPermissions.Pages_AgentManager_Create,
                L("Pages_AgentManager_Create"));

            var postManagement =
                pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_PostManagement, L("Pages_PostManagement"));
            postManagement.CreateChildPermission(AppPermissions.Pages_PostManagement_Create,
                L("Pages_PostManagement_Create"));
            postManagement.CreateChildPermission(AppPermissions.Pages_PostManagement_Edit,
                L("Pages_PostManagement_Edit"));

            var subAgentManagement = pagesFrontEnd.CreateChildPermission(AppPermissions.Pages_SubAgentManagement,
                L("Pages_SubAgentManagement"));
            subAgentManagement.CreateChildPermission(AppPermissions.Pages_SubAgentManagement_Create,
                L("Pages_SubAgentManagement_Create"));
            subAgentManagement.CreateChildPermission(AppPermissions.Pages_SubAgentManagement_Edit,
                L("Pages_SubAgentManagement_Edit"));
            subAgentManagement.CreateChildPermission(AppPermissions.Pages_SubAgentManagement_Lock,
                L("Pages_SubAgentManagement_Lock"));
            subAgentManagement.CreateChildPermission(AppPermissions.Pages_SubAgentManagement_Unlock,
                L("Pages_SubAgentManagement_Unlock"));

            var cards = pagesBackend.CreateChildPermission(AppPermissions.Pages_Cards, L("Cards"));
            cards.CreateChildPermission(AppPermissions.Pages_Cards_Create, L("CreateNewCard"));
            cards.CreateChildPermission(AppPermissions.Pages_Cards_Edit, L("EditCard"));
            cards.CreateChildPermission(AppPermissions.Pages_Cards_Delete, L("DeleteCard"));


            var cardStocks = pagesBackend.CreateChildPermission(AppPermissions.Pages_CardStocks, L("CardStocks"));
            cardStocks.CreateChildPermission(AppPermissions.Pages_CardStocks_Create, L("CreateNewCardStock"));
            cardStocks.CreateChildPermission(AppPermissions.Pages_CardStocks_Edit, L("EditCardStock"));
            cardStocks.CreateChildPermission(AppPermissions.Pages_CardStocks_Transfer, L("TransferCardStock"));
            cardStocks.CreateChildPermission(AppPermissions.Pages_CardStocks_Delete, L("DeleteCardStock"));
            cardStocks.CreateChildPermission(AppPermissions.Pages_CardStocks_EditQuantity, L("EditQuantityStock"));


            var stocksAirtimes = pages.CreateChildPermission(AppPermissions.Pages_StocksAirtimes, L("StocksAirtimes"));
            stocksAirtimes.CreateChildPermission(AppPermissions.Pages_StocksAirtimes_Create,
                L("CreateNewStocksAirtime"));
            stocksAirtimes.CreateChildPermission(AppPermissions.Pages_StocksAirtimes_Edit, L("EditStocksAirtime"));
            stocksAirtimes.CreateChildPermission(AppPermissions.Pages_StocksAirtimes_Delete, L("DeleteStocksAirtime"));
            stocksAirtimes.CreateChildPermission(AppPermissions.Pages_StocksAirtimes_Deposit,
                L("DepositStocksAirtime"));


            var batchAirtimes = pages.CreateChildPermission(AppPermissions.Pages_BatchAirtimes, L("BatchAirtimes"));
            batchAirtimes.CreateChildPermission(AppPermissions.Pages_BatchAirtimes_Create, L("CreateNewBatchAirtime"));
            batchAirtimes.CreateChildPermission(AppPermissions.Pages_BatchAirtimes_Edit, L("EditBatchAirtime"));
            batchAirtimes.CreateChildPermission(AppPermissions.Pages_BatchAirtimes_Approval, L("ApprovalBatchAirtime"));
            batchAirtimes.CreateChildPermission(AppPermissions.Pages_BatchAirtimes_Delete, L("DeleteBatchAirtime"));


            var cardBatchs = pagesBackend.CreateChildPermission(AppPermissions.Pages_CardBatchs, L("CardBatchs"));
            cardBatchs.CreateChildPermission(AppPermissions.Pages_CardBatchs_Create, L("CreateNewCardBatch"));
            cardBatchs.CreateChildPermission(AppPermissions.Pages_CardBatchs_Edit, L("EditCardBatch"));
            cardBatchs.CreateChildPermission(AppPermissions.Pages_CardBatchs_Delete, L("DeleteCardBatch"));

            var topupMessageResponse = pagesBackend.CreateChildPermission(AppPermissions.Pages_TopupGateResponseMessage,
                L("TopupGateResponseMessageManagerment"));

            topupMessageResponse.CreateChildPermission(AppPermissions.Pages_TopupGateResponseMessage_Create,
                L("CreateTopupGateResponseMessage"));
            topupMessageResponse.CreateChildPermission(AppPermissions.Pages_TopupGateResponseMessage_Edit,
                L("EditTopupGateResponseMessage"));
            topupMessageResponse.CreateChildPermission(AppPermissions.Pages_TopupGateResponseMessage_Delete,
                L("DeleteTopupGateResponseMessage"));


            var discounts = pagesBackend.CreateChildPermission(AppPermissions.Pages_Discounts, L("Discounts"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Create, L("CreateNewDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Edit, L("EditDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Delete, L("DeleteDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Approval, L("ApprovalDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Cancel, L("CancelDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Stop, L("StopDiscount"));


            var deposits = pagesBackend.CreateChildPermission(AppPermissions.Pages_Deposits, L("Deposits"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_Create, L("CreateNewDeposit"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_Edit, L("EditDeposit"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_Approval, L("ApprovalDeposit"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_Cancel, L("CancelDeposit"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_Delete, L("DeleteDeposit"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_DebtSale, L("CreateNewDebtSale"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_AccountingEntry,
                L("CreateNewAccountingEntry"));
            deposits.CreateChildPermission(AppPermissions.Pages_Deposits_Cash, L("CreateNewDepositCash"));


            var banks = pagesBackend.CreateChildPermission(AppPermissions.Pages_Banks, L("Banks"));
            banks.CreateChildPermission(AppPermissions.Pages_Banks_Create, L("CreateNewBank"));
            banks.CreateChildPermission(AppPermissions.Pages_Banks_Edit, L("EditBank"));
            banks.CreateChildPermission(AppPermissions.Pages_Banks_Delete, L("DeleteBank"));


            var providers = pagesBackend.CreateChildPermission(AppPermissions.Pages_Providers, L("Providers"));
            providers.CreateChildPermission(AppPermissions.Pages_Providers_Create, L("CreateNewProvider"));
            providers.CreateChildPermission(AppPermissions.Pages_Providers_Edit, L("EditProvider"));
            providers.CreateChildPermission(AppPermissions.Pages_Providers_LockUnLock, L("LockUnLock"));
            providers.CreateChildPermission(AppPermissions.Pages_Providers_Delete, L("DeleteProvider"));


            var products = pagesBackend.CreateChildPermission(AppPermissions.Pages_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Delete, L("DeleteProduct"));


            var categories = pagesBackend.CreateChildPermission(AppPermissions.Pages_Categories, L("Categories"));
            categories.CreateChildPermission(AppPermissions.Pages_Categories_Create, L("CreateNewCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Categories_Edit, L("EditCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Categories_Delete, L("DeleteCategory"));


            var services = pagesBackend.CreateChildPermission(AppPermissions.Pages_Services, L("Services"));
            services.CreateChildPermission(AppPermissions.Pages_Services_Create, L("CreateNewService"));
            services.CreateChildPermission(AppPermissions.Pages_Services_Edit, L("EditService"));
            services.CreateChildPermission(AppPermissions.Pages_Services_Delete, L("DeleteService"));

            var serviceConfigurations = pagesBackend.CreateChildPermission(AppPermissions.Pages_ServiceConfigurations,
                L("ServiceConfigurations"));
            serviceConfigurations.CreateChildPermission(AppPermissions.Pages_ServiceConfigurations_Create,
                L("CreateNewServiceConfiguration"));
            serviceConfigurations.CreateChildPermission(AppPermissions.Pages_ServiceConfigurations_Edit,
                L("EditServiceConfiguration"));
            serviceConfigurations.CreateChildPermission(AppPermissions.Pages_ServiceConfigurations_Delete,
                L("DeleteServiceConfiguration"));

            var systemAccountTransfers = pagesBackend.CreateChildPermission(AppPermissions.Pages_SystemAccountTransfers,
                L("SystemAccountTransfers"));
            systemAccountTransfers.CreateChildPermission(AppPermissions.Pages_SystemAccountTransfers_Create,
                L("CreateNewSystemAccountTransfer"));
            systemAccountTransfers.CreateChildPermission(AppPermissions.Pages_SystemAccountTransfers_Edit,
                L("EditSystemAccountTransfer"));
            systemAccountTransfers.CreateChildPermission(AppPermissions.Pages_SystemAccountTransfers_Approval,
                L("ApprovalSystemAccountTransfer"));
            systemAccountTransfers.CreateChildPermission(AppPermissions.Pages_SystemAccountTransfers_Cancel,
                L("CancelSystemAccountTransfer"));
            systemAccountTransfers.CreateChildPermission(AppPermissions.Pages_SystemAccountTransfers_Delete,
                L("DeleteSystemAccountTransfer"));


            var payBatchBills =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_PayBatchBills, L("PayBatchBills"));
            payBatchBills.CreateChildPermission(AppPermissions.Pages_PayBatchBills_Create, L("CreateNewPayBatchBill"));
            payBatchBills.CreateChildPermission(AppPermissions.Pages_PayBatchBills_Approval, L("ApprovalPayBatchBill"));
            payBatchBills.CreateChildPermission(AppPermissions.Pages_PayBatchBills_Cancel, L("CancelPayBatchBill"));
            payBatchBills.CreateChildPermission(AppPermissions.Pages_PayBatchBills_Delete, L("DeletePayBatchBill"));


            var accountBlockBalances = pagesBackend.CreateChildPermission(AppPermissions.Pages_AccountBlockBalances,
                L("AccountBlockBalances"));
            accountBlockBalances.CreateChildPermission(AppPermissions.Pages_AccountBlockBalances_Create,
                L("CreateNewAccountBlockBalance"));
            accountBlockBalances.CreateChildPermission(AppPermissions.Pages_AccountBlockBalances_Edit,
                L("EditAccountBlockBalance"));
            accountBlockBalances.CreateChildPermission(AppPermissions.Pages_AccountBlockBalances_Delete,
                L("DeleteAccountBlockBalance"));


            var limitProducts =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_LimitProducts, L("LimitProducts"));
            limitProducts.CreateChildPermission(AppPermissions.Pages_LimitProducts_Create, L("CreateNewLimitProduct"));
            limitProducts.CreateChildPermission(AppPermissions.Pages_LimitProducts_Edit, L("EditLimitProduct"));
            limitProducts.CreateChildPermission(AppPermissions.Pages_LimitProducts_Approval, L("ApprovalLimitProduct"));
            limitProducts.CreateChildPermission(AppPermissions.Pages_LimitProducts_Cancel, L("CancelLimitProduct"));
            limitProducts.CreateChildPermission(AppPermissions.Pages_LimitProducts_Delete, L("DeleteLimitProduct"));


            var payBacks = pagesBackend.CreateChildPermission(AppPermissions.Pages_PayBacks, L("PayBacks"));
            payBacks.CreateChildPermission(AppPermissions.Pages_PayBacks_Create, L("CreateNew"));
            payBacks.CreateChildPermission(AppPermissions.Pages_PayBacks_Edit, L("PayBacks_Edit"));
            payBacks.CreateChildPermission(AppPermissions.Pages_PayBacks_Approval, L("Approval"));
            payBacks.CreateChildPermission(AppPermissions.Pages_PayBacks_Cancel, L("Cancel"));
            payBacks.CreateChildPermission(AppPermissions.Pages_PayBacks_Delete, L("PayBacks_Delete"));

            var fees = pagesBackend.CreateChildPermission(AppPermissions.Pages_Fees, L("Fees"));
            fees.CreateChildPermission(AppPermissions.Pages_Fees_Create, L("CreateNewFee"));
            fees.CreateChildPermission(AppPermissions.Pages_Fees_Edit, L("Fees_Edit"));
            fees.CreateChildPermission(AppPermissions.Pages_Fees_Approval, L("Approval"));
            fees.CreateChildPermission(AppPermissions.Pages_Fees_Cancel, L("Cancel"));
            fees.CreateChildPermission(AppPermissions.Pages_Fees_Delete, L("Fees_Delete"));
            fees.CreateChildPermission(AppPermissions.Pages_Fees_Stop, L("Fees_Stop"));

            var agentsManage =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_AgentsManage, L("AgentsMangerment"));
            agentsManage.CreateChildPermission(AppPermissions.Pages_AgentsManage_Create, L("Create"));
            agentsManage.CreateChildPermission(AppPermissions.Pages_AgentsManage_Edit, L("Edit"));
            agentsManage.CreateChildPermission(AppPermissions.Pages_AgentsManage_Lock, L("Lock"));
            agentsManage.CreateChildPermission(AppPermissions.Pages_AgentsManage_Unlock, L("Unlock"));
            agentsManage.CreateChildPermission(AppPermissions.Pages_AgentsManage_Assign, L("AssignSale"));
            agentsManage.CreateChildPermission(AppPermissions.Pages_AgentsManage_ConvertPhone, L("ConvertPhone"));


            var agentsSupper =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_AgentsSupper, L("AgentsSupperTotal"));
            agentsSupper.CreateChildPermission(AppPermissions.Pages_AgentsSupper_Create, L("New"));
            agentsSupper.CreateChildPermission(AppPermissions.Pages_AgentsSupper_Edit, L("Edit"));
            agentsSupper.CreateChildPermission(AppPermissions.Pages_AgentsSupper_SendMailTech, L("ReSendMail"));

            var saleClearDebts =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_SaleClearDebts, L("SaleClearDebts"));
            saleClearDebts.CreateChildPermission(AppPermissions.Pages_SaleClearDebts_Create,
                L("CreateNewSaleClearDebt"));
            saleClearDebts.CreateChildPermission(AppPermissions.Pages_SaleClearDebts_Edit, L("EditSaleClearDebt"));
            saleClearDebts.CreateChildPermission(AppPermissions.Pages_SaleClearDebts_Delete, L("DeleteSaleClearDebt"));
            saleClearDebts.CreateChildPermission(AppPermissions.Pages_SaleClearDebts_Approval,
                L("ApprovalSaleClearDebt"));
            saleClearDebts.CreateChildPermission(AppPermissions.Pages_SaleClearDebts_Cancel, L("CancelSaleClearDebt"));


            var saleLimitDebts =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_SaleLimitDebts, L("SaleLimitDebts"));
            saleLimitDebts.CreateChildPermission(AppPermissions.Pages_SaleLimitDebts_Create,
                L("CreateNewSaleLimitDebt"));
            saleLimitDebts.CreateChildPermission(AppPermissions.Pages_SaleLimitDebts_Edit, L("EditSaleLimitDebt"));
            saleLimitDebts.CreateChildPermission(AppPermissions.Pages_SaleLimitDebts_Delete, L("DeleteSaleLimitDebt"));


            var saleMans = pagesBackend.CreateChildPermission(AppPermissions.Pages_SaleMans, L("SaleMans"));
            saleMans.CreateChildPermission(AppPermissions.Pages_SaleMans_Create, L("CreateNewSaleMan"));
            saleMans.CreateChildPermission(AppPermissions.Pages_SaleMans_Edit, L("EditSaleMan"));
            saleMans.CreateChildPermission(AppPermissions.Pages_SaleMans_Delete, L("DeleteSaleMan"));


            var vendors = pagesBackend.CreateChildPermission(AppPermissions.Pages_Vendors, L("Vendors"));
            vendors.CreateChildPermission(AppPermissions.Pages_Vendors_Create, L("CreateNewVendor"));
            vendors.CreateChildPermission(AppPermissions.Pages_Vendors_Edit, L("EditVendor"));
            vendors.CreateChildPermission(AppPermissions.Pages_Vendors_Delete, L("DeleteVendor"));


            var wards = pagesBackend.CreateChildPermission(AppPermissions.Pages_Wards, L("Wards"));
            wards.CreateChildPermission(AppPermissions.Pages_Wards_Create, L("CreateNewWard"));
            wards.CreateChildPermission(AppPermissions.Pages_Wards_Edit, L("EditWard"));
            wards.CreateChildPermission(AppPermissions.Pages_Wards_Delete, L("DeleteWard"));


            var districts = pagesBackend.CreateChildPermission(AppPermissions.Pages_Districts, L("Districts"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Create, L("CreateNewDistrict"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Edit, L("EditDistrict"));
            districts.CreateChildPermission(AppPermissions.Pages_Districts_Delete, L("DeleteDistrict"));


            var cities = pagesBackend.CreateChildPermission(AppPermissions.Pages_Cities, L("Cities"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Create, L("CreateNewCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Edit, L("EditCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Delete, L("DeleteCity"));


            var countries = pagesBackend.CreateChildPermission(AppPermissions.Pages_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Delete, L("DeleteCountry"));

            var reports = pagesBackend.CreateChildPermission(AppPermissions.Pages_Reports, L("Reports"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportDetailBalanceAccount,
                L("ReportDetailBalanceAccount"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportSumBalanceAccount,
                L("ReportSumBalanceAccount"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportSumTotalBalanceAccount,
                L("ReportSumTotalBalanceAccount"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_TopupTransaction, L("ReportTopupTransaction"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_CardStockHistories,
                L("ReportCardStockHistories"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_CardStockImExPort, L("ReportCardStockImExPort"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_CardStockImExProvider,
                L("ReportCardStockImExProvider"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_CardStockInventory,
                L("ReportCardStockInventory"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportAgentBalance, L("ReportAgentBalance"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportDebtDetail, L("ReportDebtDetail"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportTotalDebt, L("ReportTotalDebt"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportTransferDetail, L("ReportTransferDetail"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportServiceDetail, L("ReportServiceDetail"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportServiceTotal, L("ReportServiceTotal"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportServiceProvider,
                L("ReportServiceProvider"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportRefundDetail, L("ReportRefundDetail"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportRevenueAgent, L("ReportRevenueAgent"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportRevenueCity, L("ReportRevenueCity"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportTotalSaleAgent, L("ReportTotalSaleAgent"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportRevenueActive, L("ReportRevenueActive"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportComparePartner, L("ReportComparePartner"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportCommissionDetail,
                L("ReportCommissionDetail"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportCommissionTotal,
                L("ReportCommissionTotal"));
            reports.CreateChildPermission(AppPermissions.Pages_Report_ReportTopupRequestLogs,
                L("ReportTopupRequestLogs"));


            var topupRequests =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_TransactionManagements, L("TopupRequests"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_Create,
                L("CreateNewTopupRequest"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_Edit,
                L("EditTopupRequest"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_Delete,
                L("DeleteTopupRequest"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_Cancel,
                L("CancelTopupRequest"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_Refund,
                L("RefundTopupRequest"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_PinCode,
                L("ViewPinCodeTopupRequest"));
            topupRequests.CreateChildPermission(AppPermissions.Pages_TransactionManagements_StatusFile,
                L("SysnStatusFileTopupRequest"));

            pagesBackend.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));


            var notificationSchedules = pagesBackend.CreateChildPermission(AppPermissions.Pages_NotificationSchedules,
                L("NotificationSchedules"));
            notificationSchedules.CreateChildPermission(AppPermissions.Pages_NotificationSchedules_Create,
                L("CreateNewNotificationSchedule"));
            notificationSchedules.CreateChildPermission(AppPermissions.Pages_NotificationSchedules_Edit,
                L("EditNotificationSchedule"));
            notificationSchedules.CreateChildPermission(AppPermissions.Pages_NotificationSchedules_Delete,
                L("DeleteNotificationSchedule"));
            notificationSchedules.CreateChildPermission(AppPermissions.Pages_NotificationSchedules_Approval,
                L("Approval"));
            notificationSchedules.CreateChildPermission(AppPermissions.Pages_NotificationSchedules_Cancel, L("Cancel"));
            notificationSchedules.CreateChildPermission(AppPermissions.Pages_NotificationSchedules_Send, L("Send"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_UserAccounts, L("UserAccounts"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions,
                L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages =
                administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create,
                L("CreatingNewLanguage"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete,
                L("DeletingLanguages"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts,
                L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits =
                administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits,
                    L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(
                AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree,
                L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers,
                L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles,
                L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization,
                L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription,
                L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create,
                L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit,
                L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity,
                L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail,
                L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts,
                L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook,
                L("ResendingWebhook"));

            var dynamicProperties =
                administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties,
                    L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create,
                L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit,
                L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete,
                L("DeletingDynamicProperties"));

            var dynamicPropertyValues =
                dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue,
                    L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create,
                L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit,
                L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete,
                L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityProperties_Create,
                L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityProperties_Delete,
                L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create,
                L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit,
                L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(
                AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete,
                L("DeletingDynamicEntityPropertyValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"),
                multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"),
                multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement,
                L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"),
                multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"),
                multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"),
                multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"),
                multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition,
                L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"),
                multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"),
                multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"),
                multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"),
                multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"),
                multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"),
                multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"),
                multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard,
                L("HangfireDashboard"),
                multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"),
                multiTenancySides: MultiTenancySides.Host);

            var partnerServiceConfigurations =
                pagesBackend.CreateChildPermission(AppPermissions.Pages_PartnerServiceConfigurations,
                    L("PartnerServiceConfigurations"));
            partnerServiceConfigurations.CreateChildPermission(AppPermissions.Pages_PartnerServiceConfigurations_Create,
                L("CreateNewPartnerServiceConfiguration"));
            partnerServiceConfigurations.CreateChildPermission(AppPermissions.Pages_PartnerServiceConfigurations_Edit,
                L("EditPartnerServiceConfiguration"));
            partnerServiceConfigurations.CreateChildPermission(AppPermissions.Pages_PartnerServiceConfigurations_Delete,
                L("DeletePartnerServiceConfiguration"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TopupConsts.LocalizationSourceName);
        }
    }
}