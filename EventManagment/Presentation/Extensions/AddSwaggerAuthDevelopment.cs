using Microsoft.OpenApi.Models;

namespace Presentation.Extensions;

public static class AddSwaggerAuthDevelopment
{
  public static WebApplicationBuilder addSwaggerAuthDevelopment(this WebApplicationBuilder builder)
  {
    builder.Services.AddSwaggerGen(option =>
    {
      option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = ""
      });

      option.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "bearer"
            }
          },
          new string[] { }
        }
      });
    });
    return builder;
  }
}