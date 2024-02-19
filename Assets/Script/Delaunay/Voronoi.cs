using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
    public struct VCellId
    {
        private readonly int value;

        public VCellId(int value)
        {
            this.value = value;
        }

        public static implicit operator int(VCellId id)
        {
            return id.value;
        }

        public static implicit operator VCellId(int value)
        {
            return new VCellId(value);
        }
    }

    public struct VVertexId
    {
        private readonly int value;

        public VVertexId(int value)
        {
            this.value = value;
        }

        public static implicit operator int(VVertexId id)
        {
            return id.value;
        }

        public static implicit operator VVertexId(int value)
        {
            return new VVertexId(value);
        }
    }

    public struct VEdgeId
    {
        private readonly int value;

        public VEdgeId(int value)
        {
            this.value = value;
        }

        public static implicit operator int(VEdgeId id)
        {
            return id.value;
        }

        public static implicit operator VEdgeId(int value)
        {
            return new VEdgeId(value);
        }
    }

    public struct VoronoiCell
    {
        public List<VEdgeId> Edges;
        public HVertexId DelaunayVertexId;
        public VCellId CellId;
        public Vector2 Center;
    }

    public struct VoronoiEdge
    {
        public VCellId CellId;
        public VEdgeId EdgeId;
        public VEdgeId TwinEdgeId;
        public VEdgeId NextEdgeLeftId; // 逆时针
        public VEdgeId PreEdgeLeftId;
        public VVertexId StartVertexId;
        public VVertexId EndVertexId;
    }

    public struct VoronoiVertex
    {
        public VVertexId Id;
        public Vector2 Position;
        public HFaceId DelaunayFaceId;
        public VEdgeId Edge; // 指向这个点的环边
    }

    public class Voronoi
    {
        public List<VoronoiCell> Cells = new List<VoronoiCell>();
        public List<VoronoiEdge> Edges = new List<VoronoiEdge>();
        public List<VoronoiVertex> Vertices = new List<VoronoiVertex>();

        private VoronoiVertex CreateVertexByFace(Delaunay delaunay, HFaceId faceId)
        {
            var vertex = new VoronoiVertex();
            var delaunayFace = faceId.GetHFace(delaunay);
            vertex.Position = delaunayFace.Center;
            vertex.DelaunayFaceId = faceId;
            vertex.Id = Vertices.Count;
            Vertices.Add(vertex);
            return vertex;
        }

        private VoronoiVertex CreateVertex(Vector2 pos)
        {
            var vertex = new VoronoiVertex();
            vertex.Position = pos;
            vertex.Id = Vertices.Count;
            Vertices.Add(vertex);
            return vertex;
        }

        private VoronoiEdge CreateEdgeByVertex(VVertexId start, VVertexId end, VCellId cellId)
        {
            var edge = new VoronoiEdge();
            edge.EdgeId = Edges.Count;
            edge.StartVertexId = start;
            edge.EndVertexId = end;
            edge.CellId = cellId;
            Edges.Add(edge);
            return edge;
        }

        private VoronoiCell CreateCell(HVertexId vertexId, Delaunay delaunay)
        {
            var cell = new VoronoiCell();
            cell.CellId = Cells.Count;
            cell.Edges = new List<VEdgeId>();
            cell.Center = vertexId.GetHVertex(delaunay).Pos;
            cell.DelaunayVertexId = vertexId;
            Cells.Add(cell);
            return cell;
        }


        public void Build(Delaunay delaunay)
        {
            var edgeDict = new Dictionary<(VVertexId, VVertexId), VEdgeId>();

            bool UpdateCache(VVertexId start, VVertexId end, VEdgeId edgeId)
            {
                if (edgeDict.TryGetValue((end, start), out var twinEdgeId))
                {
                    var edge = Edges[edgeId];
                    edge.TwinEdgeId = twinEdgeId;
                    Edges[edgeId] = edge;
                    var twinEdge = Edges[twinEdgeId];
                    twinEdge.TwinEdgeId = edgeId;
                    Edges[twinEdgeId] = twinEdge;
                    return true;
                }

                edgeDict.Add((start, end), edgeId);
                return false;
            }

            // 从对偶图的顶点开始
            for (int i = 0; i < delaunay.Vertices.Count; i++)
            {
                
                var delaunayVertex = delaunay.Vertices[i];
                var neighborEdgeLast = delaunayVertex.AdjacentEdge[0];
                var vertexStart = CreateVertexByFace(delaunay, neighborEdgeLast.GetFace(delaunay).Id);
                var vertexLast = vertexStart;
                var cell = CreateCell(delaunayVertex.Id, delaunay);

                if (delaunayVertex.AdjacentEdge.Count <=1)
                {
                    continue;
                }
                
                var edgesList = cell.Edges;
                VoronoiEdge edgeLast = default;
                VoronoiEdge edgeStart = default;
                for (var neighborId = 1; neighborId < delaunayVertex.AdjacentEdge.Count; neighborId++)
                {
                    var neighborEdge = delaunayVertex.AdjacentEdge[neighborId];
                    var vcurrentVertex = CreateVertexByFace(delaunay, neighborEdge.GetFace(delaunay).Id);
                    var edge = CreateEdgeByVertex(vertexLast.Id, vcurrentVertex.Id, i);
                    
                    if (neighborId == 1)
                    {
                        edgeStart = edge;
                    }
                    
                    UpdateCache(vertexLast.Id, vcurrentVertex.Id, edge.EdgeId);
                    edgesList.Add(edge.EdgeId);
                    vertexLast = vcurrentVertex;
                    edgeLast.NextEdgeLeftId = edge.EdgeId;
                    edge.PreEdgeLeftId = edgeLast.EdgeId;
                    if(edgeLast.EdgeId > 0)
                        Edges[edgeLast.EdgeId] = edgeLast;
                    Edges[edge.EdgeId] = edge;
                    edgeLast = edge;
                }

                if (delaunayVertex.IsCornerVertex)
                {
                    var vertexVorMid = CreateVertex(delaunayVertex.Pos);
                    var e1 = CreateEdgeByVertex(vertexLast.Id, vertexVorMid.Id, i);
                    UpdateCache(vertexLast.Id, vertexVorMid.Id, e1.EdgeId);
                    edgeLast.NextEdgeLeftId = e1.EdgeId;
                    e1.PreEdgeLeftId = edgeLast.EdgeId;
                    edgesList.Add(e1.EdgeId);
                    Edges[edgeLast.EdgeId] = edgeLast;
                    Edges[e1.EdgeId] = e1;
                    edgeLast = e1;
                    var e2 = CreateEdgeByVertex(vertexVorMid.Id, vertexStart.Id, i);
                    UpdateCache(vertexVorMid.Id, vertexStart.Id, e2.EdgeId);
                    edgeLast.NextEdgeLeftId = e2.EdgeId;
                    e2.PreEdgeLeftId = edgeLast.EdgeId;
                    edgesList.Add(e2.EdgeId);
                    Edges[edgeLast.EdgeId] = edgeLast;
                    Edges[e2.EdgeId] = e2;
                    edgeLast = e2;
                    
                    edgeLast.NextEdgeLeftId = edgeStart.EdgeId;
                    edgeStart.PreEdgeLeftId = edgeLast.EdgeId;
                    Edges[edgeLast.EdgeId] = edgeLast;
                    Edges[edgeStart.EdgeId] = edgeStart;
                }
                else
                {
                    var e2 = CreateEdgeByVertex(vertexLast.Id, vertexStart.Id, i);
                    UpdateCache(vertexLast.Id, vertexStart.Id, e2.EdgeId);
                    edgeLast.NextEdgeLeftId = e2.EdgeId;
                    e2.PreEdgeLeftId = edgeLast.EdgeId;
                    edgesList.Add(e2.EdgeId);
                    Edges[edgeLast.EdgeId] = edgeLast;
                    Edges[e2.EdgeId] = e2;

                    edgeLast = e2;
                    edgeLast.NextEdgeLeftId = edgeStart.EdgeId;
                    edgeStart.PreEdgeLeftId = edgeLast.EdgeId;
                    Edges[edgeLast.EdgeId] = edgeLast;
                    Edges[edgeStart.EdgeId] = edgeStart;
                }
            }
        }
    }
}