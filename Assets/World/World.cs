using System;
using System.Collections.Generic;
using System.Linq;
using Delaunay;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace World
{
    [Serializable]
    public class WorldConfig
    {
        public Vector2 Size;
        public float Density;
        [Range(2, 20)] public int PlateCount;
        [Range(0, 1f)] public float LandHeight;
    }

    public class MapCell
    {
        public VoronoiPloygen Node;
        public float Height;
        public bool Island { get; set; }

        public MapCell(VoronoiPloygen node, float height = 0)
        {
            Node = node;
            Height = height;
        }

    }

    public class Plate
    {
        public List<MapCell> Nodes;
        public Vector2 FVector;
        public Dictionary<VoronoiPloygen, MapCell> Set = new();
    }

    public class World
    {
        WorldConfig config;
        public DelaunayGeo DelaunayGeo;
        public VoronoiNew Voronoi;
        public List<Plate> PlatesRes;

        public void BuildPlatesNew()
        {
            
        }
        
        public void BuildPlates()
        {
            var nodes = Voronoi.Ploygens;
            var vertexRemian = new HashSet<VoronoiPloygen>(nodes);
            var random = new System.Random();
            PlatesRes = new List<Plate>();
            List<List<VoronoiPloygen>> plateWaiting = new List<List<VoronoiPloygen>>();

            for (int i = 0; i < config.PlateCount; i++)
            {
                plateWaiting.Add(new List<VoronoiPloygen>());
                var plate = new Plate();
                plate.Nodes = new List<MapCell>();
                plate.FVector = (new Vector2((float)random.NextDouble(), (float)random.NextDouble()) - new Vector2(.5f, .5f)).normalized;
                PlatesRes.Add(plate);
                var ploygen = vertexRemian.ElementAt(random.Next(0, vertexRemian.Count));
                var cell = new MapCell(ploygen);
                PlatesRes[i].Nodes.Add(cell);
                PlatesRes[i].Set.Add(ploygen, cell);
                plateWaiting[i].Add(ploygen);
                vertexRemian.Remove(ploygen);
            }

            while (true)
            {
                var allEmpty = true;
                foreach (var remain in plateWaiting)
                {
                    if (remain.Count > 0)
                    {
                        allEmpty = false;
                        break;
                    }
                }
                if (allEmpty) break;

                var landIndex = random.Next(0, config.PlateCount);
                var plateWait = plateWaiting[landIndex];
                var plateRes = PlatesRes[landIndex];
                var temp = random.Next(0, plateWait.Count);
                if (temp >= plateWait.Count) continue;
                
                var ploygen = plateWait[temp];
                plateWait.Remove(ploygen);
                for (int i = 0; i < ploygen.edges.Count; i++)
                {
                    var edge = ploygen.edges[i];
                    if (edge.Twin == null) continue;
                    var toVertex = edge.Twin.VoronoiPloygen;
                    if (vertexRemian.Contains(toVertex))
                    {
                        plateWait.Add(toVertex);
                        var cell = new MapCell(toVertex);
                        plateRes.Nodes.Add(cell);
                        plateRes.Set.Add(toVertex, cell);
                        vertexRemian.Remove(toVertex);
                    }
                }
            }
        }

        public World(WorldConfig config)
        {
            this.config = config;
            var radius = this.config.Density;
            var size = this.config.Size;
            var offset = radius * MathF.Sqrt(2);
            var divSize = offset * 3;
            var centerPoints = PoissonDiscSampling.GeneratePoints(radius, size - new Vector2(1, 1) * offset);

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

            borderList.AddRange(centerPoints);


            DelaunayGeo = new DelaunayGeo();
            DelaunayGeo.Build(borderList, config.Size);
            Voronoi = new VoronoiNew();
            Voronoi.BuildFromDelaunay(DelaunayGeo);
            BuildPlates();

            BuildLandOcean();
        }

        
        
        public void BuildLandOcean()
        {
            Random random = new Random();
            var x = random.NextDouble() * random.Next(63, 1231);
            var y = random.NextDouble() * random.Next(123, 532);
            foreach (var plate in PlatesRes)
            {
                plate.Nodes.ForEach(node =>
                {
                    var pos = (node.Node.center + new Vector2((float)x, (float)y)) / new Vector2(config.Size.normalized.y, config.Size.normalized.x);
                    var h00 = noise.pnoise(pos / (config.Size / 3), 100);
                    var h01 = noise.pnoise((pos + new Vector2(0, 1)) / (config.Size / 3), 100);
                    var h10 = noise.pnoise((pos + new Vector2(1, 0)) / (config.Size / 3), 100);
                    var h11 = noise.pnoise((pos + new Vector2(1, 1)) / (config.Size / 3), 100);
                    var h = math.lerp(math.lerp(h00, h01, pos.y - (int)pos.y), math.lerp(h10, h11, pos.y - (int)pos.y), pos.x - (int)pos.x);
                    var height = h * .5f + .5f;
                    node.Height = height;
                    node.Island = height > config.LandHeight;
                });
            }
        }
    }
}