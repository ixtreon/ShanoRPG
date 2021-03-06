﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network
{
    /// <summary>
    /// The base class for all participants in the networking stuff.
    /// </summary>
    public abstract class NPeer
    {
        public const string AppIdentifier = "ShanoRpg";
        public const int DefaultPort = 6969;

        internal NetPeer peer;

        protected NPeer(NetPeer peer)
        {
            this.peer = peer;
            peer.Configuration.ConnectionTimeout = 6000000;

            //start the client
            peer.Start();
        }

        /// <summary>
        /// Reads incoming messages. To be called continuously. 
        /// </summary>
        public virtual void Update(int msElapsed)
        {
            NetIncomingMessage msg;
            while ((msg = peer.ReadMessage()) != null)
            {
                HandleIncomingMessage(msg);
                peer.Recycle(msg);
            }
        }

        internal void HandleIncomingMessage(NetIncomingMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                    NetLog.Default.Debug(msg.ReadString());
                    break;
                case NetIncomingMessageType.WarningMessage:
                    NetLog.Default.Warning(msg.ReadString());
                    break;
                case NetIncomingMessageType.ErrorMessage:
                    NetLog.Default.Error(msg.ReadString());
                    break;

                // data messages
                case NetIncomingMessageType.Data:
                    ReadMessage(msg);
                    break;

                // client connect / disconnect
                case NetIncomingMessageType.StatusChanged:
                    var status = (NetConnectionStatus)msg.ReadByte();

                    if (status == NetConnectionStatus.Connected)
                        OnConnected(msg.SenderConnection);
                    else if (status == NetConnectionStatus.Disconnected)
                        OnDisconnected(msg.SenderConnection);

                    NetLog.Default.Info($"{status}: {msg.ReadString()}");
                    break;

                default:
                    NetLog.Default.Warning($"Unhandled message type: {msg.MessageType} ({msg.LengthBytes} bytes)");
                    break;
            }
        }

        internal abstract void ReadMessage(NetIncomingMessage msg);

        internal virtual void OnConnected(NetConnection conn) { }
        internal virtual void OnDisconnected(NetConnection conn) { }
    }
}
