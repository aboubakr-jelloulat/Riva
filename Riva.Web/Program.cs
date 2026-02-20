using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Riva.API.Models;
using Riva.DTO;
using Riva.Web.Services;
using Riva.Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<VillaCreateDTO, VillaDTO>().ReverseMap();
    cfg.CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();
});


builder.Services.AddHttpClient("Riva.API", client =>
{
    var villaAPIUrl = builder.Configuration.GetValue<string>("ServiceUrls:RivaAPI");

    client.BaseAddress = new Uri(villaAPIUrl);

    client.DefaultRequestHeaders.Add("Accept", "application/json");
    /*I set the Accept header to application/json to make sure the API returns data in JSON format
       which is the standard format used in ASP.NET Core applications.*/
});

builder.Services.AddScoped<IVillaService, VillaService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
