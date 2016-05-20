﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Game
{
    /// <summary>
    /// A generic <see cref="IComparer{T}"/> which uses a lambda function to compare elements. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IComparer{T}" />
    public class GenericComparer<T> : IComparer<T>
    {
        readonly Func<T, T, int> compareFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComparer{T}"/> class.
        /// </summary>
        /// <param name="compareFunc">The compare function.</param>
        public GenericComparer(Func<T, T, int> compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(T x, T y)
            => compareFunc(x, y);
    }
}