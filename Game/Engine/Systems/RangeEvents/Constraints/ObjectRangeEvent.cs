﻿using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace Engine.Systems.RangeEvents
{

    /// <summary>
    /// A range constraint where the origin is a <see cref="GameObject"/>. 
    /// Raises the <see cref="Triggered"/> event whenever an object crosses the threshold distance <see cref="Range"/>. 
    /// </summary>
    public class ObjectRangeEvent : RangeEvent
    {
        /// <summary>
        /// The origin unit. 
        /// </summary>
        public GameObject Origin { get; }

        /// <summary>
        /// Registers a range event between two game objects. 
        /// </summary>
        /// <param name="origin">The triggering unit. </param>
        /// <param name="target">The other unit. </param>
        /// <param name="evType">When to raise the event. </param>
        /// <param name="range">At what distance to raise the event. </param>
        public ObjectRangeEvent(GameObject origin, GameObject target, double range)
            : base(range, target)
        {
            Origin = origin;
        }

        /// <summary>
        /// Registers a range event between a game object and all other game objects. 
        /// </summary>
        /// <param name="origin">The triggering unit. </param>
        /// <param name="evType">When to raise the event. </param>
        /// <param name="range">At what distance to raise the event. </param>
        public ObjectRangeEvent(GameObject origin, double range)
            : base(range)
        {
            Origin = origin;
        }

        protected override bool DoCheck(GameObject target, double newDistSq, double oldDistSq)
        {
            return (newDistSq < RangeSquared) 
                && (oldDistSq >= RangeSquared || double.IsNaN(oldDistSq));
        }
    }
    
}