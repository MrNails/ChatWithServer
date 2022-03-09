using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.ConsoleUI
{
    public static class Program
    {
        private static TcpClient _client = new TcpClient();
        private static bool _exit = false;
        
        public static async Task Main(string[] args)
        {
            Task.Run(ListenChannel);

            Console.WriteLine("Input your name: ");
            var name = Console.ReadLine();
            // var registrInfo = new RegistrationInfo(name);
            
            
            Console.WriteLine("User id: ");
            var id = int.Parse(Console.ReadLine());

            int count = 0;

            // await Sender.SendObjectAsync(_client, registrInfo, RegistrationInfoSerializator.Serializator);

            while (!_exit)
            {
                var text = Console.ReadLine();
                var message = new Message(count, id, text, DateTime.Now, 0, MessageType.Text);

                //TODO: Complete response handling.  Implement join to chat. Implement id return after create user and chat. 
                // Sender.SendObjectAsync(_client, );

                count++;
            }

            Console.WriteLine("\nEnter key to continue...");
            Console.ReadKey();
        }

        private static void ListenChannel()
        {
            NetworkStream stream = null;
            BinaryReader reader = null;
            BinaryReader dataReader = null;
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                stream = _client.GetStream();
                reader = new BinaryReader(stream);

                dataReader = new BinaryReader(memoryStream);

                byte[] endData = new byte[64];

                while (true)
                {
                    if (_exit)
                        break;

                    var transmitObject = TransmitObjectSerializator.Serializator.Deserialize(reader);
                    memoryStream.Write(transmitObject.Content, 0, transmitObject.ContentSize);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    HandleReceivedData(transmitObject, dataReader);

                    memoryStream.SetLength(0);

                    do
                    {
                        stream.Read(endData, 0, endData.Length);
                    } while (stream.DataAvailable);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Serilog.Log.Logger.Error(e);
                throw;
            }
            finally
            {
                _client?.Close();
                stream?.Close();
                reader?.Close();
                memoryStream.Close();
            }

            _exit = true;
        }
        
        private static void HandleReceivedData(TransmitObject obj, BinaryReader dataReader)
        {
            switch (obj.TransmitType)
            {
                case "SimpleChatServer.Core.Models.Message":
                    var message = MessageSerializator.Serializator.Deserialize(dataReader);

                    Console.WriteLine(
                        $"[{message.SendDate.ToString(CultureInfo.InvariantCulture)}] {message.Id}: {message.Content}");
                    break;
            }
        }
    }
}