using System.Collections.Generic;
using UnityEngine;

namespace Delaunay.Util.JAF
{
    public class BuildVoronoiByCompute
    {
        public enum JFAType
        {
            VoronoiDiagram,
            DistanceTransform
        };

        private int InitSeedKernel;
        private int JFAKernel;
        private int FillVoronoiDiagramKernel;
        private int FillDistanceTransformKernel;
        public JFAType DisplayType = JFAType.VoronoiDiagram;

        private List<Color> RandomColorList(int size)
        {
            var colorList = new List<Color>();

            for (int i = 0; i < size; i++)
            {
                Color color = new Color(Random.value, Random.value, Random.value);
                color.a = 1;
                colorList.Add(color);
            }

            return colorList;
        }

        public Texture2D BuildPixelData(List<Vector2> data, ComputeShader jfaShader, int width, int height)
        {
            Debug.Log(data.Count);
            ComputeBuffer seedBuffer = new ComputeBuffer(data.Count, sizeof(int) * 2);
            seedBuffer.SetData(data);
            InitSeedKernel = jfaShader.FindKernel("InitSeed");
            JFAKernel = jfaShader.FindKernel("JFA");
            FillVoronoiDiagramKernel = jfaShader.FindKernel("FillVoronoiDiagram");
            FillDistanceTransformKernel = jfaShader.FindKernel("FillDistanceTransform");

            var src = new RenderTexture(width, height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            src.enableRandomWrite = true;
            src.Create();

            var dst = new RenderTexture(width, height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            dst.enableRandomWrite = true;
            dst.Create();

            jfaShader.SetBuffer(InitSeedKernel, "Seeds", seedBuffer);
            jfaShader.SetTexture(InitSeedKernel, "Source", src);
            jfaShader.SetInt("Width", width);
            jfaShader.SetInt("Height", height);
            jfaShader.SetInt("SeedCount", data.Count);
            jfaShader.Dispatch(InitSeedKernel, Mathf.CeilToInt(data.Count / 64.0f), Mathf.CeilToInt(data.Count / 64.0f), 1);
            int stepAmount = (int)Mathf.Log(Mathf.Max(width, height), 2);

            int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(height / 8.0f);
            for (int i = 0; i < stepAmount; i++)
            {
                int step = (int)Mathf.Pow(2, stepAmount - i - 1);

                jfaShader.SetInt("Step", step);
                jfaShader.SetTexture(JFAKernel, "Source", src);
                jfaShader.SetTexture(JFAKernel, "Result", dst);

                jfaShader.Dispatch(JFAKernel, threadGroupsX, threadGroupsY, 1);
                (src, dst) = (dst, src);
            }


            // jfaShader.SetTexture(FillDistanceTransformKernel, "Source", src);
            // jfaShader.SetTexture(FillDistanceTransformKernel, "Result", dst);
            // jfaShader.Dispatch(FillDistanceTransformKernel, threadGroupsX, threadGroupsY, 1);
            // jfaShader.Dispatch(FillDistanceTransformKernel, threadGroupsX, threadGroupsY, 1);

            var colorBuffer = new ComputeBuffer(data.Count, sizeof(float) * 4);
            var ramdColorList = RandomColorList(data.Count);
            colorBuffer.SetData(ramdColorList);
            jfaShader.SetBuffer(FillVoronoiDiagramKernel, "Colors", colorBuffer);
            jfaShader.SetTexture(FillVoronoiDiagramKernel, "Source", src);
            jfaShader.SetTexture(FillVoronoiDiagramKernel, "Result", dst);
            jfaShader.Dispatch(FillVoronoiDiagramKernel, threadGroupsX, threadGroupsY, 1);

            RenderTexture.active = dst;
            var targetTexture = new Texture2D(dst.width, dst.height, TextureFormat.RGBAFloat, false);
            // 将RenderTexture的内容读取到目标贴图中
            targetTexture.ReadPixels(new Rect(0, 0, dst.width, dst.height), 0, 0);
            // 应用像素更改
            targetTexture.Apply();
            
            
            RenderTexture.active = null;
            seedBuffer.Release();
            src.Release();
            dst.Release();
            // colorBuffer.Release();

            return targetTexture;
        }
    }
}