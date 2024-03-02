using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GameFrame.UI;
using GameFrame.UI.UIEX;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace UI.UIEX.Editor
{
    [CustomEditor(typeof(UIHelper))]
    public class UIHelperEditor : UnityEditor.Editor
    {
        private UIHelper uiHelper => target as UIHelper;
        private List<IUIEX> Components = new List<IUIEX>();
        private List<IUIEX> InvalidList = new List<IUIEX>();
        private static String WritePath = "Assets/Script/GameScript/UI/";
        private static String UIAssetPath = "Assets/Res/UIPrefab/ui_asset.asset";

        public override void OnInspectorGUI()
        {
            Init();

            var property = serializedObject.FindProperty("Name");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, new GUIContent("UI名称"));
            serializedObject.ApplyModifiedProperties();
            property.stringValue = NameToBigCamel(property.stringValue);
            //
            var basePath = WritePath + $"/{uiHelper.Name}/";

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
                Debug.LogError(code);
                var pathViewName = basePath + uiHelper.Name + "View.cs";
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
                var pathCtrlName = basePath + uiHelper.Name + "Ctrl.cs";
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
            Type type = asmb.GetType($"UI.Ctrl.{uiHelper.Name}Ctrl");
            GameObject.DestroyImmediate(uiHelper.gameObject.GetComponent<UICtrlBase>());
            var comp = uiHelper.gameObject.AddComponent(type) ;
            Type view = asmb.GetType($"UI.Ctrl.{uiHelper.Name}View");
            // construtor by ref
            var viewInstance = Activator.CreateInstance(view);
            
            foreach (var component in Components)
            {
                viewInstance.GetType().GetField(component.UIExData.Name).SetValue(viewInstance, component);
            }
            comp.GetType().GetField("View").SetValue(comp, viewInstance);
            EditorUtility.SetDirty(comp);
            AssetDatabase.SaveAssetIfDirty(comp);
        }
        
        
        
        
        void Init()
        {
                var temp  = uiHelper.GetComponentsInChildren<IUIEX>(true).Where(comp => comp.UIExData.BGenCode == true)
                    .ToList();
                // 去除父节点存在为UIItemHelper的组件
                Components = temp.Where(comp =>
                {
                    if ( comp == uiHelper)
                    {
                        return false;
                    }
                    
                    var parent = (comp as Component).transform.parent;
                    if (parent == null)
                    {
                        return false;
                    }
                    var needAdd = true;
                    while (parent != uiHelper.transform && parent != null )
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
            AddCode($"public class {uiHelper.Name}View");
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
            AddCode($"public class {uiHelper.Name}Ctrl:UICtrlBase");
            AddCode("{");
            AddCode($"public {uiHelper.Name}View View;");
            AddCode("public override void OnInit()");
            AddCode("{");
            AddCode("}");
            AddCode("");
            AddCode("public override void OnActiveUI(object customData)");
            AddCode("{");
            AddCode("}");
            AddCode("");
            AddCode("public override void OnRefreshUI(object customData)");
            AddCode("{");
            AddCode("}");
            AddCode("");
            AddCode("public override void OnShowUI(object customData)");
            AddCode("{");
            AddCode("}");
            AddCode("");
            AddCode("public override void OnCloseUI()");
            AddCode("{");
            AddCode("}");
            AddCode("");
            AddCode("public override void OnHideUI()");
            AddCode("{");
            AddCode("}");
            AddCode("");
            AddCode("public override void OnDestroyUI()");
            AddCode("{");
            AddCode("}");
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