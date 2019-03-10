using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace BindingRx.Example
{
    public class SliderExample
    {
        public float FloatValue { get; set; }
        
        public List<NestedSliderExample> Nested { get; set; }

        public SliderExample()
        {
            FloatValue = 0;
            Nested = new List<NestedSliderExample>()
            {
                new NestedSliderExample()
                {
                    Data = Guid.NewGuid().ToString(),
                    FloatData = Random.Range(0f, 1f)
                },
                new NestedSliderExample()
                {
                    Data = Guid.NewGuid().ToString(),
                    FloatData = Random.Range(0f, 1f)
                },
                new NestedSliderExample()
                {
                    Data = Guid.NewGuid().ToString(),
                    FloatData = Random.Range(0f, 1f)
                },
                new NestedSliderExample()
                {
                    Data = Guid.NewGuid().ToString(),
                    FloatData = Random.Range(0f, 1f)
                },
                new NestedSliderExample()
                {
                    Data = Guid.NewGuid().ToString(),
                    FloatData = Random.Range(0f, 1f)
                }
            };
        }
    }

    public class NestedSliderExample
    {
        public string Data { get; set; }
        public float FloatData { get; set; }
    }
}