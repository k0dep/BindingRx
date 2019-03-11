using System.Reflection;
using TypeInspector;
using UnityEngine;
using UnityEngine.UI;

namespace BindingRx
{
    public class ButtonBinding : MonoBehaviour, IDataInstance
    {
        public object DataInstance { get; set; }

        public Button SourceButton;

        [MethodsFilter(nameof(FilterMethods))]
        public MonoMethodReference OnClickMethod;

        private void Awake()
        {
            SourceButton?.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var method = OnClickMethod.GetMethod();
            if (method == null)
            {
                Debug.LogError("Method is not valid", this);
                return;
            }

            method.Invoke(OnClickMethod.Target, new[] { DataInstance });
        }

        private bool FilterMethods(MethodInfo method)
        {
            return method.GetParameters().Length == 1;
        }
    }
}