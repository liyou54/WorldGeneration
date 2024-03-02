using UnityEngine;

namespace GameFrame.UI
{

    public abstract class UIData
    {
        
    }


    
    public abstract class UICtrlBase:MonoBehaviour 
    {
        public virtual bool IsFullScreen{ get; set; }  = true;
        public virtual UILayer Layer { get; set; } = UILayer.Middle;
        public virtual void OnCreate()
        {
        }
        
        public abstract void OnInit();
        public abstract void OnActiveUI(UIData customData);
        public abstract void OnRefreshUI(object customData);
        public abstract void OnShowUI(object customData);
        public abstract void OnCloseUI();
        public abstract void OnHideUI();
        public abstract void OnDestroyUI();
        public virtual void OnLostFocus(){}

        
        
    }
}