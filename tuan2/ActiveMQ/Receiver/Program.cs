﻿using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Waiting for messages");

        //    // Read all messages off the queue
        //    while (ReadNextMessageQueue() == true)
        //    {
        //        Console.WriteLine("Successfully read message");
        //    }

        //    Console.WriteLine("Finished");
        //    Console.ReadLine();
        //}

        //static bool ReadNextMessageQueue()
        //{
        //    string queueName = "TextQueue";
        //    IConnectionFactory factory = new ConnectionFactory("activemq:tcp://localhost:61616");
        //    IConnection connection = factory.CreateConnection("admin", "admin");
        //    connection.Start();
        //    ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
        //    IDestination dest = session.GetQueue(queueName);
        //    IMessageConsumer consumer = session.CreateConsumer(dest);
        //    IMessage msg = consumer.Receive();
        //    if (msg is ITextMessage)
        //    {
        //        ITextMessage txtMsg = msg as ITextMessage;
        //       // ActiveMQTextMessage ms = msg as ActiveMQTextMessage;
        //        Console.WriteLine($"Received message: {ms.Text}");
        //        return true;
        //    }
        //    else
        //    {
        //        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
        //    }
        //    return false;
        //}
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for messages");

            // Read all messages off the queue
            while (ReadNextMessageQueue())
            {
                Console.WriteLine("Successfully read message");
            }

            Console.WriteLine("Finished");
        }

        static bool ReadNextMessageQueue()
        {
            string queueName = "TextQueue";

            string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(queueName))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                {
                    IMessage msg = consumer.Receive();
                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;

                        Console.WriteLine($"Received message: {txtMsg.Text}");

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
            }

            return false;
        }
    }
}
