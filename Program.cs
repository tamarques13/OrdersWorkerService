using Microsoft.EntityFrameworkCore;

using OrdersWorkerService;
using OrdersWorkerService.Data;
using OrdersWorkerService.Services;
using OrdersWorkerService.Validators;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<OrderConsumerService>();
builder.Services.AddSingleton<OrderValidator>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
