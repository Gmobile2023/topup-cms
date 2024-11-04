using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Net.Mail;
using HLS.Topup.Chat;
using HLS.Topup.Editions;
using HLS.Topup.Localization;
using HLS.Topup.MultiTenancy;
using System.Net.Mail;
using System.Web;
using Abp.Runtime.Security;
using HLS.Topup.Configuration;
using HLS.Topup.Dtos.Stock;
using HLS.Topup.Net.Emailing;
using HLS.Topup.StockManagement.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using StringExtensions = ServiceStack.StringExtensions;

namespace HLS.Topup.Authorization.Users
{
    /// <summary>
    /// Used to send email to users.
    /// </summary>
    public class UserEmailer : TopupServiceBase, IUserEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;

        private readonly IConfigurationRoot _appConfiguration;

        //private readonly Logger _logger = LogManager.GetLogger("UserEmailer");
        private readonly ILogger<UserEmailer> _logger;

        // used for styling action links on email messages.
        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";

        private string _emailButtonColor = "#00bb77";

        public UserEmailer(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            EditionManager editionManager,
            UserManager userManager, ILogger<UserEmailer> logger,
            IWebHostEnvironment hostingEnvironment)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _logger = logger;
            _appConfiguration = hostingEnvironment.GetAppConfiguration();
        }

        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");
            }

            link = link.Replace("{userId}", user.Id.ToString());
            link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));

            if (user.TenantId.HasValue)
            {
                link = link.Replace("{tenantId}", user.TenantId.ToString());
            }

            link = EncryptQueryParameters(link);

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate =
                GetTitleAndSubTitle(user.TenantId, L("EmailActivation_Title"), L("EmailActivation_SubTitle"));
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailActivation_ClickTheLinkBelowToVerifyYourEmail") + "<br /><br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor +
                                   "\" href=\"" + link + "\">" + L("Verify") + "</a>");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" +
                                   L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate, mailMessage);
        }

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Reset link</param>
        public async Task SendPasswordResetLinkAsync(User user, string link = null)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("PasswordResetEmail_Title"),
                L("PasswordResetEmail_SubTitle"));
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");
            mailMessage.AppendLine("<b>" + L("ResetCode") + "</b>: " + user.PasswordResetCode + "<br />");

            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine(L("PasswordResetEmail_ClickTheLinkBelowToResetYourPassword") + "<br /><br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor +
                                       "\" href=\"" + link + "\">" + L("Reset") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" +
                                       L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            }

            await ReplaceBodyAndSend(user.EmailAddress, L("PasswordResetEmail_Subject"), emailTemplate, mailMessage);
        }

        public async Task TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName,
            ChatMessage chatMessage)
        {
            try
            {
                var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("NewChatMessageEmail_Title"),
                    L("NewChatMessageEmail_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Sender") + "</b>: " + senderTenancyName + "/" + senderUsername +
                                       "<br />");
                mailMessage.AppendLine("<b>" + L("Time") + "</b>: " +
                                       chatMessage.CreationTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") +
                                       " UTC<br />");
                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + chatMessage.Message + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(user.EmailAddress, L("NewChatMessageEmail_Subject"), emailTemplate,
                    mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task SendEmailPinCode(User user, string emailSending, string transCode,
            List<CardResponseDto> listCard)
        {
            try
            {
                _logger.LogInformation(
                    $"SendEmailPinCode request: {user.UserName}-{emailSending}-{transCode}-{StringExtensions.ToJson(listCard.Select(x => x.Serial))}");
                var emailTemplate =
                    GetTempleateMail(null, L("Email_BinCode_Title"), L("Email_BinCode_SubTitle"), "pincode");
                var mailMessage = new StringBuilder();
                var msgBody = L("Email_BinCode_Body");
                msgBody = msgBody.Replace("{FullName}", user.FullName);
                msgBody = msgBody.Replace("{TransCode}", transCode);
                var cardInfo = string.Empty;
                var stt = 0;
                foreach (var item in listCard)
                {
                    stt++;
                    cardInfo +=
                        $"<tr><td>{stt}</td><td>{item.CardCode}</td><td>{item.Serial}</td><td>{item.StockType}</td><td>{item.CardValue}</td><td>{item.ExpiredDate}</td></tr>";
                }

                mailMessage.AppendLine(msgBody);
                emailTemplate.Replace("{ListCard_Result}", cardInfo);
                await ReplaceBodyAndSend(emailSending, L("Email_BinCode_SubTitle"), emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error("SendEmailConfirmCreateWebsiteAsync error:" + exception);
            }
        }

        public async Task SendEmailCreateAgentApi(User user, string email, string password, string cliendId,
            string clientKey)
        {
            try
            {
                _logger.LogInformation($"SendEmailCreateAgentApi: {user.UserName}");
                var linkAuthen = _appConfiguration["App:ApiLinkAuthen"];
                var linkPayment = _appConfiguration["App:ApiLinkPayment"];
                var linkDoc = _appConfiguration["App:ApiLinkDocument"];
                var linkKey = _appConfiguration["App:ApiLinkKey"];
                var link = _appConfiguration["App:WebSiteRootAddress"];

                var emailTemplate = GetTempleateMail(null,
                    L("Email_CreateAccountApi_Title", user.FullName, _appConfiguration["App:Environment"]),
                    L("Email_CreateAccountApi_SubTitle", _appConfiguration["App:Environment"]), "default");
                var mailMessage = new StringBuilder();
                var msgBody = L("Email_CreateAccountApi_Body");
                msgBody = msgBody.Replace("{FullName}", user.FullName);
                //msgBody = msgBody.Replace("{AuthenUrl}", linkAuthen);
                msgBody = msgBody.Replace("{PaymentUrl}", linkPayment);
                msgBody = msgBody.Replace("{UserName}", user.UserName);
                msgBody = msgBody.Replace("{Password}", password);
                msgBody = msgBody.Replace("{UserNameStaff}", user.UserName + "_nv");
                msgBody = msgBody.Replace("{PasswordStaff}", password);
                msgBody = msgBody.Replace("{ClientID}", cliendId);
                msgBody = msgBody.Replace("{ClientKey}", clientKey);
                msgBody = msgBody.Replace("{PartnerCode}", user.AccountCode);
                msgBody = msgBody.Replace("{NT_KEY}", linkKey);
                msgBody = msgBody.Replace("{NT_DOC}", linkDoc);
                msgBody = msgBody.Replace("{NT_LINK}", link);
                mailMessage.AppendLine(msgBody);
                var mailCcc = _appConfiguration["App:EmailCCTech"].Split(";").ToList();
                var bcc = mailCcc.Select(item => new MailAddress(item)).ToList();

                await ReplaceBodyAndSend(email,
                    L("Email_CreateAccountApi_Title", user.FullName, _appConfiguration["App:Environment"]),
                    emailTemplate, mailMessage, null, bcc);
            }
            catch (Exception exception)
            {
                Logger.Error("SendEmailCreateAgentApi error:" + exception);
            }
        }

        public async Task TryToSendSubscriptionExpireEmail(int tenantId, DateTime utcNow)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(
                            LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"),
                            L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpire_Email_Body",
                            culture, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"),
                            emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionAssignedToAnotherEmail(int tenantId, DateTime utcNow,
            int expiringEditionId)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(
                            LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var expringEdition = await _editionManager.GetByIdAsync(expiringEditionId);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"),
                            L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " +
                                               L("SubscriptionAssignedToAnother_Email_Body", culture,
                                                   expringEdition.DisplayName, utcNow.ToString("yyyy-MM-dd") + " UTC") +
                                               "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"),
                            emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendFailedSubscriptionTerminationsEmail(List<string> failedTenancyNames, DateTime utcNow)
        {
            try
            {
                var hostAdmin = await _userManager.GetAdminAsync();
                if (hostAdmin == null || string.IsNullOrEmpty(hostAdmin.EmailAddress))
                {
                    return;
                }

                var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage,
                    hostAdmin.TenantId, hostAdmin.Id);
                var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                var emailTemplate = GetTitleAndSubTitle(null, L("FailedSubscriptionTerminations_Title"),
                    L("FailedSubscriptionTerminations_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("FailedSubscriptionTerminations_Email_Body",
                    culture, string.Join(",", failedTenancyNames),
                    utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(hostAdmin.EmailAddress, L("FailedSubscriptionTerminations_Email_Subject"),
                    emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionExpiringSoonEmail(int tenantId, DateTime dateToCheckRemainingDayCount)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var tenantAdminLanguage = _settingManager.GetSettingValueForUser(
                            LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(tenantAdminLanguage);

                        var emailTemplate = GetTitleAndSubTitle(null, L("SubscriptionExpiringSoon_Title"),
                            L("SubscriptionExpiringSoon_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " +
                                               L("SubscriptionExpiringSoon_Email_Body", culture,
                                                   dateToCheckRemainingDayCount.ToString("yyyy-MM-dd") + " UTC") +
                                               "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpiringSoon_Email_Subject"),
                            emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }

        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate,
            StringBuilder mailMessage, IReadOnlyCollection<MailAddress> cc = null,
            IReadOnlyCollection<MailAddress> bcc = null)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            var content = new MailMessage
            {
                To = {emailAddress},
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true,
            };
            if (cc != null)
                foreach (var item in cc)
                {
                    content.CC.Add(item);
                }

            if (bcc != null)
                foreach (var item in bcc)
                {
                    content.Bcc.Add(item);
                }

            await _emailSender.SendAsync(content);
        }

        /// <summary>
        /// Returns link with encrypted parameters
        /// </summary>
        /// <param name="link"></param>
        /// <param name="encrptedParameterName"></param>
        /// <returns></returns>
        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" +
                   HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        private StringBuilder GetTempleateMail(int? tenantId, string title, string subTitle, string teamName)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplateByName(tenantId, teamName));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);
            return emailTemplate;
        }

        public async Task SendEmailPasswordFile(string emailSending, string fileName, string password)
        {
            try
            {
                _logger.LogInformation($"SendEmailPasswordFile request: {emailSending}-{fileName}-{password}");

                var mailMessage = new StringBuilder();
                var msgBody = L("Email_PasswordZipFile_Body");
                msgBody = msgBody.Replace("{password}", password);
                msgBody = msgBody.Replace("{filename}", fileName);
                mailMessage.AppendLine(msgBody);

                var subject = L("Email_PasswordZipFile_Subject");
                subject = subject.Replace("{password}", password);
                subject = subject.Replace("{filename}", fileName);
                var emailTemplate = GetTempleateMail(null, L("Email_PasswordZipFile_Title"), "", "default");
                await ReplaceBodyAndSend(emailSending, subject, emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error("SendEmailPasswordFile error:" + exception);
            }
        }
    }
}
