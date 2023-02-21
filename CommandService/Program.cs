using CommandService.DataAccess;
using CommandService.Models;
using CommandService.MQ;
using CommandService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddDbContext<BookContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMQConfig"));


var app = builder.Build();

app.MapControllers();

app.Run();