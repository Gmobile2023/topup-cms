using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HLS.Topup.MultiTenancy.HostDashboard.Dto;

namespace HLS.Topup.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}