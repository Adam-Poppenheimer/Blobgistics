using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;

namespace Assets.Societies.Editor {

    public class MockSocietyPrivateData : ISocietyPrivateData {

        #region static fields and properties

        private static readonly IResourceBlobFactory blobFactory = new MockResourceBlobFactory();

        #endregion

        #region instance fields and properties

        #region from ISocietyPrivateData

        public IComplexityLadder ActiveComplexityLadder {
            get { return _activeComplexityLadder; }
            set { _activeComplexityLadder = value; }
        }
        private IComplexityLadder _activeComplexityLadder = new MockComplexityLadder();

        public IComplexityDefinition StartingComplexity {
            get { return _startingComplexity; }
            set { _startingComplexity = value; }
        }
        private IComplexityDefinition _startingComplexity = new MockComplexityDefinition();

        public IResourceBlobFactory BlobFactory {
            get { return blobFactory; }
        }

        #endregion

        #endregion
        
    }

}
