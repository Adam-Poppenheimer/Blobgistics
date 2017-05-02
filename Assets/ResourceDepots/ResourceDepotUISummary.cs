using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.ResourceDepots {

    public class ResourceDepotUISummary {

        #region instance fields and properties

        public int ID { get; set; }

        public Transform Transform { get; set; }

        #endregion

        #region constructors

        public ResourceDepotUISummary() { }

        public ResourceDepotUISummary(ResourceDepotBase depotToSummarize) {
            ID = depotToSummarize.ID;
            Transform = depotToSummarize.transform;
        }

        #endregion

    }

}
