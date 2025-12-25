using Application.DTOs;
using Application.Repositories;
using Application.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class AnalyticsService(IAnalyticsRepository analyticsRepo) : IAnalyticsService
    {
        public async Task<AnalyticsDto> GetAnalyticsDataAsync(AnalyticsFilters filters, CancellationToken ct)
        {
            return await analyticsRepo.GetAnalyticsAsync(filters, ct);
        }

    }
}
