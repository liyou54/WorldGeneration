using GameFrame.UI.UIEX;
using UnityEngine;

namespace GameFrame.UI
{
    public class UIItemBase:MonoBehaviour,IUIEX
    {
        public UIExData UIExData
        {
            get => _uiExData;
            set => _uiExData = value;
        }

        [SerializeField] private UIExData _uiExData;

        public virtual void SetData<T>(T data)
        {
            
        }
    }
}