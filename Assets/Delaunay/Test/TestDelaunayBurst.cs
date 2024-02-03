using System;
using andywiecko.BurstTriangulator;
using AwesomeNamespace;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using World;

namespace Delaunay.Test
{
    public class TestDelaunayBurst : MonoBehaviour
    {
        public Vector2 lowerRight = new Vector2(100, 100);
        public float minimumDistance = 2f;
        public Material material;
        
        public bool bDrawVoronoiDiagram = true;
        public bool bDrawDelaunayTriangulation = true;
       public bool bDrawRiver = true;
        
        private Delaunay delaunay;
        private Voronoi voronoi;
        private River _river;
        private int[] _riverBuffer;
        private Mesh mesh;

        [Button]
        public void Test()
        {
            var offset = minimumDistance;
            var divSize = offset * 2;
            var centerPoints = UniformPoissonDiskSampler.SampleRectangle(new Vector2(offset, offset), lowerRight - new Vector2(1, 1) * offset, minimumDistance);

            for (int i = 1; i < MathF.Ceiling((lowerRight.x) / divSize); i++)
            {
                var x = i * divSize > lowerRight.x ? lowerRight.x : i * divSize;
                centerPoints.Add(new Vector2(x + Single.Epsilon, 0));
                centerPoints.Add(new Vector2(x - Single.Epsilon, lowerRight.y));
            }

            for (int i = 1; i < MathF.Ceiling((lowerRight.y) / divSize); i++)
            {
                var y = i * divSize > lowerRight.y ? lowerRight.y : i * divSize;
                centerPoints.Add(new Vector2(0, y));
                centerPoints.Add(new Vector2(lowerRight.x, y));
            }

            centerPoints.Add(new Vector2(0, 0));
            centerPoints.Add(new Vector2(lowerRight.x, 0));
            centerPoints.Add(new Vector2(0, lowerRight.y));
            centerPoints.Add(new Vector2(lowerRight.x, lowerRight.y));

            var dataFloat2 = centerPoints.ConvertAll((vec) => new float2(vec.x, vec.y)).ToArray();


            using var positions = new NativeArray<float2>(dataFloat2, Allocator.Persistent);

            using var triangulator = new Triangulator(capacity: 1024, Allocator.Persistent)
            {
                Input = { Positions = positions }
            };
            using (new PerformanceTimer("burst"))
            {
                triangulator.Run();
            }

            Debug.Log($"TriCount:{triangulator.Output.Triangles.Length}");
            var outputTriangles = triangulator.Output.Triangles;
            var outputPositions = triangulator.Output.Positions;
            mesh = new Mesh();

            var triArray = new int[outputTriangles.Length];
            for (var i = 0; i < outputTriangles.Length; i++)
            {
                triArray[i] = outputTriangles[i];
            }

            var posArray = new Vector3[outputPositions.Length];
            for (var i = 0; i < outputPositions.Length; i++)
            {
                var temp = outputPositions[i];
                posArray[i] = new Vector3(temp.x, 0, temp.y);
            }

            mesh.vertices = posArray;
            mesh.triangles = triArray;

            delaunay = new Delaunay();
            delaunay.Build(posArray, triArray);
            voronoi = new Voronoi();
            voronoi.Build(delaunay);
            _river = new River();
            TestRiver();
        }

        private void TestRiver()
        {
            if (_river == null)
            {
                return;
            }

            _riverBuffer = _river.BuildRiver(delaunay);
            var go = GameObject.Find("River");
            if (go == null)
            {
                go = new GameObject("River");
            }

            if (go.GetComponent<MeshFilter>() == null)
            {
                go.AddComponent<MeshFilter>();
            }

            if (go.GetComponent<MeshRenderer>() == null)
            {
                go.AddComponent<MeshRenderer>();
            }

            var meshFilter = go.GetComponent<MeshFilter>();
            var meshRenderer = go.GetComponent<MeshRenderer>();
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            var gb = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _riverBuffer.Length, 4);
            gb.SetData(_riverBuffer);
            material.SetBuffer("TriTypeBuffer", gb);
        }

        private void DrawVoronoi()
        {
            if (voronoi == null) return;
            Gizmos.color = Color.red;
            foreach (var cell in voronoi.Cells)
            {
                foreach (var edgeId in cell.Edges)
                {
                    var edge = voronoi.Edges[edgeId];
                    var start = voronoi.Vertices[edge.StartVertexId].Position;
                    var end = voronoi.Vertices[edge.EndVertexId].Position;
                    var center = cell.Center;
                    var p1 = Vector2.Lerp(start, center, .1f);
                    var p2 = Vector2.Lerp(end, center, .1f);
                    Gizmos.DrawLine(new Vector3(p1.x, 0, p1.y), new Vector3(p2.x, 0, p2.y));
                }
            }

            Gizmos.color = Color.white;
        }

        public void OnDrawGizmos()
        {

      

            if (bDrawDelaunayTriangulation)
            {
                DrawDelaunay();
            }
            if (bDrawVoronoiDiagram)
            {
                DrawVoronoi();
            }
            if (bDrawRiver)
            {
                OnDrawRiver();
            }
        }

        private void OnDrawRiver()
        {
            if (_river == null)
            {
                return;
            }
            Gizmos.color = Color.blue;
            for (int i = 0; i < _riverBuffer.Length;i++)
            {
                var buf = _riverBuffer[i];
                var triRotNum = (buf >> 18) % 4;
                var triType = (buf >> 20) % 4;
                HEdgeId startEdge = delaunay.Faces[i].EdgeId;
                for (int j = 0; j < triRotNum; j++)
                {
                    startEdge = startEdge.GetNextEdge(delaunay).Id;
                }
                
                var start = startEdge.GetCenterPos(delaunay);
                
                if ((triType & 0b1)> 0)
                {
                    var end = startEdge.GetNextEdge(delaunay).GetCenterPos(delaunay);
                    Gizmos.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y));
                }
                if ((triType & 0b10)> 0)
                {
                    var end = startEdge.GetNextEdge(delaunay).GetNextEdge(delaunay).GetCenterPos(delaunay);
                    Gizmos.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y));
                }
                
            }
            Gizmos.color = Color.white;
        }
        
        private void DrawDelaunay()
        {
            if (delaunay == null) return;
            foreach (var edge in delaunay.Edges)
            {
                var start = edge.GetHEdgeStartVertex(delaunay);
                var end = edge.GetHEdgeEndVertex(delaunay);
                var face = edge.GetHFace(delaunay);
                var center = face.Center;
                var p1 = Vector2.Lerp(start.Pos, center, .0f);
                var p2 = Vector2.Lerp(end.Pos, center, .0f);
                Gizmos.DrawLine(new Vector3(p1.x, 0, p1.y), new Vector3(p2.x, 0, p2.y));
            }
            Gizmos.color = Color.white;

        }
        
        
    }
}