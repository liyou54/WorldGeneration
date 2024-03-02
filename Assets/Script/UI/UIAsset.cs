using System.Collections.Generic;
using UnityEngine;

namespace GameFrame.UI
{
    [System.Serializable]
    public class UIConfigData
    {
        public string UIName;
        public string UIPrefab;
    }    
    [CreateAssetMenu(fileName = "ui_asset", menuName = "不常用/ui_asset", order = 0)]
    public class UIAsset : ScriptableObject 
    {
       public List<UIConfigData> UIConfig;
    }
}