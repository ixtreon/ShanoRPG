﻿using Engine.Objects;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace Engine.Systems.Buffs
{
    public class BuffInstance : IBuffInstance
    {
        public readonly Buff Buff;

        public Unit Target;

        public int DurationLeft { get; private set; }

        public bool ShouldDestroy
        {
            get { return Buff.IsTimed && DurationLeft <= 0; }
        }

        public BuffInstance(Buff buff, Unit target)
        {
            Buff = buff;
            Target = target;
            DurationLeft = buff.FullDuration;

            Buff.OnApplied(this);
        }

        #region IBuff implementation
        public string Icon { get { return Buff.Icon; } }

        public int MoveSpeed { get { return Buff.MoveSpeed; } }

        public int AttackSpeed { get { return Buff.AttackSpeed; } }

        public double Life { get { return Buff.Life; } }

        public double Mana { get { return Buff.Mana; } }

        public double Defense { get { return Buff.Defense; } }

        public double MinDamage { get { return Buff.MinDamage; } }

        public double MaxDamage { get { return Buff.MaxDamage; } }

        public double Strength { get { return Buff.Strength; } }

        public double Vitality { get { return Buff.Vitality; } }

        public double Agility { get { return Buff.Agility; } }

        public double Intellect { get { return Buff.Intellect; } }

        public int FullDuration { get { return Buff.FullDuration; } }

        public BuffType Type { get { return Buff.Type; } }
        #endregion

        internal void Update(int msElapsed)
        {
            if (Buff.IsTimed)
            {
                DurationLeft -= msElapsed;
                if (ShouldDestroy)
                {
                    Buff.OnExpired(this);
                    Buff.Destroy();
                    return;
                }
            }

            Buff.OnUpdate(this);
        }
    }
}
