using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstractions
{
    public interface IAnalyticsService
    {
        Task<AnalyticsDto> GetAnalyticsDataAsync(AnalyticsFilters filters, CancellationToken ct);

    }
}
