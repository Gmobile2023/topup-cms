using System.Collections.Generic;

namespace HLS.Topup.Dtos.Sale
{
    public class AddressSaleDto
    {
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
    }

    public class AddressSaleItemDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string WardName { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

    // public class DistrictSaleSeleced : AddressSaleItemDto
    // {
    //     public int CitiId { get; set; }
    //     public string CityName { get; set; }
    // }
    //
    // public class WardSaleSeleced : AddressSaleItemDto
    // {
    //     public int DistrictId { get; set; }
    //     public string DistrictName { get; set; }
    // }

    public class AddressSaleSelected
    {
        public List<AddressSaleItemDto> Districts { get; set; }
        public List<AddressSaleItemDto> Wards { get; set; }
        public List<AddressSaleItemDto> Cities { get; set; }
    }
}
