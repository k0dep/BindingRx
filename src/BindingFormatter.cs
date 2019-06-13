using TypeInspector;
using UnityEngine;

namespace BindingRx
{
    public class BindingFormatter : MonoBehaviour
    {
        public object Value
        {
            get => null;
            set => Property.Set(string.Format(Format, value));
        }

        public string Format = "{0}";
        public MonoPropertyReference Property;
    }
}