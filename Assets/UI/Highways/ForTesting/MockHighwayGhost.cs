using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;
using UnityEngine.EventSystems;

namespace Assets.UI.Highways.ForTesting {

    public class MockHighwayGhost : BlobHighwayGhostBase {

        #region instance fields and properties

        #region from BlobHighwayGhostBase

        public override bool IsActivated {
            get { return isActivated; }
        }
        private bool isActivated = false;

        public override MapNodeUISummary FirstEndpoint  { get; set; }
        public override MapNodeUISummary SecondEndpoint { get; set; }

        public override bool GhostIsBuildable { get; set; }

        public bool WasActivated { get; set; }
        public bool WasCleared { get; set; }
        public bool WasDeactivated { get; set; }

        public bool WasUpdatedWithEventData { get; set; }
        public PointerEventData EventDataUpdatedWith { get; set; }

        #endregion

        #endregion

        #region instance methods

        #region from BlobHighwayGhostBase

        public override void Activate() {
            isActivated = true;
            WasActivated = true;
        }

        public override void Deactivate() {
            isActivated = false;
            WasDeactivated = true;
        }

        public override void Clear() {
            FirstEndpoint = null;
            SecondEndpoint = null;
            WasCleared = true;
        }

        public override void UpdateWithEventData(PointerEventData eventData) {
            WasUpdatedWithEventData = true;
            EventDataUpdatedWith = eventData;
        }

        #endregion

        #endregion

    }

}
