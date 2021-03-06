﻿using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DeviceClient
{
    class Program
    {
        static RegistryManager registryManager;
      
        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(Constants.connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();
        }
        private static async Task AddDeviceAsync()
        {
            string deviceId = "mySecondDevice";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
