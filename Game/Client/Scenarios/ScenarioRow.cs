﻿using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.GameScreens
{
    class ScenarioRow : Button
    {
        public const double CollapsedHeight = 0.085;
        public const double ExpandedHeight = 0.20;

        public readonly ScenarioConfig Scenario;

        readonly Label description, author;
        readonly Button select;

        bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                updateExpansion();
            }
        }

        public event Action<ScenarioConfig> ScenarioSelected;

        public event Action<ScenarioRow> Expanded;

        public ScenarioRow(ScenarioConfig sc)
        {
            Scenario = sc;
            Size = new Vector(0.6, CollapsedHeight);

            var nameFont = Content.Fonts.FancyFont;
            Add(new Label
            {
                Font = nameFont,
                TextColor = Color.Black,
                AutoSize = false,
                CanHover = false,
                Location = new Vector(Padding),
                Size = new Vector(Size.X - 2 * Padding, nameFont.HeightUi),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
                Text = sc.Name,
            });

            Add(select = new Button
            {
                Text = "Play",
                BackColor = Color.Red,

                Width = 0.20,
                Right = Size.X - Padding,
                Bottom = Size.Y - Padding,
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,

                IsVisible = false,
            });
            select.MouseUp += (e) => ScenarioSelected?.Invoke(Scenario);

            var authorName = string.IsNullOrEmpty(Scenario.Author)
                ? "Unknown"
                : Scenario.Author;
            Add(author = new Label
            {
                Font = Content.Fonts.NormalFont,
                Text = $"Author: {authorName}",
                TextColor = Color.Black,

                Right = Size.X - Padding,
                Top = Padding,
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,

                CanHover = false,
                IsVisible = false,
            });

            ToolTip = sc.BaseDirectory;
            MouseUp += onMouseUp;
        }

        void onMouseUp(Input.MouseButtonArgs obj)
        {
            IsExpanded = true;
        }

        void updateExpansion()
        {
            if (IsExpanded)
            {
                Height = ExpandedHeight;
                Expanded?.Invoke(this);
            }
            else
            {
                Height = CollapsedHeight;
            }

            select.IsVisible = IsExpanded;
            author.IsVisible = IsExpanded;
        }
    }
}
