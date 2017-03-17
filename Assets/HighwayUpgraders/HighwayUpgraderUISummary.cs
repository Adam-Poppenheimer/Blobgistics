using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.HighwayUpgraders {

    public class HighwayUpgraderUISummary {

        #region instance fields and properties

        public int ID { get; set; }

        #endregion

        #region constructors

        public HighwayUpgraderUISummary() { }

        public HighwayUpgraderUISummary(HighwayUpgraderBase upgradeToSummarize) {
            ID = upgradeToSummarize.ID;
        }

        #endregion

    }

}
