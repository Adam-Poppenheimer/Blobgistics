using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Highways;
using Assets.Blobs;

namespace Assets.UI.Highways.ForTesting {

    public class MockBlobHighwaySummaryDisplay : BlobHighwaySummaryDisplayBase {

        #region instance fields and properties

        #region from BlobHighwaySummaryDisplayBase

        public override BlobHighwayUISummary CurrentSummary { get; set; }
        public override bool CanBeUpgraded { get; set; }
        public override bool IsBeingUpgraded { get; set; }

        #endregion

        public bool WasCleared = false;
        public bool WasUpdated = false;

        #endregion

        #region instance methods

        #region from BlobHighwaySummaryDisplayBase

        public override void Activate() {
            throw new NotImplementedException();
        }

        public override void Deactivate() {
            throw new NotImplementedException();
        }

        public override void ClearDisplay() {
            WasCleared = true;
        }

        public override void UpdateDisplay() {
            WasUpdated = true;
        }

        #endregion

        public void ChangePriority(int newPriority) {
            RaisePriorityChanged(newPriority);
        }

        public void ChangeFirstEndpointPermission(ResourceType type, bool isNowPermitted) {
            RaiseFirstEndpointPermissionChanged(type, isNowPermitted);
        }

        public void ChangeSecondEndpointPermission(ResourceType type, bool isNowPermitted) {
            RaiseSecondEndpointPermissionChanged(type, isNowPermitted);
        }

        public void RaiseUpgradeRequest() {
            RaiseBeginHighwayUpgradeRequested();
        }

        #endregion

    }

}
