using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Script.Skill.BlackBoardParam.Editor
{
    public class BlackBoardParamSetInspector : OdinValueDrawer<BlackBoardParamSet>
    {
        public IEnumerable<Type> GetParamType()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> subclasses = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.BaseType != null && type.BaseType.IsGenericType &&
                        type.BaseType.GetGenericTypeDefinition() == typeof(BlackBoardParam<>))
                    {
                        subclasses.Add(type);
                    }
                }
            }

            return subclasses;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            this.CallNextDrawer(label);

            if (GUILayout.Button("Add"))
            {
                AddParam();
            }
        }

        private string GetDefaultName(Type type)
        {
            var baseName = type.Name.Substring(10, type.Name.Length - 5 - 10);
            var index = 0;
            while (true)
            {
                var find = false;
                foreach (var param in this.ValueEntry.SmartValue.Data)
                {
                    if ((param as BlackBoardParam).Key == baseName + "_" + index)
                    {
                        find = true;
                        break;
                    }
                }

                if (find)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }

            return baseName + "_" + index;
        }

        private void AddParam()
        {
            GenericSelector<Type> CustomGenericSelector;
            IEnumerable<GenericSelectorItem<Type>> customCollection = GetParamType().Select(x => new GenericSelectorItem<Type>(x.Name, x));
            CustomGenericSelector = new GenericSelector<Type>("添加黑板变量", false, customCollection);
            CustomGenericSelector.EnableSingleClickToSelect();
            CustomGenericSelector.SelectionConfirmed += ints =>
            {
                var result = ints.FirstOrDefault();
                if (result != null)
                {
                    var instance = Activator.CreateInstance(result);
                    var blackBoardParam = instance as BlackBoardParam;

                    blackBoardParam.Key = GetDefaultName(instance.GetType());

                    this.ValueEntry.SmartValue.Data.Add(instance);
                }
            };
            CustomGenericSelector.ShowInPopup();
        }
    }
}