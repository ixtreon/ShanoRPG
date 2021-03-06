﻿using Shanism.Engine.Entities;
using Shanism.Engine.Players;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Shanism.Common;
using Shanism.Engine.Objects.Orders;

namespace Shanism.Engine
{
    /// <summary>
    /// Represents a player connected to the game
    /// <para/>
    /// There are 2 default players for all NPC characters, 
    /// see <see cref="Aggressive"/> and <see cref="Friendly"/>. 
    /// </summary>
    public class Player : IPlayer
    {

        #region Static Members

        /// <summary>
        /// The default enemy player. Attacks human 
        /// and <see cref="Friendly"/> players on sight. 
        /// </summary>
        public static Player Aggressive { get; } = new Player("Aggressive");

        /// <summary>
        /// The default friendly player. 
        /// It's currently just chilling, although it should 
        /// probably attack <see cref="Aggressive"/>.
        /// </summary>
        public static Player Friendly { get; } = new Player("Friendly");


        static Player[] defaultPlayers = { Aggressive, Friendly };

        public static bool TryParse(string name, out Player pl)
        {
            for (int i = 0; i < defaultPlayers.Length; i++)
                if (defaultPlayers[i].Name.Equals(name))
                {
                    pl = defaultPlayers[i];
                    return true;
                }

            pl = null;
            return false;
        }

        #endregion


        internal readonly HashSet<Unit> controlledUnits = new HashSet<Unit>();

        internal readonly HashSet<Entity> visibleEntities = new HashSet<Entity>();


        /// <summary>
        /// Gets the receptor of a human-controlled player. 
        /// Returns null if this is a non-human player. 
        /// </summary>
        internal ShanoReceptor Receptor { get; }

        /// <summary>
        /// Gets the identifier of this player. 
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name { get; }


        #region Events
        /// <summary>
        /// The event raised whenever this player's main hero changes. 
        /// </summary>
        public event Action<Hero> MainHeroChanged;

        /// <summary>
        /// The event raised whenever this player sees an object. 
        /// </summary>
        public event Action<Entity> ObjectSeen;

        /// <summary>
        /// The event raised whenever an object stops being visible to this player. 
        /// </summary>
        public event Action<Entity> ObjectUnseen;
        #endregion


        #region Property Shortcuts

        /// <summary>
        /// Gets the player's hero, if they have one. 
        /// </summary>
        public Hero MainHero { get; private set; }

        /// <summary>
        /// Gets the player's alliance, if they have one.
        /// </summary>
        public Alliance Alliance { get; internal set; }

        /// <summary>
        /// Gets all entities visible by this player. 
        /// </summary>
        public IEnumerable<Entity> VisibleObjects => visibleEntities;

        /// <summary>
        /// Gets whether the player has a hero. 
        /// </summary>
        public bool HasHero => (MainHero != null);

        /// <summary>
        /// Gets whether this player is the neutral aggressive player (see <see cref="Aggressive"/>). 
        /// </summary>
        public bool IsNeutralAggressive => (this == Aggressive);

        /// <summary>
        /// Gets whether this player is the neutral friendly player (see <see cref="Friendly"/>). 
        /// </summary>
        public bool IsNeutralFriendly => (this == Friendly);

        /// <summary>
        /// Gets whether this player is an actual human player. 
        /// </summary>
        public bool IsHuman => (Receptor != null);
        #endregion



        #region Constructors
        /// <summary>
        /// Creates a new human player from the given receptor. 
        /// </summary>
        /// <param name="receptor"></param>
        /// <param name="name"></param>
        internal Player(ShanoReceptor receptor, string name) 
            : this(name)
        {
            if (receptor == null) throw new ArgumentNullException(nameof(receptor));

            Receptor = receptor;

        }

        /// <summary>
        /// Creates a new computer player with the given name. 
        /// </summary>
        Player(string name)
        {
            Id = GenericId<Player>.GetNew();
            Name = name;
        }
        #endregion


        /// <summary>
        /// Gets whether the given player is an enemy of this player. 
        /// Currently all players are friends. 
        /// </summary>
        public bool IsEnemyOf(Player p)
        {
            if (p.Id == Id)
                return false;

            var oneIsPlayer = (p.IsHuman || this.IsHuman);
            var bothArePlayers = (p.IsHuman && this.IsHuman);

            var oneIsFriendly = (p.IsNeutralFriendly || this.IsNeutralFriendly);

            var oneIsAggressive = (p.IsNeutralAggressive || this.IsNeutralAggressive);
            var bothAreAggressive = (p.IsNeutralAggressive && this.IsNeutralAggressive);

            return (oneIsPlayer && oneIsAggressive)
                || bothAreAggressive
                || (oneIsAggressive && oneIsFriendly)
                || bothArePlayers;
        }

        /// <summary>
        /// Gets whether the given unit is an enemy of this player. 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsEnemyOf(Unit u) => IsEnemyOf(u.Owner);

        /// <summary>
        /// Sets the main hero of a player. 
        /// Returns false if the player already has a hero.
        /// </summary>
        public bool TrySetMainHero(Hero h)
        {
            if (MainHero == h)
                return true;

            if (HasHero)    //todo: handle somehow
                return false;

            //add hero to the map
            if (h.Map.GetByGuid(h.Id) == null)
                h.Map.Add(h);

            MainHero = h;
            h.DefaultOrder = new MoveDirection(h);
            MainHeroChanged?.Invoke(h);
            return true;
        }

        /// <summary>
        /// Adds a unit owned by this player to the player's list. 
        /// </summary>
        /// <param name="unit"></param>
        internal bool AddControlledUnit(Unit unit)
        {
            var success = controlledUnits.Add(unit);
            Debug.Assert(success);

            onObjectSeen(unit);
            unit.ObjectSeen += onObjectSeen;
            unit.ObjectUnseen += onObjectUnseen;
            return true;
        }

        internal bool RemoveControlledUnit(Unit unit)
        {
            var success = controlledUnits.Remove(unit);
            Debug.Assert(success);

            onObjectUnseen(unit);
            unit.ObjectSeen -= onObjectSeen;
            unit.ObjectUnseen -= onObjectUnseen;
            return true;
        }


        //fired whenever an owned unit sees an object. 
        void onObjectSeen(Entity obj)
        {
            if (!visibleEntities.Add(obj))
                return;

            ObjectSeen?.Invoke(obj);
        }

        void onObjectUnseen(Entity obj)
        {
            //if noone else can see this unit, remove it
            if (!shouldUnsee(obj))
                return;

            if (!visibleEntities.Remove(obj))
                return;

            ObjectUnseen?.Invoke(obj);
        }

        //gets whether any of our units see the entity
        //using its 'visibleFromUnits'
        bool shouldUnsee(Entity e)
        {
            foreach (var u in e.visibleFromUnits)
                if (u.Owner == this)
                    return false;
            return true;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this player. 
        /// Something to do with his <see cref="Name"/>, surely.
        /// </summary>
        public override string ToString() => Name;
    }
}
