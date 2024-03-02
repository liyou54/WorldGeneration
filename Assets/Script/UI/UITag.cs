using UnityEngine;

namespace GameFrame.UI
{
    public enum UILayer
    {
        None = 0,
        Bottom = 1000,
        Middle = 2000,
        Top = 3000,
        Tips = 4000,
    }
    public class UITag:MonoBehaviour
    {
        public Camera UICamera;
        public Canvas CanvasNone;
        public Canvas CanvasBottom;
        public Canvas CanvasMiddle;
        public Canvas CanvasTop;
        public Canvas CanvasTips;
        
    }
}