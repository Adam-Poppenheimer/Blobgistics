using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;

namespace Assets.BlobDistributors.ForTesting {

    public class MockHighwayFactory : BlobHighwayFactoryBase {

        #region instance fields and properties

        private List<BlobHighwayBase> Highways = new List<BlobHighwayBase>();

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return true;
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var hostingObject = new GameObject();
            var newHighway = hostingObject.AddComponent<MockBlobHighway>();
            newHighway.SetFirstEndpoint(firstEndpoint);
            newHighway.SetSecondEndpoint(secondEndpoint);

            Highways.Add(newHighway);
            return newHighway;
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            Highways.Remove(highway);
            DestroyImmediate(highway.gameObject);
        }

        public override BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return Highways.Find(delegate(BlobHighwayBase highway) {
                return (highway.FirstEndpoint == firstEndpoint  && highway.SecondEndpoint == secondEndpoint) ||
                       (highway.FirstEndpoint == secondEndpoint && highway.SecondEndpoint == firstEndpoint );
            });
        }

        public override BlobHighwayBase GetHighwayOfID(int highwayID) {
            throw new NotImplementedException();
        }

        public override bool HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return GetHighwayBetween(firstEndpoint, secondEndpoint) != null;
        }

        public override void TickHighways(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
