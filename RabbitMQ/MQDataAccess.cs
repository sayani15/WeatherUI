using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using WeatherUI.Models;


namespace WeatherUI.RabbitMQ
{
	public class MQDataAccess : DefaultBasicConsumer, IMQDataAccess
	{
		private IModel _channel { get; set; }
		private readonly IMQMessageServices _messageServices;

		public MQDataAccess(IMQMessageServices messageServices)
		{
			CreateConnection();
			_messageServices = messageServices;
		}

		public City GetCity()
		{
			var city = new City();
			Console.WriteLine("what location's weather would you like to view? enter your city of choice below");
			city.Location = Console.ReadLine().ToLower();

			return city;
		}


		private void CreateConnection()
		{
			var factory = new ConnectionFactory()
			{
				HostName = "127.0.0.1",
				UserName = "guest",
				Password = "guest",
				Port = 5672
			};

			var connection = factory.CreateConnection();
			_channel = connection.CreateModel();
			_channel.BasicQos(0, 1, false);
			_channel.BasicConsume("Weather-Forecast-Queue", false, this);
		}

		public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
		{
			try
			{
				_messageServices.ToggleMessageReceived();
				var mqMessage = new MQMessage();
				var bodies = body.ToArray();
				var message = Encoding.UTF8.GetString(bodies);
				mqMessage.Message = message;
				mqMessage.RoutingKey = routingKey;
				ProcessMessage(mqMessage);
			}
			catch (Exception)
			{
				Console.WriteLine("could not process message");
			}
			finally
			{
				_channel.BasicAck(deliveryTag, false);
			}
		}

		private async void ProcessMessage(MQMessage message)
		{
			var weatherData = JsonConvert.DeserializeObject<APIResponse>(message.Message.ToString());

			if (weatherData != null && weatherData.Forecast != null && weatherData.Forecast.Forecastday != null)
			{
				Console.WriteLine($"Maximum temperature: {weatherData.Forecast.Forecastday.First().Day.Maxtemp_c}");
			}
			else
			{
                Console.WriteLine("something went wrong, try again");
            }
			_messageServices.ToggleMessageReceived();
		}

		public void SendMessage(City message)
		{
			var byteArray = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
			_channel.BasicPublish("Weather-Request-Exchange", "request.forecast", body: byteArray);
		}
	}
}
