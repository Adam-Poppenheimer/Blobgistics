using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    public abstract class ResourceSummaryBase<T> : MonoBehaviour, IEnumerable<ResourceType> {

        #region instance fields and properties

        public T this[ResourceType type] {
            get {
                return ValueList[(int)type];
            }
            protected set {
                ValueList[(int)type] = value;
            }
        }
        [SerializeField] protected List<T> ValueList = new List<T>();

        protected abstract T DefaultValue { get; }

        #endregion

        #region instance methods

        #region Unity event methods

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

        #endregion

    }

}
