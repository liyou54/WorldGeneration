using UnityEditor;
using UnityEditor.UI;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(RawImageExEditor))]
    public class RawImageExEditor : RawImageEditor
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