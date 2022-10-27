using Api_universal_robots_rmq.DAL;
using Api_universal_robots_rmq.RabitMQ;
using Api_universal_robots_rmq.Service;
using Microsoft.EntityFrameworkCore;

RabitMQConsumer consumer = new RabitMQConsumer();
Thread thread = new Thread(consumer.ConsumeMessages);
thread.Start();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<robotcontext>(opt =>
 opt.UseSqlServer("Data Source=mssql1.unoeuro.com;Initial Catalog=thomasblok_dk_db_softwareudvikling;Persist Security Info=True;User ID=thomasblok_dk;Password=Ea2Rrpz5GDmF"));
//builder.Services.AddScoped<IRabitMQProducer, RabitMQProducer>();

{
    var services = builder.Services;

    services.AddCors();
    
    
    services.AddScoped<IMessageService, Messageservice>();
}








var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

