namespace Script.Entity.Util
{
    public class IDAllocator
    {
        private int currentID;

        public IDAllocator()
        {
            currentID = 1;
        }

        public int AllocateID()
        {
            int newID = currentID;
            currentID++;
            return newID;
        }
    }


}