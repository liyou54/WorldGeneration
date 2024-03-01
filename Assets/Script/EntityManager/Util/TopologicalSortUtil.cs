using System;
using System.Collections.Generic;

class Graph<T>
{
    private Dictionary<T, List<T>> adjList;

    public Graph()
    {
        adjList = new Dictionary<T, List<T>>();
    }

    public void AddVertex(T vertex)
    {
        if (!adjList.ContainsKey(vertex))
        {
            adjList[vertex] = new List<T>();
        }
    }

    public void AddEdge(T from, T to)
    {
        if (!adjList.ContainsKey(from))
        {
            AddVertex(from);
        }

        if (!adjList.ContainsKey(to))
        {
            AddVertex(to);
        }

        adjList[from].Add(to);
    }

    private bool IsCyclicUtil(T vertex, Dictionary<T, bool> visited, Dictionary<T, bool> recStack)
    {
        if (!visited[vertex])
        {
            visited[vertex] = true;
            recStack[vertex] = true;

            foreach (T neighbor in adjList[vertex])
            {
                if (!visited[neighbor] && IsCyclicUtil(neighbor, visited, recStack))
                {
                    return true;
                }
                else if (recStack[neighbor])
                {
                    return true;
                }
            }
        }

        recStack[vertex] = false;
        return false;
    }

    public bool IsCyclic()
    {
        Dictionary<T, bool> visited = new Dictionary<T, bool>();
        Dictionary<T, bool> recStack = new Dictionary<T, bool>();

        foreach (var vertex in adjList.Keys)
        {
            visited[vertex] = false;
            recStack[vertex] = false;
        }

        foreach (var vertex in adjList.Keys)
        {
            if (IsCyclicUtil(vertex, visited, recStack))
            {
                return true;
            }
        }

        return false;
    }

    private void TopologicalSortUtil(T vertex, Dictionary<T, bool> visited, Stack<T> stack)
    {
        visited[vertex] = true;

        foreach (T neighbor in adjList[vertex])
        {
            if (!visited[neighbor])
            {
                TopologicalSortUtil(neighbor, visited, stack);
            }
        }

        stack.Push(vertex);
    }

    public List<T> TopologicalSort()
    {
        if (IsCyclic())
        {
            throw new InvalidOperationException("图中存在环，无法进行拓扑排序。");
        }

        Stack<T> stack = new Stack<T>();
        Dictionary<T, bool> visited = new Dictionary<T, bool>();

        foreach (var vertex in adjList.Keys)
        {
            visited[vertex] = false;
        }

        foreach (var vertex in adjList.Keys)
        {
            if (!visited[vertex])
            {
                TopologicalSortUtil(vertex, visited, stack);
            }
        }

        List<T> result = new List<T>();
        while (stack.Count > 0)
        {
            result.Add(stack.Pop());
        }

        return result;
    }
}


