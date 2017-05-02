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

        public int ID { get; set; }

        public ReadOnlyDictionary<ResourceType, int> LastUpkeep { get; set; }

        public Transform Transform { get; set; }

        #endregion

        #region constructors

        public HighwayManagerUISummary() { }

        public HighwayManagerUISummary(HighwayManagerBase manager) {
            ID = manager.ID;
            LastUpkeep = manager.LastCalculatedUpkeep;
            Transform = manager.transform;
        }

        #endregion

    }

}
