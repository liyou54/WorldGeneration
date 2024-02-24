using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Script.Skill.BlackBoardParam.Editor
{
    public class BlackBoardParamInspector<T, T2> : OdinValueDrawer<T> where T : BlackBoardParam<T2>
    {
        protected bool KeyInEditor { get; set; }
        protected string TempKey { get; set; }

        protected bool IsFoldout { get; set; }
        protected bool HotKeySave { get; set; }

        public bool KeyIsChanged(BlackBoardParam<T2> data, string key)
        {
            return data.Key != key;
        }

        public bool ChangeKeyIsValid(List<object> parent, string key)
        {
            if (parent.Any(x => x is BlackBoardParam && (x as BlackBoardParam).Key == key && x != this.ValueEntry.SmartValue))
            {
                return false;
            }

            return true;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var data = this.ValueEntry.SmartValue as BlackBoardParam<T2>;
            var parent = this.Property.Parent.ValueEntry.WeakSmartValue as List<object>;
            EditorGUILayout.BeginHorizontal();
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(30));
            IsFoldout = EditorGUI.Foldout(rect, IsFoldout, "", true);
            EditorGUILayout.LabelField(typeof(T2).Name + " : ", GUILayout.Width(100));

            if (KeyInEditor)
            {
                var defaultColor = GUI.color;

                if (!ChangeKeyIsValid(parent, TempKey))
                {
                    GUI.color = Color.red;
                }
                else if (KeyIsChanged(data, TempKey))
                {
                    GUI.color = Color.yellow;
                }

                TempKey = EditorGUILayout.TextField(TempKey, GUILayout.Width(200));
                GUI.color = defaultColor;
            }
            else
            {
                EditorGUILayout.LabelField(data.Key, GUILayout.Width(200));
            }

            HotKeySave = (Event.current.isKey && Event.current.keyCode == KeyCode.Return && KeyInEditor);

            if (GUILayout.Button(KeyInEditor ? "Save" : "Change", GUILayout.Width(100)) || HotKeySave)
            {
                if (KeyInEditor)
                {
                    if (ChangeKeyIsValid(parent, TempKey))
                    {
                        var skillTimeline = this.Property.SerializationRoot.ValueEntry.WeakSmartValue as SkillTimeline;
                        var needUpdateFieldList = skillTimeline.GetWillChangeBlackBoardKey(data.Key);
                        bool confirm = true;
                        if (needUpdateFieldList.Count > 0 && data.Key != TempKey)
                        {
                            confirm = EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to perform this sensitive operation?", "Yes", "No");
                        }
                        if (confirm)
                        {
                            foreach (var needUpdate in needUpdateFieldList)
                            {
                                needUpdate.BlackBoardKeyObject.BlackBoardKeyField = TempKey;
                            }

                            data.Key = TempKey;
                            KeyInEditor = !KeyInEditor;
                        }
                        else
                        {
                            // 用户点击了"No"按钮，取消操作
                            Debug.Log("Operation cancelled by user.");
                        }
                    }
                    else
                    {
                        TempKey = data.Key;
                        KeyInEditor = !KeyInEditor;
                    }
                }
                else
                {
                    TempKey = data.Key;
                    KeyInEditor = !KeyInEditor;
                }
            }

            EditorGUILayout.EndHorizontal();
            if (IsFoldout)
            {
                Property.Children["Value"].Draw(new GUIContent("Value"));
            }
        }
    }
}