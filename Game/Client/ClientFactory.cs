﻿using Shanism.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    public static class ClientFactory
    {
        public static IClientEngine CreateGameEngine(IGraphicsDeviceService graphics, ContentManager content)
        {
            return new ClientEngine(graphics, content);
        }
    }
}
