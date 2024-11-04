using System.Collections.Generic;
using HLS.Topup.Web.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HLS.Topup.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly TopupAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}
