using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using World;
using Random = Unity.Mathematics.Random;

namespace Delaunay.Test
{
    public class TestDelaunay : MonoBehaviour
    {
        private DelaunayGeo geo;
        public float radius = 2;
        public Vector2 size = new Vector2(100, 100);
        private VoronoiNew voronoiNew;
        public Material testMat;
        public GraphicsBuffer gb;
        private int[] riverData;

        [Button]
        public void TestGeo()
        {
            using (new PerformanceTimer("总时间"))
            {
                geo = new DelaunayGeo();
                //
                var offset = radius;
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
                using (new PerformanceTimer("构建voronoi"))
                {
                    voronoiNew = new VoronoiNew();
                    voronoiNew.BuildFromDelaunay(geo);
                };
                using (new PerformanceTimer( "生成Mesh"))
                {
                    GetMesh();
                }
            }
           
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

            meshFilter.mesh = this.geo.BuildMesh();
            var river = new River();
            riverData = river.BuildRiver(geo,meshFilter.sharedMesh);
            gb = new GraphicsBuffer(GraphicsBuffer.Target.Structured, riverData.Length, 4);
            gb.SetData(riverData);
            meshRender.sharedMaterial = testMat;
            testMat.SetBuffer("TriTypeBuffer", gb);

            transform.position = Vector3.zero;
        }

        [Button]
        public void SetBuffer()
        {
            testMat.SetBuffer("TriTypeBuffer", gb);
        }

        private void Update()
        {
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
            for (int faceIndex = 0;faceIndex < geo.faces.Count; faceIndex++)
            {
                var face = geo.faces[faceIndex];
                var e = face.edge;
                // // 设置文字内容和样式
                // string text = faceIndex.ToString();
                // GUIStyle style = new GUIStyle();
                // style.normal.textColor = Color.white; // 文字颜色
                // style.fontSize = 20; // 文字大小
                // var center = new Vector3(face.center.x, 0, face.center.y);

                // 绘制文字
                // Handles.Label(center, text, style);
                // Gizmos.DrawSphere(center, .4f);
                for (int i = 0; i < 3; i++)
                {
                    var f = new Vector3(e.FromVertex.Pos.x, 0, e.FromVertex.Pos.y);
                    var to = new Vector3(e.ToVertex.Pos.x, 0, e.ToVertex.Pos.y);
                    var cen = new Vector3(face.center.x, 0, face.center.y);
                    f = Vector3.Lerp(cen, f, .9f);
                    to = Vector3.Lerp(cen, to, .9f);
                    // // 获取Scene视图中的Scene视图的Camera
                    // SceneView sceneView = SceneView.lastActiveSceneView;
                    // Camera camera = sceneView.camera;
                    //
                    // // 在Scene视图中绘制文字的位置
                    // Vector3 position = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f)); // 位置设为屏幕中心

                    // 绘制箭头
                    // var dir = (to - f).normalized;
                    // var right = Quaternion.Euler(0, 150, 0) * dir;
                    // var left = Quaternion.Euler(0, -150, 0) * dir;
                    // if (i == 0)
                    // {
                    //     Gizmos.color = Color.red;
                    // }
                    // else if (i == 1)
                    // {
                    //     Gizmos.color = Color.green;
                    // }
                    // else
                    // {
                    //     Gizmos.color = Color.blue;
                    // }
                    var col = Color.gray;
                    col.a = .4f;
                    Gizmos.color = col; 
                    // var mid = (f + to) / 2;
                    // Gizmos.DrawLine(mid, mid + right * 1);
                    // Gizmos.DrawLine(mid, mid + left * 1);

                    Gizmos.DrawLine(f, to);
                    e = e.NextEdge;
                }
            }
        }


        private void DrawRiverDebug()
        {
            if (riverData == null || geo == null) return;
            
            
            Gizmos.color = Color.cyan;
            
            for (int i = 0; i < riverData.Length; i++)
            {
                var river = riverData[i];

                var riverType = river >> 2;
                var riverOffset = river % 4;
                
                var face = geo.faces[i];
                var startEdge = face.edge;
                for (int off =0 ;off < riverOffset;off++)
                {
                    startEdge = startEdge.NextEdge;
                }
                var next = startEdge.NextEdge;
                var nextnext = next.NextEdge;
                var startPos = (startEdge.FromVertex.Pos + startEdge.ToVertex.Pos)/2;
                var startPoint = new Vector3(startPos.x, 0, startPos.y);
                if ((riverType & 0b01)>0)
                {
                    var end = (next.ToVertex.Pos + startEdge.ToVertex.Pos)/2;
                    var endPoint = new Vector3(end.x, 0, end.y);
                    Gizmos.DrawLine(startPoint, endPoint);
                }
                if ((riverType & 0b10)>0)
                {
                    var end = (nextnext.ToVertex.Pos + next.ToVertex.Pos)/2;
                    var endPoint = new Vector3(end.x, 0, end.y);
                    Gizmos.DrawLine(startPoint, endPoint);
                }
                
            }
        }

        private void OnDrawGizmos()
        {
            DrawDelaunay();
            // DrawRiverDebug();
            DrawVoronoiNew();
        }
    }
}