using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GameFrame.UI;
using GameFrame.UI.UIEX;
using UnityEditor;
using UnityEngine;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(UIItemHelper))]
    public class UIItemHelperEditor : UnityEditor.Editor
    {
        private UIItemHelper uiItemHelper => target as UIItemHelper;
        private List<IUIEX> Components = new List<IUIEX>();
        private List<IUIEX> InvalidList = new List<IUIEX>();
        private static String WritePath = "Assets/Script/GameScript/UI/";

        public override void OnInspectorGUI()
        {
            Init();

            var property = serializedObject.FindProperty("ClassName");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, new GUIContent("Item类名称"));
            serializedObject.ApplyModifiedProperties();
            //
            var basePath = WritePath + $"/{uiItemHelper.ClassName}Item/";

            if (GUILayout.Button("生成View代码"))
            {
                if (InvalidList.Count > 0)
                {
                    var str = "";
                    foreach (var comp in InvalidList)
                    {
                        str += comp.UIExData.Name + "\n";
                    }

                    EditorUtility.DisplayDialog("错误", "以下组件命名不符合规范\n" + str, "确定");
                    return;
                }

                var code = TryGetViewCode();
                var pathViewName = basePath + uiItemHelper.ClassName + "ItemView.cs";
                if (System.IO.File.Exists(pathViewName))
                {
                    if (EditorUtility.DisplayDialog("警告", "文件已存在是否覆盖", "确定", "取消"))
                    {
                        StreamWriter writter = File.CreateText(pathViewName);
                        writter.Write(code);
                        writter.Flush();
                        writter.Close();
                        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
                    }
                }
                else
                {
                    if (Directory.Exists(basePath) == false)
                    {
                        Directory.CreateDirectory(basePath); //只有当文件不存在的话，创建新文件
                    }

                    StreamWriter writter = File.CreateText(pathViewName);
                    writter.Write(code);
                    writter.Flush();
                    writter.Close();
                    UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
                }
            }

            if (GUILayout.Button("生成Ctrl代码"))
            {
                var code = GenCtrlCode();
                var pathCtrlName = basePath + uiItemHelper.ClassName + "ItemCtrl.cs";
                if (!System.IO.File.Exists(pathCtrlName))
                {
                    if (EditorUtility.DisplayDialog("警告", "文件已存在是否覆盖", "确定", "取消"))
                    {
                        StreamWriter writter = File.CreateText(pathCtrlName);
                        writter.Write(code);
                        writter.Flush();
                        writter.Close();
                        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
                    }
                }
                else
                {
                    if (Directory.Exists(basePath) == false)
                    {
                        Directory.CreateDirectory(basePath); //只有当文件不存在的话，创建新文件
                    }

                    StreamWriter writter = File.CreateText(pathCtrlName);
                    writter.Write(code);
                    writter.Flush();
                    writter.Close();
                    UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
                }
            }

            if (GUILayout.Button("Bind"))
            {
                BindByRef();
            }


            DrawComponent();
        }


        void BindByRef()
        {
            Assembly asmb = System.Reflection.Assembly.Load("Assembly-CSharp");
            Type type = asmb.GetType($"UI.Ctrl.{uiItemHelper.ClassName}ItemCtrl");
            var item = uiItemHelper.gameObject.GetComponent<UIItemBase>();
            var compName = item?.UIExData.Name;
            var bgen = item?.UIExData.BGenCode;
            GameObject.DestroyImmediate(item);
            var comp = uiItemHelper.gameObject.AddComponent(type);
            Type view = asmb.GetType($"UI.Ctrl.{uiItemHelper.ClassName}ItemView");
            // construtor by ref
            var viewInstance = Activator.CreateInstance(view);
            foreach (var component in Components)
            {
                viewInstance.GetType().GetField(component.UIExData.Name).SetValue(viewInstance, component);
            }

            UIExData data = new UIExData();
            data.Name = compName;
            data.BGenCode = bgen ?? false;
            comp.GetType().GetField("View").SetValue(comp, viewInstance);
            (comp as UIItemBase).UIExData = data;
            EditorUtility.SetDirty(comp);
            AssetDatabase.SaveAssetIfDirty(comp);
        }


        void Init()
        {
            var temp = uiItemHelper.GetComponentsInChildren<IUIEX>(true).Where(
                    comp => comp.UIExData.BGenCode == true
                )
                .ToList();

            // 去除父节点存在为UIItemHelper的组件
            Components = temp.Where(comp =>
            {
                if ((comp as UIItemBase)?.GetComponent<UIItemHelper>() == uiItemHelper)
                {
                    return false;
                }

                var parent = (comp as Component).transform.parent;
                var needAdd = true;
                while (parent != uiItemHelper.transform && parent != null)
                {
                    if (parent.GetComponent<UIItemHelper>())
                    {
                        needAdd = false;
                        break;
                    }

                    parent = parent.parent;
                }

                return needAdd;
            }).ToList();


            InvalidList = CheckNameIsVaild();
        }


        public void DrawComponent()
        {
            GUI.enabled = false;
            var okColor = GUI.color;
            var errorColor = Color.red;

            foreach (var comp in Components)
            {
                if (InvalidList.Contains(comp))
                {
                    GUI.color = errorColor;
                }
                else
                {
                    GUI.color = okColor;
                }

                EditorGUILayout.ObjectField(comp.UIExData.Name, comp as Component, typeof(Component));
            }

            GUI.color = okColor;
            GUI.enabled = true;
        }

        public string TryGetViewCode()
        {
            var tab = "";
            var res = "";

            void Changetab(int change)
            {
                if (change == 1)
                {
                    tab += "\t";
                }

                if (change == -1)
                {
                    tab = tab.Substring(1);
                }
            }

            void AddCode(string line)
            {
                if (line.Length > 0)
                {
                    if (line[0] == '}')
                    {
                        Changetab(-1);
                    }
                }

                res += tab + line + "\n";
                if (line.Length > 0)
                {
                    if (line[0] == '{')
                    {
                        Changetab(1);
                    }
                }
            }

            AddCode("using System;");
            AddCode("using System.Collections.Generic;");
            AddCode("using UI.UIEX;");
            AddCode("using UnityEngine.UI;");
            AddCode("using UnityEngine;");
            AddCode("");
            AddCode("namespace UI.Ctrl");
            AddCode("{");
            AddCode("[Serializable]");
            AddCode($"public class {uiItemHelper.ClassName}ItemView");
            AddCode("{");
            foreach (var data in Components)
            {
                AddCode($"public {data.GetType().Name} {(data as IUIEX).UIExData.Name};");
            }

            AddCode("}");
            AddCode("}");
            return res;
        }

        // 检测命名规范
        public List<IUIEX> CheckNameIsVaild()
        {
            var invalidList = new List<IUIEX>();

            var hasName = new HashSet<String>();
            foreach (var comp in Components)
            {
                if (comp.UIExData.BGenCode && comp.UIExData.Name.Length == 0)
                {
                    invalidList.Add(comp);
                }

                if (comp.UIExData.BGenCode && hasName.Contains(comp.UIExData.Name))
                {
                    invalidList.Add(comp);
                }

                if (comp.UIExData.BGenCode)
                {
                    hasName.Add(comp.UIExData.Name);
                }
            }

            return invalidList;
        }

        public String GenCtrlCode()
        {
            var tab = "";
            var res = "";

            void Changetab(int change)
            {
                if (change == 1)
                {
                    tab += "\t";
                }

                if (change == -1)
                {
                    tab = tab.Substring(1);
                }
            }

            void AddCode(string line)
            {
                if (line.Length > 0)
                {
                    if (line[0] == '}')
                    {
                        Changetab(-1);
                    }
                }

                res += tab + line + "\n";
                if (line.Length > 0)
                {
                    if (line[0] == '{')
                    {
                        Changetab(1);
                    }
                }
            }

            AddCode("using System;");
            AddCode("using UI.UIEX;");
            AddCode("using UnityEngine.UI;");
            AddCode("using UnityEngine;");
            AddCode("");
            AddCode("namespace UI.Ctrl");
            AddCode("{");
            AddCode($"public class {uiItemHelper.ClassName}ItemCtrl:UIItemBase");
            AddCode("{");
            AddCode($"public {uiItemHelper.ClassName}ItemView View;");
            AddCode("}");
            AddCode("}");


            return res;
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
    }
}