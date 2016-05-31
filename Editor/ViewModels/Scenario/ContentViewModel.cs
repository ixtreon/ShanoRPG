﻿using Shanism.Common.Content;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Editor.ViewModels
{
    /// <summary>
    /// A ViewModel for all the content in a loaded scenario. 
    /// </summary>
    class ContentViewModel
    {

        public string ScenarioDirectory { get; private set; }


        ContentConfig Content;

        public TexturesViewModel Textures { get; private set; }
        public AnimationsViewModel Animations { get; private set; }


        public async Task Load(Scenario sc)
        {
            ScenarioDirectory = sc.Config.BaseDirectory;
            Content = sc.Config.Content;

            Textures = new TexturesViewModel(Content, ScenarioDirectory);
            Animations = new AnimationsViewModel(Content, Textures);

            await Textures.Reload();
            await Animations.Reload();
        }

        public void Save()
        {
            Textures.Save();
            Animations.Save();
        }
    }
}
