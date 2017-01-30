using System;

namespace Assets.BlobEngine {

    public interface IBlobGenerator : IBlobSource {

        #region properties

        ResourceType BlobTypeGenerated { get; }

        #endregion

    }

}