using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Profiling;

using Assets.Blobs;
using Assets.Societies;
using Assets.BlobDistributors;
using Assets.HighwayManager;
using Assets.Scoring;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation of SimulationControlBase. This class exists to create a layer-like
    /// boundary between the UI and the simulation code and thus decrease coupling. It also exists to enable pause and
    /// resume functionality from a single location.
    /// </summary>
    public class SimulationControl : SimulationControlBase {

        #region instance fields and properties

        /// <summary>
        /// The society factory to tick.
        /// </summary>
        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        /// <summary>
        /// The blob factory to tick.
        /// </summary>
        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        /// <summary>
        /// The highway manager factory to tick.
        /// </summary>
        public HighwayManagerFactoryBase HighwayManagerFactory {
            get { return _highwayManagerFactory; }
            set { _highwayManagerFactory = value; }
        }
        [SerializeField] private HighwayManagerFactoryBase _highwayManagerFactory;

        /// <summary>
        /// The blob distributor to tick.
        /// </summary>
        public BlobDistributorBase BlobDistributor{
            get { return _blobDistributor; }
            set { _blobDistributor = value; }
        }
        [SerializeField] private BlobDistributorBase _blobDistributor;

        /// <summary>
        /// The victory manager to tick.
        /// </summary>
        public VictoryManagerBase VictoryManager {
            get { return _victoryManager; }
            set { _victoryManager = value; }
        }
        [SerializeField] private VictoryManagerBase _victoryManager;

        private bool IsPaused = false;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            TickSimulation(Time.deltaTime);
        }

        #endregion

        #region from SimulationControlBase

        /// <inheritdoc/>
        /// <remarks>
        /// Here, TickSimulation allows us to specify the order of execution of the various
        /// Tick methods found throughout the codebase.
        /// </remarks>
        public override void TickSimulation(float secondsPassed) {
            if(!IsPaused) {
                if(SocietyFactory        != null) SocietyFactory.TickSocieties          (secondsPassed);

                if(BlobDistributor       != null) BlobDistributor.Tick                  (secondsPassed);

                if(BlobFactory           != null) BlobFactory.TickAllBlobs              (secondsPassed);
                if(HighwayManagerFactory != null) HighwayManagerFactory.TickAllManangers(secondsPassed);
            }
        }

        /// <inheritdoc/>
        public override void Pause() {
            IsPaused = true;
            VictoryManager.Pause();
        }

        /// <inheritdoc/>
        public override void Resume() {
            IsPaused = false;
            VictoryManager.Unpause();
        }

        /// <inheritdoc/>
        public override void PerformVictoryTasks() {
            Pause();
        }

        /// <inheritdoc/>
        public override void PerformDefeatTasks() {
            Pause();
        }

        #endregion

        #endregion

    }

}
