using Domain.Models;
using Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.ResponseModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/events/filters")]
    public sealed class EventsFiltersController : ControllerBase
    {
        private readonly IEventsFiltersService _filtersService;

        public EventsFiltersController(IEventsFiltersService filtersService)
        {
            _filtersService = filtersService;
        }


        [HttpGet("meta")]
        [Authorize]
        public async Task<ActionResult<EventFiltersMetaResponseDto>> GetMeta(
            CancellationToken cancellationToken)
        {
            // user context comes from JWT
            var userId = 1;

            var meta = await _filtersService.GetFiltersMetaAsync(userId, cancellationToken);

            return Ok(meta.Adapt<EventFiltersMetaResponseDto>());
        }
    }
}
