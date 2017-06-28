using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;

namespace Assets.UI.Highways {

    public abstract class BlobHighwayHighlighterBase : MonoBehaviour {

        #region instance fields and properties

        public abstract List<BlobHighwayUISummary> HighwaysToHighlight { get; set; }

        #endregion

        #region instance methods

        public abstract void ToggleHighlight(bool highlightIsOn);

        #endregion

    }

}
