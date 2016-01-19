﻿using IO;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    /// <summary>
    /// Provides mapping from <see cref="Keys"/> objects to the characters that show on the keyboard. 
    /// </summary>
    static class KeyMap
    {
        static readonly Dictionary<Keys, char> normalCharDict = new Dictionary<Keys, char>();
        static readonly Dictionary<Keys, char> capsCharDict = new Dictionary<Keys, char>();

        /// <summary>
        /// Gets the character that shows when the given <see cref="Keys"/> structure is held down. 
        /// Returns null in case there is no recognized character for this <see cref="Keys"/> instance. 
        /// </summary>
        /// <param name="k">The key whose character is needed. </param>
        /// <param name="isCaps">Whether to use the normal or HIGH caps character. </param>
        public static char? GetChar(Keys k, bool isCaps)
        {
            if (isCaps)
                return capsCharDict.TryGet(k);
            return normalCharDict.TryGet(k);
        }

        static KeyMap()
        {
            addSimpleMap(Keys.Space, ' ');
            addSimpleMap(Keys.Back, '\b');

            addSequentialMap(Keys.A, Keys.Z, 'a', 'A');
            addSequentialMap(Keys.NumPad0, Keys.NumPad9, '0', '0');

            addCustomMap(Keys.D0, Keys.D9, "0123456789", ")!@#$%^&*(");
            addCustomMap(Keys.OemSemicolon, Keys.OemTilde, ";=,-./`", ":+<_>?~");
            addCustomMap(Keys.OemOpenBrackets, Keys.OemQuotes, "[\\]'", "{|}\"");
        }

        static void addSimpleMap(Keys k, char c)
        {
            normalCharDict[k] = c;
            capsCharDict[k] = c;
        }

        static void addSequentialMap(Keys from, Keys to, char cFrom, char cFromCaps)
        {
            for (var i = from; i <= to; i++)
            {
                normalCharDict[i] = (char)(i - from + cFrom);
                capsCharDict[i] = (char)(i - from + cFromCaps);
            }
        }

        static void addCustomMap(Keys from, Keys to, string sNormal, string sCaps)
        {
            Debug.Assert(sNormal.Length == sCaps.Length);

            for (var i = from; i <= to; i++)
            {
                normalCharDict[i] = sNormal[i - from];
                capsCharDict[i] = sCaps[i - from];
            }
        }
    }
}