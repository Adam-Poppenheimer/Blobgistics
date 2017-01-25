using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BlobEngine {

    public class BlobPileCapacity {

        #region static fields and properties

        public static readonly BlobPileCapacity NoCapacity =
            new BlobPileCapacity(new Dictionary<ResourceType, int>());

        #endregion

        #region instance fields and properties

        private readonly Dictionary<ResourceType, int> CapacityForResourceType =
            new Dictionary<ResourceType, int>();

        public int TotalSpace {
            get {
                if(_totalSpace == -1) {
                    _totalSpace = 0;
                    foreach(int capacity in CapacityForResourceType.Values) {
                        _totalSpace += capacity;
                    }
                }
                return _totalSpace;
            }
        }
        private int _totalSpace = -1;

        #endregion

        #region constructors

        public BlobPileCapacity(Dictionary<ResourceType, int> capacityForResourceType) {
            if(capacityForResourceType == null) {
                throw new ArgumentNullException("capacityForResourceType");
            }
            CapacityForResourceType = new Dictionary<ResourceType, int>(capacityForResourceType);
        }

        #endregion

        #region instance methods

        public int GetCapacityForType(ResourceType type) {
            int retval;
            CapacityForResourceType.TryGetValue(type, out retval);
            return retval;
        }

        #endregion

    }

}
