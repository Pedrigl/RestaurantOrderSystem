using Infrastructure.Data.Contexts;
using Application.Web;
using Microsoft.EntityFrameworkCore;
using Infrastructure.ExtensionMethods;
using Application.Services;

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

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();

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
