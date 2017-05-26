using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    public class HighwayManagerUISummary {

        #region instance fields and properties

        public int ID {
            get { return UnderlyingManager.ID; }
        }

        public ReadOnlyDictionary<ResourceType, int> LastUpkeep {
            get { return UnderlyingManager.LastCalculatedUpkeep; }
        }

        public Transform Transform {
            get { return UnderlyingManager.transform; }
        }

        private HighwayManagerBase UnderlyingManager;

        #endregion

        #region constructors

        public HighwayManagerUISummary() { }

        public HighwayManagerUISummary(HighwayManagerBase underlyingManager) {
            UnderlyingManager = underlyingManager;
        }

        #endregion

    }

}
