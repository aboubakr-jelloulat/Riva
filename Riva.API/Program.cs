using Microsoft.EntityFrameworkCore;
using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Data.Repository.Repositories;
using Riva.API.Models;
using Riva.DTO;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(options => 
{
    options.CreateMap<VillaCreateDTO, Villa>();
    options.CreateMap<VillaUpdateDTO, Villa>();
});

var app = builder.Build();

await DbInitializer.SeedDataAsync(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



