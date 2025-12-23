using Infrastructure.Mappings;
using Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Mappings;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddHttpContextAccessor();



// Add services to the container.

DomainToEntityMappings.ConfigureMappings();
IdentityErrorToApplicationMappings.ConfigureMappings();



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



if (builder.Environment.IsDevelopment())
{
    builder.addSwaggerAuthDevelopment();
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.AddEmailSenders();
builder.AddAllRepositories();

builder.AddAllIdentity();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                               | ForwardedHeaders.XForwardedProto
                               | ForwardedHeaders.XForwardedHost);

var app = builder.Build();


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