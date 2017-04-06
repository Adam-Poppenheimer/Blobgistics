using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.ConstructionZones {

    public class ConstructionProjectUISummary {

        #region instance fields and properties

        public string Name { get; set; }
        public string CostSummaryString { get; set; }

        #endregion

        #region constructors

        public ConstructionProjectUISummary() { }

        public ConstructionProjectUISummary(ConstructionProjectBase projectToSummarize) {
            Name = projectToSummarize.name;
            CostSummaryString = projectToSummarize.GetCostSummaryString();
        }

        #endregion

    }

}
