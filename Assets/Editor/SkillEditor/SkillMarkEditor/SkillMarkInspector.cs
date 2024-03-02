using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Script.Skill.TimelineTrack.Editor
{
    [CustomEditor(typeof(SkillMarkBase), true)]
    public class SkillMarkInspector: OdinEditor
    {
        
        SerializedProperty m_Time;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            m_Time = serializedObject.FindProperty("m_Time");
            EditorGUILayout.PropertyField(m_Time);
        }
    }
}