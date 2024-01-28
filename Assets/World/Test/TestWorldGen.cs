using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World.Test
{
    public class TestWorldGen : MonoBehaviour
    {
        public World World;
        public WorldConfig config;
        [Button] 
        public void Test()
        {
            World = new World(config);
            World.Voronoi.BuildMesh();
            
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
            meshFilter.mesh = World.Voronoi.BuildMesh();

        }

        private Color RandomColor()
        {
            return new Color(Random.value, Random.value, Random.value);
        }
        List<Color> colorList = new ();

        void DrawArrow(Vector3 position, Vector3 direction, float length)
        {
            Handles.DrawAAPolyLine(10, position, position + direction * length);

            // 计算箭头的朝向
            Vector3 arrowEnd = position + direction * length;
            float arrowHeadLength = 5f;
            float arrowHeadAngle = 20f;

            // 计算箭头两侧的点
            Vector3 rightPoint = arrowEnd + Quaternion.Euler(0, arrowHeadAngle, 0) * (-direction) * arrowHeadLength;
            Vector3 leftPoint = arrowEnd + Quaternion.Euler(0, -arrowHeadAngle, 0) * (-direction) * arrowHeadLength;

            // 绘制箭头两侧的线段，形成三角形
            Handles.DrawAAPolyLine(7, arrowEnd, rightPoint);
            Handles.DrawAAPolyLine(7, arrowEnd, leftPoint);
        }
        
        
        
        private void OnDrawGizmos()
        {

            if (colorList.Count == 0 || colorList.Count != config.PlateCount)
            {
                colorList.Clear();
                for (int i = 0; i < config.PlateCount; i++)
                {
                    colorList.Add(RandomColor());
                }
            }
            
            
            
            
            if (World == null) return;
            if (World.Voronoi == null) return;
            var index = 0;
            foreach (var nodes in World.PlatesRes)
            {
                var color = colorList[index];
                var center = new Vector2();
                foreach (var node in nodes.Nodes)
                {
                    var colorWithH = color * node.Height;
                    Gizmos.color = colorWithH;
                    center += node.Node.center;
            
                    if (node.Height < .4)
                    {
                        continue;
                    }
                    
                    foreach (var edge in node.Node.edges)
                    {
                        var start = new Vector3(edge.StartVertex.position.x, 0, edge.StartVertex.position.y);
                        var end = new Vector3(edge.EndVertex.position.x, 0, edge.EndVertex.position.y);
                        Gizmos.DrawLine(start, end);
                    }

                    
                }
                center /= nodes.Nodes.Count;
                Handles.color = colorList[index] * 1.52f;
                DrawArrow(new Vector3(center.x, 0, center.y), new Vector3(nodes.FVector.x, 0, nodes.FVector.y), 10);
                index++;
            
            }
            Handles.color = Color.white;
            Gizmos.color = Color.white;
            
        }
        
    }
}