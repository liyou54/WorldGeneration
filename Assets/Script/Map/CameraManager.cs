using UnityEngine;

namespace Script.Map
{
    public class CameraManager:MonoBehaviour
    {
        
        public float moveSpeed = 3f;
        
        public void UpdateCamera()
        {
                        
            var cameraScale  = Camera.main.orthographicSize;
            cameraScale -= Input.mouseScrollDelta.y;
            cameraScale = Mathf.Clamp(cameraScale, 5, 20);
            Camera.main.orthographicSize = cameraScale;
            
            Vector2 move = Vector2.zero;
            move += Input.mousePosition.x < 0.05 * Screen.width ? Vector2.left : Vector2.zero;
            move += Input.mousePosition.x > 0.95 * Screen.width ? Vector2.right : Vector2.zero;
            move += Input.mousePosition.y < 0.05 * Screen.height ? Vector2.down : Vector2.zero;
            move += Input.mousePosition.y > 0.95 * Screen.height ? Vector2.up : Vector2.zero;
            
            if (move != Vector2.zero)
            {
                Camera.main.transform.position += new Vector3(move.x, move.y, 0) * moveSpeed * Time.deltaTime * cameraScale;
            }

        }
    }
}