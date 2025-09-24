using BE_SWP391.Data;
using BE_SWP391.Repositories.Implementations;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Implementations;
using BE_SWP391.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BE_SWP391 API",
        Version = "v1"
    });
});
builder.Services.AddDbContext<EvMarketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EvMarketContext")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
