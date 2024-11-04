using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Dto
{
    public class PagedAndFilteredInputDto : IPagedResultRequest
    {
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public string Filter { get; set; }

        public PagedAndFilteredInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }

    public class PagedResultDtoReport<T> : ListResultDto<T>, IPagedResult<T>, IListResult<T>, IHasTotalCount
    {
        public int TotalCount { get; set; }
        public T TotalData { get; set; }

        public string Warning { get; set; }
        public PagedResultDtoReport() { }
        public PagedResultDtoReport(int totalCount, T totalData, IReadOnlyList<T> items, string warning="")
            : base(items)
        {
            this.TotalCount = totalCount;
            this.TotalData = totalData;
            this.Warning = warning;
        }

    }
}
