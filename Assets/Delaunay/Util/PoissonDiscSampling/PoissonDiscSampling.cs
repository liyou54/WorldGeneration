using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public static class PoissonDiscSampling
    {
        public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 32)
        {
            bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
            {
                if (candidate.x - radius >= 0f && candidate.x + radius < sampleRegionSize.x && candidate.y - radius >= 0f && candidate.y + radius < sampleRegionSize.y)
                {
                    int cellX = Mathf.RoundToInt(candidate.x / cellSize);
                    int cellZ = Mathf.RoundToInt(candidate.y / cellSize);
                    int searchStartX = Mathf.Max(0, cellX - 3);
                    int searchEndX = Mathf.Min(cellX + 3, grid.GetLength(0) - 1);
                    int searchStartZ = Mathf.Max(0, cellZ - 3);
                    int searchEndZ = Mathf.Min(cellZ + 3, grid.GetLength(1) - 1);
                    //如果要检测其它格子内的球，需要遍历周围6个格子

                    for (int x = searchStartX; x <= searchEndX; x++)
                    {
                        for (int z = searchStartZ; z <= searchEndZ; z++)
                        {
                            int pointIndex = grid[x, z] - 1; //存长度不存索引，取时减1,0就变成了-1，不需要初始化数组了
                            if (pointIndex != -1)
                            {
                                float dst = (candidate - points[pointIndex]).magnitude;
                                if (dst < radius * 2f)
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    return true;
                }

                return false;
            }

            float cellSize = radius / Mathf.Sqrt(2);

            int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
            List<Vector2> points = new List<Vector2>();
            List<Vector2> spawnPoints = new List<Vector2>();

            spawnPoints.Add(new Vector2(sampleRegionSize.x / 2f, sampleRegionSize.y / 2f));
            while (spawnPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                Vector2 spawnCenter = spawnPoints[spawnIndex];

                bool candidateAccepted = false;
                for (int i = 0; i < numSamplesBeforeRejection; i++)
                {
                    float angle = Random.value * Mathf.PI * 2f;
                    Vector2 dir = new Vector2(Mathf.Sin(angle),  Mathf.Cos(angle));
                    Vector2 candidate = spawnCenter + dir * Random.Range(2f, 3f) * radius;

                    if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        grid[Mathf.RoundToInt(candidate.x / cellSize), Mathf.RoundToInt(candidate.y / cellSize)] = points.Count;
                        candidateAccepted = true;
                        break;
                    }
                }

                if (!candidateAccepted)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                }
            }

            return points;
        }
    }
}