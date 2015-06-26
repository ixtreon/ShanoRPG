﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace IO.Common
{
    [ProtoContract]
    public struct MovementState
    {
        public static MovementState Stand = new MovementState(0, 0);

        [ProtoMember(1)]
        private bool isMoving;

        [ProtoMember(2)]
        private double angle;


        public Vector DirectionVector
        {
            get { return Vector.Zero.PolarProjection(angle, 1); }
        }

        public bool IsMoving
        {
            get { return isMoving; }
        }

        public MovementState(int px, int py)
        {
            isMoving = (px != 0 || py != 0);
            angle = Math.Atan2(py, px);
        }

        public static bool operator ==(MovementState a, MovementState b)
        {
            return a.isMoving == b.isMoving && a.angle.AlmostEqualTo(b.angle, 0.0005);
        }

        public static bool operator != (MovementState a, MovementState b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (isMoving ? (2 * Math.PI + angle).GetHashCode() : -1);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MovementState))
                return false;
            return (MovementState)obj == this;
        }
    }
}
