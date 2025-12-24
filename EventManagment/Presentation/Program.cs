using System.Text.Json.Serialization;
using Application.Mapping;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Infrastructure.Mappings;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Mappings;
using Presentation;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IOtpService, OtpService>();




// Add services to the container.

DomainToEntityMappings.ConfigureMappings();
IdentityErrorToApplicationMappings.ConfigureMappings();
RequestsToDomain.ConfigureMappings();


/*builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; //mt ADRESS IF I ONLY COULS FIX MY DOCKER CONTAINER BRUH
    options.InstanceName = "Otp_"; //PREFIX
});*/

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