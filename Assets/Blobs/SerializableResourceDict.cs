using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;
using System.Collections;

namespace Assets.Blobs {

    [Serializable]
    public class SerializableResourceCountDict {

        #region static fields and properties

        private static int ResourceTypeCount {
            get {
                if(_resourceTypeCount < 0) {
                    _resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
                }
                return _resourceTypeCount;
            }
        }

        private static int _resourceTypeCount = -1;

        #endregion

        #region instance fields and properties

        public IEnumerable<int> Values {
            get { return CountList; }
        }

        public int Count {
            get { return ResourceTypeCount; }
        }

        public int this[ResourceType key] {
            get { return CountList[(int)key]; }
            set { CountList[(int)key] = value; }
        }

        [SerializeField] private List<int> CountList;

        #endregion

        #region constructors

        public SerializableResourceCountDict() {
            CountList = new List<int>(ResourceTypeCount);
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                CountList.Add(0);
            }
        }

        public SerializableResourceCountDict(SerializableResourceCountDict other) {
            CountList = new List<int>(other.CountList);
        }

        #endregion

        #region instance methods

        public void Reset() {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                CountList[(int)resourceType] = 0;
            }
        }

        #endregion

    }
}
