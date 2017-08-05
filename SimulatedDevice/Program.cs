using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace SimulatedDevice
{
    
    public class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device1\n");
            DeviceClient device1Client;
          
            device1Client = DeviceClient.Create(Constants.iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(Constants.device1.Name , Constants.device1.Key), TransportType.Mqtt);
            SendDeviceToCloudSometimesCriticalMessagesAsync(device1Client, Constants.device1);
            //SendDeviceToCloudMessagesAsync();
            Console.WriteLine("Simulated device2\n");
            DeviceClient device2Client;
            device2Client = DeviceClient.Create(Constants.iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(Constants.device2.Name, Constants.device2.Key), TransportType.Mqtt);
            SendDeviceToCloudSometimesCriticalMessagesAsync(device2Client, Constants.device2);
            Console.ReadLine();
        }
        private static async void SendDeviceToCloudMessagesAsync(DeviceClient deviceClient,Constants.device device )
        {
            double minTemperature = 20;
            double minHumidity = 60;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telemetryDataPoint = new
                {
                    messageId = messageId++,
                    deviceId = device.Name,
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                //DeviceReading data = new DeviceReading();
                //data.DeviceId = "pankaj/home/dth12";
                //data.Id = Guid.NewGuid().ToString();
                //data.QOutTime = DateTime.Now;
                //data.ReadingTime = DateTime.Now;
                //data.Value = "{ temperature : " + currentTemperature.ToString() + ",humidity:" + currentHumidity.ToString() + "}";
                //    var messageString = JsonConvert.SerializeObject(data);
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }

        private static async void SendDeviceToCloudSometimesCriticalMessagesAsync(DeviceClient deviceClient, Constants.device device)
        {
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telemetryDataPoint = new
                {
                    deviceId = device.Name,
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                DeviceReading data = new DeviceReading();
                data.Id = Guid.NewGuid().ToString();
                data.DeviceId = "pankaj/home/dth12";
                data.QOutTime = DateTime.Now;
                data.ReadingTime = DateTime.Now;
                data.Value = "{ temperature : " + currentTemperature.ToString() + ",humidity:" + currentHumidity.ToString() + "}";
                var messageString = JsonConvert.SerializeObject(data);
                //var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                string levelValue;

                if (rand.NextDouble() > 0.7)
                {
                    messageString = "This is a critical message " + device.Name;
                    levelValue = "critical";
                }
                else
                {
                    levelValue = "normal";
                }

                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("level", levelValue);

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sent message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
    }
}
