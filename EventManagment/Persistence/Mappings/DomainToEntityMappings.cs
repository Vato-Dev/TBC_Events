using Domain.Models;
using Mapster;
using Persistence.Entities;

namespace Persistence.Mappings;

public static class DomainToEntityMappings 
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<UserEntity, User>.NewConfig();
        TypeAdapterConfig<User, UserEntity>.NewConfig();

        TypeAdapterConfig<EventEntity, Event>.NewConfig();
        TypeAdapterConfig<Event, EventEntity>.NewConfig();

        TypeAdapterConfig<EventTypeEntity, EventType>.NewConfig();
        TypeAdapterConfig<EventType, EventTypeEntity>.NewConfig();

        TypeAdapterConfig<RegistrationEntity, Registration>.NewConfig();
        TypeAdapterConfig<Registration, RegistrationEntity>.NewConfig();

        TypeAdapterConfig<RegistrationStatusEntity, RegistrationStatus>.NewConfig();
        TypeAdapterConfig<RegistrationStatus, RegistrationStatusEntity>.NewConfig();

        TypeAdapterConfig<RoleEntity, Role>.NewConfig();
        TypeAdapterConfig<Role, RoleEntity>.NewConfig();

        TypeAdapterConfig<TagEntity, Tag>.NewConfig();
        TypeAdapterConfig<Tag, TagEntity>.NewConfig();

        TypeAdapterConfig<EventTagEntity, EventTag>.NewConfig();
        TypeAdapterConfig<EventTag, EventTagEntity>.NewConfig();
    }
}
