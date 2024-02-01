using System;
using System.Collections.Generic;





namespace WaveFunctionCollapse
{
    
    public class WaveFunctionCollapseSlot<T>
    {   
        T Slot;
        public bool IsSlotCompare(T data,T otherData)
        {
            return true;
        }
    }

    public class WaveFunctionCollapseNode<TNode,TSlot>
    {
        public TNode Node;
        public List<TSlot> Slots;

    }
    
    public  class WaveFunctionCollapse<TNodeData,TSlot>
    {
        public List<WaveFunctionCollapseNode<TNodeData,TSlot>> Result;

        public TSlot GetNeighbourSlot(TSlot slot)
        {
            return default;
        }
        

    }
}