﻿using Engine.Objects;
using Engine.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Output
{
    public interface OutputDevice
    {
        Entity GetEntities(Hero h);


        MapTile[,] GetNearbyTiles(Hero h);

    }
}