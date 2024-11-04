using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.AgentsManage.Dtos
{
    public class AgentsSupperDto
    {
        public long UserId { get; set; }
        public string AccountCode { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public int CrossCheckPeriod { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
