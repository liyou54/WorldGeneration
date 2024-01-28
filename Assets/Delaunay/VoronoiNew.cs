using System.Collections.Generic;
using System.Linq;
using Delaunay;
using UnityEngine;
using UnityEngine.UIElements;


public class VoronoiPloygen
{
    public List<VoronoiEdge> edges;
    public Vector2 center;
}

public class VoronoiEdge
{
    public VoronoiPloygen VoronoiPloygen;
    public VoronoiEdge Twin;
    public VoronoiEdge NextEdgeLeft; // 逆时针
    public VoronoiEdge PreEdgeLeft;
    public VoronoiVertex StartVertex;
    public VoronoiVertex EndVertex;
}

public class VoronoiVertex
{
    public Vector2 position;
    public VoronoiEdge Edge; // 指向这个点的环边
}

public class VoronoiNew
{
    public List<VoronoiPloygen> Ploygens = new List<VoronoiPloygen>();
    public List<VoronoiEdge> Edges = new List<VoronoiEdge>();
    public List<VoronoiVertex> Vertices = new List<VoronoiVertex>();

    private void InitVertex(DelaunayGeo geo, Dictionary<DFace, VoronoiVertex> dicVVertexCache)
    {
        foreach (var face in geo.faces)
        {
            var voronoiNodeVertex = new VoronoiVertex();
            voronoiNodeVertex.position = face.center;
            Vertices.Add(voronoiNodeVertex);
            dicVVertexCache.Add(face, voronoiNodeVertex);
        }
    }

    public VoronoiEdge CreateVoronoiEdge(VoronoiVertex startVertex, VoronoiVertex endVertex, VoronoiPloygen ploygen, VoronoiEdge lastVoronoiEdge)
    {
        var edge = new VoronoiEdge();
        edge.StartVertex = startVertex;
        edge.EndVertex = endVertex;
        edge.EndVertex.Edge = edge;
        edge.VoronoiPloygen = ploygen;
        edge.PreEdgeLeft = lastVoronoiEdge;
        edge.VoronoiPloygen.edges.Add(edge);
        Edges.Add(edge);
        return edge;
    }

