using System;

namespace GameFrame.UI
{
    public interface IUIManager
    {
        public void ActiveUI<T>(string uiName,T customData) where T:UIData;
        public void DeActiveUI(String uiName);
        public void RefreshUI(String uiName, Object customData);
        public void ShowUI(String uiName, Object customData);
        public void HideUI(String uiName);
    }
}