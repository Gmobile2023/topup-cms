using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using HLS.Topup.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ServiceStack;

namespace HLS.Topup.Common
{
    public class TelcoHepper : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public TelcoHepper(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public string GetTelco(string phoneNumber)
        {
            try
            {
                var config = _appConfiguration.GetSection("CardConfig:TelcoConfigs").GetChildren();
                var pex = phoneNumber.Trim().Substring(0, 3);
                var telco = string.Empty;
                foreach (var item in config)
                {
                    var values = item["Values"].Split(',').ToList();
                    if (values.Contains(pex))
                    {
                        telco = item["Key"];
                        break;
                    }
                }

                return telco;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string GetVenderCode(string name)
        {
            try
            {
                var config = _appConfiguration.GetSection("CardConfig:Vendors").GetChildren();
                foreach (var item in config)
                {
                    if (item["Name"] == name)
                    {
                        return item["Code"];
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetBatchLotValues(string batchType)
        {
            var values = _appConfiguration["CardConfig:BatchLotValues"].Split(',');
            return values.Where(c => c.StartsWith(batchType)).FirstOrDefault();
        }

        public bool CheckCardValue(string value)
        {
            var values = _appConfiguration["CardConfig:CardValues"].Split(',').ToList();
            return values.Where(c => c == value).Count() > 0;
        }

        public string GetEvnProductCode(string evnCode)
        {
            var configs = _appConfiguration.GetSection("EvnCodes").Value.FromJson<List<EvnCodes>>();
            var checkCode = evnCode.ToUpper().Substring(0,4);
            if (configs.Exists(p => p.Value == checkCode))
                return configs.FirstOrDefault(x => x.Value == checkCode)?.Code;
            checkCode = evnCode.ToUpper().Substring(0,2);
            return configs.Exists(p => p.Value == checkCode)
                ? configs.FirstOrDefault(x => x.Value == checkCode)?.Code
                : "EVN_BILL";
        }

        public class EvnCodes
        {
            public string Code { get; set; }
            public string Value { get; set; }
        }
    }
}
