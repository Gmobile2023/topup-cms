using System;
using HLS.Topup.Dtos.Common;
using ServiceStack;

namespace HLS.Topup.Common
{
    public static class CommonHelper
    {
        public static AppRequestInfoDto GetAppRequestInfo(string json)
        {
            try
            {
                return json.FromJson<AppRequestInfoDto>();
            }
            catch (Exception e)
            {
                return new AppRequestInfoDto();
            }
        }

        public static int AppGetBuildNumber(AppRequestInfoDto dto)
        {
            const int defaultBuild = 11;
            try
            {
                if (dto == null)
                    return defaultBuild;
                return string.IsNullOrEmpty(dto.AppCode) ? defaultBuild : int.Parse(dto.AppCode);
            }
            catch (Exception e)
            {
                return defaultBuild;
            }
        }

        public static bool IsAppOldVersion(string appRequestInfo, int checkBuildVersion)
        {
            try
            {
                if (string.IsNullOrEmpty(appRequestInfo)) return false;
                var info = GetAppRequestInfo(appRequestInfo);
                if (info == null) return false;
                var currentVersion = AppGetBuildNumber(info);
                return currentVersion < checkBuildVersion;
            }
            catch (Exception e)
            {
                return false;
            }
        }     
    }
}
