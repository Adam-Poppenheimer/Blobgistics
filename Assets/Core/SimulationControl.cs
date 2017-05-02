using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;
using Assets.Societies;
using Assets.ResourceDepots;
using Assets.ConstructionZones;
using Assets.BlobDistributors;
using Assets.HighwayManager;
using Assets.Generator;

namespace Assets.Core {

    public class SimulationControl : SimulationControlBase {

        #region instance fields and properties

        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public HighwayManagerFactoryBase HighwayManagerFactory {
            get { return _highwayManagerFactory; }
            set { _highwayManagerFactory = value; }
        }
        [SerializeField] private HighwayManagerFactoryBase _highwayManagerFactory;

        public BlobDistributorBase BlobDistributor{
            get { return _blobDistributor; }
            set { _blobDistributor = value; }
        }
        [SerializeField] private BlobDistributorBase _blobDistributor;

        public ResourceGeneratorFactory GeneratorFactory {
            get { return _generatorFactory; }
            set { _generatorFactory = value; }
        }
        [SerializeField] private ResourceGeneratorFactory _generatorFactory;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            TickSimulation(Time.deltaTime);
        }

        #endregion

        #region from SimulationControlBase

        public override void TickSimulation(float secondsPassed) {
            if(SocietyFactory        != null) SocietyFactory.TickSocieties          (secondsPassed);
            if(BlobDistributor       != null) BlobDistributor.Tick                  (secondsPassed);
            if(BlobFactory           != null) BlobFactory.TickAllBlobs              (secondsPassed);
            if(HighwayManagerFactory != null) HighwayManagerFactory.TickAllManangers(secondsPassed);
            if(GeneratorFactory      != null) GeneratorFactory.TickAllGenerators    (secondsPassed);
        }

        #endregion

        #endregion

    }

}
