using Application.Requests.Events;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController(IEventService eventService) : ControllerBase
{

    [HttpGet]
    [Route("api/events/{eventId:int}")]
    public async Task<IActionResult> GetEventByIdAsync(int eventId, CancellationToken cancellationToken)
    {
        return Ok(await eventService.GetEventByIdAsyncc(eventId, cancellationToken));
    }
    
    [HttpPost]
    [Route("api/events/create-event")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        return Ok( await eventService.CreateEventAsync(request, cancellationToken));
    }

    [HttpPost]
    [Route("api/events/{eventId:int}/create-agenda")]
    public async Task<IActionResult> CreateAgendaToEvent(int eventId, CreateAgendaRequest request,
        CancellationToken cancellationToken)
    {
        return Ok( await eventService.CreateAndAddAgendaToEvent(eventId, request, cancellationToken));
        
    }

    [HttpPost]
    [Authorize]
    [Route("api/events/update-event")]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await eventService.UpdateEventAsync(request, cancellationToken));
    }

    [HttpPost]
    [Route("api/events/update-agenda")]
    public async Task<IActionResult> UpdateAgendaToEvent(UpdateAgendaRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await eventService.UpdateAgendaItemAsync(request, cancellationToken));
    }

    [HttpDelete]
    [Route("api/events/delete-event")]
    public async Task<IActionResult> DeleteEventAsync(int eventId, CancellationToken cancellationToken)
    {
        await eventService.DeleteEventAsync(eventId, cancellationToken);
        return NoContent();
    }
}