using UnityEngine;

namespace BindingRx.Example
{
    public class SliderExampleInstaller : MonoBehaviour
    {
        public PropertyBinding[] Binders;

        public void Start()
        {
            var instance = new SliderExample();
            foreach (var binder in Binders)
            {
                binder.DataInstance = instance;
            }
        }
    }
}