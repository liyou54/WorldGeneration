using System;
using System.Collections.Generic;
using System.Numerics;
using andywiecko.BurstTriangulator;
using AwesomeNamespace;
using Delaunay;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using WorldGenGen;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;


namespace WorldGen
{
    public class TemperatureConfig
    {
        public float Polar = 1;
        public float Alpine = 1;
        public float Boreal = 1;
        public float Cool = 1;
        public float Warm = 1;
        public float Subtropical = 1;
        public float Tropical = 1;
    }

    [Serializable]
    public class TerrainLevel
    {
        public float OceanLevel;
        public float PlainLevel;
        public float HillLevel;
    }


    [Serializable]
    public class WorldConfig
    {
        public Vector2 Size;
        public float Density;
        public Texture2D HeightMap;
        public Texture2D PlatesMap;
        public TerrainLevel TerrainLevelConfig;
    }

    public struct MapCell
    {
        public VCellId CellId;
        public Vector3 Position;
        public float Height;
        public int Plate;
        public bool IsLand;
        public float Moisture;
        public int RiverLevel;
    }


    public class World
    {
        public WorldConfig Config;
        public Geography Geography;

        public List<MapCell> MapCells;
        public int[] OutputTriangles;
        public OceanLayer OceanLayer;
        public MountainLayer MountainLayer;
        public ElevationLayer ElevationLayer;
        public Vector3[] OutputPositions;

        public World(WorldConfig config)
        {
            Config = config;
        }


        public void Build()
        {
            
            ElevationLayer = new ElevationLayer();
            ElevationLayer.Build(this); 

            OceanLayer = new OceanLayer();
            OceanLayer.BuildOceanLayer(this);
            
            MountainLayer = new MountainLayer();
            MountainLayer.Build(this);
            
            ElevationLayer.SetOceanDepth(this);

            InitCellCenter();
            Geography = new Geography();
            Geography.Build(OutputPositions, OutputTriangles);
            MapCells = new List<MapCell>();


            for (int i = 0; i < Geography.voronoi.Cells.Count; i++)
            {
                var cell = Geography.voronoi.Cells[i];
                var cellCenter = new Vector3(cell.Center.x, 0, cell.Center.y);
                var cellId = new VCellId(i);
                var height = SampleBilinerFloat(cell.Center, ElevationLayer.Data);
                var isLand = height > 0.1f;
                var plates = SampleBilinerFloat(cellCenter, ElevationLayer.Data);
                MapCells.Add(new MapCell
                {
                    CellId = cellId,
                    Position = cellCenter,
                    Height = height,
                    IsLand = isLand,
                    Plate = (int)(plates * 64),
                    RiverLevel = 0
                });
            }
        }

        public Vector2 GetCellCenter(VCellId cellId)
        {
            VoronoiCell cell = Geography.voronoi.Cells[cellId];
            return new Vector2(cell.Center.x, cell.Center.y);
        }

        public Vector4 SampleLiner(Vector2 pos, Texture2D texture2D)
        {
            var xfloat = (pos.x * texture2D.width / Config.Size.x);
            var yfloat = (pos.y * texture2D.height / Config.Size.y);
            var x = (int)xfloat;
            var y = (int)yfloat;
            return texture2D.GetPixel(x, y);
        }

        public float SampleBilinerFloat(Vector2 pos, float[,] data)
        {
            var height = data.GetLength(0);
            var weight = data.GetLength(1);
            var xfloat = (pos.x * weight / (Config.Size.x+2));
            var yfloat = (pos.y * height / (Config.Size.y+2));
            var x = (int)xfloat;
            var y = (int)yfloat;
            var x1 = x + 1;
            var y1 = y + 1;
            var xoffset = xfloat - x;
            var yoffset = yfloat - y;
            var c00 = data[y, x];
            var c10 = data[y, x1];
            var c01 = data[y1, x];
            var c11 = data[y1, x1];
            var c0 = c00 * xoffset + c10 * (1 - xoffset);
            var c1 = c01 * xoffset + c11 * (1 - xoffset);
            var c = c0 * yoffset + c1 * (1 - yoffset);
            return c;
        }

        public void InitCellCenter()
        {
            var offset = Config.Density;
            var divSize = offset * 2;
            var centerPoints = UniformPoissonDiskSampler.SampleRectangle(new Vector2(offset, offset), Config.Size - new Vector2(1, 1) * offset, Config.Density);

            for (int i = 1; i < MathF.Ceiling((Config.Size.x) / divSize); i++)
            {
                var x = i * divSize > Config.Size.x ? Config.Size.x : i * divSize;
                centerPoints.Add(new Vector2(x + Single.Epsilon, 0));
                centerPoints.Add(new Vector2(x - Single.Epsilon, Config.Size.y));
            }

            for (int i = 1; i < MathF.Ceiling((Config.Size.y) / divSize); i++)
            {
                var y = i * divSize > Config.Size.y ? Config.Size.y : i * divSize;
                centerPoints.Add(new Vector2(0, y));
                centerPoints.Add(new Vector2(Config.Size.x, y));
            }

            centerPoints.Add(new Vector2(0, 0));
            centerPoints.Add(new Vector2(Config.Size.x, 0));
            centerPoints.Add(new Vector2(0, Config.Size.y));
            centerPoints.Add(new Vector2(Config.Size.x, Config.Size.y));

            var dataFloat2 = centerPoints.ConvertAll((vec) => new float2(vec.x, vec.y)).ToArray();


            using var positions = new NativeArray<float2>(dataFloat2, Allocator.Persistent);

            using var triangulator = new Triangulator(capacity: 1024, Allocator.Persistent)
            {
                Input = { Positions = positions }
            };
            using (new PerformanceTimer("burst"))
            {
                triangulator.Run();
            }

            Debug.Log($"TriCount:{triangulator.Output.Triangles.Length}");
            var outputTriangles = triangulator.Output.Triangles;
            var outputPositions = triangulator.Output.Positions;
            OutputTriangles = new int[outputTriangles.Length];
            for (var i = 0; i < outputTriangles.Length; i++)
            {
                OutputTriangles[i] = outputTriangles[i];
            }

            OutputPositions = new Vector3[outputPositions.Length];
            for (var i = 0; i < outputPositions.Length; i++)
            {
                var temp = outputPositions[i];
                OutputPositions[i] = new Vector3(temp.x, 0, temp.y);
            }
        }
    }
}