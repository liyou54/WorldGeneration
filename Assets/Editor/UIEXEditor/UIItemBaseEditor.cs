using GameFrame.UI;
using SuperScrollView;
using UnityEditor;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(UIItemBase))]
    public class UIItemBaseEditor : LoopListViewEditor2
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