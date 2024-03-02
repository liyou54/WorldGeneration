using GameFrame.UI.UIEX;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.UIEX.Editor
{
    [CustomPropertyDrawer(typeof(UIExData))]
    public class UIExDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            var name = property.FindPropertyRelative("Name");
            var genCode = property.FindPropertyRelative("BGenCode");
            EditorGUILayout.PropertyField(name, new GUIContent("组件名称"));
            EditorGUILayout.PropertyField(genCode, new GUIContent("生成代码"));
            name.stringValue = NameToBigCamel(name.stringValue);


        }
        public static string NameToBigCamel(string name)
        {
            var res = "";
            var isUpper = true;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '_' || name[i] == ' ')
                {
                    isUpper = true;
                    continue;
                }

                if (!((name[i] >= 'a' && name[i] <= 'z') ||
                      (name[i] >= 'A' && name[i] <= 'Z') ||
                      (name[i] >= '0' && name[i] <= '9')))
                {
                    continue;
                }

                if (isUpper)
                {
                    res += name[i].ToString().ToUpper();
                    isUpper = false;
                }
                else
                {
                    res += name[i];
                }
            }
            return res;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}