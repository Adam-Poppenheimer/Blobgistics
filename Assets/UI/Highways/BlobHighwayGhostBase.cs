using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

namespace Assets.UI.Highways {

    public abstract class BlobHighwayGhostBase : MonoBehaviour {

        #region instance fields and properties

        public abstract bool IsActivated { get; }

        public abstract MapNodeUISummary FirstEndpoint { get; set; }
        public abstract MapNodeUISummary SecondEndpoint { get; set; }

        public abstract bool GhostIsBuildable { get; set; }

        #endregion

        #region instance methods

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Clear();

        public abstract void UpdateWithEventData(PointerEventData eventData);

        #endregion

    }

}
