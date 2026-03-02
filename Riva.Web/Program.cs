using Microsoft.AspNetCore.Authentication.Cookies;
using Riva.DTO;
using Riva.Web.Services;
using Riva.Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});


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
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/accessdenied";
    });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
