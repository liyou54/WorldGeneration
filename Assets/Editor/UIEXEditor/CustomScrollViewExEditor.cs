using GameFrame.UI.UIEX;
using SuperScrollView;
using UnityEditor;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(CustomScrollViewEx))]
    public class CustomScrollViewExEditor : LoopListViewEditor2
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            UIExEditorUtil.DrawExtraData(serializedObject);
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}