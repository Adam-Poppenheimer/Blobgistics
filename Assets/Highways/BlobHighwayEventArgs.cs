using System;

namespace Assets.Highways {

    [Serializable]
    public class BlobHighwayEventArgs : EventArgs {

        #region instance fields and properties

        public readonly BlobHighwayBase Highway;

        #endregion

        #region constructors

        public BlobHighwayEventArgs(BlobHighwayBase highway) {
            Highway = highway;
        }

        #endregion

    }

}