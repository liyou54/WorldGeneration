using System;
using System.Collections.Generic;
using Script.GameLaunch;
using UnityEngine;

namespace AreaManager
{
    public class AreaManager : GameSingleton<AreaManager>
    {
        [SerializeField] private int ChunkSize = 32;

        public void AddEntity(AreaComponent component)
        {
            var chunkId = PositionToChunkId(component.Entity.transform.position);
            var chunk = TryGetOrCreateChunk(chunkId);
            chunk.Add(component, cacheDic);
        }

        public List<AreaComponent> GetEntity(Rect rect)
        {
            var result = new List<AreaComponent>();
            foreach (var chunk in GetEntityIEnumerator(rect))
            {
                chunk.GetEntity(result, rect);
            }

            return result;
        }
        
        
        public void UpdateChunkPosition(AreaComponent component)
        {
            var oldPos = component.LastPosition;
            var newPos = component.Entity.transform.position;
            if (oldPos == newPos)
            {
                return;
            }

            var tree = cacheDic[component.Entity.Id];
            if (tree.Rect.Contains(newPos))
            {
                return;
            }

            tree.Datas.Remove(component);
            var chunkId = PositionToChunkId(newPos);
            var chunk = TryGetOrCreateChunk(chunkId);
            chunk.Add(component, cacheDic);
        }
        
        private QuadTree TryGetOrCreateChunk(Vector2Int chunkId)
        {
            if (!entitiesTree.TryGetValue(chunkId, out var chunk))
            {
                chunk = new QuadTree();
                chunk.Rect = new Rect(chunkId.x * ChunkSize, chunkId.y * ChunkSize, ChunkSize, ChunkSize);
                entitiesTree.Add(chunkId, chunk);
            }

            return chunk;
        }
        
        

        private IEnumerable<QuadTree> GetEntityIEnumerator(Rect rect)
        {
            var start = PositionToChunkId(rect.min);
            var end = PositionToChunkId(rect.max);
            for (int x = start.x; x <= end.x; x++)
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    if (entitiesTree.TryGetValue(new Vector2Int(x, y), out var chunk))
                    {
                        yield return chunk;
                    }
                }
            }
        }



        // 基于cell的四叉树管理
        private Dictionary<Vector2Int, QuadTree> entitiesTree = new();
        private Dictionary<long, QuadTree> cacheDic = new Dictionary<long, QuadTree>();

        private Vector2Int PositionToChunkId(Vector2 pos)
        {
            return new Vector2Int(pos.x > 0 ? (int)pos.x / 128 : (int)pos.x / 128 - 1,
                pos.y > 0 ? (int)pos.y / 128 : (int)pos.y / 128 - 1);
        }
        

        private class QuadTree
        {
            public List<AreaComponent> Datas = new List<AreaComponent>();
            public Rect Rect;
            private QuadTree[] Children;

            private void SplitChild(Dictionary<long, QuadTree> cacheDic)
            {
                var size = Rect.size / 2;
                Children = new QuadTree[4];
                for (int i = 0; i < 4; i++)
                {
                    Children[i] = new QuadTree();
                    Children[i].Rect = new Rect(Rect.x + size.x * (i % 2), Rect.y + size.y * (i / 2), size.x, size.y);
                }

                foreach (var areaComponent in Datas)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Children[i].Rect.Contains(areaComponent.LastPosition))
                        {
                            Children[i].Add(areaComponent, cacheDic);
                            break;
                        }
                    }
                }

                Datas.Clear();
            }

            private QuadTree GetChildByPos(Vector2 pos)
            {
                var index = 0;
                if (pos.x > Rect.x + Rect.width / 2)
                {
                    index += 1;
                }

                if (pos.y > Rect.y + Rect.height / 2)
                {
                    index += 2;
                }

                return Children[index];
            }


            public void GetEntity(List<AreaComponent> result, Rect rect)
            {
                if (Children != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Children[i].Rect.Overlaps(rect))
                        {
                            Children[i].GetEntity(result, rect);
                        }
                    }
                }
                else
                {
                    foreach (var entity in Datas)
                    {
                        if (rect.Contains(entity.LastPosition))
                        {
                            result.Add(entity);
                        }
                    }
                }
            }

            public void Add(AreaComponent component, Dictionary<long, QuadTree> cacheDic)
            {
                if (Children != null)
                {
                    var child = GetChildByPos(component.LastPosition);
                    child.Add(component, cacheDic);
                    return;
                }

                Datas.Add(component);
                cacheDic[component.Entity.Id] = this;
                if (Datas.Count > 31)
                {
                    SplitChild(cacheDic);
                }
            }
        }

        public override void OnInit()
        {
            base.OnInit();
        }
    }
}