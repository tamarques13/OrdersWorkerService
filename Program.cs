using Microsoft.EntityFrameworkCore;

using OrdersWorkerService.Application.Interfaces;
using OrdersWorkerService.Application.Services;

using OrdersWorkerService.Infrastructure.Hosting;
using OrdersWorkerService.Infrastructure.Data;
using OrdersWorkerService.Infrastructure.Interfaces;
using OrdersWorkerService.Infrastructure.Repositories;
using OrdersWorkerService.Infrastructure.RabbitMQ;

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
