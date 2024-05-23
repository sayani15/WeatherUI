using RabbitMQ.Client;
using WeatherUI.Models;

namespace WeatherUI.RabbitMQ
{
	public interface IMQDataAccess
	{
		City GetCity();
		void SendMessage(City message);
	}
}