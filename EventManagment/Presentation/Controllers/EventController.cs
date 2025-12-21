using Domain.Models;
using Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.ResponseModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/events")]
    public sealed class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService filtersService)
        {
            _eventService = filtersService;
        }


        [HttpGet("filters-meta")]
        [Authorize]
        public async Task<ActionResult<EventFiltersMetaResponseDto>> GetMeta(
            CancellationToken cancellationToken)
        {
            // user context comes from JWT
            var userId = 1;

            var meta = await _eventService.GetFiltersMetaAsync(userId, cancellationToken);

            return Ok(meta.Adapt<EventFiltersMetaResponseDto>());
        }
    }
}
