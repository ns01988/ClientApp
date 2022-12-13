using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ClientWrite
{
    class Program
    {
        static IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1236);
        TcpListener listener = new TcpListener(ep);
        private static byte[] sendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 2048;
            try // Try connecting and send the message bytes  
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 1234); // Create a new connection  
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes  
                Console.WriteLine("================================");
                Console.WriteLine("=   Connected to the server    =");
                Console.WriteLine("================================");
                Console.WriteLine("Waiting for response...");

                messageBytes = new byte[bytesize]; // Clear the message   

                // Receive the stream of bytes  
                stream.Read(messageBytes, 0, messageBytes.Length);

                // Clean up  
                stream.Dispose();
                client.Close();
            }
            catch (Exception e) // Catch exceptions  
            {
                Console.WriteLine(e.Message);
            }

            return messageBytes; // Return response  
        }
        string startservice()
        {


            listener.Start();

            Console.WriteLine(@"  
            ===================================================  
                   Started listening requests at: {0}:{1}  
            ===================================================",
            ep.Address, ep.Port);

            // Run the loop continuously; this is the server.  
            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                // Read the message and perform different actions  
                message = cleanMessage(buffer);

                // Save the data sent by the client;  
               // 

                byte[] bytes = System.Text.Encoding.Unicode.GetBytes("Thank you for your message, ");
                sender.GetStream().Write(bytes, 0, bytes.Length); // Send the response  



                return message;

            }
           

        }
        private static string cleanMessage(byte[] bytes)
        {
            // Get the string of the message from bytes  
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            // Loop through each character in that message  
            foreach (var nullChar in message)
            {
                // Only store the characters, that are not null character  
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }

            // Return the message without null characters.   
            return messageToPrint;
        }
        static void Main(string[] args)
            {
                Person person = new Person();

                Console.Write("Enter your Fname: ");
                person.Fname = Console.ReadLine();
                Console.Write("Enter your Lname: ");
                person.Lname = Console.ReadLine();
                Console.Write("Enter your message :");
                person.message = Console.ReadLine();

                person.FullName = person.Fname + person.Lname;

                // Send the message  
                byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(person)));
            
            }
        

        }
    }
