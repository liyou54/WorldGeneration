using GameFrame.UI.UIEX;
using UnityEditor;
using UnityEditor.UI;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(ImageEx))]
    public class ImageExEditor : ImageEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            UIExEditorUtil.DrawExtraData(serializedObject);
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}