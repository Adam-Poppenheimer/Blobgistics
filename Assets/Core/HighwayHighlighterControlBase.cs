using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Core {

    public abstract class HighwayHighlighterControlBase : MonoBehaviour {

        #region instance methods

        public abstract void HighlightHighway(int highwayID);
        public abstract void UnhighlightHighway(int highwayID);
        public abstract void UnhighlightAllHighways();

        #endregion

    }

}
