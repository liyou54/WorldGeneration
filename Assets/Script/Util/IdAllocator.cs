namespace Util
{
    public class IdAllocator
    {
        private int nextId = 1;

        public int AllocateId() {
            return nextId++;
        }
    }
}