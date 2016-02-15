﻿using IO.Common;
using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Objects
{
    public interface IUnit : IGameObject
    {

        /// <summary>
        /// Gets the owner of the unit. 
        /// </summary>
        //IPlayer Owner { get; }

        #region Movement
        /// <summary>
        /// Gets whether the unit is currently moving. 
        /// </summary>
        bool IsMoving { get; }

        /// <summary>
        /// Gets the direction in which this unit is moving, if <see cref="IsMoving"/> is true. 
        /// </summary>
        double MoveDirection { get; }

        /// <summary>
        /// Gets the movement speed of this unit. 
        /// </summary>
        double MoveSpeed { get; }
        #endregion

        #region Combat
        /// <summary>
        /// Gets the attack cooldown of this unit. 
        /// </summary>
        int AttackCooldown { get; }

        /// <summary>
        /// Gets whether this unit is invulnerable. 
        /// </summary>
        bool Invulnerable { get; }

        /// <summary>
        /// Gets the base defense of the unit. 
        /// </summary>
        double BaseDefense { get; }

        /// <summary>
        /// Gets the defense of this unit. 
        /// </summary>
        double Defense { get; }

        /// <summary>
        /// Gets the minimum damage dealt by this unit. 
        /// </summary>
        double MinDamage { get; }

        /// <summary>
        /// Gets the maximum damage dealt by this unit. 
        /// </summary>
        double MaxDamage { get; }

        /// <summary>
        /// Gets the bonus magic damage this unit deals. 
        /// </summary>
        double MagicDamage { get; }
        #endregion

        #region Life & Mana
        /// <summary>
        /// Gets the current life of this unit. 
        /// </summary>
        double Life { get; }

        /// <summary>
        /// Gets the maximum life of this unit. 
        /// </summary>
        double MaxLife { get; }

        /// <summary>
        /// Gets the life regeneration of this unit in points per seecond. 
        /// </summary>
        double LifeRegen { get; }


        /// <summary>
        /// Gets the current mana of this unit. 
        /// </summary>
        double Mana { get; }

        /// <summary>
        /// Gets the maximum mana of this unit. 
        /// </summary>
        double MaxMana { get; }

        /// <summary>
        /// Gets the mana regeneration of this unit in points per seecond. 
        /// </summary>
        double ManaRegen { get; }

        #endregion


        /// <summary>
        /// Gets the level of the unit. 
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets whether this unit is dead. 
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Gets the vision range of this unit. 
        /// </summary>
        double VisionRange { get; }


        /// <summary>
        /// Gets the progress of the ability the unit is currently casting, if <see cref="CastingAbility"/> is not null.  
        /// </summary>
        int CastingProgress { get; }

        /// <summary>
        /// Gets the progress of the ability the unit is currently casting, if <see cref="CastingAbility"/> is not null.  
        /// </summary>
        int TotalCastingTime { get; }


        /// <summary>
        /// Gets the collection of buffs currently affecting the unit. 
        /// </summary>
        IEnumerable<IBuffInstance> Buffs { get; }

        /// <summary>
        /// Gets the collection of abilities this unit has. 
        /// </summary>
        IEnumerable<IAbility> Abilities { get; }

        /// <summary>
        /// Gets the type of this unit's current order. 
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        /// Gets the unit state flags that currently affect this unit. 
        /// </summary>
        UnitFlags StateFlags { get; }

        ///// <summary>
        ///// Gets the ability this unit is currently casting or null otherwise. 
        ///// </summary>
        //IAbility CastingAbility { get; }
    }
}
