using Cysharp.Threading.Tasks;
using GameFrame.UI;
using ModsFramework;
using Script.Entity;
using Script.GameLaunch;
using Script.Map;
using UnityEngine;
using YooAsset;

public class GameLaunch : MonoBehaviour
{
    private MapManager mapManager;

    public bool UseSimulateMode = false;

    async UniTask Start()
    {
#if !UNITY_EDITOR
        UseSimulateMode = false;
#endif
        CustomResourceManager.Instance.UseSimulateMode = UseSimulateMode;
        await CustomResourceManager.Instance.OnInit();
        CustomResourceManager.Instance.GetSandboxRootDirectory();
        await UIManager.Instance.OnInit();
        await ModsManager.Instance.OnInit();
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;
        EntityManager.Instance.OnUpdate(deltaTime);
    }
}