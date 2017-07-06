using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Societies {

    [Serializable]
    public class ComplexityAscentPermissionEventArgs : EventArgs {

        #region instance fields and properties

        public readonly ComplexityDefinitionBase Complexity;
        public readonly bool AscensionIsPermitted;

        #endregion

        #region constructors

        public ComplexityAscentPermissionEventArgs(ComplexityDefinitionBase complexity, bool ascensionIsPermitted) {
            Complexity = complexity;
            AscensionIsPermitted = ascensionIsPermitted;
        }

        #endregion

    }

}
