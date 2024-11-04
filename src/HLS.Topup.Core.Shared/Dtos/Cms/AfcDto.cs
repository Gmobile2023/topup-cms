using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HLS.Topup.Dtos.Cms
{
    public class ImageSize
    {
        [DataMember(Name = "thumbnail")]
        public string Thumbnail { get; set; }
        [DataMember(Name = "medium")]
        public string Medium { get; set; }
        [DataMember(Name = "medium_large")]
        public string MediumLarge { get; set; }
        [DataMember(Name = "large")]
        public string Large { get; set; }
    }

    public class ImageAcf
    {
        [DataMember(Name = "id")] public int Id { get; set; }
        [DataMember(Name = "title")] public string Title { get; set; }
        [DataMember(Name = "filename")] public string FileName { get; set; }
        [DataMember(Name = "filesize")] public int FileSize { get; set; }
        [DataMember(Name = "url")] public string Url { get; set; }
        [DataMember(Name = "link")] public string Link { get; set; }
        [DataMember(Name = "alt")] public string Alt { get; set; }
        [DataMember(Name = "description")] public string Description { get; set; }
        [DataMember(Name = "cation")] public string Caption { get; set; }
        [DataMember(Name = "name")] public string Name { get; set; }
        [DataMember(Name = "status")] public string Status { get; set; }
        [DataMember(Name = "date")] public string Date { get; set; }
        [DataMember(Name = "type")] public string Type { get; set; }
        [DataMember(Name = "subtype")] public string Subtype { get; set; }
        [DataMember(Name = "icon")] public string Icon { get; set; }
        [DataMember(Name = "width")] public int Width { get; set; }
        [DataMember(Name = "height")] public int Height { get; set; }
        [DataMember(Name = "sizes")] public List<ImageSize> Sizes { get; set; }
    }

    public class UrlAcf
    {
        [DataMember(Name = "title")] public string Title { get; set; }
        [DataMember(Name = "url")] public string Url { get; set; }
        [DataMember(Name = "target")] public string Target { get; set; }
    }


    public class AdvertiseItem
    {
        [DataMember(Name = "title")] public string Title { get; set; }
        [DataMember(Name = "position")] public string Position { get; set; }
        [DataMember(Name = "image")] public ImageAcf Image { get; set; }
        [DataMember(Name = "url")] public UrlAcf Url { get; set; }
        [DataMember(Name = "createdate")] public DateTime? CreateDate { get; set; }
        [DataMember(Name = "contents")] public AcfContentDto Contents { get; set; }
    }

    public class AcfAdvertiseAppDto
    {
        [DataMember(Name = "acf")] public AcfAdvertiseField Acf { get; set; }
    }

    public class AcfAdvertiseField
    {
        [DataMember(Name = "items")] public List<AdvertiseItem> Items { get; set; }
    }

    public class AcfContentDto
    {
        [DataMember(Name = "fromdate")] public DateTime? FromDate { get; set; }
        [DataMember(Name = "todate")] public DateTime? ToDate { get; set; }
        [DataMember(Name = "content")] public string Content { get; set; }
        [DataMember(Name = "description")] public string Description { get; set; }
        [DataMember(Name = "locations")] public string Locations { get; set; }
        public string TimeExpire { get; set; }
    }
}
