using System;
using System.Collections.Generic;
using System.Linq;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Script.Skill.TimelineTrack.Editor
{
    [CustomPropertyDrawer(typeof(BoneSelectionAttribute))]
    public class BoneSelectionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            if (property.propertyType == SerializedPropertyType.String)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(label );
                if (GUILayout.Button(GetLastNameFromPath(property.stringValue)))
                {
                    AddParam(property);
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }


        string GetLastNameFromPath(string path)
        {
            // 使用"/"作为分隔符将路径拆分成字符串数组
            string[] pathParts = path.Split('/');
            
            if (pathParts.Length == 0)
            {
                return "";
            }
            
            // 获取数组中的最后一个元素，即最后一个节点的名称
            string lastName = pathParts[^1];

            return lastName;
        }
        
        private void AddParam(SerializedProperty property)
        {
            GenericSelector<string> CustomGenericSelector;
            IEnumerable<GenericSelectorItem<string>> customCollection = GetBoneNames().Select(x => new GenericSelectorItem<string>(x, x));
            CustomGenericSelector = new GenericSelector<string>("骨骼节点", false, customCollection);
            CustomGenericSelector.EnableSingleClickToSelect();
            CustomGenericSelector.SelectionConfirmed += ints =>
            {
                var result = ints.FirstOrDefault();
                if (result != null)
                {
                    property.stringValue = result;
                    Debug.Log(result);
                    property.serializedObject.ApplyModifiedProperties();
                }
            };
            CustomGenericSelector.ShowInPopup();
        }
        
        string GetRelativePathToParent(Transform target)
        {
            // 如果目标节点没有父节点，返回空字符串
            if (target.parent == null)
            {
                return "";
            }

            // 初始化相对路径
            string path = target.name;

            // 循环遍历父节点，直到达到根节点
            Transform parent = target.parent;
            while (parent != null)
            {
                // 将父节点名称添加到相对路径中
                path = parent.name + "/" + path;

                // 继续向上遍历父节点
                parent = parent.parent;
            }

            return path;
        }

        private string[] GetBoneNames()
        {
            var gameObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SPUM/Prefab/SpriteEditorPrefab.prefab");
            var trs = gameObject.GetComponentsInChildren<Transform>();
            var res = new List<string>();
            foreach (var tr in trs)
            {
                if (!tr.GetComponent<SpriteRenderer>())
                {
                    var path = GetRelativePathToParent(tr);
                    res.Add(path);
                }
            }
            return res.ToArray();
        }
    }
}