using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.BlobSites;
using Assets.ConstructionZones;
using Assets.Highways;
using Assets.ResourceDepots;
using Assets.Societies;
using Assets.HighwayManager;

using Assets.Core.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.Editor {

    public class SimulationControlTests {

        #region instance methods

        #region tests

        [Test]
        public void OnTickSimulationIsCalled_AllSimulationTickingIsPerformed() {
            //Setup
            var societyFactory = BuildMockSocietyFactory();
            var blobDistributor = BuildMockBlobDistributor();
            var blobFactory = BuildMockBlobFactory();

            float amountTickedOnSocietyFactory = 0f;
            societyFactory.FactoryTicked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnSocietyFactory = e.Value;
            };

            float amountTickedOnBlobDistributor = 0f;
            blobDistributor.Ticked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnBlobDistributor = e.Value;
            };

            float amountTickedOnBlobFactory = 0f;
            blobFactory.Ticked += delegate(object sender, FloatEventArgs e) {
                amountTickedOnBlobFactory = e.Value;
            };

            var controlToTest = BuildSimulationControl();
            controlToTest.SocietyFactory = societyFactory;
            controlToTest.BlobDistributor = blobDistributor;
            controlToTest.BlobFactory = blobFactory;

            //Execution
            controlToTest.TickSimulation(5f);

            //Validation
            Assert.AreEqual(5f, amountTickedOnSocietyFactory,  "Incorrect amount ticked on SocietyFactory");
            Assert.AreEqual(5f, amountTickedOnBlobDistributor, "Incorrect amount ticked on BlobDistributor");
            Assert.AreEqual(5f, amountTickedOnBlobFactory,     "Incorrect amount ticked on BlobFactory");
        }

        #endregion

        #region utilities

        private SimulationControl BuildSimulationControl() {
            return (new GameObject()).AddComponent<SimulationControl>();
        }

        private MockSocietyFactory BuildMockSocietyFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockSocietyFactory>();
        }

        private MockBlobDistributor BuildMockBlobDistributor() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobDistributor>();
        }

        private MockResourceBlobFactory BuildMockBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        #endregion

        #endregion

    }

}


