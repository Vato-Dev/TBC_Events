using Application.Services;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Mappings;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DomainToEntityMappings.ConfigureMappings();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.AddAllIdentity();
builder.AddAllRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();