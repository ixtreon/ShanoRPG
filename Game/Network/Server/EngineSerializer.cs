﻿using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Network;
using Shanism.Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shanism.Network.Server
{
    public class EngineSerializer
    {
        GameSerializer serializer = new GameSerializer();


        public GameFrameMessage WriteServerFrame(IReceptor receptor)
        {
            var visibleObjects = new HashSet<ObjectData>();
            foreach (var e in receptor.VisibleEntities)
                FetchObjectIds(receptor.Id, e, visibleObjects);

            using (var ms = new MemoryStream())
            {
                //write # items followed by the items
                using (var wr = new BinaryWriter(ms))
                {
                    wr.Write(visibleObjects.Count);

                    foreach (var obj in visibleObjects)
                        serializer.Write(wr, obj.Object, obj.Type);
                }

                var bytes = ms.ToArray();
                return new GameFrameMessage(bytes);
            }
        }

        public bool TryReadClientFrame(GameFrameMessage msg, 
            out uint clientFrameId,
            out ClientState state)
        {
            try
            {
                ClientState state2;
                using (var ms = new MemoryStream(msg.Data))
                {
                    clientFrameId = ms.ReadUint24();
                    state2 = ProtoBuf.Serializer.Deserialize<ClientState>(ms);
                }

                //var state3 = new ClientState();
                //using (var ms = new MemoryStream(msg.Data))
                //{
                //    ProtoBuf.Serializer.Merge(ms, state3);
                //}

                clientFrameId = 0;
                state = state2;
                return true;
            }
            catch
            {
                clientFrameId = 0;
                state = null;
                return false;
            }
        }

        /// <summary>
        /// Appends all objects related to the given entity <paramref name="nearbyEntity"/> 
        /// as seen by the player with id <paramref name="playerId"/> 
        /// into the collection <paramref name="c"/>. 
        /// </summary>
        static void FetchObjectIds(uint playerId, IEntity nearbyEntity, 
            ICollection<ObjectData> c)
        {
            var writeAsType = nearbyEntity.ObjectType;

            if (nearbyEntity is IHero && ((IHero)nearbyEntity).OwnerId != playerId)
                writeAsType = ObjectType.Unit;

            c.Add(new ObjectData { Object = nearbyEntity, Type = writeAsType });
        }


        struct ObjectData
        {
            public IGameObject Object;

            public ObjectType Type;

            //dreams of FP lands...
            public static bool operator ==(ObjectData a, ObjectData b)
                => a.Object?.Id == b.Object?.Id;

            public static bool operator !=(ObjectData a, ObjectData b)
                => a.Object?.Id != b.Object?.Id;

            public override bool Equals(object obj)
                => (obj is ObjectData) && ((ObjectData)obj) == this;

            public override int GetHashCode() => Object.Id.GetHashCode();
        }

    }
}
