using ModsFramework;
using UnityEngine;

namespace ModPlugin.TestMod
{
    public class TestModPlugin: IModPlugin
    {
        public void OnRegister()
        {
            Debug.Log("1111");
        }
    }
}