using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Repositories
{
    public interface IAnalyticsRepository
    {
        Task<AnalyticsDto> GetAnalyticsAsync(AnalyticsFilters filters, CancellationToken ct);
    }
}
