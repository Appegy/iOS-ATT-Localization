using System;
using System.Collections.Generic;
using UnityEngine;

namespace Appegy.Att.Localization
{
    [Serializable]
    internal class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct Item
        {
            public TKey Key;
            public TValue Value;
        }

        [SerializeField]
        private List<Item> _map = new List<Item>();

        public void OnBeforeSerialize()
        {
            _map.Clear();
            foreach (var pair in this)
            {
                _map.Add(new Item {Key = pair.Key, Value = pair.Value});
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var item in _map)
            {
                this[item.Key] = item.Value;
            }
        }
    }
}