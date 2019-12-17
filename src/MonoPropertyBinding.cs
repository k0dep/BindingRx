using System;
using TypeInspector;
using UniRx;
using UnityEngine;

namespace BindingRx
{
    /// <summary>
    ///     Binding for source and destination is MonoBehaviour
    /// </summary>
    public class MonoPropertyBinding : MonoBehaviour
    {
        [SerializeField]
        private BindingWayType _bindingWayType = BindingWayType.BothWays;
        
        [SerializeField]
        private MonoPropertyReference _source;
        
        [SerializeField]
        private MonoPropertyReference _destination;

        private IWatcher _dstWatcher;
        private IWatcher _srcWatcher;

        private IDisposable _srcSubscription;
        private IDisposable _dstSubscription;

        public void OnEnable()
        {
            Initialize();
        }

        public void OnDisable()
        {
            _srcSubscription?.Dispose();
            _dstSubscription?.Dispose();
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
            
            CreateSrcStateWatcher();
            CreateDstStateWatcher();
        }

        private void CreateSrcStateWatcher()
        {
            if (!_bindingWayType.HasFlag(BindingWayType.SourceToDestination))
            {
                return;
            }
            
            _srcWatcher = new StateWatcher<MonoPropertyReference>(_source, o => o.Get());
            _srcSubscription = _srcWatcher.Watch().Subscribe(SrcStateChanged);
        }

        private void CreateDstStateWatcher()
        {
            if (!_bindingWayType.HasFlag(BindingWayType.DestinationToSource))
            {
                return;
            }

            _dstWatcher = new StateWatcher<MonoPropertyReference>(_destination, o => o.Get());
            _dstSubscription = _dstWatcher.Watch().Subscribe(DstStateChanged);
        }

        private void DstStateChanged(object value)
        {
            _source.Set(value);
            _srcWatcher?.UpdateState();
        }
        
        private void SrcStateChanged(object value)
        {
            _destination.Set(value);
            _dstWatcher?.UpdateState();
        }
    }
}