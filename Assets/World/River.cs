using System.Collections.Generic;
using Delaunay;
using UnityEngine;

namespace World
{
    public class River
    {
        class RiverTree
        {
            public RiverTree Left;
            public RiverTree Right;
            public DFace Face;
            public DEdge FromEdge;
            public RiverTree Parent;
            public int Width;
        }


        public uint[] RiverBuffer;

        public int[] BuildRiver(DelaunayGeo geo, Mesh mesh)
        {
            HashSet<DFace> hasUsed = new HashSet<DFace>();
            var buffer = new int[geo.faces.Count];
            var res = new List<RiverTree>();
            var leafNode = new List<RiverTree>();
            // 面以及来自的边
            var childNode = new List<RiverTree>();
            for (var faceId = 0; faceId < geo.faces.Count; faceId++)
            {
                DEdge cornerEdge = null;
                for (int i = 0; i < 3; i++)
                {
                    var edge = geo.faces[faceId].edge;
                    if (edge.NextEdge.TwinEdge == null)
                    {
                        cornerEdge = edge.NextEdge;
                        break;
                    }
                }

                if (cornerEdge != null)
                {
                    RiverTree riverTree = new RiverTree();
                    riverTree.Face = geo.faces[faceId];
                    riverTree.FromEdge = cornerEdge;
                    hasUsed.Add(geo.faces[faceId]);
                    childNode.Add(riverTree);
                    res.Add(riverTree);
                }
            }

            while (childNode.Count > 0)
            {
                var random = Random.Range(0, childNode.Count - 1);
                var riverTree = childNode[random];
                var leftEdge = riverTree.FromEdge.NextEdge.TwinEdge;
                var leftFace = leftEdge?.Face;
                var rightEdge = riverTree.FromEdge.NextEdge.NextEdge.TwinEdge;
                var rightFace = rightEdge?.Face;

                var canAddLeft = leftFace != null && !hasUsed.Contains(leftFace);
                var canAddRight = rightFace != null && !hasUsed.Contains(rightFace);

                if (!canAddRight && !canAddLeft)
                {
                    if (riverTree.Left == null && riverTree.Right == null)
                    {
                        leafNode.Add(riverTree);
                    }

                    childNode.RemoveAt(random);
                    continue;
                }

                RiverTree child = new RiverTree();
                child.Parent = riverTree;
                if (!canAddLeft)
                {
                    child.Face = rightFace;
                    child.FromEdge = rightEdge;
                    riverTree.Right = child;
                }
                else if (!canAddRight)
                {
                    child.Face = leftFace;
                    child.FromEdge = leftEdge;
                    riverTree.Left = child;
                }
                else
                {
                    var faceType = Random.Range(0, 2);
                    child.Face = faceType == 0 ? leftFace : rightFace;
                    child.FromEdge = faceType == 0 ? leftEdge : rightEdge;
                    if (faceType == 0)
                    {
                        riverTree.Left = child;
                    }
                    else
                    {
                        riverTree.Right = child;
                    }
                }

                hasUsed.Add(child.Face);
                childNode.Add(child);
                if (!canAddRight || !canAddLeft)
                {
                    childNode.RemoveAt(random);
                }
            }

            // 宽度优先遍历后序遍历
            foreach (var node in res)
            {
                Stack<RiverTree> riverTrees = new Stack<RiverTree>();
                riverTrees.Push(node);

                while (riverTrees.Count > 0)
                {
                    var currentNode = riverTrees.Peek();
                    if (currentNode.Left != null && currentNode.Width == 0)
                    {
                        riverTrees.Push(currentNode.Left);
                    }

                    if (currentNode.Right != null && currentNode.Width == 0)
                    {
                        riverTrees.Push(currentNode.Right);
                    }

                    if ((currentNode.Left == null && currentNode.Right == null) || currentNode.Width > 0)
                    {
                        if (currentNode.Width == 0)
                        {
                            currentNode.Width = 1;
                        }

                        if (currentNode.Parent != null)
                        {
                            currentNode.Parent.Width = Mathf.Max(currentNode.Parent.Width, currentNode.Width + 1);
                        }

                        riverTrees.Pop();
                    }
                }
            }

            // Encode Tree To Hex
            foreach (var node in res)
            {
                Queue<RiverTree> nodeQue = new Queue<RiverTree>();
                nodeQue.Enqueue(node);
                while (nodeQue.Count > 0)
                {
                    var current = nodeQue.Dequeue();
                    var hexNodeType = 0;
                    if (current.Left != null)
                    {
                        nodeQue.Enqueue(current.Left);
                        hexNodeType |= 0b1;
                    }

                    if (current.Right != null)
                    {
                        nodeQue.Enqueue(current.Right);
                        hexNodeType |= 0b10;
                    }

                    var startEdgeHex = 0;
                    var edge = current.Face.edge;
                    var startEdge = current.FromEdge;
                    for (int j = 0; j < 3; j++)
                    {
                        if (edge != startEdge)
                        {
                            startEdgeHex++;
                            edge = edge.NextEdge;
                        }
                    }

                    var faceId = geo.faces.IndexOf(current.Face);
                    var hex = (hexNodeType << 2) + startEdgeHex;
                    hex = (hex << 6) + (current.Width > 63 ? 63 : current.Width);
                    var leftWidth = Mathf.Min((current.Left?.Width ?? 0), 63);
                    var rightWidth = Mathf.Min((current.Right?.Width ?? 0), 63);
                    hex = (hex << 6) + leftWidth;
                    hex = (hex << 6) + rightWidth;

                    buffer[faceId] = hex;
                }
            }


            return buffer;
        }
    }
}