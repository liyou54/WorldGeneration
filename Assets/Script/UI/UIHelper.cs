using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrame.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public partial class UIHelper : MonoBehaviour
    {

        [LabelText("UI名称")] public String Name;

    }
}