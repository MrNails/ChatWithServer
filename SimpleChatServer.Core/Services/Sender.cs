using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SimpleChatServer.Core.SerializationResolvers;

namespace SimpleChatServer.Core.Services
{
    public static class Sender
    {
        /// <summary>
        /// Class with streams for send data for client.
        /// </summary>
        private class ObjectSendData
        {
            public ObjectSendData(MemoryStream dataStream, MemoryStream transmitObjStream, 
                                  BinaryWriter dataWriter, BinaryWriter transmitObjWriter)
            {
                DataStream = dataStream;
                TransmitObjStream = transmitObjStream;
                DataWriter = dataWriter;
                TransmitObjWriter = transmitObjWriter;
            }
            
            public MemoryStream DataStream { get; }
            public MemoryStream TransmitObjStream { get; }
            public BinaryWriter DataWriter { get; }
            public BinaryWriter TransmitObjWriter { get; }
        }

        private static readonly Dictionary<TcpClient, ObjectSendData> _objectSendDatas = new  Dictionary<TcpClient, ObjectSendData>();

        public static async Task<Guid> SendObjectAsync<T>(TcpClient client, T data, ISerializator<T> resolver)
        {
            ObjectSendData sendData = null;
            
            if (_objectSendDatas.TryGetValue(client, out sendData))
            {
                var tempDataStream = new MemoryStream();
                var tempTransmitObjStream = new MemoryStream();
                var tempDataWriter = new BinaryWriter(tempDataStream);
                var tempTransmitObjWriter = new BinaryWriter(tempTransmitObjStream);

                sendData = new ObjectSendData(tempDataStream, tempTransmitObjStream, 
                    tempDataWriter, tempTransmitObjWriter);
                
                _objectSendDatas.Add(client, sendData);
            }
            
            var dataStream = sendData.DataStream;
            var transmitObjStream = sendData.TransmitObjStream;
            var dataWriter = sendData.DataWriter;
            var transmitObjWriter = sendData.TransmitObjWriter;
            var msgId = Guid.NewGuid();

            try
            {
                resolver.Serialize(dataWriter, data);

                var transmitObj = new TransmitObject(typeof(T).FullName!, dataStream.GetBuffer(), (int)dataStream.Length, msgId);
                
                TransmitObjectSerializator.Serializator.Serialize(transmitObjWriter, transmitObj);

                // Console.WriteLine($"Sent {transmitObjStream.Length.ToString()} bytes of data.");

                await client.GetStream().WriteAsync(transmitObjStream.GetBuffer().AsMemory(0, (int)transmitObjStream.Length));

                return msgId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Serilog.Log.Logger.Error(e);
                throw;
            }
            finally
            {
                dataStream.SetLength(0);
                transmitObjStream.SetLength(0);
            }
        }
        
        public static Task SendObjectAsync<T>(TcpClient client, T data, ISerializator<T> resolver, Guid msgId)
        {
            ObjectSendData sendData = null;
            
            if (_objectSendDatas.TryGetValue(client, out sendData))
            {
                var tempDataStream = new MemoryStream();
                var tempTransmitObjStream = new MemoryStream();
                var tempDataWriter = new BinaryWriter(tempDataStream);
                var tempTransmitObjWriter = new BinaryWriter(tempTransmitObjStream);

                sendData = new ObjectSendData(tempDataStream, tempTransmitObjStream, 
                    tempDataWriter, tempTransmitObjWriter);
                
                _objectSendDatas.Add(client, sendData);
            }
            
            var dataStream = sendData.DataStream;
            var transmitObjStream = sendData.TransmitObjStream;
            var dataWriter = sendData.DataWriter;
            var transmitObjWriter = sendData.TransmitObjWriter;
            try
            {
                resolver.Serialize(dataWriter, data);

                var transmitObj = new TransmitObject(typeof(T).FullName!, dataStream.GetBuffer(), (int)dataStream.Length, msgId);
                
                TransmitObjectSerializator.Serializator.Serialize(transmitObjWriter, transmitObj);

                // Console.WriteLine($"Sent {transmitObjStream.Length.ToString()} bytes of data.");
                
                return client.GetStream().WriteAsync(transmitObjStream.GetBuffer(), 0, (int)transmitObjStream.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Serilog.Log.Logger.Error(e);
                throw;
            }
            finally
            {
                dataStream.SetLength(0);
                transmitObjStream.SetLength(0);
            }
        }
    }
}