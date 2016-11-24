﻿using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Shanism.Client.UI
{
    enum FlowDirection
    {
        Vertical, Horizontal
    }

    /// <summary>
    /// Lists all standard keybinds (exluding actionbars). 
    /// Reflows with each control added/removed...
    /// </summary>
    class FlowPanel : Control, IEnumerable<Control>
    {
        bool _autoSize = false;
        FlowDirection _direction = FlowDirection.Horizontal;

        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
                Reflow();
            }
        }

        public FlowDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                Reflow();
            }
        }


        public FlowPanel(FlowDirection direction = FlowDirection.Vertical)
        {
            _direction = direction;
            SizeChanged += (c) => Reflow();
        }

        protected override void OnControlAdded(Control c)
        {
            Reflow();
            c.SizeChanged += onControlSizeChanged;
        }

        protected override void OnControlRemoved(Control c)
        {
            Reflow();
            c.SizeChanged -= onControlSizeChanged;
        }

        void onControlSizeChanged(Control c) => Reflow();

        public void Reflow()
        {
            var startPos = new Vector(Padding);
            var v = (Direction == FlowDirection.Horizontal) ? new Vector(1, 0) : new Vector(0, 1);

            var curPos = startPos;

            foreach (var btn in Controls)
            {
                if (!btn.IsVisible)
                    continue;

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

                var newSize = max + Padding;
                if(newSize != Size)
                    Size = max + Padding;
            }
        }

        public IEnumerator<Control> GetEnumerator()
            => Controls.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Controls.GetEnumerator();
    }
}
