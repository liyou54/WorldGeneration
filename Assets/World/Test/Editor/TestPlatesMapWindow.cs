using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Util;

namespace World.Test.Editor
{
    [Serializable]
    public struct PlateConfig
    {
        public Vector2Int size;
        public int plateNum;
        public int seed;
        public float SeaLevel;
        public uint ErsionPeriod;
        public float FoldingRatio;
        public uint AggrOverlapAbs;
        public float AggrOverlapRel;
        public uint CycleCount;

        public PlateConfig(int temp = 1)
        {
            size = new Vector2Int(128, 128);
            plateNum = 10;
            seed = 1;
            SeaLevel = 0.5f;
            ErsionPeriod = 100;
            FoldingRatio = 0.02f;
            AggrOverlapAbs = 100000;
            AggrOverlapRel = 0.33f;
            CycleCount = 100;
        }
    }

    public class TestPlatesMapWindow : EditorWindow
    {
        public Vector2Int size;
        public int plateNum;
        public int seed;
        public float SeaLevel;
        public uint ErsionPeriod;
        public float FoldingRatio;
        public uint AggrOverlapAbs;
        public float AggrOverlapRel;
        public uint CycleCount;
        public Texture2D heightMap;
        public IntPtr simulationPlatec;

        [MenuItem("Window/My Editor Window")] // 定义菜单项
        public static void ShowWindow()
        {
            // 创建窗口实例并显示
            EditorWindow.GetWindow(typeof(TestPlatesMapWindow), false, "My Window");
        }

        public void BuildTex()
        {
            if (this.heightMap == null || this.heightMap.width != size.x || this.heightMap.height != size.y)
            {
                this.heightMap = new Texture2D(size.x, size.y);
            }

            var platesMapData = PlatecWarp.platec_api_get_platesmap(simulationPlatec);
            var heightMapData = PlatecWarp.platec_api_get_heightmap(simulationPlatec);


            var heightMapRes = new float[size.x * size.y];
            Marshal.Copy(heightMapData, heightMapRes, 0, size.x * size.y);
            var platesMapRes = new int[size.x * size.y];
            Marshal.Copy(platesMapData, platesMapRes, 0, size.x * size.y);
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var color = new Color(heightMapRes[i * size.y + j], heightMapRes[i * size.y + j], heightMapRes[i * size.y + j]);
                    this.heightMap.SetPixel(i, j, color);
                }
            }

            this.heightMap.Apply();
        }

        IEnumerator RunPlatecCoroutine()
        {
            var i = 0;
            Debug.Log("Platec任务开始。");
            while (!PlatecWarp.platec_api_is_finished(simulationPlatec))
            {
                Debug.Log($"Platec任务进行中。{i++}");
                PlatecWarp.platec_api_step(simulationPlatec);
                BuildTex();
                yield return null;
            }

            BuildTex();
            PlatecWarp.platec_api_destroy(simulationPlatec);
            Debug.Log("Platec任务完成。");
        }

        void OnGUI()
        {
            size = EditorGUILayout.Vector2IntField("Size", size);
            plateNum = EditorGUILayout.IntField("Plate Num", plateNum);
            seed = EditorGUILayout.IntField("Seed", seed);
            SeaLevel = EditorGUILayout.FloatField("Sea Level", SeaLevel);
            ErsionPeriod = (uint)EditorGUILayout.IntField("Ersion Period", (int)ErsionPeriod);
            FoldingRatio = EditorGUILayout.FloatField("Folding Ratio", FoldingRatio);
            AggrOverlapAbs = (uint)EditorGUILayout.IntField("Aggr Overlap Abs", (int)AggrOverlapAbs);
            AggrOverlapRel = EditorGUILayout.FloatField("Aggr Overlap Rel", AggrOverlapRel);
            CycleCount = (uint)EditorGUILayout.IntField("Cycle Count", (int)CycleCount);

            if (GUILayout.Button("Generate"))
            {
                simulationPlatec =  PlatecWarp.platec_api_create(seed, (uint)size.x, (uint)size.y, SeaLevel, ErsionPeriod, FoldingRatio, AggrOverlapAbs, AggrOverlapRel, CycleCount, (uint)plateNum);
                Task.Run(() =>
                {
                    // 执行一些耗时的操作
                    Debug.Log("子线程执行中...");
                    System.Threading.Thread.Sleep(5000);
                    Debug.Log("子线程执行完毕。");
                });
                EditorCoroutineExtensions.StartCoroutine(this,"RunPlatecCoroutine");
            }
        }
    }
}