using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.GameLaunch;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;

namespace GameFrame.UI
{
    [Flags]
    public enum UIState
    {
        Show = 0b1,
        Hide = 0b01,
        BeHide = 0b001,
    }

    public class UIStack
    {
        public UICtrlBase UICtrlBase;
        public Object CustomData;
        public UIState UIState;
    }

    public class UIManager : GameSingleton<UIManager>, IUIManager
    {
        public Dictionary<string, UICtrlBase> UigGameObjects = new Dictionary<string, UICtrlBase>();
        public UITag UITag;
        public Dictionary<UILayer, List<UIStack>> UIStacksData = new Dictionary<UILayer, List<UIStack>>();
        public ObjectPool<UIStack> UIStackPool = new ObjectPool<UIStack>(CreateStack, null, OnUIStackRelease);


        public static UIStack CreateStack()
        {
            return new UIStack();
        }


        public static void OnUIStackRelease(UIStack data)
        {
            data.CustomData = null;
            data.UICtrlBase = null;
            data.UIState = UIState.Show;
        }

        public override async UniTask OnInit()
        {
            // var uiAsset = await YooAsset.<UIAsset>("Assets/GlobalConfig/ui_asset.asset");
            // UITag = GameObject.FindObjectOfType<UITag>();
            //
            // foreach (var config in uiAsset.UIConfig)
            // {
            //     var uiPrefab = await config.UIPrefab.LoadAssetAsync<GameObject>();
            //     var instance = GameObject.Instantiate(uiPrefab);
            //     instance.SetActive(false);
            //     var uiCtrl = instance.GetComponent<UICtrlBase>();
            //     UigGameObjects.Add(config.UIName, uiCtrl);
            //     instance.transform.SetParent(UITag.CanvasNone.transform);
            //     var rect = instance.GetComponent<RectTransform>();
            //     rect.localScale = Vector3.one;
            //     rect.localPosition = Vector3.zero;
            // }

            UIStacksData.Add(UILayer.Tips, new List<UIStack>(32));
            UIStacksData.Add(UILayer.Top, new List<UIStack>(32));
            UIStacksData.Add(UILayer.Middle, new List<UIStack>(32));
            UIStacksData.Add(UILayer.Bottom, new List<UIStack>(32));
            // var a = CustomResourceManager.Instance.LoadAssetAsync<UIAsset>("Assets/GlobalConfig/ui_asset.asset");
            // var uiAsset = (await a);
            return;
        }


        public UIState RemoveActiveBit(UIState uiState)
        {
            uiState &= ~UIState.Show;
            return uiState;
        }

        public void ActiveUI<T>(string uiName, T customData) where T : UIData
        {
            var uiCtrl = UigGameObjects[uiName];

            var uiLayer = uiCtrl.Layer;
            if (uiCtrl.IsFullScreen)
            {
                foreach (var uiStackListKv in UIStacksData)
                {
                    var stackLayer = (int)uiStackListKv.Key;
                    var findFullPage = false;
                    if ((int)uiCtrl.Layer >= stackLayer)
                    {
                        for (int i = uiStackListKv.Value.Count - 1; i >= 0; i--)
                        {
                            var stack = uiStackListKv.Value[i];
                            stack.UIState |= UIState.BeHide;
                            stack.UIState = RemoveActiveBit(stack.UIState);
                            stack.UICtrlBase.gameObject.SetActive(false);
                            stack.UICtrlBase.OnHideUI();
                            if (stack.UICtrlBase.IsFullScreen)
                            {
                                findFullPage = true;
                                break;
                            }
                        }
                    }

                    if (findFullPage)
                    {
                        break;
                    }
                }
            }

            uiCtrl.OnActiveUI(customData);
            var uiStack = UIStackPool.Get();
            uiStack.CustomData = customData;
            uiStack.UICtrlBase = uiCtrl;
            UIStacksData[uiLayer].Add(uiStack);
            SetParent(uiCtrl, uiLayer);
            uiCtrl.gameObject.SetActive(true);
        }


        private void SetParent(UICtrlBase uiCtrl, UILayer uiLayer)
        {
            switch (uiLayer)
            {
                case UILayer.None:
                    uiCtrl.transform.SetParent(UITag.CanvasNone.transform);
                    break;
                case UILayer.Bottom:
                    uiCtrl.transform.SetParent(UITag.CanvasBottom.transform);
                    break;
                case UILayer.Middle:
                    uiCtrl.transform.SetParent(UITag.CanvasMiddle.transform);
                    break;
                case UILayer.Top:
                    uiCtrl.transform.SetParent(UITag.CanvasTop.transform);
                    break;
                case UILayer.Tips:
                    uiCtrl.transform.SetParent(UITag.CanvasTips.transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(uiLayer), uiLayer, null);
            }
        }


        public void DeActiveUI(string uiName)
        {
            var uiCtrl = UigGameObjects[uiName];
            var uiLayer = uiCtrl.Layer;
            var uiStackList = UIStacksData[uiLayer];
            UIStack currenrtStack = null;
            for (int i = uiStackList.Count - 1; i >= 0; i--)
            {
                var uiStack = uiStackList[i];
                if (uiStack.UICtrlBase == uiCtrl)
                {
                    currenrtStack = uiStack;
                }
            }

            if (currenrtStack == null)
            {
                return;
            }

            foreach (var uiStackListKv in UIStacksData)
            {
                var stackLayer = (int)uiStackListKv.Key;
                var findFullPage = false;
                var start = false;

                if ((int)uiCtrl.Layer >= stackLayer)
                {
                    for (int i = uiStackListKv.Value.Count - 1; i >= 0; i--)
                    {
                        if (!start && uiStackListKv.Value[i] == currenrtStack)
                        {
                            currenrtStack.UICtrlBase.gameObject.SetActive(false);
                            currenrtStack.UICtrlBase.gameObject.transform.SetParent(UITag.CanvasNone.transform);
                            start = true;
                            if (!currenrtStack.UICtrlBase.IsFullScreen || (currenrtStack.UIState & UIState.Show) == 0)
                            {
                                UIStackPool.Release(currenrtStack);
                                return;
                            }

                            UIStackPool.Release(currenrtStack);
                        }

                        if (!start)
                        {
                            continue;
                        }

                        var stack = uiStackListKv.Value[i];
                        stack.UIState &= ~UIState.BeHide;
                        if ((stack.UIState & UIState.Hide) == 0)
                        {
                            stack.UICtrlBase.gameObject.SetActive(true);
                            stack.UICtrlBase.OnShowUI(stack.CustomData);
                            stack.UIState |= UIState.Show;
                        }

                        if (stack.UICtrlBase.IsFullScreen)
                        {
                            findFullPage = true;
                            break;
                        }
                    }
                }

                if (findFullPage)
                {
                    break;
                }
            }
        }

        public void RefreshUI(string uiName, object customData)
        {
        }


        public void ShowUI(string uiName, object customData)
        {
        }

        public void HideUI(string uiName)
        {
        }
    }
}