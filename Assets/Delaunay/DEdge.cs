using System.Numerics;

namespace Delaunay
{
   public class DEdge
    {
        // 边的起点
        public DVertex FromVertex;

        // 边的终点
        public DVertex ToVertex;

        // 孪生边
        public DEdge TwinEdge;

        // 组成面
        public DFace Face;

        // 前驱边
        public DEdge NextEdge;

        public int id;

        public DEdge()
        {
            id = 0;
        }
        
        public string ToString()
        {
            return  FromVertex.Pos.ToString() + " -> " + ToVertex.Pos.ToString();
        }

        public DEdge(DVertex fromVertex, DVertex toVertex, int id)
        {
            this.FromVertex = fromVertex;
            this.ToVertex = toVertex;
            this.id = id;
        }
    };


}
