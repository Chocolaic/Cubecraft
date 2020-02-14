using Cubecraft.Net.Protocol.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Cubecraft.Net.Protocol
{
    class Protocol : IDisposable
    {

        internal const int MC112Version = 335;
        internal const int MC1121Version = 338;
        internal const int MC1122Version = 340;

        private IDictionary<int, Type> incoming = new Dictionary<int, Type>();
        private IDictionary<Type, int> outgoing = new Dictionary<Type, int>();

        protected BlockingCollection<Packet> outgoingQueue = new BlockingCollection<Packet>(new ConcurrentQueue<Packet>());
        protected BlockingCollection<Packet> incomingQueue = new BlockingCollection<Packet>(new ConcurrentQueue<Packet>());

        protected SocketWrapper socketWrapper;
        InputBuffer reader;
        OutputBuffer writer;

        int compression_treshold = 0;

        protected Thread netRead, netSend;
        public int ProtocolVersion { get; private set; }

        public Protocol(SocketWrapper socket, int protocol)
        {
            this.socketWrapper = socket;
            this.reader = new InputBuffer(socketWrapper.GetStream());
            this.writer = new OutputBuffer(socketWrapper.GetStream());
            this.ProtocolVersion = protocol;
        }

        public void RegisterIncoming<T>(int id) where T : Packet
        {
            this.incoming.Add(id, typeof(T));
        }
        public void RegisterOutgoing<T>(int id) where T : Packet
        {
            this.outgoing.Add(typeof(T), id);
        }
        public void ClearPackets()
        {
            this.incoming.Clear();
            this.outgoing.Clear();
        }
        public Packet CreateIncomingPacket(int id)
        {
            if (!this.incoming.ContainsKey(id))
                return null;
            return Activator.CreateInstance(this.incoming[id]) as Packet;
        }
        public int GetOutgoingID(Packet packet)
        {
            if (!this.outgoing.ContainsKey(packet.GetType()))
                throw new KeyNotFoundException();
            return this.outgoing[packet.GetType()];
        }
        public void SetCompress(int treshold)
        {
            this.compression_treshold = treshold;
        }
        protected void SendPacket(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                OutputBuffer outgoing = new OutputBuffer(stream);
                outgoing.WriteVarInt(GetOutgoingID(packet));
                packet.Write(outgoing);
                if (compression_treshold > 0)
                {
                    var content = stream.ToArray();
                    outgoing = new OutputBuffer();
                    if (content.Length >= compression_treshold)
                    {
                        byte[] compressed_packet = ZlibUtils.Compress(content, (MemoryStream)outgoing.GetStream());
                        outgoing.WriteVarInt(compressed_packet.Length);
                        outgoing.WriteData(compressed_packet);
                    }
                    else
                    {
                        outgoing.WriteVarInt(0);
                        outgoing.WriteData(content);
                    }
                }
                writer.WriteVarInt((int)outgoing.GetStream().Length);
                writer.WriteData(outgoing.ToArray());
                outgoing.Dispose();
            }
        }
        protected Packet ReadPacket()
        {
            int size = reader.ReadVarInt();
            Packet packet = null;
            using (MemoryStream stream = new MemoryStream(reader.ReadData(size), false))
            {
                InputBuffer input = new InputBuffer(stream);
                if (ProtocolVersion >= 47 && compression_treshold > 0)
                {
                    int sizeUncompressed = input.ReadVarInt();
                    if (sizeUncompressed != 0)
                    {
                        input = new InputBuffer(ZlibUtils.Decompress(stream, sizeUncompressed));
                    }
                }
                int packetID = input.ReadVarInt();
                packet = CreateIncomingPacket(packetID);
                if (packet != null)
                    packet.Read(input);
                input.Dispose();
            }
            return packet;
        }

        public void Dispose()
        {
            if (netRead != null)
                netRead.Abort();
            if (netSend != null)
                netSend.Abort();
            socketWrapper.Disconnect();
            ClearPackets();
        }
    }
}
