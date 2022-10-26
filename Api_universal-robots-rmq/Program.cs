using Api_universal_robots_rmq.RabitMQ;
using Api_universal_robots_rmq.Service;

RabitMQConsumer consumer = new RabitMQConsumer();
consumer.ConsumeMessages();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddScoped<IRabitMQConsumer, RabitMQConsumer>();
//builder.Services.AddSwaggerGen();
// add services to dependency injection container
{
    var services = builder.Services;

    services.AddCors();
    
    // configure dependency injection for application services
    services.AddScoped<IMessageService, Messageservice>();
}

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

