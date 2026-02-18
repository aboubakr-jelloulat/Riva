using Riva.API.Models;
using Riva.DTO;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<VillaCreateDTO, VillaDTO>().ReverseMap();
    cfg.CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();
});

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
