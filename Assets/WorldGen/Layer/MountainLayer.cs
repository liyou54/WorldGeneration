using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace WorldGen
{
    public class MountainLayer
    {
        public int[,] Data;
        public float SeaThreshold;
        public float PlainThreshold;
        public float HillThreshold;
        public float MountainThreshold;

        public void Build(World world)
        {
            var oceanLayer = world.OceanLayer.Data;
            var elevationLayer = world.ElevationLayer.Data;
            SeaThreshold = world.Config.TerrainLevelConfig.OceanLevel;
            PlainThreshold = world.Config.TerrainLevelConfig.PlainLevel;
            HillThreshold = world.Config.TerrainLevelConfig.HillLevel;
            MountainThreshold = 99999999;
            var height = oceanLayer.GetLength(0);
            var weight = oceanLayer.GetLength(1);
            Data = new int[height, weight];

            for (int y = 0; y <height; y++)
            {
                for (int x = 0; x < weight; x++)
                {
                    var isOcean = oceanLayer[y, x];
                    if (isOcean > 0)
                    {
                        Data[y, x] = 0;
                    }
                    else if(elevationLayer[y , x] < PlainThreshold)
                    {
                        Data[y, x] = 1;
                    }else if (elevationLayer[y , x] < HillThreshold)
                    {
                        Data[y, x] = 2;
                    }
                    else
                    {
                        Data[y, x] = 3;
                    }
                }
            }

        }
        
    }
}