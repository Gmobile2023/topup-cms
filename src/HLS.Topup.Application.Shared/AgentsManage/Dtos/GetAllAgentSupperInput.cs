using System;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;

namespace HLS.Topup.AgentsManage.Dtos
{
    public class GetAllAgentSupperInput : PagedAndSortedResultRequestDto
    {
        public string AccountAgent { get; set; }

        public int CrossCheckPeriod { get; set; }

        public int Status { get; set; }
    }
}
