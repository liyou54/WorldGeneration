using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Script.Skill.Effect;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Script.Skill.Editor
{
    public class MyCustomAttributeDrawer<T> : OdinAttributeDrawer<Enum2StaticClassAttribute, T>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            this.Property.Update();
            EditorGUILayout.BeginHorizontal();
            var labelText = this.Property.GetAttribute<LabelTextAttribute>();

            labelText ??= typeof(T).GetAttribute<LabelTextAttribute>();

            if (labelText != null)
            {
                EditorGUILayout.LabelField(labelText.Text);
            }
            else
            {
                EditorGUILayout.LabelField(this.Property.Name);
            }
            
            var attr = this.Property.GetAttribute<Enum2StaticClassAttribute>();
            var enumObj = GetSubClasses(attr.StaticClassType, attr.EnumStructType);

            var value = this.Property.Children["Value"].ValueEntry.WeakSmartValue as int?;
            
            var enumDrawerFieldData = enumObj.FirstOrDefault(x => x.GetIntValueByReflection() == value);
            
            
            if (GUILayout.Button(enumDrawerFieldData?.GetNameFromFieldAndLabelTextAttribute()))
            {
                AddParam(this.Property.Children["Value"].ValueEntry, enumObj);
            }

            EditorGUILayout.EndHorizontal();
        }

        private class EnumDrawerFieldData
        {
            public FieldInfo Field;
            public object Value;

            public string GetNameFromFieldAndLabelTextAttribute()
            {
                var name = "";
                var LabelTextAttribute = Field.GetCustomAttribute<LabelTextAttribute>();
                if (LabelTextAttribute != null)
                {
                    name = LabelTextAttribute.Text;
                }
                else
                {
                    name = Field.Name;
                }
                return name;
            }

            public int GetIntValueByReflection()
            {
                var type = Value.GetType();
                var fields = type.GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(int) && field.Name == "Value")
                    {
                        var res =  (int)field.GetValue(Value);
                        return res;
                    }
                }

                return -1;
            }
        }

        private List<EnumDrawerFieldData> GetSubClasses(Type staticClassType, Type enumStructType)
        {
            List<EnumDrawerFieldData> effectMajorTypeList = new List<EnumDrawerFieldData>();


            FieldInfo[] fields = staticClassType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                var value = field.GetValue(null);

                if (value.GetType() == enumStructType)
                {
                    var data = new EnumDrawerFieldData();
                    data.Field = field;
                    data.Value = value;
                    effectMajorTypeList.Add(data);
                }
            }

            return effectMajorTypeList;
        }


        private void AddParam(IPropertyValueEntry property, List<EnumDrawerFieldData> enumObj)
        {
            GenericSelector<EnumDrawerFieldData> CustomGenericSelector;
            IEnumerable<GenericSelectorItem<EnumDrawerFieldData>> customCollection =
                enumObj.Select(x => new GenericSelectorItem<EnumDrawerFieldData>(x.GetNameFromFieldAndLabelTextAttribute(), x));
            CustomGenericSelector = new GenericSelector<EnumDrawerFieldData>("枚举选择", false, customCollection);
            CustomGenericSelector.EnableSingleClickToSelect();
            CustomGenericSelector.SelectionConfirmed += ints =>
            {
                var result = ints.FirstOrDefault();
                if (result != null)
                {
                    property.WeakSmartValue = result.GetIntValueByReflection();
                    property.ApplyChanges();
                }
            };
            CustomGenericSelector.ShowInPopup();
        }
    }
}