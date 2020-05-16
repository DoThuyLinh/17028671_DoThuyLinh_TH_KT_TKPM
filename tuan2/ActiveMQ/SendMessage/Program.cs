using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMessage
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    while (true)
        //    {
        //        string text = Console.ReadLine();
        //        if (string.IsNullOrWhiteSpace(text)) return;
        //        SendNewMessage(text);
        //    }
        //}

        //private static void SendNewMessage(string text)
        //{
        //    string topic = "TextQueue";
        //    IConnectionFactory factory = new ConnectionFactory("activemq:tcp://localhost:61616");
        //    IConnection connection = factory.CreateConnection("admin","admin");
        //    connection.Start();
        //    ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge); 
        //    IDestination dest = session.GetTopic(topic);
        //    IMessageProducer producer = session.CreateProducer(dest);
        //    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
        //    producer.Send(session.CreateTextMessage(text));
        //    Console.WriteLine($"Sent {text} messages");
        //}

        static void Main(string[] args)
        {
            while (true)
            {
                string text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text)) return;
                SendNewMessageQueue(text);
            }
        }

        private static void SendNewMessageQueue(string text)
        {
            string queueName = "TextQueue";

            Console.WriteLine($"Adding message to queue topic: {queueName}");

            string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(queueName))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

                    producer.Send(session.CreateTextMessage(text));
                    Console.WriteLine($"Sent {text} messages");
                }
            }
        }
    }
}
