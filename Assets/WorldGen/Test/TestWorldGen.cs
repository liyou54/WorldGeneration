using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WorldGen.Test
{
    public class TestWorldGen : MonoBehaviour
    {
        public World World;
        public WorldConfig config;
        public Dictionary<int, List<MapCell>> EdgePlates = new();
        private bool IsDebugPlateEdge;
        public WorldRender.WorldRender WorldRender;
        [PreviewField(200)] public Texture2D LayerDebug;

        [Button]
        public void Generate()
        {
            World = new World(config);
            World.Build();
            WorldRender = transform.GetOrAddComponent<WorldRender.WorldRender>();
            WorldRender.World = World;
            WorldRender.Render();
        }

        [Button]
        public void TestOceanLayer()
        {
            if (LayerDebug)
            {
                DestroyImmediate(LayerDebug);
            }

            var height = World.ElevationLayer.Data.GetLength(0);
            var width = World.ElevationLayer.Data.GetLength(1);
            LayerDebug = new Texture2D(width, height);

            var pixels = new Color[width * height];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    var c = Color.white;
                    pixels[width * h + w] =  c * (3 - World.OceanLayer.Data[h, w]) / 3f;
                }
            }

            LayerDebug.SetPixels(pixels);
            LayerDebug.Apply();
        }

        [Button]
        public void TestMountainLayer()
        {
            if (LayerDebug)
            {
                DestroyImmediate(LayerDebug);
            }

            var height = World.ElevationLayer.Data.GetLength(0);
            var width = World.ElevationLayer.Data.GetLength(1);
            LayerDebug = new Texture2D(width, height);

            var pixels = new Color[width * height];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    var color = Color.white;
                    pixels[width * h + w] = color * (World.MountainLayer.Data[h, w] / 4.0f);
                }
            }

            LayerDebug.SetPixels(pixels);
            LayerDebug.Apply();
        }
    }
}