

using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public class DVertex
    {
        public Vector2 Pos;
        public DFace Belong;
        public List<DEdge> AdjacentEdge = new List<DEdge>();
        public bool IsCorner;
        
        public int id;
        public DVertex()
        {
            id = 0;
            Pos = new Vector2(float.MinValue, float.MinValue);
        }

        public DVertex(int id, float x, float y)
        {
            this.id = id;
            Pos = new Vector2(x, y);
        }

        public DVertex(int id, Vector2 pos)
        {
            this.id = id;
            this.Pos = pos;
        }
        
        public bool TestAttachToFace( DFace f)
        {
            var e = f.edge;
            for (int i = 0; i < 3; i++) {
                Vector2 dir = e.ToVertex.Pos - e.FromVertex.Pos;
                Vector2 p = Pos - e.FromVertex.Pos;
                if (Vector2Util.Cross(dir,p) < 0) return false;
                e = e.NextEdge;
            }
            Belong = f;
            f.owned.Add(this);
            return true;
        }
    };
}