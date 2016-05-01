﻿using Engine.Objects;
using Engine.Systems.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems.Behaviours
{
    /// <summary>
    /// A behaviour that makes the controlled unit follow the specified target unit. 
    /// </summary>
    class FollowBehaviour : Behaviour
    {
        public Unit Target { get; set; }

        public double Distance { get; set; }

        public FollowBehaviour(Behaviour b) : base(b)
        {
        }

        public override bool TakeControl(int msElapsed)
        {
            if (Target == null)
                return false;

            return Owner.Position.DistanceTo(Target.Position) > Distance;
        }

        public override void Update(int msElapsed)
        {
            CurrentOrder = new MoveUnit(Target, Distance, true);
        }

        public override string ToString() => $"Following {Target}";
    }
}