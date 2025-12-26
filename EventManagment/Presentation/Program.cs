using System.Text.Json.Serialization;
using Application.Mapping;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Application.Extensions;
using dotenv.net;
using Infrastructure.BackGroundJobs;
using Infrastructure.Mappings;
using Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Mappings;
using Presentation;
using Presentation.Extensions;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Fluent()
    .WithTrimValues()
    .WithOverwriteExistingVars().WithProbeForEnv(6)
    .Load();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IOtpService, OtpService>();

builder.Services.AddQuaertzJobs();



// Add services to the container.

DomainToEntityMappings.ConfigureMappings();
IdentityErrorToApplicationMappings.ConfigureMappings();
RequestsToDomain.ConfigureMappings();

/*
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "redis:6379";
    options.InstanceName = "Otp_";
});*/
builder.Services.AddApplicationServices();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.AddApplicationServices();
if (builder.Environment.IsDevelopment())
{
    builder.addSwaggerAuthDevelopment();
    builder.Services.AddScoped<ICurrentUserService, FakeCurrentUserService>();
}
else
{
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
}
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.AddEmailSenders();
builder.AddAllRepositories();

builder.AddAllIdentity();
builder.Services.AddCors(options =>
{
    options.AddPolicy("NgrokGuidPolicy", builder =>
    {
        builder.AllowAnyOrigin() 
            .AllowAnyMethod()   
            .AllowAnyHeader(); 
    });
});
var app = builder.Build();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseMiddleware<GlobalErrorHandlingMiddleWare>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();