using Microsoft.EntityFrameworkCore;
using Riva.API.Data.Context;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

builder.Services.AddControllers();

builder.Services.AddOpenApi();


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



