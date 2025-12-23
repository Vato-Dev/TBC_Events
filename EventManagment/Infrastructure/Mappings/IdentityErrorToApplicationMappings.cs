using Application.IdentityModels.Results;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Mappings;

public static class IdentityErrorToApplicationMappings
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<IdentityError, ApplicationError>.NewConfig()
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Description, src => src.Description);
    }
}