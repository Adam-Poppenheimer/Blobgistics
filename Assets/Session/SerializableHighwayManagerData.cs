using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.HighwayManager;

namespace Assets.Session {

    [Serializable]
    public class SerializableHighwayManagerData {

        #region instance fields and properties

        public int LocationID;

        #endregion

        #region constructors

        public SerializableHighwayManagerData(HighwayManagerBase highwayManager) {
            LocationID = highwayManager.Location.ID;
        }

        #endregion

    }

}
