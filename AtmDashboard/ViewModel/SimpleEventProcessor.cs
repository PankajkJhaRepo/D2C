using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using AtmDashboard.Models;
using Newtonsoft.Json;
using System.Text;

namespace AtmDashboard.ViewModel
{
    public class SimpleEventProcessor : IEventProcessor
    {
        Stopwatch checkpointStopWatch;
        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Debug.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                  await context.CheckpointAsync();
            }
        }

        public Task OpenAsync(PartitionContext context)
        {
            Debug.WriteLine("SimpleEventProcessor initialized. Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Debug.WriteLine(string.Format("Message received. Partition: '{0}', Data: '{1}'",
                context.Lease.PartitionId, data));
                // Log the event
                ATMEvent e = JsonConvert.DeserializeObject<ATMEvent>(data);
                ATMEventAggregator.LogEvent(e);
            }
            if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                this.checkpointStopWatch.Restart();
            }
        }
    }
}