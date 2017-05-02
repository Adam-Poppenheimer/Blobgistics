using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.HighwayManager;
using Assets.Highways;
using Assets.Map;

namespace Assets.Core.ForTesting {

    public class MockHighwayManagerFactory : HighwayManagerFactoryBase {

        #region instance methods

        #region from HighwayManagerFactoryBase

        public override bool CanConstructHighwayManagerAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase ConstructHighwayManagerAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override void DestroyHighwayManager(HighwayManagerBase manager) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase GetHighwayManagerOfID(int id) {
            throw new NotImplementedException();
        }

        public override IEnumerable<BlobHighwayBase> GetHighwaysServedByManager(HighwayManagerBase manager) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        public override void SubscribeHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        public override void TickAllManangers(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeHighwayManager(HighwayManagerBase manager) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
