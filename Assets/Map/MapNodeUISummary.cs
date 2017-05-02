using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

namespace Assets.Map {

    public class MapNodeUISummary {

        #region instance fields and properties

        public int ID { get; set; }
        public BlobSiteUISummary BlobSite { get; set; }

        public Transform Transform { get; set; }

        #endregion

        #region constructors

        public MapNodeUISummary() { }

        public MapNodeUISummary(MapNodeBase nodeToSummarize) {
            ID = nodeToSummarize.ID;
            BlobSite = new BlobSiteUISummary(nodeToSummarize.BlobSite);
            Transform = nodeToSummarize.transform;
        }

        #endregion

    }

}
