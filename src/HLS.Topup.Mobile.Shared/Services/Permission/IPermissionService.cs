namespace HLS.Topup.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}