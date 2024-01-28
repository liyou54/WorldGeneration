using UnityEngine;

namespace Delaunay
{
    public static class Vector2Util
    {
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }   
    }
}