using UnityEngine;

namespace BindingRx.Example
{
    public class SliderExampleInstaller : MonoBehaviour
    {
        public PropertyBinding[] Binders;
        private SliderExample instance;

        public void Start()
        {
            instance = new SliderExample();
            foreach (var binder in Binders)
            {
                binder.DataInstance = instance;
            }
        }

        public void Change()
        {
            instance.Nested[1].Data = "test change";
        }

        public void Delete(NestedSliderExample item)
        {
            instance.Nested.Remove(item);
        }
    }
}