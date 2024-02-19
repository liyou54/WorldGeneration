

using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{

    public class Geography
    {
       public Delaunay delaunay;
       public Voronoi voronoi;
        
        
        public void Build(Vector3[] vertices, int[] triangles)
        {
      
            delaunay = new Delaunay();
            delaunay.Build(vertices, triangles);
            voronoi = new Voronoi();
            voronoi.Build(delaunay);
        }

    }
}