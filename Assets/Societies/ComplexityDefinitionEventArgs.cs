using System;

namespace Assets.Societies {

    public class ComplexityDefinitionEventArgs : EventArgs {

        #region instance fields and properties

        public readonly ComplexityDefinitionBase Complexity;

        #endregion

        #region constructors

        public ComplexityDefinitionEventArgs(ComplexityDefinitionBase complexity) {
            Complexity = complexity;
        }

        #endregion

    }

}