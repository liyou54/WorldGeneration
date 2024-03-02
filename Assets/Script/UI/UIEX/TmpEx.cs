using System;
using GameFrame.UI.UIEX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrame.UI.UIEX
{
    public class TmpEx:TextMeshProUGUI,IUIEX
    {
        public UIExData UIExData
        {
            get => _uiExData;
            set => _uiExData = value;
        }

        [SerializeField] private UIExData _uiExData;
    }
}