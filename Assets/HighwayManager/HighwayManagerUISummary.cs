using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager {

    public class HighwayManagerUISummary {

        #region instance fields and properties

        public int ID { get; set; }

        public float LastEfficiency { get; set; }

        public ReadOnlyDictionary<ResourceType, int> LastUpkeep { get; set; }

        #endregion

        #region constructors

        public HighwayManagerUISummary() { }

        public HighwayManagerUISummary(HighwayManagerBase manager) {
            ID = manager.ID;
            LastEfficiency = manager.LastCalculatedEfficiency;
            LastUpkeep = manager.LastCalculatedUpkeep;
        }

        #endregion

    }

}
