using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Societies {

    public abstract class SocietyUISummaryDisplayBase : IntelligentPanelBase {

        #region instance fields and properties

        public abstract SocietyUISummary CurrentSummary { get; set; }

        #endregion

        #region events

        public event EventHandler<EventArgs> DestructionRequested;
        public event EventHandler<BoolEventArgs> AscensionPermissionChangeRequested;

        protected void RaiseDestructionRequested() {
            if(DestructionRequested != null) {
                DestructionRequested(this, EventArgs.Empty);
            }
        }

        protected void RaiseAscensionPermissionChangeRequested(bool ascensionPermitted) {
            if(AscensionPermissionChangeRequested != null) {
                AscensionPermissionChangeRequested(this, new BoolEventArgs(ascensionPermitted));
            }
        }

        #endregion

    }

}
