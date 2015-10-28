﻿using IxSerializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    [SerialKiller]
    public struct Point
    {
        public static readonly Point Zero = new Point();

        [SerialMember]
        public int X;
        [SerialMember]
        public int Y;

        public Point(int v)
        {
            X = Y = v;
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public IEnumerable<Point> IterateToInclusive(Point b)
        {
            for (int ix = X; ix <= b.X; ix++)
                for (int iy = Y; iy <= b.Y; iy++)
                    yield return new Point(ix, iy);
        }

        #region Point-point operators
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Constrains the first point between the other two. 
        /// </summary>
        public Point ConstrainWithin(Point low, Point high)
        {
            var x = Math.Min(high.X, Math.Max(low.X, X));
            var y = Math.Min(high.Y, Math.Max(low.Y, Y));
            return new Point(x, y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }
        #endregion

        #region Point-int operators
        public static Point operator -(Point a, int other)
        {
            return new Point(a.X - other, a.Y - other);
        }

        public static Point operator *(Point a, int multiplier)
        {
            return new Point(a.X * multiplier, a.Y * multiplier);
        }
        public static Point operator /(Point a, int divisor)
        {
            return new Point(a.X / divisor, a.Y / divisor);
        }

        public static Point operator %(Point a, int modulus)
        {
            return new Point(a.X % modulus, a.Y % modulus);
        }
        #endregion

        #region Point-double operators
        public static Vector operator /(Point a, double divisor)
        {
            return new Vector(a.X / divisor, a.Y / divisor);
        }
        #endregion


        public double DistanceTo(Point other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            var p = (Point)obj;
            return p == this;
        }

        public override string ToString()
        {
            return "[{0}, {1}]".Format(X, Y);
        }

        public override int GetHashCode()
        {
            unchecked       // http://stackoverflow.com/questions/5221396/what-is-an-appropriate-gethashcode-algorithm-for-a-2d-point-struct-avoiding
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public Point Clamp(Point min, Point max)
        {
            return new Point(
                Math.Min(Math.Max(min.X, X), max.X),
                Math.Min(Math.Max(min.Y, Y), max.Y));
        }
    }
}
