using SuperScrollView;
using UnityEngine;

namespace GameFrame.UI.UIEX
{
    public class CustomScrollViewEx:LoopListView2,IUIEX
    {
        public UIExData UIExData
        {
            get => _uiExData;
            set => _uiExData = value;
        }

        [SerializeField] private UIExData _uiExData;
    }
}