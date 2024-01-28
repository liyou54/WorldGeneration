using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Delaunay.Test
{
    public class TestDelaunay : MonoBehaviour
    {
        private DelaunayGeo geo;
        public float radius = 2;
        public Vector2 size = new Vector2(100, 100);
        private Voronoi voronoi;
        private VoronoiNew voronoiNew;

        [Button]
        public void TestGeo()
        {
            geo = new DelaunayGeo();
            //
            var offset = radius ;
            var divSize = offset * 3;
            var centerPoints = PoissonDiscSampling.GeneratePoints(radius, size - new Vector2(1, 1) * offset * 2);

            for (int i = 0; i < centerPoints.Count; i++)
            {
                centerPoints[i] += new Vector2(offset, offset);
            }

            var borderList = new List<Vector2>();
            for (int i = 1; i < MathF.Ceiling((size.x) / divSize); i++)
            {
                var x = i * divSize > size.x ? size.x : i * divSize;
                borderList.Add(new Vector2(x + Single.Epsilon, 0));
                borderList.Add(new Vector2(x - Single.Epsilon, size.y));
            }

            for (int i = 1; i < MathF.Ceiling((size.y) / divSize); i++)
            {
                var y = i * divSize > size.y ? size.y : i * divSize;
                borderList.Add(new Vector2(0, y));
                borderList.Add(new Vector2(size.x, y));
            }

            borderList.Add(new Vector2(0, 0));
            borderList.Add(new Vector2(size.x, 0));
            borderList.Add(new Vector2(0, size.y));
            borderList.Add(new Vector2(size.x, size.y));

            borderList = borderList.Distinct().ToList();
            borderList.AddRange(centerPoints);

            geo.Build(borderList, size);
            voronoi = new Voronoi();
            voronoi.BuildFromDelaunay(geo);
            voronoiNew = new VoronoiNew();
            voronoiNew.BuildFromDelaunay(geo);
            GetMesh();
        }

        private void GetMesh()
        {
            var meshRender = GetComponent<MeshRenderer>();
            if (meshRender == null)
            {
                meshRender = gameObject.AddComponent<MeshRenderer>();
            }
            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh =this.voronoiNew.BuildMesh();
            transform.position = Vector3.zero;
        }
        
        private void DrawVoronoiNew()
        {
            Gizmos.color = Color.blue;
            if (voronoiNew == null) return;
            foreach (var node in voronoiNew.Ploygens)
            {
                foreach (var edge in node.edges)
                {
                    var f = new Vector3(edge.StartVertex.position.x, 0, edge.StartVertex.position.y);
                    var to = new Vector3(edge.EndVertex.position.x, 0, edge.EndVertex.position.y);
                    var cen = new Vector3(node.center.x, 0, node.center.y);
                    f = Vector3.Lerp(cen, f, .9f);
                    to = Vector3.Lerp(cen, to, .9f);

                    // 绘制箭头
                    var dir = (to - f).normalized;
                    var right = Quaternion.Euler(0, 30, 0) * dir;
                    var left = Quaternion.Euler(0, -30, 0) * dir;
                    var mid = (f + to) / 2;
                    Gizmos.DrawLine(mid, mid + right * 1);
                    Gizmos.DrawLine(mid, mid + left * 1);

                    Gizmos.DrawLine(f, to);
                }
            }
        }

        private void DrawDelaunay()
        {
            if (geo == null) return;
            foreach (var face in geo.faces)
            {
                var e = face.edge;
                var center = new Vector3(face.center.x, 0, face.center.y);
                Gizmos.DrawSphere(center, .4f);
                for (int i = 0; i < 3; i++)
                {
                    var f = new Vector3(e.FromVertex.Pos.x, 0, e.FromVertex.Pos.y);
                    var to = new Vector3(e.ToVertex.Pos.x, 0, e.ToVertex.Pos.y);
                    var cen = new Vector3(face.center.x, 0, face.center.y);
                    f = Vector3.Lerp(cen, f, .9f);
                    to = Vector3.Lerp(cen, to, .9f);
                    
                    // 绘制箭头
                    var dir = (to - f).normalized;
                    var right = Quaternion.Euler(0, 30, 0) * dir;
                    var left = Quaternion.Euler(0, -30, 0) * dir;
                    var mid = (f + to) / 2;
                    Gizmos.DrawLine(mid, mid + right * 1);
                    Gizmos.DrawLine(mid, mid + left * 1);

                    Gizmos.DrawLine(f, to);
                    e = e.NextEdge;
                }
            }
        }

        private void DrawVoronoi()
        {
            Gizmos.color = Color.blue;
            if (voronoi == null) return;
            foreach (var node in voronoi.nodes)
            {
                foreach (var edge in node.Value.edges)
                {
                    var f = new Vector3(edge.FromVertex.Pos.x, 0, edge.FromVertex.Pos.y);
                    var to = new Vector3(edge.ToVertex.Pos.x, 0, edge.ToVertex.Pos.y);
                    var cen = new Vector3(node.Key.Pos.x, 0, node.Key.Pos.y);
                    f = Vector3.Lerp(cen, f, .9f);
                    to = Vector3.Lerp(cen, to, .9f);
                    
                    // 绘制箭头
                    var dir = (to - f).normalized;
                    var right = Quaternion.Euler(0, 30, 0) * dir;
                    var left = Quaternion.Euler(0, -30, 0) * dir;
                    var mid = (f + to) / 2;
                    Gizmos.DrawLine(mid, mid + right * 1);
                    Gizmos.DrawLine(mid, mid + left * 1);
                    
                    Gizmos.DrawLine(f, to);
                }


            }

            Gizmos.color = Color.white;
        }

        private void OnDrawGizmos()
        {
            DrawDelaunay();
            DrawVoronoiNew();
        }
    }
}