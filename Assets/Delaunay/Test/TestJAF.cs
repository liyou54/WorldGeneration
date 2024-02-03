using System.Collections.Generic;
using AwesomeNamespace;
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
            var data = UniformPoissonDiskSampler.SampleRectangle( new Vector2(0,0), new Vector2(100, 100),1);
            TestTexture = voronoi.BuildPixelData(data, JfaShader, (int)MapSize.x, (int)MapSize.y);
        }
    }
}