using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace WorldGen
{
    public class OceanLayer
    {
        public int[,] Data;


        private void InitOceanLayer(World world)
        {
            var height = world.ElevationLayer.Data.GetLength(0);
            var width = world.ElevationLayer.Data.GetLength(1);
            var oceanLevel = world.Config.TerrainLevelConfig.OceanLevel;
            Data = new int[height, width];
            Queue<int2> oceanQueue = new();
            for (int i = 0; i < width; i++)
            {
                if (world.ElevationLayer.Data[0, i] < oceanLevel)
                {
                    oceanQueue.Enqueue(new int2(0, i));
                }

                if (world.ElevationLayer.Data[height - 1, i] < oceanLevel)
                {
                    oceanQueue.Enqueue(new int2(height - 1, i));
                }
            }

            for (int i = 0; i < height; i++)
            {
                if (world.ElevationLayer.Data[i, 0] < oceanLevel)
                {
                    oceanQueue.Enqueue(new int2(i, 0));
                }

                if (world.ElevationLayer.Data[i, width - 1] < oceanLevel)
                {
                    oceanQueue.Enqueue(new int2(i, width - 1));
                }
            }

            while (oceanQueue.Count > 0)
            {
                var pos = oceanQueue.Dequeue();
                if (Data[pos.x, pos.y] == 0)
                {
                    Data[pos.x, pos.y] = 1;
                    for (int tempY = -1; tempY < 2; tempY++)
                    {
                        for (int tempX = -1; tempX < 2; tempX++)
                        {
                            var tempPos = pos + new int2(tempY, tempX);
                            tempPos.x = Mathf.Clamp(tempPos.x, 0, height - 1);
                            tempPos.y = Mathf.Clamp(tempPos.y, 0, width - 1);

                            if (Data[tempPos.x, tempPos.y] == 0 && world.ElevationLayer.Data[tempPos.x, tempPos.y] <= oceanLevel)
                            {
                                oceanQueue.Enqueue(new(tempPos.x, tempPos.y));
                            }
                        }
                    }
                }
            }
        }

        public void BuildOceanLayer(World world)
        {
            InitOceanLayer(world);
            HarmonizeOcean(world);
        }

        private void HarmonizeOcean(World world)
        {
            var shallowSea = world.Config.TerrainLevelConfig.OceanLevel * 0.85f;
            var midPoint = shallowSea / 2.0f;
            var elevationLayer = world.ElevationLayer.Data;
            var oceanLayer = world.OceanLayer.Data;
            var width = world.ElevationLayer.Data.GetLength(1);
            var height = world.ElevationLayer.Data.GetLength(0);
            var oceanPoints = new bool[height, width];

            for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                oceanPoints[h, w] = elevationLayer[h, w] < shallowSea && oceanLayer[h, w] > 0; 
            
            
            var shallowOcean = new bool[height, width];
            for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                shallowOcean[h, w] = elevationLayer[h, w] < midPoint && oceanLayer[h, w] > 0; 
            
            for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                if (shallowOcean[h, w])
                {
                    elevationLayer[h, w] = midPoint - ((midPoint - elevationLayer[h, w]) / 5.0f);
                }
            
            var deepOcean = new bool[height, width];
            for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                deepOcean[h, w] = elevationLayer[h, w] > midPoint && oceanLayer[h, w] > 0; 
            
            for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                if (deepOcean[h, w])
                {
                    elevationLayer[h, w] = midPoint + ( elevationLayer[h, w] - midPoint) / 5.0f;
                }
            
        }
    }
}