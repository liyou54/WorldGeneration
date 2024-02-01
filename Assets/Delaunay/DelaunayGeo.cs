using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Delaunay
{
    public class DelaunayGeo
    {
        public List<DEdge> edges = new List<DEdge>();
        public List<DVertex> vertexs = new List<DVertex>();
        public List<DFace> faces = new List<DFace>();
        private IDAllocator FaceIDAllocator = new IDAllocator();
        private IDAllocator VertexIDAllocator = new IDAllocator();
        private IDAllocator EdgeIDAllocator = new IDAllocator();
        private Vector2 MaxPos;
        private Vector2 MinPos;
        const double EPS = 1e-9;

        private DVertex CreateVertex(float x, float y)
        {
            var id = VertexIDAllocator.AllocateID();
            var v = new DVertex(id, x, y);
            vertexs.Add(v);
            return v;
        }

        private DEdge CreateEdge(DVertex from, DVertex to)
        {
            var id = EdgeIDAllocator.AllocateID();
            var e = new DEdge(from, to, id);
            edges.Add(e);
            return e;
        }

        private DFace CreateFace(DEdge edge)
        {
            var id = FaceIDAllocator.AllocateID();
            var f = new DFace(id, edge);
            faces.Add(f);
            return f;
        }

        private DFace CreateFace()
        {
            var id = FaceIDAllocator.AllocateID();
            var f = new DFace(id, null);
            faces.Add(f);
            return f;
        }

        Vector2 GetCenter(DVertex a, DVertex b, DVertex c)
        {
            Vector2 A = a.Pos;
            Vector2 B = b.Pos;
            Vector2 C = c.Pos;
            float a1 = B.x - A.x;
            float b1 = B.y - A.y;
            float c1 = (a1 * a1 + b1 * b1) / 2;
            float a2 = C.x - A.x;
            float b2 = C.y - A.y;
            float c2 = (a2 * a2 + b2 * b2) / 2;
            float d = a1 * b2 - a2 * b1;
            var res = new Vector2(A.x + (c1 * b2 - c2 * b1) / d, A.y + (a1 * c2 - a2 * c1) / d);
            return res;
        }

        bool InCircumcircle(DVertex a, DVertex b, DVertex c, DVertex d)
        {
            var center = GetCenter(a, b, c);
            var radius = Vector2.Distance(center, a.Pos);
            return Vector2.Distance(center, d.Pos) < radius;
        }

        private void ConstructFace(DFace face, DVertex a, DVertex b, DVertex c)
        {
            DEdge[] tempEdges = new DEdge[3];
            tempEdges[0] = CreateEdge(a, b);
            tempEdges[1] = CreateEdge(b, c);
            tempEdges[2] = CreateEdge(c, a);
            for (int i = 0; i < 3; i++)
            {
                tempEdges[i].NextEdge = tempEdges[(i + 1) % 3];
                tempEdges[i].Face = face;
            }

            face.edge = tempEdges[0];
        }

        private void ConstructFace(DFace face, DEdge a, DEdge b, DEdge c)
        {
            DEdge[] tempEdges = new DEdge[3];
            tempEdges[0] = a;
            tempEdges[1] = b;
            tempEdges[2] = c;
            for (int i = 0; i < 3; i++)
            {
                tempEdges[i].NextEdge = tempEdges[(i + 1) % 3];
                tempEdges[i].Face = face;
            }

            face.edge = a;
        }

        private void BuildFacedNeighbor()
        {
            Dictionary<DVertex, List<DEdge>> adjacentEdge = new();
            foreach (var edge in edges)
            {
                if (!adjacentEdge.ContainsKey(edge.FromVertex))
                {
                    var res = new List<DEdge>();
                    adjacentEdge[edge.FromVertex] = res;
                    var start = edge;
                    var cur = edge;

                    while (true)
                    {
                        if (cur.NextEdge.NextEdge.TwinEdge == null)
                        {
                            break;
                        }

                        cur = cur.NextEdge.NextEdge.TwinEdge;
                        if (cur == null || cur == start)
                        {
                            break;
                        }
                    }

                    edge.FromVertex.IsCorner = cur.NextEdge.NextEdge.TwinEdge == null;
                    start = cur;
                    res.Add(cur);
                    while (true)
                    {
                        if (cur.TwinEdge == null)
                        {
                            break;
                        }

                        cur = cur.TwinEdge.NextEdge;
                        res.Add(cur);
                        if (cur == start || cur.TwinEdge == null)
                        {
                            break;
                        }
                    }
                }
            }

            foreach (var kv in adjacentEdge)
            {
                kv.Key.AdjacentEdge = kv.Value;
            }
        }

        private void Insert(int x)
        {
            DVertex v = vertexs[x];
            DFace face = v.Belong;

            // 打碎原三角形，生成三个新的三角形

            DEdge fEdge = face.edge;
            DFace[] nFaces = new DFace[3];
            nFaces[0] = face;
            nFaces[1] = CreateFace();
            nFaces[2] = CreateFace();

            // nEdges 是逆时针的第一条边，cEdges是组成面的逆时针最后一条边
            DEdge[] nEdges = new DEdge[3];
            DEdge[] cEdges = new DEdge[3];
            DEdge[] oEdges = new DEdge[3];

            for (int i = 0; i < 3; i++)
            {
                nEdges[i] = CreateEdge(v, fEdge.FromVertex);
                cEdges[i] = CreateEdge(fEdge.ToVertex, v);
                oEdges[i] = fEdge;
                fEdge = fEdge.NextEdge;
            }

            for (int i = 0; i < 3; i++)
            {
                ConstructFace(nFaces[i], nEdges[i], oEdges[i], cEdges[i]);
                nEdges[i].TwinEdge = cEdges[(i + 2) % 3];
                cEdges[(i + 2) % 3].TwinEdge = nEdges[i];
            }


            var children = new List<DVertex>(nFaces[0].owned);
            for (int i = 0; i < 3; i++)
            {
                nFaces[i].owned.Clear();
            }

            foreach (var vs in children)
            {
                if (vs.id == v.id) continue;
                for (int i = 0; i < 3; i++)
                {
                    if (vs.TestAttachToFace(nFaces[i])) break;
                }
            }

            Queue<DEdge> Q = new Queue<DEdge>();

            for (int i = 0; i < 3; i++)
            {
                Q.Enqueue(oEdges[i]);
            }

            while (Q.Count > 0)
            {
                // var curEdge = Q.First();
                var curEdge = Q.Dequeue();

                var twin = curEdge.TwinEdge;
                if (twin == null) continue;

                var targetV = twin.NextEdge.ToVertex;
                if (targetV == v) continue;

                if (InCircumcircle(curEdge.FromVertex, curEdge.ToVertex, v, targetV))
                {
                    var nxt1 = twin.NextEdge;
                    // 添加可疑边
                    Q.Enqueue(nxt1);
                    Q.Enqueue(nxt1.NextEdge);

                    var A = curEdge.Face;
                    var B = twin.Face;

                    children.Clear();
                    foreach (var vs in A.owned) children.Add(vs);
                    foreach (var vs in B.owned) children.Add(vs);
                    A.owned.Clear();
                    B.owned.Clear();

                    var lastNxt = curEdge.NextEdge;
                    var lastPrev = lastNxt.NextEdge;

                    // Do edge flip
                    curEdge.FromVertex = v;
                    curEdge.ToVertex = targetV;
                    twin.FromVertex = targetV;
                    twin.ToVertex = v;

                    ConstructFace(A, curEdge, nxt1.NextEdge, lastNxt);
                    ConstructFace(B, twin, lastPrev, nxt1);

                    foreach (var vs in children)
                    {
                        if (vs.id == v.id) continue;
                        if (!vs.TestAttachToFace(A))
                        {
                            vs.Belong = B;
                            B.owned.Add(vs);
                        }
                    }
                }
            }
        }

        private void RemoveCorner()
        {
            var edgeToRemove = new HashSet<DEdge>();
            foreach (var corner in cornerVertexs)
            {
                var edgesToRemove = edges.Where(e => e.FromVertex == corner || e.ToVertex == corner);
                foreach (var edge in edgesToRemove)
                {
                    edgeToRemove.Add(edge);
                }
            }

            foreach (var edge in edgeToRemove)
            {
                if (edge.NextEdge.TwinEdge != null)
                {
                    edge.NextEdge.TwinEdge.TwinEdge = null;
                }

                // 删除边关联的面
                faces.Remove(edge.Face);
                faces.Remove(edge.NextEdge.Face);
                faces.Remove(edge.NextEdge.NextEdge.Face);

                // 删除边
                edges.Remove(edge.NextEdge.NextEdge);
                edges.Remove(edge.NextEdge);
                edges.Remove(edge);
            }

            // 删除关联的顶点
            foreach (var vertex in cornerVertexs)
            {
                vertexs.Remove(vertex);
            }
        }

        private DVertex[] cornerVertexs = new DVertex[4];

        private void CreateVertices(List<Vector2> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                CreateVertex(points[i].x, points[i].y);
            }
        }


        public void InitAABB(DFace f1, DFace f2, Vector2 maxPos, Vector2 minPos = default)
        {
            MaxPos = maxPos;
            MinPos = minPos;

            var size = maxPos - minPos;

            var min = -size;
            var max = size * 2;

            var p1 = CreateVertex(max.x, max.y);
            var p2 = CreateVertex(min.x, max.y);
            var p3 = CreateVertex(min.x, min.y);
            var p4 = CreateVertex(max.x, min.y);

            cornerVertexs[0] = p1;
            cornerVertexs[1] = p2;
            cornerVertexs[2] = p3;
            cornerVertexs[3] = p4;

            ConstructFace(f1, p1, p2, p4);
            ConstructFace(f2, p4, p2, p3);
            f1.edge.NextEdge.TwinEdge = f2.edge;
            f2.edge.TwinEdge = f1.edge.NextEdge;
        }

        public void Build(List<Vector2> points, Vector2 maxPos, Vector2 minPos = default)
        {
            using (new PerformanceTimer("Build Delaunay"))
            {
                CreateVertices(points);

                var f1 = CreateFace();
                var f2 = CreateFace();

                InitAABB(f1, f2, maxPos, minPos);

                for (int i = 0; i < points.Count; i++)
                {
                    DVertex v = vertexs[i];
                    if (!v.TestAttachToFace(f1))
                    {
                        v.Belong = f2;
                        f2.owned.Add(v);
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    Insert(i);
                }
            }


            using (new PerformanceTimer("Remove Center"))
            {
                RemoveCorner();
            }

            using (new PerformanceTimer("GetCenter"))
            {
                CalculateFaceCenters();
            }

            using (new PerformanceTimer("BuildFacedNeighbor"))
            {
                BuildFacedNeighbor();
            }

            Debug.Log($"顶点数量:{vertexs.Count} 边数量:{edges.Count} 面数量:{faces.Count}");
        }


        // TODO move toWorld
        public Mesh BuildMesh()
        {
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var uv = new List<Vector2>();
            var triangles = new List<int>();
            var hasBuildVertex = new Dictionary<DVertex, int>();
            foreach (var vertex in vertexs)
            {
                if (hasBuildVertex.ContainsKey(vertex))
                {
                    continue;
                }

                hasBuildVertex.Add(vertex, vertices.Count);
                vertices.Add(new Vector3(vertex.Pos.x, 0, vertex.Pos.y));
                uv.Add(vertex.Pos);
            }

            foreach (var face in faces)
            {
                var edge = face.edge;
                for (int i = 0; i < 3; i++)
                {
                    var vertex = edge.FromVertex;
                    var index = hasBuildVertex[vertex];
                    if (uv[index].x < 0)
                    {
                        uv[index] = new Vector2(face.center.x, face.center.y);
                    }

                    triangles.Add(index);
                    edge = edge.NextEdge.NextEdge;
                }
            }


            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uv);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            return mesh;
        }


        private void CalculateFaceCenters()
        {
            foreach (var face in faces)
            {
                if (face.edge.TwinEdge == null)
                {
                    face.center = (face.edge.FromVertex.Pos + face.edge.ToVertex.Pos) / 2;
                }
                else if (face.edge.NextEdge.TwinEdge == null)
                {
                    face.center = (face.edge.NextEdge.FromVertex.Pos + face.edge.NextEdge.ToVertex.Pos) / 2;
                }
                else if (face.edge.NextEdge.NextEdge.TwinEdge == null)
                {
                    face.center = (face.edge.NextEdge.NextEdge.FromVertex.Pos + face.edge.NextEdge.NextEdge.ToVertex.Pos) / 2;
                }
                else
                {
                    face.center = GetCenter(face.edge.FromVertex, face.edge.ToVertex, face.edge.NextEdge.ToVertex);
                    // clamp 
                    var x = Mathf.Clamp(face.center.x, MinPos.x, MaxPos.x);
                    var y = Mathf.Clamp(face.center.y, MinPos.y, MaxPos.y);
                    face.center = new Vector2(x, y);
                }
            }
        }
    }
}