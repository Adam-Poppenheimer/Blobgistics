using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Highways;
using Assets.Map;
using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockHighwayFactory : BlobHighwayFactoryBase {

        #region events

        public event EventHandler<FloatEventArgs> FactoryTicked;

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            throw new NotImplementedException();
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            throw new NotImplementedException();
        }

        public override BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            throw new NotImplementedException();
        }

        public override BlobHighwayBase GetHighwayOfID(int highwayID) {
            throw new NotImplementedException();
        }

        public override bool HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            throw new NotImplementedException();
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
