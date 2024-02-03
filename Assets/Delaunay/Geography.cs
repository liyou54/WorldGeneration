

using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class Geography
    {
        Delaunay delaunay;
        Voronoi voronoi;
        
        public void Build(Vector3[] vertices, int[] triangles)
        {
            delaunay = new Delaunay();
            delaunay.Build(vertices, triangles);
            voronoi = new Voronoi();
        }

    }
}