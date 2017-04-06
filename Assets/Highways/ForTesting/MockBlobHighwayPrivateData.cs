using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;
using Assets.Core;
using Assets.HighwayUpgraders;
using Assets.Blobs;

namespace Assets.Highways.ForTesting {

    public class MockBlobHighwayPrivateData : BlobHighwayPrivateDataBase {

        #region instance fields and properties

        #region from BlobHighwayPrivateDataBase

        public override UIControlBase UIControl {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                throw new NotImplementedException();
            }
        }

        public override BlobTubeBase TubePullingFromFirstEndpoint {
            get { return _tubePullingFromFirstEndpoint; }
        }
        public void SetTubePullingFromFirstEndpoint(BlobTubeBase value) {
            _tubePullingFromFirstEndpoint = value;
        }
        private BlobTubeBase _tubePullingFromFirstEndpoint;

        public override BlobTubeBase TubePullingFromSecondEndpoint {
            get { return _tubePullingFromSecondEndpoint; }
        }
        public void SetTubePullingFromSecondEndpoint(BlobTubeBase value) {
            _tubePullingFromSecondEndpoint = value;
        }
        private BlobTubeBase _tubePullingFromSecondEndpoint;

        public override MapNodeBase FirstEndpoint {
            get { return _firstEndpoint; }
        }
        public void SetFirstEndpoint(MapNodeBase value) {
            _firstEndpoint = value;
        }
        private MapNodeBase _firstEndpoint;

        public override MapNodeBase SecondEndpoint {
            get { return _secondEndpoint; }
        }
        public void SetSecondEndpoint(MapNodeBase value) {
            _secondEndpoint = value;
        }
        private MapNodeBase _secondEndpoint;

        #endregion

        #endregion

    }

}
