﻿using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Server;

namespace Shanism.Common
{
    /// <summary>
    /// A prpospective client looking to play on the server.
    ///  
    /// It could be either a local or remote (network'd) one. 
    /// </summary>
    public interface IShanoClient
    {
        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The event raised when a client wants to move. 
        /// </summary>
        event Action<MoveMessage> MovementStateChanged;

        /// <summary>
        /// The event raised when a client wants to do stuff. 
        /// </summary>
        event Action<ActionMessage> ActionActivated;

        /// <summary>
        /// The event raised when a client wants to chat. 
        /// </summary>
        event Action<ChatMessage> ChatMessageSent;

        /// <summary>
        /// The event raised when a client wants a map chunk. 
        /// </summary>
        event Action<MapRequestMessage> MapRequested;

        /// <summary>
        /// The event raised when a client asks to start playing. 
        /// </summary>
        event Action HandshakeInit;
    }
}