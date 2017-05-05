using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;

namespace Assets.UI {

    public class ToggleResourceSummary : ResourceSummaryBase<Toggle> {

        #region instance fields and properties

        #region from ResourceSummaryBase<Toggle>

        protected override Toggle DefaultValue {
            get { return null; }
        }

        #endregion

        #endregion
        
    }

}
