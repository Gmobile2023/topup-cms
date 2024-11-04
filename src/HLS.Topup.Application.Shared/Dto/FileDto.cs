using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using HLS.Topup.Common;

namespace HLS.Topup.Dto
{
    public class FileDto
    {
        [Required] public string FileName { get; set; }

        public string FileType { get; set; }

        [Required] public string FileToken { get; set; }

        public FileDto()
        {
        }

        public string FilePath { get; set; }

        public FileDto(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
            FileToken = Guid.NewGuid().ToString("N");
        }
    }

    public class UploadFileResult : FileDto
    {
        public DetectImageInfo DetectImageInfo { get; set; }
    }

    public class DetectImageInfo
    {
        [DataMember(Name = "address")] public string Address { get; set; }

        [DataMember(Name = "birthday")] public string Birthday { get; set; }

        [DataMember(Name = "country")] public string Country { get; set; }

        [DataMember(Name = "district")] public string District { get; set; }

        [DataMember(Name = "district_code")] public string DistrictCode { get; set; }

        [DataMember(Name = "document")] public string Document { get; set; }

        [DataMember(Name = "ethnicity")] public string Ethnicity { get; set; }

        [DataMember(Name = "expiry")] public string Expiry { get; set; }

        [DataMember(Name = "home_town")] public string HomeTown { get; set; }

        [DataMember(Name = "id")] public string Id { get; set; }

        [DataMember(Name = "id_check")] public string IdCheck { get; set; }

        [DataMember(Name = "id_type")] public long IdType { get; set; }

        [DataMember(Name = "idconf")] public string Idconf { get; set; }

        [DataMember(Name = "issue_by")] public string IssueBy { get; set; }

        [DataMember(Name = "issue_date")] public string IssueDate { get; set; }

        [DataMember(Name = "name")] public string Name { get; set; }

        [DataMember(Name = "national")] public string National { get; set; }

        [DataMember(Name = "precinct")] public string Precinct { get; set; }

        [DataMember(Name = "precinct_code")] public string PrecinctCode { get; set; }

        [DataMember(Name = "province")] public string Province { get; set; }

        [DataMember(Name = "province_code")] public string ProvinceCode { get; set; }

        [DataMember(Name = "religion")] public string Religion { get; set; }

        [DataMember(Name = "result_code")] public long ResultCode { get; set; }

        [DataMember(Name = "result_message")] public string ResultMessage { get; set; }

        [DataMember(Name = "sex")] public string Sex { get; set; }
        public CommonConst.IdType DocumentType { get; set; }
    }
}
