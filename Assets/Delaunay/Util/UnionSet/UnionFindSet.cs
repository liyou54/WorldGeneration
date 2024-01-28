using System.Collections.Generic;

namespace Delaunay.UnionSet
{
    public class UnionFindSet
    {
        public List<int> data = new List<int>();

        public UnionFindSet(int n)
        {
            for (int i = 0; i < n; i++)
            {
                data.Add(i);
            }
        }
        
        public int Find(int a)
        {
            if (data[a] == a) return a;
            return data[a] = Find(data[a]);
        }
        
        public int Merge(int a, int b)
        {
            int fa = Find(a);
            int fb = Find(b);
            if (fa == fb) return fa;
            data[fa] = fb;
            return fb;
        }
        
    }
}