using System;
using System.Collections.Generic;
using HLS.Topup.Dtos.Cms;

namespace HLS.Topup.Cms.Dots
{
    public class AdvertiseItemsDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
        public AcfContentDto Contents { get; set; }
        public List<ImageSize> ImageSizes { get; set; }
    }
}
