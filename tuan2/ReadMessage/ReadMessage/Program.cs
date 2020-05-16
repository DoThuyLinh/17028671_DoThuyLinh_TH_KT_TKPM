using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for messages");

            // Read all messages off the queue
            while (ReadNextMessageQueue() == true)
            {
                Console.WriteLine("Successfully read message");
            }

            Console.WriteLine("Finished");
        }

        static bool ReadNextMessageQueue()
        {
            string queueName = "TextQueue";
            NMSConnectionFactory factory = new NMSConnectionFactory("activemq: tcp://localhost:61616");
            IConnection connection = factory.CreateConnection("admin", "admin");
            connection.Start();
            ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            IDestination dest = session.GetQueue(queueName);
            IMessageConsumer consumer = session.CreateConsumer(dest);
            IMessage msg = consumer.Receive();
            if (msg is ITextMessage)
            {
                ITextMessage txtMsg = msg as ITextMessage;
                Console.WriteLine($"Received message: {txtMsg.Text}");
                return true;
            }
            else
            {
                Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
            }
            return false;
        }
    }
}
