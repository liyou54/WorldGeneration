using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrame.UI.UIEX
{
    public class RawImageEx:RawImage,IUIEX
    {
        public UIExData UIExData
        {
            get => _uiExData;
            set => _uiExData = value;
        }

        [SerializeField] private UIExData _uiExData;
    }
}