#nullable enable
using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Services;
using SimpleChatServer.Services.StaticMappableServices;
using Action = SimpleChatServer.Core.Models.Action;

namespace SimpleChatServer
{
    public class ServerClientHandler
    {
        private TcpClient m_client;

        public ServerClientHandler(TcpClient client)
        {
            m_client = client;
        }

        public async void Process()
        {
            NetworkStream stream = null;
            BinaryReader reader = null;
            BinaryReader dataReader = null;
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                stream = m_client.GetStream();
                reader = new BinaryReader(stream);

                dataReader = new BinaryReader(memoryStream);

                byte[] endData = new byte[64];

                while (true)
                {
                    var transmitObject = TransmitObjectSerializator.Serializator.Deserialize(reader);
                    memoryStream.Write(transmitObject.Content, 0, transmitObject.ContentSize);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var response = await HandleReceivedData(transmitObject, dataReader);

                    memoryStream.SetLength(0);

                    do
                    {
                        stream.Read(endData, 0, endData.Length);
                    } while (stream.DataAvailable);

                    if (response != ActionResponse.VoidResponse)
                    {
                        await Sender.SendObjectAsync(m_client, response, ActionResponseSerializator.Serializator, transmitObject.Id);
                    }
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
                m_client?.Close();
                stream?.Close();
                reader?.Close();
                memoryStream.Close();
            }
        }

        private Task<ActionResponse> HandleReceivedData(TransmitObject obj, BinaryReader dataReader)
        {
            switch (obj.TransmitType)
            {
                case "SimpleChatServer.Core.Models.Message":
                    var message = MessageSerializator.Serializator.Deserialize(dataReader);
                    var fromUser = UserManager.GetUserById(message.FromUser);
                    ChatManager.SendMessageAsync(message);

                    Console.WriteLine(
                        $"[{message.SendDate.ToString(CultureInfo.InvariantCulture)}] {fromUser.UserInfo.Name}: {message.Content}");
                    break;
                case "SimpleChatServer.Core.Models.RegistrationInfo":
                    var registrationInfo = RegistrationInfoSerializator.Serializator.Deserialize(dataReader);

                    UserManager.CreateUser(registrationInfo, m_client);
                    break;
                case "SimpleChatServer.Core.Models.Action":
                    var action = ActionSerializator.Serializator.Deserialize(dataReader);
                    
                    return ActionHandler.PerformAction(action);
                default:
                    break;
            }

            return Task.FromResult(ActionResponse.VoidResponse);
        }
    }
}