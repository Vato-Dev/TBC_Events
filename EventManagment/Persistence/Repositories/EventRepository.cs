using System.Xml;
using Application.Repositories;
using Application.Requests.Events;
using Application.Services.Implementations;
using Domain.Models;
using Mapster;
using Persistence.Data;
using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class EventRepository(AppDbContext context)  : IEventRepository
{
     public async Task<int> CreateEventAsync(Event @event,List<int>tagIds, CancellationToken cancellationToken)
     {
        var entity = @event.Adapt<EventEntity>();
        entity.EventTags = tagIds.Select<int, EventTagEntity>(id => new EventTagEntity { TagId = id }).ToList();
        await context.AddAsync(entity, cancellationToken); 
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
     }
     
     public async Task<Event> GetEventByIdAsync(int id, CancellationToken cancellationToken)
     {
         var entity = await context.Events.Include(e=>e.Agendas).SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
         var @event = entity.Adapt<Event>();
         return @event;
     }

     public async Task<int?> UpdateEventAsync(UpdateEventRequest request, CancellationToken cancellationToken) //Performance was dead so i throwed out my "Perfect architecture" with 3 mappings
     { 
         var entity = await context.Events
             .Include(e => e.EventTags)
             .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

         if (entity == null) return null;
 
         request.Adapt(entity);
         entity.UpdatedAt = DateTime.UtcNow;
         var tagsToRemove = entity.EventTags
             .Where(et => !request.TagIds.Contains(et.TagId))
             .ToList();
    
         foreach (var tag in tagsToRemove)
             entity.EventTags.Remove(tag);

         var existingTagIds = entity.EventTags.Select(et => et.TagId).ToList();
         var tagsToAdd = request.TagIds
             .Where(id => !existingTagIds.Contains(id))
             .Select(id => new EventTagEntity { EventId = entity.Id, TagId = id });

         foreach (var newTag in tagsToAdd)
             entity.EventTags.Add(newTag);

         context.Update(entity);
        return await context.SaveChangesAsync(cancellationToken);
     }
     public async Task<int?> AddEventAgendaAsync( //result pattern jobda
         int eventId,
         AgendaItem agenda,
         CancellationToken cancellationToken)
     {
         var existingEntity = await context.Events
             .Include(e => e.Agendas)
             .ThenInclude(a => a.Tracks)
             .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

         if (existingEntity == null) return null;

         var agendaEntity = agenda.Adapt<AgendaItemEntity>();

         agendaEntity.EventId = existingEntity.Id;
         agendaEntity.Event = existingEntity;

         foreach (var track in agendaEntity.Tracks)
         {
             track.AgendaItem = agendaEntity;
         }

         existingEntity.Agendas.Add(agendaEntity);

         await context.SaveChangesAsync(cancellationToken);
         return agendaEntity.Id;
     }
     
     public async Task<int?> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken cancellationToken)
     {
         var entity = await context.Agendas
             .Include(a => a.Tracks)
             .SingleOrDefaultAsync(a => a.Id == request.AgendaId, cancellationToken);

       
         if (entity == null) return null ;

         request.Adapt(entity);

         var requestTrackIds = request.Tracks.Where(t => t.Id > 0).Select(t => t.Id).ToList();
         var tracksToRemove = entity.Tracks
             .Where(t => !requestTrackIds.Contains(t.Id))
             .ToList();

         foreach (var track in tracksToRemove)
             entity.Tracks.Remove(track); // there is an issue if track does not exist? it's error

         foreach (var trackDto in request.Tracks)
         {
             if (trackDto.Id > 0)
             {
                 var existingTrack = entity.Tracks.FirstOrDefault(t => t.Id == trackDto.Id);
                 if (existingTrack != null)
                 {
                     trackDto.Adapt(existingTrack);
                 }
             }
             else
             {
                 var newTrack = trackDto.Adapt<AgendaTrackEntity>();
                 entity.Tracks.Add(newTrack);
             }
         }
         await context.SaveChangesAsync(cancellationToken);
         return entity.Id;
     }
}

