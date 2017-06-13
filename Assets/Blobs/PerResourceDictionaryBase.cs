using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    public abstract class PerResourceDictionaryBase<T> : MonoBehaviour, IEnumerable<ResourceType> {

        #region instance fields and properties

        public T this[ResourceType type] {
            get {
                return ValueList[(int)type];
            }
            set {
                ValueList[(int)type] = value;
                if(DictionaryRepresentation != null) {
                    DictionaryRepresentation[type] = value;
                }
            }
        }
        [SerializeField] protected List<T> ValueList = new List<T>();

        protected abstract T DefaultValue { get; }

        private Dictionary<ResourceType, T> DictionaryRepresentation;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = ValueList.Count; i < resourceTypeCount; ++i) {
                ValueList.Add(DefaultValue);
            }
        }

        private void OnValidate() {
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = ValueList.Count; i < resourceTypeCount; ++i) {
                ValueList.Add(DefaultValue);
            }
        }

        private void Reset() {
            ValueList.Clear();
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = ValueList.Count; i < resourceTypeCount; ++i) {
                ValueList.Add(DefaultValue);
            }
        }

        #endregion

        #region from IEnumerable<ResourceType>

        public IEnumerator<ResourceType> GetEnumerator() {
            return EnumUtil.GetValues<ResourceType>().GetEnumerator();
        }

        #endregion

        #region from IEnumerable

        IEnumerator IEnumerable.GetEnumerator() {
            return ValueList.GetEnumerator();
        }

        #endregion

        public bool TryGetValue(ResourceType key, out T value) {
            value = DefaultValue;
            if(ValueList.Count > (int)key) {
                value = this[key];
                return true;
            }else {
                return false;
            }
        }

        public ReadOnlyDictionary<ResourceType, T> ToReadOnlyDictionary() {
            if(DictionaryRepresentation == null) {
                DictionaryRepresentation = new Dictionary<ResourceType, T>();
                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    DictionaryRepresentation[resourceType] = this[resourceType];
                }
            }
            return new ReadOnlyDictionary<ResourceType, T>(DictionaryRepresentation);
        }

        #endregion

    }

}
