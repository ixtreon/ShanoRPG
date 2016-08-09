﻿using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects.Orders;
using Shanism.Engine.Objects.Abilities;

namespace Shanism.Engine.Entities
{
    //The part of the unit class which deals with orders such as moving and attacking. 
    partial class Unit
    {
        public Order DefaultOrder { get; set; }

        /// <summary>
        /// Gets or sets the current order of this unit. 
        /// </summary>
        public Order CurrentOrder { get; set; }


        /// <summary>
        /// The event executed whenever the unit's order is changed. 
        /// </summary>
        public event Action<Order> OrderChanged;


        #region Property Shortcuts


        public MovementState MovementState { get; internal set; }

        #endregion

        #region Casting

        /// <summary>
        /// Tries to cast the given ability on the provided target entity. 
        /// </summary>
        /// <param name="ability">The ability to cast.</param>
        /// <param name="target">The targeted entity.</param>
        /// <returns>Whether the unit began casting the ability.</returns>
        public bool TryCastAbility(Ability ability, Entity target)
            => abilities.BeginCasting(ability, target);

        /// <summary>
        /// Tries to cast the given ability on the provided target location. 
        /// </summary>
        /// <param name="ability">The ability to cast.</param>
        /// <param name="location">The target location.</param>
        /// <returns>Whether the unit began casting the ability.</returns>
        public bool TryCastAbility(Ability ability, Vector location)
            => abilities.BeginCasting(ability, location);

        /// <summary>
        /// Tries to cast the given ability without a target. 
        /// </summary>
        /// <param name="ability">The ability to cast.</param>
        /// <returns>Whether the unit began casting the ability.</returns>
        public bool TryCastAbility(Ability ability)
            => abilities.BeginCasting(ability);


        internal bool TryCastAbility(ClientState state)
        {
            if (state.ActionId == 0) return false;

            var ability = abilities.TryGet(state.ActionId);
            if (ability == null) return false;

            Entity targetEntity;

            switch(ability.TargetType)
            {
                case AbilityTargetType.Passive:
                    return false;

                case AbilityTargetType.NoTarget:
                    return TryCastAbility(ability);

                case AbilityTargetType.PointTarget:
                    return TryCastAbility(ability, state.ActionTargetLoc);

                case AbilityTargetType.UnitTarget:
                    targetEntity = Map.GetByGuid(state.ActionTargetId);
                    if (targetEntity != null)
                        return TryCastAbility(ability, targetEntity);

                    return false;

                case AbilityTargetType.PointOrUnitTarget:
                    targetEntity = Map.GetByGuid(state.ActionTargetId);
                    if (targetEntity != null)
                        return TryCastAbility(ability, targetEntity);

                    return TryCastAbility(ability, state.ActionTargetLoc);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Custom Orders

        internal void SetOrder(Order newState)
        {
            CurrentOrder = newState;

            //raise the order changed event
            OrderChanged?.Invoke(CurrentOrder);
        }

        /// <summary>
        /// Clears the current order of the unit. 
        /// </summary>
        public void ClearOrder() => SetOrder(null);

        /// <summary>
        /// Orders the unit to start moving towards the target in-game position. 
        /// </summary>
        public void OrderMove(Vector target) => SetOrder(new MoveToGround(this, target));

        /// <summary>
        /// Orders the unit to start moving in the specified direction. 
        /// </summary>
        public void OrderMove(float direction) => SetOrder(new MoveDirection(this, direction));

        /// <summary>
        /// Orders the unit to go to the target unit. 
        /// Similar to <see cref="OrderMove(Vector)"/> but makes sure the target is reached
        /// even if it moves after the order was issued. 
        /// </summary>
        public void OrderMove(Unit target) => SetOrder(new MoveToUnit(this, target));


        public void OrderFollow(Unit target) => SetOrder(new FollowUnit(this, target));

        public void OrderAttack(Unit target)
        {
            if (!Owner.IsEnemyOf(target))
                return;

            throw new NotImplementedException();
        }

        public void OrderPatrol(Vector target)
        {
            throw new NotImplementedException();
        }

        public void OrderMoveAttack(Vector target)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
