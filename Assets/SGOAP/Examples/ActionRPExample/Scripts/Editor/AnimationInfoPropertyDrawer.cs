using SGoap;
using UnityEditor;
using UnityEngine;

namespace SGOAP.Examples
{
    [CustomPropertyDrawer(typeof(AnimationInfo))]
    public class AnimationInfoPropertyDrawer : PropertyDrawer
    {
        public float NormalizedTime = 0;
        public float ClipTime = 0;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            if (Application.isPlaying)
                return;

            if (property.isExpanded)
            {
                var info = property.GetValue<AnimationInfo>();

                if (!string.IsNullOrEmpty(info.StateName))
                {
                    var actionObject = property.serializedObject.targetObject as SGoap.Action;
                    var agent = actionObject.GetComponentInParent<Agent>();
                    var animator = agent.GetComponentInChildren<Animator>();
                    var duration = animator.GetCurrentClipDuration();
                    NormalizedTime = Mathf.InverseLerp(0, duration, ClipTime);
                    animator.speed = 1f;
                    animator.Play(info.StateName, 0, NormalizedTime);
                    animator.Update(Time.deltaTime);

                    var rect = new Rect(position.xMin + 30f, position.yMax - 20f, position.width - 30f, 20f);
                    ClipTime = EditorGUI.Slider(rect, ClipTime, 0, duration);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
                return EditorGUI.GetPropertyHeight(property) + 20f;
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}