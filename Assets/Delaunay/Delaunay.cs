using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Delaunay
{
    public struct HVertexId
    {
        private readonly int value;

        public HVertexId(int value)
        {
            this.value = value;
        }

        public static implicit operator int(HVertexId id)
        {
            return id.value;
        }

        public static implicit operator HVertexId(int value)
        {
            return new HVertexId(value);
        }

        public override string ToString()
        {
            return string.Format("HVertexId {0}", value);
        }
    }

    public struct HEdgeId
    {
        private readonly int value ;

        public HEdgeId(int value)
        {
            this.value = value;
        }

        public static implicit operator int(HEdgeId id)
        {
            return id.value;
        }

        public static implicit operator HEdgeId(int value)
        {
            return new HEdgeId(value);
        }
    }

    public struct HFaceId
    {
        private readonly int value;

        public HFaceId(int value)
        {
            this.value = value;
        }

        public static implicit operator int(HFaceId id)
        {
            return id.value;
        }

        public static implicit operator HFaceId(int value)
        {
            return new HFaceId(value);
        }

        public override string ToString()
        {
            return string.Format("HFaceId {0}", value);
        }
    }

    public struct HEdge
    {
        public HEdgeId TwinEdgeId ;
        public HEdgeId NextEdgeId  ;
        public HFaceId FaceId;
        public HVertexId StartVertexId;
        public HVertexId EndVertexId;
        public HEdgeId Id;
        
        public HEdge(HEdgeId id = default)
        {
            TwinEdgeId = -1;
            NextEdgeId = -1;
            FaceId = -1;
            StartVertexId = -1;
            EndVertexId = -1;
            Id = id;
        }
        
        public HEdge(HVertexId formVertexId, HVertexId toVertexId, HEdgeId id)
        {
            TwinEdgeId = -1;
            NextEdgeId = -1;
            FaceId = -1;
            StartVertexId = formVertexId;
            EndVertexId = toVertexId;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("HEdge {0} : {1} -> {2}", Id, StartVertexId, EndVertexId);
        }
    }

    public struct HFace
    {
        public HEdgeId EdgeId;
        public HFaceId Id;
        public Vector2 Center;
        public int VoronoiVertexId;

        public HFace(HEdgeId edgeId =default)
        {
            EdgeId = -1;
            Id = -1;
            Center = Vector2.zero;
            VoronoiVertexId = -1;
        }

        
        public HFace(HEdgeId edgeId, HFaceId id)
        {
            EdgeId = edgeId;
            Id = id;
            Center = Vector2.zero;
            VoronoiVertexId = -1;
        }

        public override string ToString()
        {
            return string.Format("HFace {0} : {1}", Id, EdgeId);
        }
    }

    public struct HVertex
    {
        public Vector2 Pos;
        public HFaceId BelongFaceId;
        public int VoronoiCellId;
        public List<HEdgeId> AdjacentEdge;
        public bool IsCornerVertex;
        public HVertexId Id;
        
        public HVertex(HVertexId id = default)
        {
            this.Id = id;
            Pos = Vector2.zero;
            AdjacentEdge = new List<HEdgeId>();
            IsCornerVertex = false;
            BelongFaceId = -1;
            VoronoiCellId = -1;
        }
        public HVertex(HVertexId id, float x, float y)
        {
            this.Id = id;
            Pos = new Vector2(x, y);
            AdjacentEdge = new List<HEdgeId>();
            IsCornerVertex = false;
            BelongFaceId = -1;
            VoronoiCellId = -1;
        }
    }

    public class Delaunay
    {
        public List<HEdge> Edges;
        public List<HFace> Faces;
        public List<HVertex> Vertices;

        public enum RotationDirection
        {
            Clockwise,
            CounterClockwise
        }

        // 自定义的向量排序方法，按照指定的旋转方向排序
        public static List<Vector2> SortVectorsByRotation(List<Vector2> vectors, RotationDirection direction)
        {
            // 计算每个向量的极角（从 x 轴正方向开始）
            List<(Vector2 vector, double angle)> vectorAngles = vectors
                .Select(v => (v, Math.Atan2(v.y, v.x)))
                .ToList();

            // 根据旋转方向进行排序
            List<Vector2> sortedVectors;
            if (direction == RotationDirection.CounterClockwise)
            {
                sortedVectors = vectorAngles
                    .OrderByDescending(va => va.angle)
                    .Select(va => va.vector)
                    .ToList();
            }
            else // 如果是顺时针旋转
            {
                sortedVectors = vectorAngles
                    .OrderBy(va => va.angle)
                    .Select(va => va.vector)
                    .ToList();
            }

            return sortedVectors;
        }

        public void Build(Vector3[] vertices, int[] triangles)
        {
            Edges = Enumerable.Repeat(new HEdge(), triangles.Length).ToList();
            Faces = Enumerable.Repeat(new HFace(), triangles.Length / 3).ToList();
            Vertices = Enumerable.Repeat(new HVertex(), vertices.Length).ToList();

            // 开始顶点 结束顶点 边索引
            var chacheSingleEdge = new Dictionary<(HVertexId, HVertexId), HEdgeId>();
            var hasBuildVertex = new HashSet<int>();
            for (int i = 0; i < triangles.Length / 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var e = new HEdge();
                    e.Id = i * 3 + j;
                    e.StartVertexId = triangles[i * 3 + j];
                    e.EndVertexId = triangles[i * 3 + (j + 1) % 3];
                    e.FaceId = i;
                    e.NextEdgeId = i * 3 + (j + 1) % 3;
                    e.TwinEdgeId = -1;
                    // 顶点
                    HVertex hVertex = default;
                    if (!hasBuildVertex.Contains(e.StartVertexId))
                    {
                        hVertex = new HVertex(e.StartVertexId, vertices[e.StartVertexId].x, vertices[e.StartVertexId].z);
                        hVertex.BelongFaceId = i;
                        hasBuildVertex.Add(e.StartVertexId);
                    }
                    else
                    {
                        hVertex = Vertices[e.StartVertexId];
                    }

                    if (chacheSingleEdge.ContainsKey((e.EndVertexId, e.StartVertexId)))
                    {
                        var twinEdgeId = chacheSingleEdge[(e.EndVertexId, e.StartVertexId)];
                        chacheSingleEdge.Remove((e.EndVertexId, e.StartVertexId));
                        e.TwinEdgeId = twinEdgeId;
                        var twin = Edges[twinEdgeId];
                        twin.TwinEdgeId = e.Id;
                        Edges[twinEdgeId] = twin;
                    }
                    else
                    {
                        chacheSingleEdge.Add((e.StartVertexId, e.EndVertexId), e.Id);
                    }

                    hVertex.AdjacentEdge.Add(e.Id);
                    Vertices[e.StartVertexId] = hVertex;
                    Edges[e.Id] = e;
                }

                var f = new HFace();
                f.Id = i;
                f.EdgeId = i * 3;
                var v1 = Vertices[triangles[(i * 3)]];
                var v2 = Vertices[triangles[(i * 3 + 1)]];
                var v3 = Vertices[triangles[(i * 3 + 2)]];
                var center = (v1.Pos + v2.Pos + v3.Pos) / 3;
                f.Center = center;
                Faces[i] = f;
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                var vertex = Vertices[i];
                var adjacentEdge = vertex.AdjacentEdge;
                var start = vertex.AdjacentEdge[0];
                vertex.AdjacentEdge.Clear();
                var cur = start;
                while (true)
                {
                    var temp = cur.GetNextEdge(this).GetNextEdge(this).TwinEdgeId;
                    if (temp == -1)
                    {
                        break;
                    }

                    cur = temp;
                    if (cur == start)
                    {
                        break;
                    }
                }

                adjacentEdge.Add(cur);
                start = cur;
                while (true)
                {
                    if (cur.GetHEdge(this).TwinEdgeId == -1)
                    {
                        break;
                    }

                    cur = cur.GetTwinEdge(this).NextEdgeId;
                    adjacentEdge.Add(cur);
                    if (cur == start || cur.GetHEdge(this).TwinEdgeId == -1)
                    {
                        break;
                    }
                }

                var endNeighbor = adjacentEdge[^1];
                
                vertex.IsCornerVertex = endNeighbor.GetHEdge(this).TwinEdgeId == -1;

                // 边界中心需要重新构建

                if (vertex.IsCornerVertex)
                {
                    var startNeightbor = adjacentEdge[0];
                    var center = (vertex.Pos + startNeightbor.GetPreEdge(this).GetStartVertex(this).Pos) / 2;
                    var face = startNeightbor.GetHEdge(this).GetHFace(this);
                    face.Center =center ;
                    if (adjacentEdge.Count == 1)
                    {
                        face.Center =vertex.Pos ;
                    }
                    Faces[face.Id] = face;
                }
                
                Vertices[i] = vertex;


                
            }
        }
    }
}