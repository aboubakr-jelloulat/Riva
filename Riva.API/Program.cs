using Microsoft.EntityFrameworkCore;
using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;
using Riva.API.Data.Repository.Repositories;
using Riva.API.Models;
using Riva.API.Services;
using Riva.API.Services.IServices;
using Riva.API.Utils;
using Riva.DTO;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

builder.Services.JwtConfigureServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(options => 
{
    options.CreateMap<VillaCreateDTO, Villa>().ReverseMap();
    options.CreateMap<VillaUpdateDTO, Villa>().ReverseMap();
    options.CreateMap<VillaDTO,       Villa>().ReverseMap();
    options.CreateMap<UserDTO,        User>().ReverseMap();
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

await DbInitializer.SeedDataAsync(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



