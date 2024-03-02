using UnityEngine;

namespace Script.Map.Util
{
    public static class MapUtil
    {
        public static Vector2Int PositionToMapIndex(this MapManager mapManager, Vector2 position)
        {
            return new Vector2Int((int)(position.x / mapManager.CellSize), (int)(position.y / mapManager.CellSize));
        }

        public static Vector2 MapIndexToPositionStart(this MapManager mapManager, Vector2Int index)
        {
            return new Vector2(index.x * mapManager.CellSize, index.y * mapManager.CellSize);
        }

        public static Vector2 MapIndexToPositionCenter(this MapManager mapManager, Vector2Int index)
        {
            return new Vector2(index.x * mapManager.CellSize + mapManager.CellSize / 2f, index.y * mapManager.CellSize + mapManager.CellSize / 2f);
        }
        
    }
}