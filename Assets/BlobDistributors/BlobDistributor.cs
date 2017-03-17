using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;

namespace Assets.BlobDistributors {

    public class BlobDistributor : BlobDistributorBase {

        #region instance fields and properties

        #region from BlobDistributorBase

        public override float SecondsToPerformDistributionTick { get; set; }

        #endregion

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set { _highwayFactory = value; }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        #endregion

        #region instance methods

        #region from BlobDistributorBase

        public override void Tick(float secondsPassed) {
            throw new NotImplementedException();
        }

        protected override void PerformDistribution() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
