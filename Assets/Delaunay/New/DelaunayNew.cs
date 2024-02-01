// using System.Collections.Generic;
// using System.Numerics;
//
// namespace Delaunay
// {
//     public struct HEdge
//     {
//         public int TwinEdgeId;
//         public int NextEdgeId;
//         public int FaceId;
//         public int StartVertexId;
//         public int EndVertexId;
//         public int Id;
//         public HEdge(int formVertexId, int toVertexId,int id)
//         {
//             TwinEdgeId = -1;
//             NextEdgeId = -1;
//             FaceId = -1;
//             StartVertexId = formVertexId;
//             EndVertexId = toVertexId;
//             Id = id;
//         }
//         
//         public override string ToString()
//         {
//             return string.Format("HEdge {0} : {1} -> {2}", Id, StartVertexId, EndVertexId);
//         }
//     }
//     
//     public struct HFace
//     {
//         public int EdgeId;
//         public int Id;
//         public HFace(int edgeId,int id)
//         {
//             EdgeId = edgeId;
//             Id = id;
//         }
//         public override string ToString()
//         {
//             return string.Format("HFace {0} : {1}", Id, EdgeId);
//         }
//     }
//
//     public struct HVertex
//     {
//         public Vector2 Pos;
//         public int BelongFaceId;
//         public List<int> AdjacentEdge;
//         public bool IsCorner;
//         public int Id;
//         
//         public HVertex(int id, float x, float y)
//         {
//             this.Id = id;
//             Pos = new Vector2(x, y);
//             AdjacentEdge = new List<int>();
//             IsCorner = false;
//             BelongFaceId = -1;
//         }
//         
//
//     }
//
//     public class DelaunayNew
//     {
//         public List<HEdge> Edges;
//         public List<HFace> Faces;
//         public List<HVertex> Vertices;
//     }
// }