using System;

namespace Assets.Societies {

    public class SocietyEventArgs : EventArgs {

        #region instance fields and properties

        public readonly SocietyBase Society;

        #endregion

        #region constructors

        public SocietyEventArgs(SocietyBase society) {
            Society = society;
        }

        #endregion

    }

}