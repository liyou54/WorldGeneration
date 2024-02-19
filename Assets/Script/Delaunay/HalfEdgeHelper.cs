using UnityEngine;

namespace Delaunay
{
    
    
    public static class HalfEdgeHelper
    {
        public static VoronoiCell GetVoronoiCell(this VCellId cellId, Voronoi voronoi)
        {
            if (cellId == -1)
            {
                return new VoronoiCell();
            }
            
            return voronoi.Cells[cellId];
        } 

        public static HVertex GetHEdgeStartVertex(this HEdge edge, Delaunay delaunay)
        {

            if (edge.EndVertexId == -1)
            {
                return new HVertex();
            }
            
            return delaunay.Vertices[edge.StartVertexId];
        }

        public static HVertex GetHEdgeStartVertex(this HEdgeId edgeId, Delaunay delaunay)
        {
            var edge = edgeId.GetHEdge(delaunay);
            return edge.GetHEdgeStartVertex(delaunay);
        }

        public static HVertex GetHEdgeEndVertex(this HEdge edge, Delaunay delaunay)
        {
            
            if (edge.EndVertexId == -1)
                return new HVertex();
            
            return delaunay.Vertices[edge.EndVertexId];
        }

        public static HVertex GetHEdgeEndVertex(this HEdgeId edgeId, Delaunay delaunay)
        {
            var edge = edgeId.GetHEdge(delaunay);
            return edge.GetHEdgeEndVertex(delaunay);
        }

        public static HVertex GetHVertex(this HVertexId vertexId, Delaunay delaunay)
        {
            return delaunay.Vertices[vertexId];
        }

        public static HFace GetHFace(this HEdge edge, Delaunay delaunay)
        {

            if (edge.FaceId == -1)
            {
                return new HFace();
            }
            
            return delaunay.Faces[edge.FaceId];
        }
        
        

        public static HEdge GetHEdge(this HEdgeId id, Delaunay delaunay)
        {
            
            if (id == -1) return new HEdge();
            return delaunay.Edges[id];
        }
        
        public static HFace GetFace(this HEdgeId id,Delaunay delaunay)
        {
            if (id == -1)
            {
                return new HFace();
            }
            
            var edge = GetHEdge(id, delaunay);
            return GetHFace(edge, delaunay);
        }
        
        public static HVertex GetEndVertex(this HEdge edge, Delaunay delaunay)
        {
            if (edge.EndVertexId == -1)
            {
                return new HVertex();

            }
            
            return delaunay.Vertices[edge.EndVertexId];
        }
        
        public static Vector2 GetCenterPos(this HEdgeId edgeId, Delaunay delaunay)
        {
            var edge = GetHEdge(edgeId, delaunay);
            var start = GetStartVertex(edge, delaunay);
            var end = GetEndVertex(edge, delaunay);
            return (start.Pos + end.Pos) / 2;
        }
        
        public static Vector2 GetCenterPos(this HEdge edge, Delaunay delaunay)
        {

            return GetCenterPos(edge.Id, delaunay);
        }
        
        public static HVertex GetEndVertex(this HEdgeId edgeId, Delaunay delaunay)
        {

            if (edgeId == -1)
            {
                return new HVertex();
            }
            
            var sEdge = GetHEdge(edgeId, delaunay);
            return GetEndVertex(sEdge, delaunay);
        }
        
        public static HVertex GetStartVertex(this HEdge edge, Delaunay delaunay)
        {
            if (edge.StartVertexId == -1)
            {
                return new HVertex();
            }
            
            return delaunay.Vertices[edge.StartVertexId];
        }
        
        public static HVertex GetStartVertex(this HEdgeId edgeId, Delaunay delaunay)
        {
            
            if (edgeId == -1)
            {
                return new HVertex();
            }
            
            var sEdge = GetHEdge(edgeId, delaunay);
            return GetStartVertex(sEdge, delaunay);
        }
        
        
        public static HEdge GetNextEdge(this HEdge edge, Delaunay delaunay)
        {
            
            if (edge.NextEdgeId == -1)
            {
                return new HEdge();
            }
            
            return delaunay.Edges[edge.NextEdgeId];
        }

        public static HEdge GetNextEdge(this HEdgeId edgeId, Delaunay delaunay)
        {
            if (edgeId == -1)
            {
                return new HEdge();
            }
            var sEdge = GetHEdge(edgeId, delaunay);
            return GetNextEdge(sEdge, delaunay);
        }
        
        public static HEdge GetPreEdge(this HEdgeId edgeId, Delaunay delaunay)
        {
            if (edgeId == -1)
            {
                return new HEdge();
            }
            
            var sEdge = GetHEdge(edgeId, delaunay);
            return GetPreEdge(sEdge, delaunay);
        }
        
        public static HEdge GetTwinEdge(this HEdgeId edgeId, Delaunay delaunay)
        {
            if (edgeId == -1)
            {
                return new HEdge();
            }
            
            var sEdge = GetHEdge(edgeId, delaunay);
            return GetTwinEdge(sEdge, delaunay);
        }
        
        public static HEdge GetTwinEdge(this HEdge edge, Delaunay delaunay)
        {
            if (edge.TwinEdgeId == -1 || edge.TwinEdgeId >= delaunay.Edges.Count)
            {
                return new HEdge();
            }

            return delaunay.Edges[edge.TwinEdgeId];
        }

        public static HEdge GetPreEdge(this HEdge edge, Delaunay delaunay)
        {
            
            return GetNextEdge(edge.NextEdgeId, delaunay);
        }

        public static HFace GetHFace(this HFaceId id, Delaunay delaunay)
        {
            if (id == -1)
            {
                return new HFace();
            }
            
            return delaunay.Faces[id];
        }
    }
}