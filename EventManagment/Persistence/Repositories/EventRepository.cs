using Application.Repositories;
using Domain.Models;
using Mapster;
using Persistence.Data;
using Persistence.Entities;

namespace Persistence.Repositories;

public class EventRepository(AppDbContext context)  : IEventRepository
{
    //Just example of mapping 
    //await _userRepository.AddAsync(user);
    
    
    //var userEntity = user.Adapt<UserEntity>();
   // _context.Users.Add(userEntity);
   // await _context.SaveChangesAsync();
   
   //this is from reading 
   
 // var entities = await _context.Users.AsQueryable //To make it lazy and send request in db after calling To list or something , 
     //  .Include(u => u.Events) this include can be in if(filter is true) { entity.Include} if it's false just continue , but before 
     
     // after mapping (for one or list) var user = entity.Adapt<User>(); 
     public async Task<int> CreateEventAsync(Event @event,List<int>tagIds, CancellationToken cancellationToken)
     {
        /* var entity = @event.Adapt<EventEntity>();
         foreach (var tagId in tagIds)
         {
              context.EventTags.Add(new EventTagEntity
             {
                 EventId = entity.Id,
                 TagId = tagId
             });
         }
         context.Events.Add(entity);*/
        throw new NotImplementedException();
     }

     public Task<Event> GetEventByIdAsync(int id, CancellationToken cancellationToken)
     {
         throw new NotImplementedException();
     }

     public Task UpdateEventAsync(Event @event, CancellationToken cancellationToken)
     {
         throw new NotImplementedException();
     }
}