using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace profiler.consumers.Models.Settings
{
    public class BusSettings
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string SchedulerQueueName { get; set; }
        public int ConcurrencyExceptionRetryInMilliseconds { get; set; }

        public string BuildEndpoint() => string.IsNullOrEmpty(VirtualHost) ? Host : $"{Host}/{VirtualHost}";

        public string BuildConnectionString() => $"amqp://{HttpUtility.UrlEncode(UserName)}:{HttpUtility.UrlEncode(Password)}@{BuildEndpoint().Replace("rabbitmq://", string.Empty)}";

        public int InitialConnectionRetryAttempts { get; set; } = 10;
    }
}
