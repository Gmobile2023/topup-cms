namespace HLS.Topup.Net.Emailing
{
    public interface IEmailTemplateProvider
    {
        string GetDefaultTemplate(int? tenantId);
        string GetTemplateByName(int? tenantId, string temName);
    }
}
