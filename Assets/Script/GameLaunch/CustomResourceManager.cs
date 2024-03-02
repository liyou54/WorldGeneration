using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace Script.GameLaunch
{
    public class CustomResourceManager : GameSingleton<CustomResourceManager>
    {
        ResourcePackage DefaultPackage;
        Dictionary<string, ResourcePackage> packageDic = new Dictionary<string, ResourcePackage>();
        public bool UseSimulateMode = false;

        public override UniTask OnInit()
        {
            YooAssets.Initialize();
            DefaultPackage = YooAssets.CreatePackage("MainPackage");
            if (UseSimulateMode)
            {
                Debug.Log("EditorSimulateModeParameters");
                var initParameters = new EditorSimulateModeParameters();
                var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "MainPackage");
                initParameters.SimulateManifestFilePath = simulateManifestFilePath;
                return DefaultPackage.InitializeAsync(initParameters).ToUniTask();
            }
            else
            {
                Debug.Log("OfflinePlayModeParameters");
                var initParameters = new OfflinePlayModeParameters();
                initParameters.BuildinRootDirectory = Application.dataPath.Replace("/Assets", "/Mods");
                return DefaultPackage.InitializeAsync(initParameters).ToUniTask();
            }
        }

        public void GetSandboxRootDirectory()
        {
            Debug.Log(DefaultPackage.GetPackageSandboxRootDirectory());
        }


        public async UniTask<ResourcePackage> LoadPackageAsync(string packageName)
        {
            if (packageDic.TryGetValue(packageName, out var res))
            {
                return res;
            }
            else
            {
                var overridePackage = YooAssets.CreatePackage(packageName);
                var initParameters = new OfflinePlayModeParameters();
                initParameters.BuildinRootDirectory = Application.dataPath.Replace("/Assets", "/Mods");
                Debug.Log(initParameters.BuildinRootDirectory);
                await overridePackage.InitializeAsync(initParameters).ToUniTask();
                packageDic.Add(packageName, overridePackage);
                return overridePackage;
            }
        }

        public async UniTask<AssetHandle> LoadAssetAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            var a = DefaultPackage.LoadAssetAsync<T>(assetPath);
            await a.ToUniTask();
            return a;
        }
    }
}