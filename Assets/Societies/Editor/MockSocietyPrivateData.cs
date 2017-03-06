using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.BlobSites;

namespace Assets.Societies.Editor {

    public class MockSocietyPrivateData : SocietyPrivateDataBase {

        #region static fields and properties

        private static readonly ResourceBlobFactoryBase blobFactory = new MockResourceBlobFactory();

        #endregion

        #region instance fields and properties

        #region from ISocietyPrivateData

        public override ComplexityLadderBase ActiveComplexityLadder {
            get { return _activeComplexityLadder; }
        }
        public void SetActiveComplexityLadder(ComplexityLadderBase value) {
            _activeComplexityLadder = value;
        }
        private ComplexityLadderBase _activeComplexityLadder = new MockComplexityLadder();

        public override ComplexityDefinitionBase StartingComplexity {
            get { return _startingComplexity; }
        }
        public void SetStartingComplexity(MockComplexityDefinition value) {
            _startingComplexity = value;
        }
        private ComplexityDefinitionBase _startingComplexity = new MockComplexityDefinition();

        public override ResourceBlobFactoryBase BlobFactory {
            get { return blobFactory; }
        }

        public override BlobSiteBase BlobSite {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
