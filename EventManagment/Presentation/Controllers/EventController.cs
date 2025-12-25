using Application.DTOs;
using Application.Services.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<EventFiltersMetaResponseDto>> GetMeta(
            CancellationToken cancellationToken)
        {
         //   var currentUser = this.User.FindFirstValue("Sid");
            var userId = 1;

            var meta = await _eventService.GetFiltersMetaAsync(userId, cancellationToken);

            return Ok(meta.Adapt<EventFiltersMetaResponseDto>());
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CategoriesResponse>> GetCategories(
            [FromQuery] bool withCounts = false,
            CancellationToken ct = default)
        {
            var userId = 1; // later from JWT

            var result = await _eventService.GetCategoriesAsync(userId, withCounts, ct);

            return Ok(result.Adapt<CategoriesResponse>());
        }

        [HttpGet("{eventId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPost("{eventId:int}/registrations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromRoute] int eventId, CancellationToken ct)
        {
            try
            {
                var currentUser = this.User.FindFirstValue("Sid");
                var userId = 1;

                await _eventService.RegisterOnEventAsync(userId, eventId, ct);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{eventId:int}/registrations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Unregister([FromRoute] int eventId, CancellationToken ct)
        {
            try
            {
                var currentUser = this.User.FindFirstValue("Sid");
                var userId = 1;

                await _eventService.UnregisterFromEventAsync(userId, eventId, ct);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
