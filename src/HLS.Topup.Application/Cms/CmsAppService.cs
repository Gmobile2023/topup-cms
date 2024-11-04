using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Caching;
using Abp.UI;
using HLS.Topup.Cms.Dots;
using HLS.Topup.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ServiceStack;

namespace HLS.Topup.Cms
{
    [AbpAuthorize]
    public class CmsAppService : TopupAppServiceBase, ICmsAppService
    {
        private readonly ICmsManager _cmsManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ICacheManager _cacheManager;

        public CmsAppService(ICmsManager cmsManager, ICacheManager cacheManager, IWebHostEnvironment env)
        {
            _cmsManager = cmsManager;
            _cacheManager = cacheManager;
            _appConfiguration = env.GetAppConfiguration();
        }

        public virtual async Task<List<AdvertiseItemsDto>> GetAdvertiseItems(string position)
        {
            try
            {
                // var item = await _cacheManager.GetCache("Cms").GetAsync(
                //     $"{AbpSession.TenantId}:GetAdvertiseImages:{position}:{AbpSession.TenantId}", async () =>
                //     {
                //         var dto = await _cmsManager.GetAdvertiseAcfByPage(int.Parse(_appConfiguration["CmsConfig:PageAdvertiseAcfMobileId"]));
                //         var data = dto?.Acf.Items.FindAll(x => x.Position == position);
                //         if (dto == null) return new List<AdvertiseItemsDto>();
                //         {
                //             var lst = data?.Select(x => new AdvertiseItemsDto
                //             {
                //                 Image = x.Image?.Url,
                //                 Position = x.Position,
                //                 Title = x.Title,
                //                 Url = x.Url?.Url,
                //                 Contents = x.Contents,
                //                 ImageSizes = x.Image?.Sizes
                //             }).ToList();
                //             var av= GetConvertList(lst);
                //             return av;
                //         }
                //     });
                // return GetConvertList(item.ToList());
                var dto = await _cmsManager.GetAdvertiseAcfByPage(
                    int.Parse(_appConfiguration["CmsConfig:PageAdvertiseAcfMobileId"]));
                var data = dto?.Acf.Items.FindAll(x => x.Position == position);
                if (dto == null) return new List<AdvertiseItemsDto>();
                {
                    var lst = data?.Select(x => new AdvertiseItemsDto
                    {
                        Image = x.Image?.Url,
                        Position = x.Position,
                        Title = x.Title,
                        Url = x.Url?.Url,
                        Contents = x.Contents,
                        ImageSizes = x.Image?.Sizes
                    }).ToList();
                    var av = GetConvertList(lst);
                    return av;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException("Lỗi");
            }
        }

        public async Task<List<FaqsDto>> GetFaqsItems()
        {
            try
            {
                var dto = await _cmsManager.GetFaqsAcfByPage(67);
                var data = dto?.Acf.Question.FindAll(x => x.QuestionType != null);
                var lst = data?.ConvertTo<List<FaqsDto>>().ToList();

                return lst;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException("Lỗi");
            }
        }

        private List<AdvertiseItemsDto> GetConvertList(IReadOnlyCollection<AdvertiseItemsDto> lst)
        {
            if (!lst.Any()) return null;
            {
                var newList = new List<AdvertiseItemsDto>();
                foreach (var ad in lst)
                {
                    if (ad.CreateDate == DateTime.MinValue)
                    {
                        ad.CreateDate = DateTime.Now;
                    }

                    if (ad.Position == "PromotionPage")
                    {
                        if (ad.Contents != null && ad.Contents.FromDate != null && ad.Contents.ToDate != null &&
                            ad.Contents.FromDate <= DateTime.Now && ad.Contents.ToDate >= DateTime.Now)
                        {
                            var checkDate = (DateTime.Now - ad.Contents.FromDate.Value).Days;
                            ad.Contents.TimeExpire = $"Còn {checkDate} ngày nữa kết thúc";
                            newList.Add(ad);
                        }
                    }
                    else
                    {
                        newList.Add(ad);
                    }
                }

                return newList.OrderByDescending(x => x.CreateDate).ToList();
            }
        }
    }
}
