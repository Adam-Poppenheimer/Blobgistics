using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.HighwayManager;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableHighwayManagerData {

        #region instance fields and properties

        [DataMember()] public int LocationID;

        #endregion

        #region constructors

        public SerializableHighwayManagerData(HighwayManagerBase highwayManager) {
            LocationID = highwayManager.Location.ID;
        }

        #endregion

    }

}
