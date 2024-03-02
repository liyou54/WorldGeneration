using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Script.GameLaunch;
using UnityEngine;
using YooAsset;

namespace ModsFramework
{
    public class ModsManager : GameSingleton<ModsManager>
    {
        public override async UniTask OnInit()
        {
            await RegisterMod("ModTest");
        }

        public async UniTask RegisterMod(string modName)
        {
            var resourceManager = CustomResourceManager.Instance;
            var package = await resourceManager.LoadPackageAsync(modName);
            var dllInfo = package.GetAssetInfos("ModDll").FirstOrDefault();
            var dllBuff = package.LoadAssetSync(dllInfo);
            await dllBuff.ToUniTask();
            var assembly = System.Reflection.Assembly.Load((dllBuff.AssetObject as TextAsset).bytes);
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (typeof(IModPlugin).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    IModPlugin instance = (IModPlugin)Activator.CreateInstance(type);
                    instance.OnRegister();
                }
            }
        }
    }
}