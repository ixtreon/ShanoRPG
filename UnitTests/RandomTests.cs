﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IO.Common;
using IO;
using System.Diagnostics;
using IO.Message;
using Engine.Maps;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class RandomTests
    {
        public TestContext TestContext { get; set; }

        //[TestMethod]
        public void TestMethod1()
        {
            TerrainType[,] terrainArray = new TerrainType[5, 5];

            //var protoArr = terrainArray.ToProtoArray<TerrainType>();

            //var oldArr = protoArr.ToArray();
        }

        //[TestMethod]
        public void testEnumConvert()
        {

        }
    }
}