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


builder.Services.AddScalarBearerAuth("v1", "Riva API");


builder.Services.AddControllers();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Villa, VillaDTO>();
    cfg.CreateMap<VillaCreateDTO, Villa>();
    cfg.CreateMap<VillaUpdateDTO, Villa>();

    cfg.CreateMap<User, UserDTO>().ReverseMap();

    cfg.CreateMap<VillaAmenities, VillaAmentiesDTO>().ForMember(dest => dest.VillaName, opt => opt.MapFrom(src => src.Villa.Name));

    cfg.CreateMap<VillaAmentiesCreateDTO, VillaAmenities>();
    cfg.CreateMap<VillaAmentiesUpdateDTO, VillaAmenities>();
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



