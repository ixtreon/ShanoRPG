﻿using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Content;
using IO.Objects;

namespace Network.Objects
{
    /// <summary>
    /// Represents an abstract empty game object as reconstructed by a network client. 
    /// </summary>
    abstract class ObjectStub : IGameObject
    {
        //public readonly int Guid;

        public Vector Position { get; internal set; }
        
        public string Name { get; internal set; }

        public double Scale { get; internal set; }

        public uint Guid { get; internal set; }

        public ObjectType ObjectType { get; internal set; }

        public IEnumerable<IUnit> SeenBy { get; internal set; }

        public string ModelName { get; internal set; }

        public string AnimationName { get; internal set; }

        public RectangleF Bounds { get; internal set; }

        public RectangleF TextureBounds { get; internal set; }

        public ObjectStub(uint guid)
        {
            this.Guid = guid;
        }
    }
}
