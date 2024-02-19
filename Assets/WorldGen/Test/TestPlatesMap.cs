using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Util;

namespace WorldGen.Test
{
    public class TestPlatesMap : MonoBehaviour
    {
        public Vector2Int size = new Vector2Int(100, 100);

        public int plateNum = 10;
        public int seed = 1;
        public float SeaLevel = 0.5f;
        public uint ErsionPeriod = 100;
        public float FoldingRatio = 0.02f;
        public uint AggrOverlapAbs = 100000;
        public float AggrOverlapRel = 0.33f;
        public uint CycleCount = 100;
        public IntPtr simulationPlatec;

        [NonSerialized] public Texture2D heightMap;
        private Color[] colors;

        private float[] heightMapRes;
        private int[] platesMapRes;

        [Button]
        public void Test()
        {
            colors = new Color[200];
            for (int i = 0; i < 200; i++)
            {
                colors[i] = new Color(UnityEngine.Random.Range(.3f, 1f), UnityEngine.Random.Range(.3f, 1f), UnityEngine.Random.Range(.1f, 1f));
            }

            heightMapRes = new float[size.x * size.y];
            platesMapRes = new int[size.x * size.y];

            simulationPlatec = PlatecWarp.platec_api_create(seed, (uint)size.x, (uint)size.y, SeaLevel, ErsionPeriod, FoldingRatio, AggrOverlapAbs, AggrOverlapRel, CycleCount, (uint)plateNum);
            StartCoroutine(RunPlatecCoroutine());
        }

        public void OnGUI()
        {
            if (heightMap == null)
            {
                return;
            }


            if (GUI.Button(new Rect(10, 10, 100, 30), "Save"))
            {
                Save();
            }

            var screenSize = new Vector2(Screen.width, Screen.height);
            var textureSize = new Vector2(heightMap.width, heightMap.height);
            var scale = Mathf.Min(screenSize.x / textureSize.x, screenSize.y / textureSize.y);
            var position = new Rect((screenSize.x - textureSize.x * scale) / 2, (screenSize.y - textureSize.y * scale) / 2, textureSize.x * scale, textureSize.y * scale);
            GUI.DrawTexture(position, heightMap);
        }


        public void Save()
        {
            var path1 = "Assets/heightMap.asset";
            var path2 = "Assets/plateMap.asset";


            var heightTexData = new Color[heightMap.width * heightMap.height];
            var heightTexRes = new Texture2D(heightMap.width, heightMap.height, TextureFormat.RFloat, false);
            for (int i = 0; i < heightMapRes.Length; i++)
            {
                heightTexData[i] = new Color(heightMapRes[i] , heightMapRes[i] , heightMapRes[i] );
            }
            
            var platesMapTexData = new Color[heightMap.width * heightMap.height];
            var platesTexRes = new Texture2D(heightMap.width, heightMap.height, TextureFormat.R16, false);
            for (int i = 0; i < platesMapRes.Length; i++)
            {
                var c =  new Color(); 
                c.r = platesMapRes[i] / 64f;
                platesMapTexData[i] = c;
            }
            
            heightTexRes.SetPixels(heightTexData);
            heightTexRes.Apply();
            
            platesTexRes.SetPixels(platesMapTexData);
            platesTexRes.Apply();
            
            AssetDatabase.CreateAsset(platesTexRes, path2);
            AssetDatabase.CreateAsset(heightTexRes, path1);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void BuildTex()
        {
            if (this.heightMap == null || this.heightMap.width != size.x || this.heightMap.height != size.y)
            {
                this.heightMap = new Texture2D(size.x, size.y);
            }

            var platesMapData = PlatecWarp.platec_api_get_platesmap(simulationPlatec);
            var heightMapData = PlatecWarp.platec_api_get_heightmap(simulationPlatec);

            Marshal.Copy(heightMapData, heightMapRes, 0, size.x * size.y);
            Marshal.Copy(platesMapData, platesMapRes, 0, size.x * size.y);
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var height = heightMapRes[j * size.x + i] / 10;

                    var color = colors[platesMapRes[j * size.x + i] % 200] * (.1f + height);
                    color.a = 1;
                    this.heightMap.SetPixel(i, j, color);
                }
            }

            this.heightMap.Apply();
        }

        IEnumerator RunPlatecCoroutine()
        {
            var step = 0;
            while (!PlatecWarp.platec_api_is_finished(simulationPlatec))
            {
                PlatecWarp.platec_api_step(simulationPlatec);
                step++;

                if (step % 10 == 0)
                {
                    BuildTex();
                }

                Debug.Log("Platec任务进行中。" + step);
                yield return null;
            }

            PlatecWarp.platec_api_destroy(simulationPlatec);

            simulationPlatec = IntPtr.Zero;
        }
    }
}