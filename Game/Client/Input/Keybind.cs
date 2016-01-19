﻿using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    /// <summary>
    /// A keybind that consists of a main key and zero or more modifier keys. 
    /// </summary>
    struct Keybind
    {
        public ModifierKeys Modifiers { get; set; }

        public Keys Key { get; set; }

        public Keybind(Keys key)
        {
            Modifiers = ModifierKeys.None;
            Key = key;
        }

        public Keybind(ModifierKeys mod, Keys key)
        {
            Modifiers = mod;
            Key = key;
        }

        public string ToShortString()
        {
            if (Key == Keys.None)
                return "N/A";

            var ans = "";
            if (Modifiers.HasFlag(ModifierKeys.Control))    ans += "C";
            if (Modifiers.HasFlag(ModifierKeys.Alt))        ans += "A";
            if (Modifiers.HasFlag(ModifierKeys.Shift))      ans += "S";
            ans += KeyMap.GetChar(Key, false) ?? '?';
            return ans;
        }

        public static bool operator ==(Keybind a, Keybind b)
        {
            return a.Key == b.Key && a.Modifiers == b.Modifiers;
        }

        public static bool operator !=(Keybind a, Keybind b)
        {
            return !(a == b);
        }

        //a key is always a keybind; the opposite's not always true
        public static implicit operator Keybind(Keys k)
        {
            return new Keybind(k);
        }

        public override string ToString()
        {
            if (Key == Keys.None)
                return "N/A";

            if (Modifiers != ModifierKeys.None)
                return Modifiers.ToString() + "+" + Key.ToString();
            return Key.ToString();
        }

    }
}
