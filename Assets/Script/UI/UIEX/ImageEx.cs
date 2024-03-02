using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrame.UI.UIEX
{
    public class ImageEx:Image,IUIEX
    {
        public UIExData UIExData
        {
            get => _uiExData;
            set => _uiExData = value;
        }

        [SerializeField] private UIExData _uiExData;
    }
}