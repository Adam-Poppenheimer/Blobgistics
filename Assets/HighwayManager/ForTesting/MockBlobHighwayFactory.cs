using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;

namespace Assets.HighwayManager.ForTesting {

    public class MockBlobHighwayFactory : BlobHighwayFactoryBase {

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
            return true;
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var hostingObject = new GameObject();
            var newHighway = hostingObject.AddComponent<MockBlobHighway>();
            newHighway.SetEndpoints(firstEndpoint, secondEndpoint);
            highways.Add(newHighway);

            RaiseHighwaySubscribed(newHighway);
            return newHighway;
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            highways.Remove(highway);
            RaiseHighwayUnsubscribed(highway);
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

        public override void SubscribeHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
