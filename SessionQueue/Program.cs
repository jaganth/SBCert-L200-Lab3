using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SessionQueue
{
    class Program
    {
        
        static string connectionString = "[REPLACE-WITH-CONNECTION-STRING]";
        static string queueName = "SBLab3Queue";
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            var factory = MessagingFactory.CreateFromConnectionString(connectionString);
            //create a receiver on the queue
            var receiver = factory.CreateQueueClient(queueName);
            //create a sender on the queue
            var sender = await factory.CreateMessageSenderAsync(queueName);

            //send a message to queue
            await sender.SendAsync(new BrokeredMessage("Hello World!") { MessageId = "deadbeef-dead-beef-dead-beef00000075", SessionId = "First" });

            Thread.Sleep(10000);
            var msg = receiver.ReceiveAsync().GetAwaiter().GetResult();

            Console.WriteLine("Receiving message - {0}", msg.MessageId);

            await Task.WhenAny(
                Task.Run(() => Console.ReadKey()),
                Task.Delay(TimeSpan.FromSeconds(30))
            );
        }
    }
}
