using Infrastructure.Data.Contexts;
using Application.Web;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DependencyInjection;
using Application.Services;
using Application.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger=>
{
    swagger.EnableAnnotations();
});

builder.Services.AddAutoMapper(typeof(AutoMapping));

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(new JwtManager(builder.Configuration).GenerateKey());

builder.Services.AddDbContext<RestaurantDbContext>(options =>
{
    options.UseInMemoryDatabase("RestaurantDatabase");
});

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
