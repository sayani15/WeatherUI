using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherUI
{
	public class MQMessageServices : IMQMessageServices
	{
		public bool HasReceivedMessage { get; private set; } = false;

		public void ToggleMessageReceived()
		{
			HasReceivedMessage = !HasReceivedMessage;
		}
	}

	public interface IMQMessageServices
	{
		bool HasReceivedMessage { get; }
		void ToggleMessageReceived();
	}
}
