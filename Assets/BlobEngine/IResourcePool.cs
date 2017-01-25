namespace Assets.BlobEngine {

    public interface IResourcePool : IBlobSource, IBlobTarget {

        #region methods

        void ClearAllBlobs();

        #endregion

    }

}