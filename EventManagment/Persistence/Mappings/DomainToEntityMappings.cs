using Domain.Models;
using Mapster;
using Persistence.Entities;

namespace Persistence.Mappings;

public static class DomainToEntityMappings
{
    public static void ConfigureMappings()
    {
        ConfigureEvent();
        ConfigureAgenda();
        ConfigureTag();
        ConfigureRegistration();
        ConfigureUser();
        ConfigureLookups();
    }

    private static void ConfigureEvent()
    {
        TypeAdapterConfig<LocationEntity, Location>
            .NewConfig()
            .Map(dest => dest.Address, src => src.Address.Adapt<Address>());

        TypeAdapterConfig<Location, LocationEntity>
            .NewConfig()
            .Map(dest => dest.Address, src => src.Address.Adapt<AddressEntity>());

        TypeAdapterConfig<EventEntity, Event>
            .NewConfig()
            .Map(dest => dest.Location, src => src.Location.Adapt<Location>())
            .Ignore(dest => dest.Agendas)
            .Map(dest => dest.Tags,
                src => src.EventTags
                    .Select(et => et.TagEntity.Adapt<Tag>())
                    .ToList());


        TypeAdapterConfig<Event, EventEntity>
            .NewConfig()
            .Map(dest => dest.Location, src => src.Location.Adapt<LocationEntity>())
            .Ignore(dest => dest.Agendas)
            .Ignore(dest => dest.EventTags)
            .Ignore(dest => dest.Agendas)
            .Ignore(dest => dest.Registrations)
            .Ignore(dest => dest.CreatedBy)
            .Ignore(dest => dest.EventTypeEntity);
    }

    private static void ConfigureAgenda()
    {
        TypeAdapterConfig<AgendaItemEntity, AgendaItem>
            .NewConfig()
            .Map(dest => dest.Tracks, src => src.Tracks.Adapt<List<AgendaTrack>>());
        TypeAdapterConfig<AgendaTrackEntity, AgendaTrack>.NewConfig();

        TypeAdapterConfig<AgendaTrack, AgendaTrackEntity>
            .NewConfig()
            .Ignore(dest => dest.AgendaItem)
            .Ignore(dest => dest.AgendaItemId);
    }

    private static void ConfigureTag()
    {
        TypeAdapterConfig<TagEntity, Tag>.NewConfig();

        TypeAdapterConfig<Tag, TagEntity>
            .NewConfig()
            .Ignore(dest => dest.EventTags);
    }

    private static void ConfigureRegistration()
    {
        TypeAdapterConfig<RegistrationEntity, Registration>.NewConfig();

        TypeAdapterConfig<Registration, RegistrationEntity>
            .NewConfig()
            .Ignore(dest => dest.EventEntity)
            .Ignore(dest => dest.UserEntity)
            .Ignore(dest => dest.StatusEntity);
    }

    private static void ConfigureUser()
    {
        TypeAdapterConfig<UserEntity, User>
            .NewConfig()
            .Ignore(dest => dest.Events)
            .Ignore(dest => dest.Registrations);

        TypeAdapterConfig<User, UserEntity>
            .NewConfig()
            .Ignore(dest => dest.Events)
            .Ignore(dest => dest.Registrations)
            .Ignore(dest => dest.ApplicationUser);
    }

    private static void ConfigureLookups()
    {
        TypeAdapterConfig<EventTypeEntity, EventType>.NewConfig();
        TypeAdapterConfig<EventType, EventTypeEntity>
            .NewConfig()
            .Ignore(dest => dest.Events);

        TypeAdapterConfig<RegistrationStatusEntity, RegistrationStatus>.NewConfig();
        TypeAdapterConfig<RegistrationStatus, RegistrationStatusEntity>
            .NewConfig()
            .Ignore(dest => dest.Registrations);
    }
}