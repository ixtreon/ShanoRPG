﻿using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Client.Systems;
using Shanism.Client.Map;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shanism.Common.Interfaces.Entities;
using Shanism.Client.Drawing;

namespace Shanism.Client
{

    // A top level control, contains all the client systems
    class SystemGod : Control
    {

        readonly List<ClientSystem> systems = new List<ClientSystem>();

        readonly GraphicsDevice device;

        #region Systems

        /// <summary>
        /// Lists and draws objects. 
        /// </summary>
        readonly ObjectSystem objects;

        Terrain terrain;

        Interface @interface;


        /// <summary>
        /// Listens for ability casts and informs the server. 
        /// </summary>
        ActionSystem actions;

        /// <summary>
        /// Listens for key presses and informs the server. 
        /// </summary>
        MoveSystem movement;

        /// <summary>
        /// Listens for chat messages and sends them to the chatbox
        /// </summary>
        ChatSystem chat;


        #endregion


        public SystemGod(GraphicsDevice device, AssetList content)
        {
            this.device = device;

            Location = Vector.Zero;
            CanFocus = true;
            Control.SetContent(content);

            GameActionActivated += onActionActivated;

            objects = new ObjectSystem(device, Content);
            Reload();
        }


        public IHero MainHero => objects.MainHero;


        public void SetUiVisible(bool isVisible) 
            => @interface.Root.IsVisible = isVisible;

        public void DrawUi()
            => @interface.Draw();

        public void DrawObjects()
            => objects.Draw();

        public void DrawTerrain()
            => terrain.Draw();

        /// <summary>
        /// Raised whenever a system sends a message to the server.
        /// </summary>
        public event Action<IOMessage> MessageSent;



        public void Reload()
        {
            //systems
            systems.Clear();
            systems.Add(terrain = new Terrain(device, Content.Terrain));
            systems.Add(objects);
            systems.Add(movement = new MoveSystem());
            systems.Add(chat = new ChatSystem());
            systems.Add(@interface = new Interface(device, objects, chat));
            systems.Add(actions = new ActionSystem(@interface, objects));

            foreach (var sys in systems)
                sys.MessageSent += (m) => MessageSent?.Invoke(m);

            //controls
            ClearControls();
            Add(@interface.Root);
        }

        public void HandleMessage(IOMessage msg)
        {
            foreach (var sys in systems)
                sys.HandleMessage(msg);
        }


        void onActionActivated(ClientAction ga)
        {
            switch (ga)
            {
                case ClientAction.ToggleDebugInfo:
                    ClientEngine.ShowDebugStats = !ClientEngine.ShowDebugStats;
                    break;

                case ClientAction.ReloadUi:
                    Reload();
                    break;

                case ClientAction.ShowHealthBars:
                    Settings.Current.AlwaysShowHealthBars = !Settings.Current.AlwaysShowHealthBars;
                    break;

                default:
                    //propagate to interface (objects?), let them handle it
                    @interface.Root.ActivateAction(ga);
                    break;
            }
        }


        protected override void OnUpdate(int msElapsed)
        {
            Size = Screen.UiSize;
            Location = Screen.UiSize / -2;

            actions.Hero = objects.MainHero;

            UpdateMain(msElapsed);
            foreach (var sys in systems)
                sys.Update(msElapsed);
        }

    }
}