using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

namespace Script.Map
{
    public class ClickDragData
    {
        public static float ClickTimeThreshold = 0.3f;
        public static float DragDistanceThreshold = 2f;

        public bool IsClick = false;
        public bool IsDrag = false;
        public Vector3 ClickStartPos;
        public Vector3 CurrentPos;
        public float ClickTime;

        public void UpdateClickStatus(Vector3 pos, float currentTime)
        {
            CurrentPos = pos;
            bool drag = Vector2.Distance(ClickStartPos, CurrentPos) > DragDistanceThreshold && !IsDrag;
            if (currentTime - ClickTime > ClickTimeThreshold && !IsDrag)
            {
                drag = true;
            }

            if (drag)
            {
                IsClick = false;
                IsDrag = true;
            }
        }

        public void Reset()
        {
            IsClick = false;
            IsDrag = false;
            CurrentPos = Vector2.zero;
            ClickStartPos = Vector2.zero;
            ClickTime = 0;
        }
    }


    [RequireComponent(typeof(SpriteRenderer))]
    public class TilemapSelector : MonoBehaviour
    {
        private MapManager mapManager => MapManager.Instance;

        ClickDragData clickDragData = new ClickDragData();
        SpriteRenderer spriteRenderer;


        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        public void UpdateSelect()
        {
            if (Input.GetMouseButtonDown(0) == false && Input.GetMouseButton(0) == false)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (clickDragData.IsClick)
                    {
                        Debug.Log("click");
                    }

                    if (clickDragData.IsDrag)
                    {
                    }

                    spriteRenderer.size = Vector2.zero;
                    clickDragData.Reset();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                clickDragData.IsClick = true;
                var startWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startWorldPos.z = 0;
                clickDragData.ClickStartPos = startWorldPos;
                clickDragData.ClickTime = Time.time;
            }

            if (Input.GetMouseButton(0))
            {
                var isDragStart = clickDragData.IsDrag;
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                clickDragData.UpdateClickStatus(worldPos, Time.time);
                var isDragEnd = clickDragData.IsDrag;

                if (!isDragStart && isDragEnd)
                {
                    gameObject.transform.position = clickDragData.ClickStartPos;
                }

                if (isDragEnd)
                {
                    Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPos.z = 0;
                    var startPos = clickDragData.ClickStartPos;
                    var size = currentPos - startPos;
                    spriteRenderer.size = size;
                }
            }
        }


        

        
        void Update()
        {
            UpdateSelect();
        }
    }
}