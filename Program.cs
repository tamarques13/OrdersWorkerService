using Microsoft.EntityFrameworkCore;

using OrdersWorkerService;

using OrdersWorkerService.Application.Interfaces;
using OrdersWorkerService.Application.Services;

using OrdersWorkerService.Infrastructure.Data;
using OrdersWorkerService.Infrastructure.Interfaces;
using OrdersWorkerService.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IOrderConsumerService, OrderConsumerService>();
builder.Services.AddSingleton<IOrderValidatorService, OrderValidatorService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
