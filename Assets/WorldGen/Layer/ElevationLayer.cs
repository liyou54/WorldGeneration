using System;
using System.Linq;
using ANoiseGPU;
using Unity.Mathematics;
using UnityEngine;
using Util.Noise;
using WorldGen;

namespace WorldGenGen
{
    public class ElevationLayer
    {
        public float[,] Data;

        private void AddNoiseToElevation(int height, int weight)
        {
            var size = (int)MathF.Max(height, weight);
            var noise = NoiseGeneration.GenNoiseData(size, 8, 2f,
                -.3f, .3f, 2f, .5f, NoiseType.PERLIN, FractalType.FBM, 2331);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < weight; j++)
                {
                    Data[i, j] += noise[i * weight + j];
                }
            }
        }

        public void Build(World world)
        {
            var heightMapPixel = world.Config.HeightMap.GetPixels();
            var weight = world.Config.HeightMap.width;
            var height = world.Config.HeightMap.height;
            Data = new float[height, weight];

            // find offset to move land to center 
            var tempH = new int[height];
            var tempW = new int[weight];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < weight; w++)
                {
                    var isOcean = heightMapPixel[weight * h + w].r <
                                  world.Config.TerrainLevelConfig.OceanLevel
                        ? 1
                        : 0;
                    tempH[h] += isOcean;
                    tempW[w] += isOcean;
                }
            }

            var offsetH = tempH
                .Select((num, index) => new { Number = num, Index = index })
                .OrderByDescending(x => x.Number)
                .First()
                .Index;
            var offsetW = tempW
                .Select((num, index) => new { Number = num, Index = index })
                .OrderByDescending(x => x.Number)
                .First()
                .Index;
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < weight; w++)
                {
                    var indexW = (weight + offsetW + w) % weight;
                    var indexH = (height + offsetH + h) % height;
                    Data[h, w] = heightMapPixel[weight * indexH + indexW].r;
                }
            }

            AddNoiseToElevation(height, weight);
        }

        public void SetOceanDepth(World world)
        {
            var width = world.OceanLayer.Data.GetLength(1);
            var height = world.OceanLayer.Data.GetLength(0);

            int[,] GetNextLand(int radius)
            {
                var oceanLayer = world.OceanLayer.Data;
                var res = new int[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        res[i, j] = oceanLayer[i, j] == 1 ? -1 : 0;
                    }
                }

                for (int dist = 0; dist < radius; dist++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        for (int w = 0; w < width; w++)
                        {
                            if (res[h, w] == dist)
                            {
                                for (int tempY = -1; tempY < 2; tempY++)
                                {
                                    for (int tempX = -1; tempX < 2; tempX++)
                                    {
                                        var tempPos = new int2(h + tempY, w + tempX);
                                        tempPos.x = Mathf.Clamp(tempPos.x, 0, height - 1);
                                        tempPos.y = Mathf.Clamp(tempPos.y, 0, width - 1);
                                        if (res[tempPos.x, tempPos.y] == -1)
                                        {
                                            res[tempPos.x, tempPos.y] = dist + 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return res;
            }
            
            var oceanDepth = GetNextLand(10);
            
            
        }
    }
}