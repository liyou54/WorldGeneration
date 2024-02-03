public class IDAllocator
{
    private int currentID;

    public IDAllocator()
    {
        currentID = 0;
    }

    public int AllocateID()
    {
        int newID = currentID;
        currentID++;
        return newID;
    }
}

