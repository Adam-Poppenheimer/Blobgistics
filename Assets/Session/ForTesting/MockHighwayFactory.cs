using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;
using System.Collections.ObjectModel;

namespace Assets.Session.ForTesting {

    public class MockHighwayFactory : BlobHighwayFactoryBase {

        #region instance fields and properties

        #region from BlobHighwayFactoryBase

        public override ReadOnlyCollection<BlobHighwayBase> Highways {
            get { return highways.AsReadOnly(); }
        }
        private List<BlobHighwayBase> highways = new List<BlobHighwayBase>();

        #endregion

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            throw new NotImplementedException();
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var newHighway = (new GameObject()).AddComponent<MockBlobHighway>();
            newHighway.firstEndpoint = firstEndpoint;
            newHighway.secondEndpoint = secondEndpoint;
            highways.Add(newHighway);
            return newHighway;
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            highways.Remove(highway);
            DestroyImmediate(highway.gameObject);
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

        #endregion

        #endregion

    }
}
