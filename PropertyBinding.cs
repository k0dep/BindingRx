using System;
using TypeInspector;
using UniRx;
using UnityEngine;

namespace BindingRx
{
    public class PropertyBinding : MonoBehaviour, IDataInstance
    {
        [SerializeField]
        private BindingWayType _bindingWayType = BindingWayType.BothWays;
        
        [SerializeField]
        private PropertyReference _source;
        
        [SerializeField]
        private MonoPropertyReference _destination;

        private object _srcState;
        private bool _isSrcRx;

        private IWatcher _dstWatcher;
        private IWatcher _srcWatcher;

        private object _dataInstance;
        public object DataInstance
        {
            get { return _dataInstance; }
            set
            {
                _dataInstance = value;
                Initialize();
            }
        }
        
        public bool IsValid()
        {
            return _source != null && _source.IsValid() && _destination != null && _destination.IsValid();
        }

        public void Initialize()
        {
            if (!IsValid())
            {
                enabled = false;
                Debug.LogError("State is not valid!", this);
                return;
            }
            
            var rxLeft = GetAndValidateLeft();

            var propertyType = _source.GetProperty().PropertyType;
            if (IsAssignableToGenericType(propertyType, typeof(ReactiveProperty<>)))
            {
                _isSrcRx = true;
                SubscribeToLeft(rxLeft);
            }
            else
            {
                CreateSrcStateWatcher();
            }
            
            CreateDstStateWatcher();
        }

        private void CreateSrcStateWatcher()
        {
            if (!_bindingWayType.HasFlag(BindingWayType.SourceToDestination))
            {
                return;
            }
            
            _srcWatcher = new StateWatcher<PropertyReference>(_source, o => o.Get(DataInstance));
            _srcWatcher.Watch().Subscribe(SrcStateChanged);
        }

        private void CreateDstStateWatcher()
        {
            if (!_bindingWayType.HasFlag(BindingWayType.DestinationToSource))
            {
                return;
            }

            _dstWatcher = new StateWatcher<MonoPropertyReference>(_destination, o => o.Get());
            _dstWatcher.Watch().Subscribe(DstStateChanged);
        }

        private void SubscribeToLeft(object rxLeft)
        {
            if (_bindingWayType.HasFlag(BindingWayType.SourceToDestination))
            {
                return;
            }
            
            var methodInfo = rxLeft.GetType().GetMethod("Subscribe");
            methodInfo.Invoke(rxLeft, new[] { Observer.Create<object>(SrcStateChanged) });
        }

        private object GetAndValidateLeft()
        {
            if (DataInstance == null)
            {
                Debug.LogError("Data instance is null!");
                return null;
            }

            var rxLeft = _source.Get(DataInstance);

            if (rxLeft != null)
            {
                return rxLeft;
            }
            
            Debug.LogError("Property at left side is null! Check data instance initialization.");
            return null;
        }

        private void DstStateChanged(object value)
        {
            if (_isSrcRx)
            {
                //TODO: оптимизировать
                _source.GetSourceType().GetProperty("Value").SetValue(_source.Get(DataInstance), value);
            }
            else
            {
                _source.Set(DataInstance, value);
            }
            
            _srcWatcher?.UpdateState();
        }
        
        private void SrcStateChanged(object value)
        {
            _destination.Set(value);
            _dstWatcher?.UpdateState();
        }
        
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}