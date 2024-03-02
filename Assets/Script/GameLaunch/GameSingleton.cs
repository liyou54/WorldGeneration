using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.GameLaunch
{
    public class GameSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }

                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }
        
        
        public virtual async UniTask OnInit()
        {
            await UniTask.CompletedTask;
        }
    }
}