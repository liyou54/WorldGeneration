using System;

namespace SGOAP.Examples
{
    public class Enemy : Character
    {
        public int Points;
        public Action<int> OnPointChanged;
        public void AddPoint(int amount)
        {
            Points += amount;
            OnPointChanged?.Invoke(Points);
        }
    }
}