﻿using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Common
{
    enum FlowDirection
    {
        Vertical, Horizontal
    }

    /// <summary>
    /// Lists all standard keybinds (exluding actionbars). 
    /// Reflows with each control added/removed...
    /// </summary>
    class FlowPanel : Control
    {
        bool _autoSize = false;
        FlowDirection _direction = FlowDirection.Horizontal;

        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
                reflow();
            }
        }

        public FlowDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                reflow();
            }
        }


        public FlowPanel()
        {
            SizeChanged += (c) => reflow();
        }

        protected override void OnControlAdded(Control c)
        {
            reflow();
        }

        protected override void OnControlRemoved(Control c)
        {
            reflow();
        }

        void reflow()
        {
            var startPos = new Vector(Padding);
            var v = (Direction == FlowDirection.Horizontal) ? new Vector(1, 0) : new Vector(0, 1);

            var curPos = startPos;

            foreach (var btn in Controls)
            {
                var farPos = curPos + btn.Size;

                if (!AutoSize)
                {
                    if (Direction == FlowDirection.Horizontal)
                    {
                        if (farPos.X + Padding > Size.X)
                            curPos = startPos + new Vector(0, farPos.Y + Padding);
                    }
                    else
                    {
                        if (farPos.Y + Padding > Size.Y)
                            curPos = startPos + new Vector(farPos.X + Padding, 0);
                    }
                }

                btn.Location = curPos;
                curPos += (btn.Size + Padding) * v;
            }

            if (AutoSize)
            {
                var max = Vector.Zero;
                foreach (var btn in Controls)
                    max = Vector.Max(max, btn.Location + btn.Size);

                Size = max + Padding;
            }
        }
    }
}