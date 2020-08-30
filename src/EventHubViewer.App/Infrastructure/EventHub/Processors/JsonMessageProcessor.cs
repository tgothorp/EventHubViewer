using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azure.Messaging.EventHubs.Processor;
using Newtonsoft.Json.Linq;

namespace EventHubViewer.App.Infrastructure.EventHub.Processors
{
    public class JsonMessageProcessor : IMessageProcessor
    {
        public Message Process(ProcessEventArgs @event)
        {
            var rawMessage = Encoding.UTF8.GetString(@event.Data.Body.ToArray());
            
            var properties = JObject.Parse(rawMessage).Properties();
            var propertyValues = properties.ToDictionary(x => x.Name, y => y.Value.ToString());
            
            return new JsonMessage(propertyValues, @event);
        }
        
        public class JsonMessage : Message
        {
            public JsonMessage(Dictionary<string, string> jsonValues, ProcessEventArgs @event)
            {
                JsonValues = jsonValues;
                
                Enqueued = @event.Data.EnqueuedTime.DateTime.ToString("G");
                Received = DateTime.Now.ToString("G");
                RawMessage = Encoding.UTF8.GetString(@event.Data.Body.ToArray());
                MessageFormat = MessageFormat.Json;
                MessageLength = @event.Data.Body.Length;
            }
            
            public Dictionary<string, string> JsonValues { get; set; }
        }
    }
}