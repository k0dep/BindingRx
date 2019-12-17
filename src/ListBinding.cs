using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BindingRx
{
    /// <summary>
    ///     Create runtime map from enumerable to game objects and inject data instance to created objects
    /// </summary>
    public class ListBinding : MonoBehaviour, IDataInstance
    {
        public object DataInstance
        {
            get => _dataInstance;
            set
            {
                if (_dataInstance != value)
                {
                    _dataInstance = value as IEnumerable;
                    Init();
                }
            }
        }

        public GameObject ItemPrefab;

        private IWatcher _srcSequenceWatcher;
        private IEnumerable _dataInstance;

        private List<GameObject> _cache = new List<GameObject>();

        private IDisposable _subscription;

        private void Init()
        {
            _subscription?.Dispose();

            _srcSequenceWatcher = new StateWatcher<IEnumerable>(_dataInstance,
                data => data.OfType<object>()
                    .Select(item => item.GetHashCode())
                    .Aggregate(0, (int acc, int item) => unchecked(acc += acc * 314159 + item)));
            _subscription = _srcSequenceWatcher.Watch().Subscribe(OnSequenceChange);
        }

        public void OnEnable()
        {
            if(DataInstance != null)
            {
                Init();
            }
        }

        public void OnDisable()
        {
            _subscription?.Dispose();
        }

        private void Awake()
        {
            if (ItemPrefab == null)
            {
                throw new Exception("ListBinding item prefab is null! check it.");
            }

            if (ItemPrefab.scene.name != null)
            {
                ItemPrefab.SetActive(false);
            }
        }

        private void OnSequenceChange(object obj)
        {
            foreach (var cacheItem in _cache)
            {
                Destroy(cacheItem);
            }

            _cache.Clear();

            foreach (var item in _dataInstance)
            {
                var itemInstance = Instantiate(ItemPrefab, transform);
                itemInstance.SetActive(true);
                var dataInstanceComponents = itemInstance.GetComponents<IDataInstance>();
                foreach (var instanceComponent in dataInstanceComponents)
                {
                    instanceComponent.DataInstance = item;
                }

                _cache.Add(itemInstance);
            }
        }
    }
}