using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDV.MultiTenancy.HostDashboard.Dto;

namespace TDV.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}