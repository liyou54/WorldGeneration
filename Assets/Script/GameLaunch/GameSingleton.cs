using UnityEngine;

namespace Script.GameLaunch
{
    public class GameSingleton<T>:MonoBehaviour
    {
        public static GameSingleton<T> Instance;
        public virtual void OnInit()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}