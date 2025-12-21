using Application.Repositories;
using Persistence.Data;

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
}