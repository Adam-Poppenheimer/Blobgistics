using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockHighwayFactory : BlobHighwayFactoryBase {

        #region instance fields and properties

        #region from BlobHighwayFactoryBase

        public override ReadOnlyCollection<BlobHighwayBase> Highways {
            get { return highways.AsReadOnly(); }
        }
        private List<BlobHighwayBase> highways = new List<BlobHighwayBase>();

        #endregion

        #endregion

        #region events

        public event EventHandler<FloatEventArgs> FactoryTicked;

        #endregion

        #region instance methods

        #region from BlobHighwayFactoryBase

        public override bool CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return GetHighwayBetween(firstEndpoint, secondEndpoint) == null;
        }

        public override BlobHighwayBase ConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var newHighway = (new GameObject()).AddComponent<MockBlobHighway>();
            newHighway.firstEndpoint = firstEndpoint;
            newHighway.secondEndpoint = secondEndpoint;

            highways.Add(newHighway);
            return newHighway;
        }

        public override BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return highways.Where(delegate(BlobHighwayBase highway) {
                return (
                    (highway.FirstEndpoint == firstEndpoint && highway.SecondEndpoint == secondEndpoint) ||
                    (highway.SecondEndpoint == firstEndpoint && highway.FirstEndpoint == secondEndpoint)
                );
            }).FirstOrDefault();
        }

        public override BlobHighwayBase GetHighwayOfID(int highwayID) {
            return highways.Where(highway => highway.ID == highwayID).FirstOrDefault();
        }

        public override bool HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            return GetHighwayBetween(firstEndpoint, secondEndpoint) != null;
        }

        public override void DestroyHighway(BlobHighwayBase highway) {
            DestroyImmediate(highway.gameObject);
        }

        #endregion

        #endregion

    }

}
