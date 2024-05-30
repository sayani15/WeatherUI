using Microsoft.Extensions.DependencyInjection;
using WeatherUI;
using WeatherUI.RabbitMQ;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
{
	services.AddSingleton<IMQMessageServices, MQMessageServices>();
	services.AddSingleton<IMQDataAccess, MQDataAccess>();
}).Build();
var svc = ActivatorUtilities.CreateInstance<Startup>(host.Services);
svc.Run();




