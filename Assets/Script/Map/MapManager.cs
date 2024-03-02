using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.GameLaunch;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Map
{
    public class MapManager : GameSingleton<MapManager>
    {
        public float CellSize = 1;
        public int TileChunkSize = 32;
        
        
        protected Grid grid;
        public List<Tilemap> Tilemaps = new List<Tilemap>();
        public override UniTask OnInit()
        {
            var gridGo = new GameObject("Grid");
            grid = gridGo.AddComponent<Grid>();
            grid.cellSize = new Vector3(CellSize, CellSize, 0);
            grid.transform.position = new Vector3(-CellSize / 2f, -CellSize / 2f, 0);
            return UniTask.CompletedTask;
        }
    }
}