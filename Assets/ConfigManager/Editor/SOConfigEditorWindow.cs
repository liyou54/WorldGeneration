using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEditor;
using UnityEngine;

[TableList]
public class Temp
{
    public int A;
    public int B;
}

public class SOConfigEditorWindow : OdinEditorWindow
{
    [ConfigTableList(AlwaysExpanded = true,IsReadOnly = true)]
    public List<Temp> temps = new List<Temp>();

    [MenuItem("Tools/ConfigEditorWindow")]
    private static void OpenWindow()
    {
        GetWindow<SOConfigEditorWindow>().Show();
    }
}
