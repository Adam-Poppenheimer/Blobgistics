using System;

using Assets.BlobEngine;

namespace Assets.Societies {

    public interface ISocietyPrivateData {

        #region properties

        IComplexityLadder ActiveComplexityLadder { get; }
        IComplexityDefinition StartingComplexity { get; }

        #endregion

    }

}