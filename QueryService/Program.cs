using Microsoft.EntityFrameworkCore;
using QueryService.DataAccess;
using QueryService.Models;
using QueryService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<RabbitMqConsumerService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<BookContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped<IBookQueryService, BookQueryService>();
builder.Services.AddSingleton<IBookUpdateService, BookUpdateService>();
builder.Services.AddSingleton<BookUpdateContext>();
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMQConfig"));


var app = builder.Build();

app.MapControllers();

app.Run();