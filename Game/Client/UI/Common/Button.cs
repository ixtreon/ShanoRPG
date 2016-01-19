﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Textures;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;
using Client.Assets;

namespace Client.UI.Common
{
    /// <summary>
    /// A button that shows an image and can be clicked. 
    /// </summary>
    class Button : Control
    {
        bool _isSelected = false;

        /// <summary>
        /// Gets or sets the texture that is drawn stretched on the button. 
        /// </summary>
        public Texture2D Texture { get; set; }


        public event Action<Button> Selected;

        /// <summary>
        /// Gets the texture color (tint). 
        /// </summary>
        public Color TextureColor { get; set; } = Color.White;

        public string Text { get; set; }

        public TextureFont Font { get; set; } = Content.Fonts.NormalFont;

        public Color FontColor { get; set; } = Color.Black;


        public Color BackHoverColor { get; set; }


        /// <summary>
        /// Gets or sets whether this button has a border drawn around it. 
        /// </summary>
        public bool HasBorder { get; set; }

        /// <summary>
        /// Gets or sets whether this button is currently selected. 
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                    select(value);
            }
        }

        /// <summary>
        /// Gets or sets whether this button can be selected. 
        /// </summary>
        public bool CanSelect { get; set; }

        public Button(string text = null, Texture2D texture = null)
        {
            var sz = Font.MeasureStringUi("WOWyglj");
            Size = new Vector(0.05f, sz.Y);

            Text = text;
            Texture = texture;

            BackColor = Color.White.Darken(95);
            BackHoverColor = BackColor.Darken(50);

            MouseDown += Button_MouseDown;
        }

        void Button_MouseDown(Input.MouseButtonEvent obj)
        {
            if (CanSelect && !IsSelected)
                select(true);
        }

        void select(bool val)
        {
            _isSelected = val;
            Selected?.Invoke(this);
        }

        public override void OnDraw(Graphics g)
        {
            //draw background
            var bgc = MouseOver ? BackHoverColor : BackColor;
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, bgc);

            //draw texture
            if (Texture != null)
                g.Draw(Texture, Vector.Zero, Size, TextureColor);

            //draw text
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Font, Text, FontColor, Size / 2, 0.5f, 0.5f);

            //draw border
            if (HasBorder)
            {
                var border = Content.Textures.TryGetIcon("border_hover");// : Content.Textures.TryGetIcon("border");
                var tint = MouseOver ? Color.White : new Color(32, 32, 32);
                tint = (IsSelected && CanSelect) ? Color.Gold.Brighten(20) : tint;

                g.Draw(border, Vector.Zero, Size, tint);
            }
        }

    }
}
