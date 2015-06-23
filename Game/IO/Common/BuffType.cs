﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    public enum BuffType
    {
        Aura,

        /// <summary>
        /// A buff that does not stack. 
        /// </summary>
        NonStacking,

        /// <summary>
        /// A buff that has a number of stacks that expire independently. 
        /// </summary>
        StackingNormal,

        /// <summary>
        /// A buff that has stacks with a shared duration. 
        /// </summary>
        StackingRefresh,

    }
}
