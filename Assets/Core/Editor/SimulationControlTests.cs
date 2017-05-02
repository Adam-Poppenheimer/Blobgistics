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

        private MapGraph BuildMapGraph(ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();

            var newMapGraph = hostingObject.AddComponent<MapGraph>();
            var newBlobSiteFactory = hostingObject.AddComponent<BlobSiteFactory>();
            var newBlobSitePrivateData = hostingObject.AddComponent<BlobSitePrivateData>();

            newBlobSitePrivateData.SetBlobRealignmentSpeedPerSecond(1f);
            newBlobSitePrivateData.SetAlignmentStrategy(hostingObject.AddComponent<BoxyBlobAlignmentStrategy>());
            newBlobSitePrivateData.SetBlobFactory(blobFactory);

            newBlobSiteFactory.BlobSitePrivateData = newBlobSitePrivateData;

            newMapGraph.BlobSiteFactory = newBlobSiteFactory;

            return newMapGraph;
        }

        private BlobHighwayFactory BuildHighwayFactory(MapGraphBase mapGraph, ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobHighwayFactory>();
            var newBlobTubeFactory = hostingObject.AddComponent<BlobTubeFactory>();

            var newTubePrivateData = hostingObject.AddComponent<BlobTubePrivateData>();
            newTubePrivateData.SetBlobFactory(blobFactory);
            newBlobTubeFactory.TubePrivateData = newTubePrivateData;

            newFactory.MapGraph = mapGraph;
            newFactory.BlobTubeFactory = newBlobTubeFactory;
            newFactory.BlobFactory = blobFactory;
            newFactory.StartingProfile = BuildBlobHighwayProfile(1f, 10, 1f);

            return newFactory;
        }

        private ConstructionZoneFactory BuildConstructionZoneFactory(ResourceDepotFactoryBase depotFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<ConstructionZoneFactory>();

            var resourceDepotProject = hostingObject.AddComponent<ResourceDepotConstructionProject>();
            resourceDepotProject.Cost = ResourceSummary.BuildResourceSummary(hostingObject);
            resourceDepotProject.name = "Resource Depot";

            newFactory.AvailableProjects = new List<ConstructionProjectBase>() {
                resourceDepotProject
            };
            return newFactory;
        }

        private SocietyFactory BuildSocietyFactory(ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<SocietyFactory>();
            
            newFactory.BlobFactory = blobFactory;
            newFactory.SetStandardComplexityLadder(BuildComplexityLadder());

            return newFactory;
        }

        private ResourceDepotFactory BuildDepotFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ResourceDepotFactory>();
        }

        private SimulationControl BuildSimulationControl() {
            var hostingObject = new GameObject();
            var newControl = hostingObject.AddComponent<SimulationControl>();
            var newBlobFactory = BuildResourceBlobFactory();
            var newMapGraph = BuildMapGraph(newBlobFactory);

            newControl.SocietyFactory = BuildSocietyFactory(newBlobFactory);
            newControl.HighwayManagerFactory = BuildHighwayManagerFactory(newMapGraph, BuildHighwayFactory(newMapGraph, newBlobFactory));

            return newControl;
        }

        

        private ResourceBlobFactoryBase BuildResourceBlobFactory() {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<ResourceBlobFactory>();
            return newFactory;
        }

        private ComplexityLadderBase BuildComplexityLadder() {
            var hostingObject = new GameObject();

            var newLadder = hostingObject.AddComponent<ComplexityLadder>();
            var complexity1 = hostingObject.AddComponent<ComplexityDefinition>();
            var complexity2 = hostingObject.AddComponent<ComplexityDefinition>();

            complexity1.SetName("Complexity1");
            complexity1.SetProduction(ResourceSummary.BuildResourceSummary(
                complexity1.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 2)
            ));

            complexity1.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(complexity1.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)),
                ResourceSummary.BuildResourceSummary(complexity1.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.White, 1))
            });
            complexity1.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                complexity1.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            complexity2.SetName("Complexity2");
            complexity2.SetProduction(ResourceSummary.BuildResourceSummary(
                complexity2.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 2)
            ));
            complexity2.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(complexity2.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.White, 1))
            });
            complexity2.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                complexity2.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            newLadder.ComplexityHierarchy = new List<ComplexityDefinitionBase>() {
                complexity1, 
                complexity2
            };

            return newLadder;
        }

        private MockSocietyFactory BuildMockSocietyFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockSocietyFactory>();
        }

        private MockHighwayFactory BuildMockHighwayFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayFactory>();
        }

        private ResourceBlobBase BuildResourceBlob(ResourceType type) {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ResourceBlob>();
        }

        private MockBlobDistributor BuildMockBlobDistributor() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobDistributor>();
        }

        private MockResourceBlobFactory BuildMockBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        private BlobHighwayProfile BuildBlobHighwayProfile(float blobSpeedPerSecond, int capacity, float BlobPullCooldownInSeconds) {
            var hostingObject = new GameObject();
            var newProfile = hostingObject.AddComponent<BlobHighwayProfile>();

            newProfile.SetBlobSpeedPerSecond(blobSpeedPerSecond);
            newProfile.SetCapacity(capacity);
            newProfile.SetBlobPullCooldownInSeconds(BlobPullCooldownInSeconds);

            return newProfile;
        }

        private HighwayManagerFactoryBase BuildHighwayManagerFactory(MapGraphBase mapGraph, BlobHighwayFactoryBase highwayFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<HighwayManagerFactory>();
            var newPrivateData = hostingObject.AddComponent<HighwayManagerPrivateData>();

            newPrivateData.SetNeedStockpileCoefficient(1);
            newPrivateData.SetSecondsToPerformConsumption(10f);

            newFactory.ManagementRadius = 2;
            newFactory.ManagerPrivateData = newPrivateData;
            newFactory.MapGraph = mapGraph;
            newFactory.HighwayFactory = highwayFactory;

            return newFactory;
        }

        #endregion

        #endregion

    }

}


