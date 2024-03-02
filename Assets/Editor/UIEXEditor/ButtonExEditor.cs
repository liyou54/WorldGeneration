using GameFrame.UI.UIEX;
using UnityEditor;
using UnityEditor.UI;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(ButtonEx))]
    public class ButtonExEditor : ButtonEditor
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