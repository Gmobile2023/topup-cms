namespace HLS.Topup.Services
{
    public class ServiceConsts
    {

		public const int MinServiceCodeLength = 0;
		public const int MaxServiceCodeLength = 50;
						
		public const int MinServicesNameLength = 0;
		public const int MaxServicesNameLength = 255;
						
		public const int MinServiceConfigLength = 0;
		public const int MaxServiceConfigLength = 255;
						
		public const int MinDescriptionLength = 0;
		public const int MaxDescriptionLength = 255;
						
    }
    public enum ServiceStatus : byte
    {
	    Init = 0,
	    Active = 1,
	    Lock = 2
    }
}