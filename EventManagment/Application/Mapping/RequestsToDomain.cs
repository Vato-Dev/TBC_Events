using Application.Requests.Events;
using Application.Services.Implementations;
using Domain.Models;
using Mapster;

namespace Application.Mapping;

public static class RequestsToDomain{

    public static void ConfigureMappings()
    {
        TypeAdapterConfig<CreateEventRequest, Event>.NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Tags);

        TypeAdapterConfig<UpdateEventRequest, Event>.NewConfig()
            .Ignore(dest => dest.Tags)
            .Ignore(dest => dest.Agendas);
        
        TypeAdapterConfig<CreateAgendaRequest, AgendaItem>.NewConfig()
            .Map(dest => dest.Tracks, src => src.AgendaTracks, src => src.AgendaTracks != null && src.AgendaTracks.Any())
            .AfterMapping((src, dest) => {
                dest.Tracks?.RemoveAll(t => string.IsNullOrWhiteSpace(t.Title) || string.IsNullOrWhiteSpace(t.Speaker));
            });

        TypeAdapterConfig<AgendaTrackDTO, AgendaTrack>.NewConfig();
    }
}