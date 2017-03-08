using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Map;

namespace Assets.Societies.ForTesting {

    public class MockSocietyPrivateData : SocietyPrivateDataBase {

        #region instance fields and properties

        #region from ISocietyPrivateData

        public override ComplexityLadderBase ActiveComplexityLadder {
            get { return _activeComplexityLadder; }
        }
        public void SetActiveComplexityLadder(ComplexityLadderBase value) {
            _activeComplexityLadder = value;
        }
        private ComplexityLadderBase _activeComplexityLadder = new MockComplexityLadder();

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        private ResourceBlobFactoryBase _blobFactory;

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        private MapNodeBase _location;

        #endregion

        #endregion

    }

}
