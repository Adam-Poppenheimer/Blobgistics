using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Societies;

namespace Assets.Core.ForTesting {

    public class MockSocietyControl : SocietyControlBase {

        #region events

        public event Action<int, bool> OnAscensionPermissionChangeRequested;
        public event Action<int> OnSocietyDestructionRequested;

        #endregion

        #region instance methods

        #region from SocietyControlBase

        public override void DestroySociety(int societyID) {
            if(OnSocietyDestructionRequested != null) {
                OnSocietyDestructionRequested(societyID);
            }
        }

        public override void SetGeneralAscensionPermissionForSociety(int societyID, bool ascensionPermitted) {
            if(OnAscensionPermissionChangeRequested != null) {
                OnAscensionPermissionChangeRequested(societyID, ascensionPermitted);
            }
        }

        public override void SetSpecificAscensionPermissionForSociety(int societyID, ComplexityDefinitionBase complexity, bool ascensionPermitted) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
