using System;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;

namespace Assets.Societies {

    public abstract class SocietyPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }
        public abstract ComplexityDefinitionBase StartingComplexity { get; }

        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        public abstract BlobSiteBase BlobSite { get; }

        #endregion

    }

}