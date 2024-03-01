// Decompiled with JetBrains decompiler
// Type: Sirenix.OdinInspector.Editor.Drawers.ConfigTableListAttributeDrawer
// Assembly: Sirenix.OdinInspector.Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C7E3BB32-3266-4EF4-BE3A-8D9615BF0A19
// Assembly location: D:\Project\unity\WorldGeneration\Assets\Plugins\Sirenix\Assemblies\Sirenix.OdinInspector.Editor.dll
// XML documentation location: D:\Project\unity\WorldGeneration\Assets\Plugins\Sirenix\Assemblies\Sirenix.OdinInspector.Editor.xml

using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sirenix.OdinInspector.Editor.Drawers
{
    /// <summary>The TableList attirbute drawer.</summary>
    /// <seealso cref="T:Sirenix.OdinInspector.ConfigTableListAttribute" />
    public class ConfigTableListAttributeDrawer : OdinAttributeDrawer<ConfigTableListAttribute>
    {
        private IOrderedCollectionResolver resolver;
        private LocalPersistentContext<bool> isPagingExpanded;
        private LocalPersistentContext<Vector2> scrollPos;
        private LocalPersistentContext<int> currPage;
        private GUITableRowLayoutGroup table;
        private HashSet<string> seenColumnNames;
        private List<ConfigTableListAttributeDrawer.ConfigColumn> columns;
        private ObjectPicker picker;
        private int colOffset;
        private GUIContent indexLabel;
        private bool isReadOnly;
        private int indexLabelWidth;
        private Rect columnHeaderRect;
        private GUIPagingHelper paging;
        private bool drawAsList;
        private bool isFirstFrame = true;

        /// <summary>
        /// Determines whether this instance [can draw attribute property] the specified property.
        /// </summary>
        protected override bool CanDrawAttributeProperty(InspectorProperty property) => property.ChildResolver is IOrderedCollectionResolver;

        /// <summary>Initializes this instance.</summary>
        protected override void Initialize()
        {
            this.drawAsList = false;
            this.isReadOnly = this.Attribute.IsReadOnly || !this.Property.ValueEntry.IsEditable;
            this.indexLabelWidth = (int)SirenixGUIStyles.Label.CalcSize(new GUIContent("100")).x + 15;
            this.indexLabel = new GUIContent();
            this.colOffset = 0;
            this.seenColumnNames = new HashSet<string>();
            this.table = new GUITableRowLayoutGroup();
            this.table.MinScrollViewHeight = this.Attribute.MinScrollViewHeight;
            this.table.MaxScrollViewHeight = this.Attribute.MaxScrollViewHeight;
            this.resolver = this.Property.ChildResolver as IOrderedCollectionResolver;
            this.scrollPos = this.GetPersistentValue<Vector2>("scrollPos", Vector2.zero);
            this.currPage = this.GetPersistentValue<int>("currPage");
            this.isPagingExpanded = this.GetPersistentValue<bool>("expanded");
            this.columns = new List<ConfigTableListAttributeDrawer.ConfigColumn>(10);
            this.paging = new GUIPagingHelper();
            this.paging.NumberOfItemsPerPage = this.Attribute.NumberOfItemsPerPage > 0 ? this.Attribute.NumberOfItemsPerPage : GlobalConfig<GeneralDrawerConfig>.Instance.NumberOfItemsPrPage;
            this.paging.IsExpanded = this.isPagingExpanded.Value;
            this.paging.IsEnabled = GlobalConfig<GeneralDrawerConfig>.Instance.ShowPagingInTables || this.Attribute.ShowPaging;
            this.paging.CurrentPage = this.currPage.Value;
            this.Property.ValueEntry.OnChildValueChanged += new Action<int>(this.OnChildValueChanged);
            if (this.Attribute.AlwaysExpanded)
                this.Property.State.Expanded = true;
            int cellPadding = this.Attribute.CellPadding;
            if (cellPadding > 0)
                this.table.CellStyle = new GUIStyle()
                {
                    padding = new RectOffset(cellPadding, cellPadding, cellPadding, cellPadding)
                };
            GUIHelper.RequestRepaint();
            if (this.Attribute.ShowIndexLabels)
            {
                ++this.colOffset;
                this.columns.Add(new ConfigTableListAttributeDrawer.ConfigColumn(this.indexLabelWidth, true, false, (string)null, ConfigTableListAttributeDrawer.ColumnType.Index));
            }

            if (this.isReadOnly)
                return;
            this.columns.Add(new ConfigTableListAttributeDrawer.ConfigColumn(22, true, false, (string)null, ConfigTableListAttributeDrawer.ColumnType.DeleteButton));
        }

        /// <summary>Draws the property layout.</summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (this.drawAsList)
            {
                if (GUILayout.Button("Draw as table"))
                    this.drawAsList = false;
                this.CallNextDrawer(label);
            }
            else
            {
                this.picker = ObjectPicker.GetObjectPicker((object)this, this.resolver.ElementType);
                this.paging.Update(this.resolver.MaxCollectionLength);
                this.currPage.Value = this.paging.CurrentPage;
                this.isPagingExpanded.Value = this.paging.IsExpanded;
                Rect rect = SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.PropertyMargin);
                if (!this.Attribute.HideToolbar)
                    this.DrawToolbar(label);
                if (this.Attribute.AlwaysExpanded)
                {
                    this.Property.State.Expanded = true;
                    this.DrawColumnHeaders();
                    this.DrawTable();
                }
                else
                {
                    if (SirenixEditorGUI.BeginFadeGroup((object)this, this.Property.State.Expanded) && this.Property.Children.Count > 0)
                    {
                        this.DrawColumnHeaders();
                        this.DrawTable();
                    }

                    SirenixEditorGUI.EndFadeGroup();
                }

                SirenixEditorGUI.EndIndentedVertical();
                if (Event.current.type == UnityEngine.EventType.Repaint)
                    SirenixEditorGUI.DrawBorders(rect, 1, 1, this.Attribute.HideToolbar ? 0 : 1, 1);
                this.DropZone(rect);
                this.HandleObjectPickerEvents();
                if (Event.current.type != UnityEngine.EventType.Repaint)
                    return;
                this.isFirstFrame = false;
            }
        }

        private void OnChildValueChanged(int index)
        {
            IPropertyValueEntry valueEntry = this.Property.Children[index].ValueEntry;
            if (valueEntry == null || !typeof(ScriptableObject).IsAssignableFrom(valueEntry.TypeOfValue))
                return;
            for (int index1 = 0; index1 < valueEntry.ValueCount; ++index1)
            {
                UnityEngine.Object weakValue = valueEntry.WeakValues[index1] as UnityEngine.Object;
                if ((bool)weakValue)
                    EditorUtility.SetDirty(weakValue);
            }
        }

        private void DropZone(Rect rect)
        {
            if (this.isReadOnly)
                return;
            UnityEngine.EventType type = Event.current.type;
            switch (type)
            {
                case UnityEngine.EventType.DragUpdated:
                case UnityEngine.EventType.DragPerform:
                    if (!rect.Contains(Event.current.mousePosition))
                        break;
                    UnityEngine.Object[] objectArray = (UnityEngine.Object[])null;
                    if (((IEnumerable<UnityEngine.Object>)DragAndDrop.objectReferences).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>)(n => n != (UnityEngine.Object)null && this.resolver.ElementType.IsAssignableFrom(((object)n).GetType()))))
                        objectArray = ((IEnumerable<UnityEngine.Object>)DragAndDrop.objectReferences).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>)(x => x != (UnityEngine.Object)null && this.resolver.ElementType.IsAssignableFrom(((object)x).GetType()))).Reverse<UnityEngine.Object>().ToArray<UnityEngine.Object>();
                    else if (this.resolver.ElementType.InheritsFrom(typeof(Component)))
                        objectArray = (UnityEngine.Object[])Enumerable.OfType<GameObject>(DragAndDrop.objectReferences).Select<GameObject, Component>((Func<GameObject, Component>)(x => x.GetComponent(this.resolver.ElementType))).Where<Component>((Func<Component, bool>)(x => (UnityEngine.Object)x != (UnityEngine.Object)null)).Reverse<Component>().ToArray<Component>();
                    else if (this.resolver.ElementType.InheritsFrom(typeof(Sprite)) && ((IEnumerable<UnityEngine.Object>)DragAndDrop.objectReferences).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>)(n => n is Texture2D && AssetDatabase.Contains(n))))
                        objectArray = (UnityEngine.Object[])Enumerable.OfType<Texture2D>(DragAndDrop.objectReferences).Select<Texture2D, Sprite>((Func<Texture2D, Sprite>)(x => AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath((UnityEngine.Object)x)))).Where<Sprite>((Func<Sprite, bool>)(x => (UnityEngine.Object)x != (UnityEngine.Object)null)).Reverse<Sprite>().ToArray<Sprite>();
                    if (objectArray == null || objectArray.Length == 0)
                        break;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    Event.current.Use();
                    if (type != UnityEngine.EventType.DragPerform)
                        break;
                    DragAndDrop.AcceptDrag();
                    foreach (UnityEngine.Object @object in objectArray)
                    {
                        object[] values = new object[this.Property.ParentValues.Count];
                        for (int index = 0; index < values.Length; ++index)
                            values[index] = (object)@object;
                        this.resolver.QueueAdd(values);
                    }

                    break;
            }
        }

        private void AddColumns(int rowIndexFrom, int rowIndexTo)
        {
            if (Event.current.type != UnityEngine.EventType.Layout)
                return;
            for (int index1 = rowIndexFrom; index1 < rowIndexTo; ++index1)
            {
                int num = 0;
                InspectorProperty child1 = this.Property.Children[index1];
                for (int index2 = 0; index2 < child1.Children.Count; ++index2)
                {
                    InspectorProperty child2 = child1.Children[index2];
                    if (this.seenColumnNames.Add(child2.Name))
                    {
                        if (this.GetColumnAttribute<HideInTablesAttribute>(child2) != null)
                        {
                            ++num;
                        }
                        else
                        {
                            bool preserveWidth = false;
                            bool resizable = true;
                            bool flag = true;
                            int minWidth = this.Attribute.DefaultMinColumnWidth;
                            TableColumnWidthAttribute columnAttribute = this.GetColumnAttribute<TableColumnWidthAttribute>(child2);
                            if (columnAttribute != null)
                            {
                                preserveWidth = !columnAttribute.Resizable;
                                resizable = columnAttribute.Resizable;
                                minWidth = columnAttribute.Width;
                                flag = false;
                            }

                            ConfigTableListAttributeDrawer.ConfigColumn configColumn = new ConfigTableListAttributeDrawer.ConfigColumn(minWidth, preserveWidth, resizable, child2.Name, ConfigTableListAttributeDrawer.ColumnType.Property)
                            {
                                NiceName = child2.NiceName
                            };
                            configColumn.NiceNameLabelWidth = (int)SirenixGUIStyles.Label.CalcSize(new GUIContent(configColumn.NiceName)).x;
                            configColumn.PreferWide = flag;
                            this.columns.Insert(Math.Min(index2 + this.colOffset - num, this.columns.Count), configColumn);
                            GUIHelper.RequestRepaint();
                        }
                    }
                }
            }
        }

        private void DrawToolbar(GUIContent label)
        {
            Rect rect1 = GUILayoutUtility.GetRect(0.0f, 22f);
            bool flag = Event.current.type == UnityEngine.EventType.Repaint;
            if (flag)
                SirenixGUIStyles.ToolbarBackground.Draw(rect1, GUIContent.none, 0);
            if (!this.isReadOnly)
            {
                Rect rect2 = rect1.AlignRight(23f);
                rect1.xMax = rect2.xMin;
                if (GUI.Button(rect2, GUIContent.none, SirenixGUIStyles.ToolbarButton))
                    this.picker.ShowObjectPicker((object)null, this.Property.GetAttribute<AssetsOnlyAttribute>() == null && !typeof(ScriptableObject).IsAssignableFrom(this.resolver.ElementType), rect1, !this.Property.ValueEntry.SerializationBackend.SupportsPolymorphism);
                SdfIcons.DrawIcon(rect2.AlignCenter(13f), SdfIconType.Plus);
            }

            if (!this.isReadOnly)
            {
                Rect rect3 = rect1.AlignRight(23f);
                rect1.xMax = rect3.xMin;
                if (GUI.Button(rect3, GUIContent.none, SirenixGUIStyles.ToolbarButton))
                    this.drawAsList = !this.drawAsList;
                SdfIcons.DrawIcon(rect3.AlignCenter(13f), SdfIconType.ListOl);
            }

            this.paging.DrawToolbarPagingButtons(ref rect1, this.Property.State.Expanded, true);
            if (label == null)
                label = GUIHelper.TempContent("");
            Rect rect4 = rect1;
            rect4.x += 5f;
            rect4.y += 3f;
            rect4.height = 16f;
            if (this.Property.Children.Count > 0)
            {
                GUIHelper.PushHierarchyMode(false);
                if (this.Attribute.AlwaysExpanded)
                    GUI.Label(rect4, label);
                else
                    this.Property.State.Expanded = SirenixEditorGUI.Foldout(rect4, this.Property.State.Expanded, label);
                GUIHelper.PushHierarchyMode(true);
            }
            else
            {
                if (!flag)
                    return;
                GUI.Label(rect4, label);
            }
        }

        private void DrawColumnHeaders()
        {
            if (this.Property.Children.Count == 0)
                return;
            this.columnHeaderRect = GUILayoutUtility.GetRect(0.0f, 21f);
            ++this.columnHeaderRect.height;
            --this.columnHeaderRect.y;
            if (Event.current.type == UnityEngine.EventType.Repaint)
            {
                SirenixEditorGUI.DrawBorders(this.columnHeaderRect, 1);
                EditorGUI.DrawRect(this.columnHeaderRect, SirenixGUIStyles.ColumnTitleBg);
            }

            this.columnHeaderRect.width -= this.columnHeaderRect.width - this.table.ContentRect.width;
            GUITableUtilities.ResizeColumns<ConfigTableListAttributeDrawer.ConfigColumn>(this.columnHeaderRect, (IList<ConfigTableListAttributeDrawer.ConfigColumn>)this.columns);
            if (Event.current.type != UnityEngine.EventType.Repaint)
                return;
            GUITableUtilities.DrawColumnHeaderSeperators<ConfigTableListAttributeDrawer.ConfigColumn>(this.columnHeaderRect, (IList<ConfigTableListAttributeDrawer.ConfigColumn>)this.columns, SirenixGUIStyles.BorderColor);
            Rect columnHeaderRect = this.columnHeaderRect;
            for (int index = 0; index < this.columns.Count; ++index)
            {
                ConfigTableListAttributeDrawer.ConfigColumn configColumn = this.columns[index];
                if ((double)columnHeaderRect.x > (double)this.columnHeaderRect.xMax)
                    break;
                columnHeaderRect.width = configColumn.ColWidth;
                columnHeaderRect.xMax = Mathf.Min(this.columnHeaderRect.xMax, columnHeaderRect.xMax);
                if (configColumn.NiceName != null)
                {
                    GUI.Label(columnHeaderRect, configColumn.NiceName, SirenixGUIStyles.LabelCentered);
                    var buttonUpRect = columnHeaderRect;
                    buttonUpRect.x = buttonUpRect.xMax - 22;
                    buttonUpRect.width = 20;
                    GUI.Button(buttonUpRect, "▲");
                    
                    var buttonDownRect = columnHeaderRect;
                    buttonDownRect.x = buttonDownRect.xMax - 43;
                    buttonDownRect.width = 20;
                    GUI.Button(buttonDownRect, "▼");
                    
                    
                }
                columnHeaderRect.x += configColumn.ColWidth;
            }
        }

        private void DrawTable()
        {
            GUIHelper.PushHierarchyMode(false);
            this.table.DrawScrollView = this.Attribute.DrawScrollView && (this.paging.IsExpanded || !this.paging.IsEnabled);
            this.table.ScrollPos = this.scrollPos.Value;
            this.table.BeginTable(this.paging.EndIndex - this.paging.StartIndex);
            this.AddColumns(this.table.RowIndexFrom, this.table.RowIndexTo);
            this.DrawListItemBackGrounds();
            float xOffset = 0.0f;
            for (int index = 0; index < this.columns.Count; ++index)
            {
                ConfigTableListAttributeDrawer.ConfigColumn configColumn = this.columns[index];
                int width = (int)configColumn.ColWidth;
                if (this.isFirstFrame && configColumn.PreferWide)
                    width = 200;
                this.table.BeginColumn((int)xOffset, width);
                GUIHelper.PushLabelWidth((float)width * 0.3f);
                xOffset += configColumn.ColWidth;
                for (int rowIndexFrom = this.table.RowIndexFrom; rowIndexFrom < this.table.RowIndexTo; ++rowIndexFrom)
                {
                    this.table.BeginCell(rowIndexFrom);
                    this.DrawCell(configColumn, rowIndexFrom);
                    this.table.EndCell(rowIndexFrom);
                }

                GUIHelper.PopLabelWidth();
                this.table.EndColumn();
            }

            this.DrawRightClickContextMenuAreas();
            this.table.EndTable();
            this.scrollPos.Value = this.table.ScrollPos;
            this.DrawColumnSeperators();
            GUIHelper.PopHierarchyMode();
            if (this.columns.Count <= 0 || this.columns[0].ColumnType != ConfigTableListAttributeDrawer.ColumnType.Index)
                return;
            this.columns[0].ColWidth = (float)this.indexLabelWidth;
            this.columns[0].MinWidth = (float)this.indexLabelWidth;
        }

        private void DrawColumnSeperators()
        {
            if (Event.current.type != UnityEngine.EventType.Repaint)
                return;
            Color borderColor = SirenixGUIStyles.BorderColor;
            borderColor.a *= 0.4f;
            GUITableUtilities.DrawColumnHeaderSeperators<ConfigTableListAttributeDrawer.ConfigColumn>(this.table.OuterRect, (IList<ConfigTableListAttributeDrawer.ConfigColumn>)this.columns, borderColor);
        }

        private void DrawListItemBackGrounds()
        {
            if (Event.current.type != UnityEngine.EventType.Repaint)
                return;
            for (int rowIndexFrom = this.table.RowIndexFrom; rowIndexFrom < this.table.RowIndexTo; ++rowIndexFrom)
            {
                Color color = new Color();
                EditorGUI.DrawRect(this.table.GetRowRect(rowIndexFrom), rowIndexFrom % 2 == 0 ? SirenixGUIStyles.ListItemColorEven : SirenixGUIStyles.ListItemColorOdd);
            }
        }

        private void DrawRightClickContextMenuAreas()
        {
            for (int rowIndexFrom = this.table.RowIndexFrom; rowIndexFrom < this.table.RowIndexTo; ++rowIndexFrom)
            {
                Rect rowRect = this.table.GetRowRect(rowIndexFrom);
                this.Property.Children[rowIndexFrom].Update();
                PropertyContextMenuDrawer.AddRightClickArea(this.Property.Children[rowIndexFrom], rowRect);
            }
        }

        private void DrawCell(ConfigTableListAttributeDrawer.ConfigColumn col, int rowIndex)
        {
            rowIndex += this.paging.StartIndex;
            if (col.ColumnType == ConfigTableListAttributeDrawer.ColumnType.Index)
            {
                Rect rect = GUILayoutUtility.GetRect(0.0f, 16f);
                rect.xMin += 5f;
                rect.width -= 2f;
                if (Event.current.type != UnityEngine.EventType.Repaint)
                    return;
                this.indexLabel.text = rowIndex.ToString();
                GUI.Label(rect, this.indexLabel, SirenixGUIStyles.Label);
                this.indexLabelWidth = Mathf.Max(this.indexLabelWidth, (int)SirenixGUIStyles.Label.CalcSize(this.indexLabel).x + 15);
            }
            else if (col.ColumnType == ConfigTableListAttributeDrawer.ColumnType.DeleteButton)
            {
                if (!SirenixEditorGUI.SDFIconButton(GUILayoutUtility.GetRect(20f, 20f).AlignCenter(13f, 13f), SdfIconType.X, IconAlignment.LeftOfText, SirenixGUIStyles.IconButton))
                    return;
                this.resolver.QueueRemoveAt(rowIndex);
            }
            else
            {
                if (col.ColumnType != ConfigTableListAttributeDrawer.ColumnType.Property)
                    throw new NotImplementedException(col.ColumnType.ToString());
                this.Property.Children[rowIndex].Children[col.Name]?.Draw((GUIContent)null);
            }
        }

        private void HandleObjectPickerEvents()
        {
            if (!this.picker.IsReadyToClaim || Event.current.type != UnityEngine.EventType.Repaint)
                return;
            object obj = this.picker.ClaimObject();
            object[] values = new object[this.Property.Tree.WeakTargets.Count];
            values[0] = obj;
            for (int index = 1; index < values.Length; ++index)
                values[index] = Sirenix.Serialization.SerializationUtility.CreateCopy(obj);
            this.resolver.QueueAdd(values);
        }

        private IEnumerable<InspectorProperty> EnumerateGroupMembers(InspectorProperty groupProperty)
        {
            for (int i = 0; i < groupProperty.Children.Count; ++i)
            {
                if (groupProperty.Children[i].Info.PropertyType != PropertyType.Group)
                {
                    yield return groupProperty.Children[i];
                }
                else
                {
                    foreach (InspectorProperty enumerateGroupMember in this.EnumerateGroupMembers(groupProperty.Children[i]))
                        yield return enumerateGroupMember;
                }
            }
        }

        private T GetColumnAttribute<T>(InspectorProperty col) where T : System.Attribute => col.Info.PropertyType != PropertyType.Group ? col.GetAttribute<T>() : this.EnumerateGroupMembers(col).Select<InspectorProperty, T>((Func<InspectorProperty, T>)(c => c.GetAttribute<T>())).FirstOrDefault<T>((Func<T, bool>)(c => (object)c != null));

        private enum ColumnType
        {
            Property,
            Index,
            DeleteButton,
        }

        private class ConfigColumn : IResizableColumn
        {
            public string Name;
            public float ColWidth;
            public float MinWidth;
            public bool Preserve;
            public bool Resizable;
            public string NiceName;
            public int NiceNameLabelWidth;
            public ConfigTableListAttributeDrawer.ColumnType ColumnType;
            public bool PreferWide;

            public ConfigColumn(
                int minWidth,
                bool preserveWidth,
                bool resizable,
                string name,
                ConfigTableListAttributeDrawer.ColumnType colType)
            {
                this.MinWidth = (float)minWidth;
                this.ColWidth = (float)minWidth;
                this.Preserve = preserveWidth;
                this.Name = name;
                this.ColumnType = colType;
                this.Resizable = resizable;
            }

            float IResizableColumn.ColWidth
            {
                get => this.ColWidth;
                set => this.ColWidth = value;
            }

            float IResizableColumn.MinWidth => this.MinWidth;

            bool IResizableColumn.PreserveWidth => this.Preserve;

            bool IResizableColumn.Resizable => this.Resizable;
        }
    }
}