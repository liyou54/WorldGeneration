using UnityEditor;
using UnityEngine;

namespace UI.UIEX.Editor
{
    public static class UIExEditorUtil
    {
        public static void DrawExtraData(SerializedObject sObj)
        {
            sObj.Update();
            var UIexDataProperty = sObj.FindProperty("_uiExData");
            var name = UIexDataProperty.FindPropertyRelative("Name");
            var genCode = UIexDataProperty.FindPropertyRelative("BGenCode");
            EditorGUILayout.PropertyField(name, new GUIContent("组件名称"));
            EditorGUILayout.PropertyField(genCode, new GUIContent("生成代码"));
            sObj.ApplyModifiedProperties();
            sObj.Update();
            name.stringValue = NameToBigCamel(name.stringValue);
            sObj.ApplyModifiedProperties();


        }

        // 命名规范化为大驼峰 去掉下划线 空个非法字符
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

    }

}