    public void BuildFromDelaunay(DelaunayGeo geo)
    {
        var dicVVertexCache = new Dictionary<DFace, VoronoiVertex>();
        var edgeCache = new Dictionary<(VoronoiVertex, VoronoiVertex), VoronoiEdge>();
        InitVertex(geo, dicVVertexCache);
        foreach (var vertex in geo.vertexs)
        {
            
            if (vertex.AdjacentEdge.Count == 0) continue;
            var edgeVoronoiPloygen = new VoronoiPloygen();
            edgeVoronoiPloygen.center = vertex.Pos;
            edgeVoronoiPloygen.edges = new List<VoronoiEdge>();
            Ploygens.Add(edgeVoronoiPloygen);
            VoronoiEdge lastVoronoiEdge = null;
            VoronoiEdge currentEdge = null;
            
            for (int i = 0; i < vertex.AdjacentEdge.Count - 1; i++)
            {
                var startFace = vertex.AdjacentEdge[i].Face;
                var endFace = vertex.AdjacentEdge[i + 1].Face;
                if (endFace == null || startFace == null) continue;
                currentEdge = CreateVoronoiEdge(dicVVertexCache[startFace], dicVVertexCache[endFace], edgeVoronoiPloygen, lastVoronoiEdge);
                if (lastVoronoiEdge != null)
                {
                    lastVoronoiEdge.NextEdgeLeft = currentEdge;
                }

                if (!edgeCache.TryGetValue((currentEdge.StartVertex, currentEdge.EndVertex), out var existingEdge))
                {
                    edgeCache.Add((currentEdge.EndVertex, currentEdge.StartVertex), currentEdge);
                }
                else
                {
                    existingEdge.Twin = currentEdge;
                    currentEdge.Twin = existingEdge;
                    edgeCache.Add((currentEdge.EndVertex, currentEdge.StartVertex), currentEdge);
                }

                lastVoronoiEdge = currentEdge;
            }

            if (!vertex.IsCorner)
            {
                var endFace = vertex.AdjacentEdge[0].Face;
                var startFace = vertex.AdjacentEdge.Last().Face;
                if (endFace == null || startFace == null) continue;
                
                 currentEdge = CreateVoronoiEdge(dicVVertexCache[startFace], dicVVertexCache[endFace], edgeVoronoiPloygen, lastVoronoiEdge);

                if (lastVoronoiEdge != null)
                {
                    lastVoronoiEdge.NextEdgeLeft = currentEdge;
                }

                if (!edgeCache.TryGetValue((currentEdge.StartVertex, currentEdge.EndVertex), out var existingEdge))
                {
                    if (!edgeCache.ContainsKey((currentEdge.StartVertex, currentEdge.EndVertex)))
                    {
                        edgeCache[(currentEdge.StartVertex, currentEdge.EndVertex)] = currentEdge;
                    }
                }
                else
                {
                    existingEdge.Twin = currentEdge;
                    currentEdge.Twin = existingEdge;
                }
                lastVoronoiEdge = currentEdge;
            }
            else
            {
                var startVertex = dicVVertexCache[vertex.AdjacentEdge.Last().Face];
                var endVertex = dicVVertexCache[vertex.AdjacentEdge[0].Face];
                if (vertex.AdjacentEdge.Count == 1)
                {
                    endVertex = new VoronoiVertex();
                    endVertex.position = vertex.AdjacentEdge[0].NextEdge.ToVertex.Pos;
                    Vertices.Add(endVertex);
                    CreateVoronoiEdge(endVertex, startVertex, edgeVoronoiPloygen, lastVoronoiEdge);
                }
                var midVertex = new VoronoiVertex();
                midVertex.position = vertex.Pos;
                Vertices.Add(midVertex);
                currentEdge = CreateVoronoiEdge(startVertex, midVertex, edgeVoronoiPloygen, lastVoronoiEdge);
                if (lastVoronoiEdge != null)
                {
                    lastVoronoiEdge.NextEdgeLeft = currentEdge;
                }
                lastVoronoiEdge = currentEdge;
                currentEdge = CreateVoronoiEdge(midVertex, endVertex, edgeVoronoiPloygen, lastVoronoiEdge);
                lastVoronoiEdge.NextEdgeLeft = currentEdge;
                lastVoronoiEdge = currentEdge;
            }
            currentEdge.NextEdgeLeft = edgeVoronoiPloygen.edges[0];
            currentEdge.NextEdgeLeft.PreEdgeLeft = currentEdge;
        }
    }

    public  Mesh BuildMesh()
    {
        Dictionary<VoronoiPloygen,int> dicPloygenIndex = new Dictionary<VoronoiPloygen, int>();
        Dictionary<VoronoiVertex,int> dicVertexIndex = new Dictionary<VoronoiVertex, int>();
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        var uv = new List<Vector2>();
        for (int i = 0; i < Ploygens.Count; i++)
        {
            var ploygen = Ploygens[i];
            var centerIndex = 0;
            if (!dicPloygenIndex.TryGetValue(ploygen, out centerIndex ))
            {
                uv.Add(new Vector2(0,0));
                centerIndex = vertices.Count;
                dicPloygenIndex.Add(ploygen, centerIndex );
                var centerVertex = new Vector3(ploygen.center.x, 0, ploygen.center.y);
                vertices.Add(centerVertex);
            }
            for (int j = 0; j < ploygen.edges.Count; j++)
            {
                var edge = ploygen.edges[j];
                var startVertex = edge.StartVertex;
                var endVertex = edge.EndVertex;
                if (!dicVertexIndex.TryGetValue(startVertex, out var startIndex))
                {
                    startIndex =  vertices.Count;
                    uv.Add(new Vector2(1,1));
                    dicVertexIndex.Add(startVertex,  startIndex );
                    vertices.Add(new Vector3(startVertex.position.x, 0, startVertex.position.y));

                }
                if (!dicVertexIndex.TryGetValue(endVertex, out var  endIndex))
                {
                    endIndex =  vertices.Count;
                    uv.Add(new Vector2(1,1));
                    dicVertexIndex.Add(endVertex,endIndex );
                    vertices.Add(new Vector3(endVertex.position.x, 0, endVertex.position.y));
                }
                
                triangles.Add(centerIndex);
                triangles.Add(startIndex);
                triangles.Add(endIndex);
            }
        }
        
        var mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetUVs(0,uv);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        return mesh;
    }
}