using System.Collections.Generic;
using System.Linq;
using Delaunay;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

// Voronoi环边
public class VoronoiNode
{
    public List<DEdge> edges;
    public DVertex center;
}

public class Voronoi
{
    public List<DFace> faces;
    public List<DEdge> edges;
    public List<DVertex> vertices = new List<DVertex>();

    // 环边
    public Dictionary<DVertex, VoronoiNode> nodes = new();

    private IDAllocator FaceIDAllocator = new IDAllocator();
    private IDAllocator VertexIDAllocator = new IDAllocator();
    private IDAllocator EdgeIDAllocator = new IDAllocator();
    const double EPS = 1e-9;

    public DVertex CreateVertex(float x, float y)
    {
        var id = VertexIDAllocator.AllocateID();
        var v = new DVertex(id, x, y);
        vertices.Add(v);
        return v;
    }

    public DEdge CreateEdge(DVertex from, DVertex to)
    {
        var id = EdgeIDAllocator.AllocateID();
        var e = new DEdge(from, to, id);
        edges.Add(e);
        return e;
    }

    public DFace CreateFace(DEdge edge)
    {
        var id = FaceIDAllocator.AllocateID();
        var f = new DFace(id, edge);
        faces.Add(f);
        return f;
    }


    public Voronoi()
    {
        faces = new List<DFace>();
        edges = new List<DEdge>();
        vertices = new List<DVertex>();
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


public void BuildFromDelaunay(DelaunayGeo geo)
{
    var vertexs = geo.vertexs;
    var vertexCache = new Dictionary<DFace, DVertex>();
    var edgeCache = new Dictionary<(DVertex, DVertex), DEdge>();

    foreach (var vertex in vertexs)
    {
        
        if (vertex.AdjacentEdge.Count == 0) continue;
        
        var center = CreateVertex(vertex.Pos.x, vertex.Pos.y);
        center.IsCorner = vertex.IsCorner;
        var node = new VoronoiNode { center = center, edges = new List<DEdge>() };
        nodes.Add(center, node);
        
        for (int i = 0; i < vertex.AdjacentEdge.Count - 1; i++)
        {
            var startFace = vertex.AdjacentEdge[i].Face;
            var endFace = vertex.AdjacentEdge[i + 1].Face;
            if (endFace == null || startFace == null) continue;

            ProcessEdge(vertexCache, edgeCache, center, startFace, endFace, node);
        }

        if (!vertex.IsCorner)
        {
            var endFace = vertex.AdjacentEdge[0].Face;
            var startFace = vertex.AdjacentEdge.Last().Face;
            if (endFace == null || startFace == null) continue;

            ProcessEdge(vertexCache, edgeCache, center, startFace, endFace, node);
        }
        else
        {
            var start = vertex.AdjacentEdge[0].Face.center;
            var mid = vertex;
            var end = vertex.AdjacentEdge.Last().Face.center;
            var startVertex = CreateVertex(start.x, start.y);
            var midVertex = CreateVertex(mid.Pos.x, mid.Pos.y);
            var endVertex = CreateVertex(end.x, end.y);

            var edge1 = CreateEdge(midVertex, startVertex);
            var edge2 = CreateEdge(startVertex, endVertex);
            var edge3 = CreateEdge(endVertex, midVertex);
            var face = CreateFace(edge1);
            node.edges.Add(edge1);
            node.edges.Add(edge3);

            ConstructFace(face, edge1, edge2, edge3);
        }
    }
}

private void ProcessEdge(Dictionary<DFace, DVertex> vertexCache, Dictionary<(DVertex, DVertex), DEdge> edgeCache, DVertex center, DFace startFace, DFace endFace, VoronoiNode node)
{
    DVertex startVertex = GetOrCreateVertex(vertexCache, startFace);
    DVertex endVertex = GetOrCreateVertex(vertexCache, endFace);

    var edge1 = CreateEdge(center, startVertex);
    var edge2 = CreateEdge(startVertex, endVertex);
    var edge3 = CreateEdge(endVertex, center);
    var face = CreateFace(edge1);
    node.edges.Add(edge2);
    ConstructFace(face, edge1, edge2, edge3);

    if (!edgeCache.TryGetValue((endVertex, startVertex), out var existingEdge))
    {
        if (!edgeCache.ContainsKey((startVertex, endVertex)))
        {
            edgeCache[(startVertex, endVertex)] = edge2;
        }
    }
    else
    {
        existingEdge.TwinEdge = edge2;
        edge2.TwinEdge = existingEdge;
    }
}

private DVertex GetOrCreateVertex(Dictionary<DFace, DVertex> vertexCache, DFace face)
{
    if (vertexCache.TryGetValue(face, out var vertex))
    {
        return vertex;
    }
    else
    {
        var newVertex = CreateVertex(face.center.x, face.center.y);
        vertexCache[face] = newVertex;
        return newVertex;
    }
}
}