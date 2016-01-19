﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Assets
{
    /// <summary>
    /// Contains a listing of some common fonts. 
    /// </summary>
    class FontCache
    {

        /// <summary>
        /// A fancy-looking, large font. 
        /// </summary>
        public TextureFont FancyFont { get; private set; }

        /// <summary>
        /// A normal-sized standard font. 
        /// </summary>
        public TextureFont NormalFont { get; private set; }

        /// <summary>
        /// A small-sized standard font. 
        /// </summary>
        public TextureFont SmallFont { get; private set; }

        /// <summary>
        /// A large-sized standard font. 
        /// </summary>
        public TextureFont LargeFont { get; private set; }

        /// <summary>
        /// Loads all fonts to memory using the provided <see cref="ContentManager"/>. 
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            content.RootDirectory = @"Content\";
            //load default fonts
            FancyFont = new TextureFont(content, "Fonts\\UI", 0.5, 4);
            NormalFont = new TextureFont(content, "Fonts\\ui-text", 0.5);
            SmallFont = new TextureFont(NormalFont, 0.8);
            LargeFont = new TextureFont(NormalFont, 1.6);
        }
    }
}
