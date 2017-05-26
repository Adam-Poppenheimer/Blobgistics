using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.HighwayManager;
using Assets.Map;
using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockHighwayManager : HighwayManagerBase {

        #region instance fields and properties

        #region from HighwayManagerBase

        public override int ID {
            get {
                if(_id == 0) {
                    _id = GetInstanceID();
                }
                return _id;
            }
        }
        public void SetID(int value) {
            _id = value;
        }
        private int _id = 0;

        public override ReadOnlyDictionary<ResourceType, int> LastCalculatedUpkeep {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase Location {
            get { return location; }
        }
        public MapNodeBase location;

        #endregion

        #endregion

        #region instance methods

        #region from HighwayManagerBase

        public override void TickConsumption(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
