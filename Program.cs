using Microsoft.EntityFrameworkCore;

using OrdersWorkerService;
using OrdersWorkerService.Application.Services;
using OrdersWorkerService.Application.Validators;
using OrdersWorkerService.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<OrderConsumerService>();
builder.Services.AddSingleton<OrderValidatorService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
