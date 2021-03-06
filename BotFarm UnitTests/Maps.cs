﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BotFarm.UnitTests.Properties;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BotFarm.UnitTests
{
    [TestClass]
    public class Maps
    {
        [TestMethod]
        public void MMapsRaceConditions()
        {
            DetourCLI.Detour.Initialize(Settings.Default.MMAPsFolderPath);
            VMapCLI.VMap.Initialize(Settings.Default.VMAPsFolderPath);
            MapCLI.Map.Initialize(Settings.Default.MAPsFolderPath);

            var queuedTask = new List<Task>();
            for (int i = 0; i < 4; i++)
                queuedTask.Add(new Task(() =>
                {
                    using (var detour = new DetourCLI.Detour())
                    {
                        List<DetourCLI.Point> resultPath;
                        bool successful = detour.FindPath(-8896.072266f, -82.352325f, 86.421661f,
                                                -8915.272461f, -111.634041f, 82.275642f,
                                                0, out resultPath);
                        Assert.IsTrue(successful);
                    }
                }));

            queuedTask.ForEach(task => task.Start());
            Task.WaitAll(queuedTask.ToArray());
        }
    }
}
