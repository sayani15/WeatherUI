﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherUI.RabbitMQ;

namespace WeatherUI
{
	public class Startup
	{
		private readonly IMQDataAccess _mqDataAccess;
		private readonly IMQMessageServices _mqMessageServices;

        public Startup(IMQDataAccess mqDataAccess, IMQMessageServices mqMessageServices)
        {
			_mqDataAccess = mqDataAccess;
			_mqMessageServices = mqMessageServices;
        }
        public void Run()
		{
			
			var mQMessageServices = new MQMessageServices();
			var city = _mqDataAccess.GetCity();

			while (city.Location != "q")
			{
				_mqDataAccess.SendMessage(city);
				while (!_mqMessageServices.HasReceivedMessage)
				{
					Thread.Sleep(5000);
					break;
				}
				city = _mqDataAccess.GetCity();
			}

		}
	}
}
