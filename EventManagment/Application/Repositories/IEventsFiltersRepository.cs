using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IEventsFiltersRepository
    {
        Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct);
    }
}
