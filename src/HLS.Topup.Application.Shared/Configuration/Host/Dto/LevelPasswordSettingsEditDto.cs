namespace HLS.Topup.Configuration.Host.Dto
{
    public class LevelPasswordSettingsEditDto
    {
        public bool IsEnabled { get; set; }
        public int MaxFailedLelve2Password { get; set; }
        public int DefaultLockTransSeconds { get; set; }
    }
}
