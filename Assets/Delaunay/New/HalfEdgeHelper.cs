// namespace Delaunay
// {
//     public static class HalfEdgeHelper
//     {
//         public static HVertex GetHEdgeStartVertex( this HEdge edge,DelaunayNew delaunay)
//         {
//             return delaunay.Vertices[edge.StartVertexId];
//         }
//         
//         public static HVertex GetHEdgeEndVertex( this HEdge edge,DelaunayNew delaunay)
//         {
//             return delaunay.Vertices[edge.EndVertexId];
//         }
//         
//         public static bool TestAttachToFace( HFace f)
//         {
//             var e = f.EdgeId;
//             for (int i = 0; i < 3; i++) {
//                 Vector2 dir = HEdgeUtil.GetEdgeDir(e);
//                 Vector2 p = Pos - HEdgeUtil.GetEdgeStartPos(e);
//                 if (Vector2Util.Cross(dir,p) < 0) return false;
//                 e = HEdgeUtil.GetNextEdge(e);
//             }
//             BelongFaceId = f.Id;
//             return true;
//         }
//         
//     }
// }