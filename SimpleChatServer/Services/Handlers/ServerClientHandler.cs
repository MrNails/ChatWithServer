#nullable enable
using System;
using System.IO;
using System.Net.Sockets;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Handlers;
using SimpleChatServer.DAL;

namespace SimpleChatServer.Services.Handlers
{
    public class ServerClientHandler
    {
        private TcpClient m_client;
        // private MasterDataContext m_dbContext;

        public ServerClientHandler(TcpClient client)
        {
            m_client = client;
            // m_dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);
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

                    var response = ActionResponse.VoidResponse;
                    IRequestHandler? requestHandler = null;
                    
                    if (Server.ServerData.Handlers.TryGetValue(transmitObject.TransmitType, out requestHandler))
                        response = await requestHandler.HandleAsync(dataReader, m_client);

                    memoryStream.SetLength(0);

                    do
                    {
                        stream.Read(endData, 0, endData.Length);
                    } while (stream.DataAvailable);

                    if (response != ActionResponse.VoidResponse)
                        await Sender.SendObjectAsync(m_client, response, ActionResponseSerializator.Serializator, transmitObject.Id); 
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
                // m_dbContext.Dispose();
            }
        }
    }
}