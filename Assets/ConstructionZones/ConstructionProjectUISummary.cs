using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    public class ConstructionProjectUISummary {

        #region instance fields and properties

        public string Name { get; set; }
        public ResourceDisplayInfo Cost { get; set; }

        #endregion

        #region constructors

        public ConstructionProjectUISummary() { }

        public ConstructionProjectUISummary(string name, ResourceDisplayInfo costDisplayInfo) {
            Name = name;
            Cost = costDisplayInfo;
        }

        public ConstructionProjectUISummary(ConstructionProjectBase projectToSummarize) {
            Name = projectToSummarize.name;
            Cost = projectToSummarize.GetCostInfo();
        }

        #endregion

    }

}
