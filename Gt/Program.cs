using GtCores;
using GtCores.Consoles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddGtCores(builder.Configuration);
var app = builder.Build();
await app.StartAsync();
await app.Services.GetRequiredService<ICommandHandlerFactory>().StartAsync();
await app.WaitForShutdownAsync(ConsoleCore.TokenSource.Token);
