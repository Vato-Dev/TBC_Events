using Domain.Models;
using Mapster;
using Persistence.Entities;

namespace Persistence.Mappings;

public static class DomainToEntityMappings 
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<UserEntity, User>.NewConfig()
            .Map(d=>d.Role, s=>s.Role)
            .Map(d => d.Events, e => e.Events)
            .Map(d => d.Registrations, s => s.Registrations);
        TypeAdapterConfig<User, UserEntity>.NewConfig()
            .Map(d=>d.Role, s=>s.Role)
            .Map(d => d.Events, e => e.Events)
            .Map(d => d.Registrations, s => s.Registrations);
        
        
        TypeAdapterConfig<EventEntity, Event>.NewConfig();
        TypeAdapterConfig<Event, EventEntity>.NewConfig();

        TypeAdapterConfig<EventTypeEntity, EventType>.NewConfig();
        TypeAdapterConfig<EventType, EventTypeEntity>.NewConfig();

        TypeAdapterConfig<RegistrationEntity, Registration>.NewConfig();
        TypeAdapterConfig<Registration, RegistrationEntity>.NewConfig();

        TypeAdapterConfig<RegistrationStatusEntity, RegistrationStatus>.NewConfig();
        TypeAdapterConfig<RegistrationStatus, RegistrationStatusEntity>.NewConfig();
        

        TypeAdapterConfig<TagEntity, Tag>.NewConfig();
        TypeAdapterConfig<Tag, TagEntity>.NewConfig();

        TypeAdapterConfig<EventTagEntity, EventTag>.NewConfig();
        TypeAdapterConfig<EventTag, EventTagEntity>.NewConfig();
    }
}
