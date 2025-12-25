using Application.DTOs;
using Application.Requests.Events;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.RequestModels;
using Presentation.DTOs.ResponseModels;
using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Presentation.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController(IEventService eventService) : ControllerBase
{
    
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("test/{eventId:int}")]
    public async Task<IActionResult> GetEventByIdAsync(int eventId, CancellationToken cancellationToken)
    {
        return Ok(await eventService.GetEventByIdAsyncc(eventId, cancellationToken));
    }
    
    [HttpGet("filters-meta")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<EventFiltersMetaResponseDto>> GetMeta(
    CancellationToken cancellationToken)
    {
   

        var meta = await eventService.GetFiltersMetaAsync(GetCurrentUserId(), cancellationToken);

        return Ok(meta.Adapt<EventFiltersMetaResponseDto>());
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<EventsSearchResponse>> GetEvents(
        [FromQuery] EventsSearchRequestDto query,
        CancellationToken ct)
    {
       
        var filters = query.Adapt<EventsSearchFilters>();
        var result = await eventService.GetEventsAsync(GetCurrentUserId(), filters, ct);

        return Ok(result.Adapt<EventsSearchResponse>());
    }

    [HttpGet("categories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<CategoriesResponse>> GetCategories(
        [FromQuery] bool withCounts = false,
        CancellationToken ct = default)
    {

        var result = await eventService.GetCategoriesAsync(GetCurrentUserId(), withCounts, ct);

        return Ok(result.Adapt<CategoriesResponse>());
    }

    [HttpGet("{eventId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(EventDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDetailsResponse>> GetById([FromRoute] int eventId, CancellationToken ct)
    {

        var result = await eventService.GetEventDetailsAsync(GetCurrentUserId(), eventId, ct);
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
            await eventService.RegisterOnEventAsync(GetCurrentUserId(), eventId, ct);
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
        

            await eventService.UnregisterFromEventAsync(GetCurrentUserId(), eventId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("create-event")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        return Ok( await eventService.CreateEventAsync(request, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("{eventId:int}/create-agenda")]
    public async Task<IActionResult> CreateAgendaToEvent(int eventId, CreateAgendaRequest request,
        CancellationToken cancellationToken)
    {
        return Ok( await eventService.CreateAndAddAgendaToEvent(eventId, request, cancellationToken));
        
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  
    [Route("update-event")]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await eventService.UpdateEventAsync(request, cancellationToken));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("update-agenda")]
    public async Task<IActionResult> UpdateAgendaToEvent(UpdateAgendaRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await eventService.UpdateAgendaItemAsync(request, cancellationToken));
    }

    [HttpGet("{eventId:int}/registrations/grouped")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(EventRegistrationsGroupedDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<EventRegistrationsGroupedDto>> GetRegistrationsGrouped(
    [FromRoute] int eventId,
    CancellationToken ct)
    {
        var result = await eventService.GetEventRegistrationsGroupedAsync(eventId, ct);
        return Ok(result.Adapt<EventRegistrationsGroupedResponseDto>());
    }

    [HttpPost("{eventId:int}/registrations/{userId:int}/confirm")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // optionally restrict to admins
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ConfirmWaitlisted(
    [FromRoute] int eventId,
    [FromRoute] int userId,
    CancellationToken ct)
    {
        try
        {
            await eventService.ConfirmWaitlistedAsync(eventId, userId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{eventId:int}/waitlist/{userId:int}/reject")] [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // e.g. policy/role later
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RejectWaitlisted(
    [FromRoute] int eventId,
    [FromRoute] int userId,
    CancellationToken ct)
    {
        try
        {
            await eventService.RejectWaitlistedAsync(eventId, userId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("delete-event")]
    public async Task<IActionResult> DeleteEventAsync(int eventId, CancellationToken cancellationToken)
    {
        await eventService.DeleteEventAsync(eventId, cancellationToken);
        return NoContent();
    }

    [HttpGet("export")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ExportEventsAsCsv(
    [FromQuery] EventsSearchRequestDto query,
    CancellationToken ct)
    {
        var filters = query.Adapt<EventsSearchFilters>();

        var csv = await eventService.GetEventsAsCsvAsync(GetCurrentUserId(), filters, ct);

        return File(csv, "text/csv", "events.csv");
    }

    private int GetCurrentUserId()
    {
       return int.Parse(User.FindFirstValue("Sid")??"1");
    }
}