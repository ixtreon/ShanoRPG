﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    public interface IPlayer
    {
        /// <summary>
        /// Gets whether this player is the Neutral Aggressive player. 
        /// </summary>
        bool IsNeutralAggressive { get; }

        /// <summary>
        /// Gets whether this player is the Neutral Friendly player. 
        /// </summary>
        bool IsNeutralFriendly { get; }

        /// <summary>
        /// Gets whether this player is human. 
        /// </summary>
        bool IsHuman { get; }

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        string Name { get; }
    }
}
