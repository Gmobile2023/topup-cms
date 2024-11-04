using System;
using HLS.Topup.Core;
using HLS.Topup.Core.Dependency;
using HLS.Topup.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HLS.Topup.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}