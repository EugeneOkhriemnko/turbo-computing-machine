global using Test.Client.Context;
global using Test.Client.Http;
global using Test.Client.Exceptions;
global using Test.Client.Models;
global using Test.Client.Commands.ProductCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Serilog;
using Test.Client;
using Test.Client.Menues;

if (args.Length == 0)
{
    Console.WriteLine("ERROR: The URL of the backend must be provided in the arguments.");
    return;
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Application starting...");
    using IHost host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.None);
        })
        .ConfigureServices((_, services) =>
        {
            services.AddTransient<AuthorizationMessageHandler>();
            services.AddSingleton<IContext, Test.Client.Context.Context>();
            services.AddSingleton<LoginMenu>();
            services.AddSingleton<ProductMenu>();
            services.AddTransient<Application>();
            services.AddHttpClient<ServerHttpClient>(client =>
                client.BaseAddress = new Uri(args[0]))
                .AddTransientHttpErrorPolicy(policy =>
                    policy.WaitAndRetryAsync(
                    [
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(3)
                    ]))
                .AddHttpMessageHandler<AuthorizationMessageHandler>();
        }).Build();
    var app = host.Services.GetRequiredService<Application>();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
    return;
}