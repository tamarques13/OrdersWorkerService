using Microsoft.EntityFrameworkCore;
using OrdersWorkerService.src.Application.Interfaces;
using OrdersWorkerService.src.Application.Services;
using OrdersWorkerService.src.Infrastructure.Data;
using OrdersWorkerService.src.Infrastructure.Hosting;
using OrdersWorkerService.src.Infrastructure.Interfaces;
using OrdersWorkerService.src.Infrastructure.RabbitMQ;
using OrdersWorkerService.src.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IOrderValidationService, OrderValidationService>();
builder.Services.AddScoped<IOrderProcessService, OrderProcessService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddSingleton<IOrderConsumerService, OrderConsumerService>();

builder.Services.AddHostedService<BackgroundWorker>();

var host = builder.Build();
host.Run();
