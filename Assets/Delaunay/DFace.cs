using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;


namespace Delaunay
{
   public class DFace
    {
        public int id;

        // 其中一个组成边
        public DEdge edge;
        public Vector2 center;
        /// <summary>
        ///  在构造时 将子节点放在owned中
        /// </summary>
        public List<DVertex> owned = new List<DVertex>();
        

        public DFace()
        {
            id = 0;
        }

        public DFace(int id, DEdge edge)
        {
            this.id = id;
            this.edge = edge;
        }
    };

}