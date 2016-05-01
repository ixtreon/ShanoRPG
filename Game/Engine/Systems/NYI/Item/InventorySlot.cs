﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Engine.Systems.Item
{
    /* supports up to 32 (64) slots */

    /// <summary>
    /// Gets the enumeration of viable inventory slots. 
    /// </summary>
    [Flags]
    public enum InventorySlots
    {
        None = 0,

        Head     = 1 << 0,
        Neck     = 1 << 1,
        Shoulder = 1 << 2,
        Torso    = 1 << 3,
        Back     = 1 << 4,
        Legs     = 1 << 5,
        Feet     = 1 << 6,
        Arms     = 1 << 7,
        MainHand = 1 << 8,
        OffHand  = 1 << 9,
        

        All      = (1 << 16) - 1,
    }
}