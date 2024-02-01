using System.Collections.Generic;
using Delaunay.Util.JAF;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Delaunay.Test
{
    public class TestJAF : MonoBehaviour
    {
        public Vector2 MapSize = new Vector2(4096, 4096);
        public float Radius = 10;
        public ComputeShader JfaShader;

         public Texture2D TestTexture;

        [Button]
        public void Test()
        {
            BuildVoronoiByCompute voronoi = new BuildVoronoiByCompute();
            var data = PoissonDiscSampling.GeneratePoints(Radius, MapSize);
            TestTexture = voronoi.BuildPixelData(data, JfaShader, (int)MapSize.x, (int)MapSize.y);
        }
    }
}