﻿using Shanism.Client.Drawing;
using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client
{
    /// <summary>
    /// A graphics object used to draw on top of some or all the MonoGame SpriteBatch. 
    /// </summary>
    class Graphics : SpriteBatch
    {
        readonly Stack<RectangleF> drawStack = new Stack<RectangleF>();

        readonly GraphicsDevice device;


        /// <summary>
        /// Gets the bounds of the drawing area. 
        /// </summary>
        public RectangleF Bounds { get; set; }

        /// <summary>
        /// Gets the position of the drawing area. 
        /// </summary>
        public Vector Position => Bounds.Position;

        /// <summary>
        /// Gets the size of the drawing area. 
        /// </summary>
        public Vector Size => Bounds.Size;

        public Graphics(GraphicsDevice gd)
            : base(gd)
        {
            device = gd;
        }

        public void Begin()
        {
            Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone);
        }

        public void PushWindow(Vector offset, Vector sz)
        {
            offset = Vector.Max(Vector.Zero, offset);
            sz = Vector.Min(sz, Size - offset);

            drawStack.Push(Bounds);
            Bounds = new RectangleF(Position + offset, sz);
        }

        public void PopWindow()
        {
            Bounds = drawStack.Pop();
        }


        public void Draw(Texture2D tex, Vector pos, Vector size, Color? color = null, float depth = 0)
        {
            if (tex == null)
                throw new ArgumentNullException(nameof(tex), "The texture cannot be null!");

            clampPositionAndSize(ref pos, ref size);
            uiToScreen(ref pos, ref size);

            Draw(tex,
                position: pos.ToVector2(),
                scale: new Vector(size.X / tex.Width, size.Y / tex.Height).ToVector2(),
                color: (color ?? Color.White).ToXnaColor(),
                layerDepth: depth);
        }


        public void DrawSprite(EntitySprite s, Vector pos, Vector size,
            float depth = 0)
        {
            clampPositionAndSize(ref pos, ref size);
            uiToScreen(ref pos, ref size);
            var screenRect = new RectangleF(pos, size);

            s.Draw(this, screenRect, depth);
        }


        public void Draw(IconSprite s, Vector pos, Vector size, Color? tint = null)
        {
            clampPositionAndSize(ref pos, ref size);
            uiToScreen(ref pos, ref size);
            var screenRect = new RectangleF(pos, size);

            s.Draw(this, screenRect, tint);
        }


        public void DrawString(TextureFont f, string text, Common.Color color,
            Vector txtPos,
            float xAnchor, float yAnchor, double? txtMaxWidth = null)
        {

            var screenPos = Screen.UiToScreen(Position + txtPos);
            var screenMaxWidth = Screen.UiScale * txtMaxWidth;
            f.DrawString(this, text, color, screenPos,
                xAnchor, yAnchor, screenMaxWidth);
        }

        void clampPositionAndSize(ref Vector pos, ref Vector size)
        {
            //pos = pos.Clamp(Vector.Zero, Size - size);
            //size = size.Clamp(Vector.Zero, (Size - pos));
        }

        void uiToScreen(ref Vector pos, ref Vector size)
        {
            pos = Screen.UiToScreen(Position + pos);
            size = Screen.UiToScreenSize(size);
        }

        public override string ToString() => $"Graphics @ {Position} : {Size}";
    }
}
