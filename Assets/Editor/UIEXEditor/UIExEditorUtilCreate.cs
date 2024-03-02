using System.Reflection;
using GameFrame.UI.UIEX;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GameFrame.UI.Editor
{
    public class UIExEditorUtilCreate
    {
        [MenuItem("GameObject/UIEx/ButtonEx")]
        public static void CreatButtonEx()
        {
            var go = Selection.activeTransform;
            var btn = new GameObject("Btn_");
            var rect =btn.AddComponent<RectTransform>();
            btn.AddComponent<ButtonEx>();
            rect.sizeDelta = new Vector2(160, 30);
            var tmpObj = new GameObject("Tmp_");
            
           var render= tmpObj.AddComponent<CanvasRenderer>();
           render.cullTransparentMesh = true;
            tmpObj.transform.SetParent(btn.transform);
            rect = tmpObj.AddComponent<RectTransform>();
            var img = btn.AddComponent<ImageEx>();

            var tmp = tmpObj.AddComponent<TmpEx>();
            tmp.text = "Button";
            tmp.fontSize = 16;
            tmp.color = Color.black;
            tmp.alignment = TextAlignmentOptions.Center;
            btn.transform.SetParent(go);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }
        [MenuItem("GameObject/UIEx/RawImageEx")]
        public static void CreatRawImageEx()
        {
            var go = Selection.activeTransform;
            var btn = new GameObject("Raw_");
            btn.AddComponent<RawImageEx>();
            btn.transform.SetParent(go);
        }
        
        [MenuItem("GameObject/UIEx/ImageEx")]
        public static void CreatImageEx()
        {
            var go = Selection.activeTransform;
            var btn = new GameObject("Image_");
            btn.AddComponent<ImageEx>();

            btn.transform.SetParent(go);
        }
        [MenuItem("GameObject/UIEx/TmpEx")]
        public static void CreatTmpEx()
        {
            var go = Selection.activeTransform;
            var btn = new GameObject("Tmp_");
            var render= btn.AddComponent<CanvasRenderer>();
            render.cullTransparentMesh = true;
            btn.AddComponent<TmpEx>();
            btn.transform.SetParent(go);
        }
        
        [MenuItem("GameObject/UIEx/CustomScrollViewEx")]
        public static void CreatCustomScrollViewEx(MenuCommand menuCommand)
        {
            //反射调用 UnityEditor.UI 空间下MenuOptions  AddScrollView(MenuCommand menuCommand) 方法
            Assembly asmb = System.Reflection.Assembly.Load("UnityEditor.UI");
            var type =asmb.GetType("UnityEditor.UI.MenuOptions");
            var method = type.GetMethod("AddScrollView",BindingFlags.Static|BindingFlags.Public);
            method.Invoke(null,new object[]{menuCommand});
            var go = Selection.activeTransform;
            go.gameObject.AddComponent<CustomScrollViewEx>();
        }
        
    }
        
}