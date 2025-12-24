using Application.DTOs;
using Application.Services.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.RequestModels;
using Presentation.DTOs.ResponseModels;
using System.Security.Claims;

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
       // [Authorize]
        public async Task<ActionResult<EventFiltersMetaResponseDto>> GetMeta(
            CancellationToken cancellationToken)
        {
            // user context comes from JWT
         //   var currentUser = this.User.FindFirstValue("Sid");
            var userId = 1;

            var meta = await _eventService.GetFiltersMetaAsync(userId, cancellationToken);

            return Ok(meta.Adapt<EventFiltersMetaResponseDto>());
        }

        [HttpGet]
       // [Authorize]
        public async Task<ActionResult<EventsSearchResponse>> GetEvents(
            [FromQuery] EventsSearchRequestDto query,
            CancellationToken ct)
        {
            var userId = 4; // later from JWT

            var filters = query.Adapt<EventsSearchFilters>();
            var result = await _eventService.GetEventsAsync(userId, filters, ct);

            return Ok(result.Adapt<EventsSearchResponse>());
        }

        [HttpGet("categories")]
     //   [Authorize]
        public async Task<ActionResult<CategoriesResponse>> GetCategories(
            [FromQuery] bool withCounts = false,
            CancellationToken ct = default)
        {
            var userId = 1; // later from JWT

            var result = await _eventService.GetCategoriesAsync(userId, withCounts, ct);

            return Ok(result.Adapt<CategoriesResponse>());
        }

        [HttpGet("{eventId:int}")]
        [ProducesResponseType(typeof(EventDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventDetailsResponse>> GetById([FromRoute] int eventId, CancellationToken ct)
        {
            var userId = 1;

            var result = await _eventService.GetEventDetailsAsync(userId, eventId, ct);
            return result is null
                ? NotFound()
                : Ok(result.Adapt<EventDetailsResponse>());
        }


    }
}
