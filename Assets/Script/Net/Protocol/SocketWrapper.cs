using Cubecraft.Net.Crypto;
using System;
using System.IO;
using System.Net.Sockets;

namespace Cubecraft.Net.Protocol
{
    /// <summary>
    /// Wrapper for handling unencrypted & encrypted socket
    /// </summary>
    class SocketWrapper
    {
        TcpClient c;
        Stream s;
        bool encrypted = false;

        /// <summary>
        /// Initialize a new SocketWrapper
        /// </summary>
        /// <param name="client">TcpClient connected to the server</param>
        public SocketWrapper(TcpClient client)
        {
            this.c = client;
            this.s = client.GetStream();
        }
        public SocketWrapper(string host, int port) : this(new TcpClient(host, port) { ReceiveBufferSize = 1024 * 1024 }) { }

        /// <summary>
        /// Check if the socket is still connected
        /// </summary>
        /// <returns>TRUE if still connected</returns>
        /// <remarks>Silently dropped connection can only be detected by attempting to read/write data</r emarks>
        public bool IsConnected()
        {
            return c.Client != null && c.Connected;
        }

        /// <summary>
        /// Check if the socket has data available to read
        /// </summary>
        /// <returns>TRUE if data is available to read</returns>
        public bool HasDataAvailable()
        {
            return c.Client.Available > 0;
        }

        /// <summary>
        /// Switch network reading/writing to an encrypted stream
        /// </summary>
        /// <param name="secretKey">AES secret key</param>
        public void SwitchToEncrypted(byte[] secretKey)
        {
            if (encrypted)
                throw new InvalidOperationException("Stream is already encrypted!?");
            this.s = CryptoHandler.getAesStream(c.GetStream(), secretKey);
            this.encrypted = true;
        }

        /// <summary>
        /// Network reading method. Read bytes from the socket or encrypted socket.
        /// </summary>
        private void Receive(byte[] buffer, int start, int offset)
        {
            int read = 0;
            while (read < offset)
            {
                read += s.Read(buffer, start + read, offset - read);
            }
        }

        /// <summary>
        /// Read some data from the server.
        /// </summary>
        /// <param name="length">Amount of bytes to read</param>
        /// <returns>The data read from the network as an array</returns>
        public byte[] ReadDataRAW(int length)
        {
            if (length > 0)
            {
                byte[] cache = new byte[length];
                Receive(cache, 0, length);
                return cache;
            }
            return new byte[] { };
        }
        /// <summary>
        /// Send raw data to the server.
        /// </summary>
        /// <param name="buffer">data to send</param>
        public void SendDataRAW(byte[] buffer)
        {
            s.Write(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Get the Stream
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            return this.s;
        }
        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            try
            {
                c.Dispose();
                c = null;
            }
            catch (SocketException) { }
            catch (IOException) { }
            catch (NullReferenceException) { }
            catch (ObjectDisposedException) { }
        }
    }
}
