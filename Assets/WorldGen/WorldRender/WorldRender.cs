using UnityEngine;

namespace WorldGen.WorldRender
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class WorldRender:MonoBehaviour
    {
        
        
        public MeshRenderer meshRenderer { get; set; }
        public MeshFilter meshFilter { get; set; }
        public World World { get; set; }
        public Mesh mesh { get; set; }

        public void Render()
        {
            mesh = new Mesh();
            mesh.vertices = World.OutputPositions;
            mesh.triangles = World.OutputTriangles;
            mesh.RecalculateNormals();
            
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

        }
        
    }
}