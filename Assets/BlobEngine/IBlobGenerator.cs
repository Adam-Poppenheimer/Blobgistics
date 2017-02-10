using System;

namespace Assets.BlobEngine {

    public interface IBlobGenerator : IBlobSite {

        #region properties

        ResourceType BlobTypeGenerated { get; }

        #endregion

    }

}