using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Map;
using Assets.Highways;
using Assets.HighwayManager.ForTesting;

namespace Assets.HighwayManager.Editor {

    public class HighwayManagerTests {

        #region instance methods

        #region tests

        #region for HighwayManagerFactory

        [Test]
        public void Factory_OnHighwayManagerConstructed_ManagerHasAUniqueID() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();
            var location4 = BuildMockMapNode();

            //Execution
            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);
            var manager3 = factoryToTest.ConstructHighwayManagerAtLocation(location3);
            var manager4 = factoryToTest.ConstructHighwayManagerAtLocation(location4);

            //Validation
            Assert.AreNotEqual(manager1.ID, manager2.ID, "Managers 1 and 2 share the same ID");
            Assert.AreNotEqual(manager1.ID, manager3.ID, "Managers 1 and 3 share the same ID");
            Assert.AreNotEqual(manager1.ID, manager4.ID, "Managers 1 and 4 share the same ID");

            Assert.AreNotEqual(manager2.ID, manager3.ID, "Managers 2 and 3 share the same ID");
            Assert.AreNotEqual(manager2.ID, manager4.ID, "Managers 2 and 4 share the same ID");

            Assert.AreNotEqual(manager3.ID, manager4.ID, "Managers 3 and 4 share the same ID");
        }

        [Test]
        public void Factory_OnHighwayManagerConstructed_GetManagerOfIDReturnsManagerCorrectly() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();
            var location4 = BuildMockMapNode();

            //Execution
            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);
            var manager3 = factoryToTest.ConstructHighwayManagerAtLocation(location3);
            var manager4 = factoryToTest.ConstructHighwayManagerAtLocation(location4);

            //Validation
            Assert.AreEqual(manager1, factoryToTest.GetHighwayManagerOfID(manager1.ID), "Manager1.ID did not return manager1");
            Assert.AreEqual(manager2, factoryToTest.GetHighwayManagerOfID(manager2.ID), "Manager2.ID did not return manager1");
            Assert.AreEqual(manager3, factoryToTest.GetHighwayManagerOfID(manager3.ID), "Manager3.ID did not return manager1");
            Assert.AreEqual(manager4, factoryToTest.GetHighwayManagerOfID(manager4.ID), "Manager4.ID did not return manager1");
        }

        [Test]
        public void Factory_OnHighwayManagerConstructed_ManagerHasTheProperLocation() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();
            var location4 = BuildMockMapNode();

            //Execution
            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);
            var manager3 = factoryToTest.ConstructHighwayManagerAtLocation(location3);
            var manager4 = factoryToTest.ConstructHighwayManagerAtLocation(location4);

            //Validation
            Assert.AreEqual(location1, manager1.Location, "Manager1 has the incorrect location");
            Assert.AreEqual(location2, manager2.Location, "Manager2 has the incorrect location");
            Assert.AreEqual(location3, manager3.Location, "Manager3 has the incorrect location");
            Assert.AreEqual(location4, manager4.Location, "Manager4 has the incorrect location");
        }

        [Test]
        public void Factory_OnHighwayManagerConstructed_GetHighwayManagerAtLocationReturnsTheCorrectManager_OrNullIfNoneExists() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();
            var location4 = BuildMockMapNode();
            var location5 = BuildMockMapNode();

            //Execution
            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);
            var manager3 = factoryToTest.ConstructHighwayManagerAtLocation(location3);
            var manager4 = factoryToTest.ConstructHighwayManagerAtLocation(location4);

            //Validation
            Assert.AreEqual(manager1, factoryToTest.GetHighwayManagerAtLocation(location1), "GetHighwayManagerAtLocation for location1 returned the wrong manager");
            Assert.AreEqual(manager2, factoryToTest.GetHighwayManagerAtLocation(location2), "GetHighwayManagerAtLocation for location2 returned the wrong manager");
            Assert.AreEqual(manager3, factoryToTest.GetHighwayManagerAtLocation(location3), "GetHighwayManagerAtLocation for location3 returned the wrong manager");
            Assert.AreEqual(manager4, factoryToTest.GetHighwayManagerAtLocation(location4), "GetHighwayManagerAtLocation for location4 returned the wrong manager");
            Assert.IsNull(factoryToTest.GetHighwayManagerAtLocation(location5), "GetHighwayManagerAtLocation for location5 does not return null");
        }

        [Test]
        public void Factory_OnDestroyHighwayManagerCalled_HighwayManagerIsDestroyed_AndGetHighwayManagerAtLocationBecomesNull() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();

            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);
            var manager3 = factoryToTest.ConstructHighwayManagerAtLocation(location3);

            //Execution
            factoryToTest.DestroyHighwayManager(manager1);
            factoryToTest.DestroyHighwayManager(manager2);

            //Validation
            Assert.That(manager1.Equals(null), "Manager1 has not been destroyed");
            Assert.That(manager2.Equals(null), "Manager2 has not been destroyed");
            Assert.IsFalse(manager3.Equals(null), "Manager3 has incorrectly been destroyed");

            Assert.IsNull(factoryToTest.GetHighwayManagerAtLocation(location1), "Factory still registers a highway manager at location1");
            Assert.IsNull(factoryToTest.GetHighwayManagerAtLocation(location2), "Factory still registers a highway manager at location2");
        }

        [Test]
        public void Factory_OnUnsubscribeHighwayManagerCalled_GetHighwayManagerAtLocationReturnsNull() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();

            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);
            var manager3 = factoryToTest.ConstructHighwayManagerAtLocation(location3);

            //Execution
            factoryToTest.UnsubscribeHighwayManager(manager1);
            factoryToTest.UnsubscribeHighwayManager(manager2);
            factoryToTest.UnsubscribeHighwayManager(manager3);

            //Validation
            Assert.IsNull(factoryToTest.GetHighwayManagerAtLocation(location1), "Factory still registers a highway manager at location1");
            Assert.IsNull(factoryToTest.GetHighwayManagerAtLocation(location2), "Factory still registers a highway manager at location2");
            Assert.IsNull(factoryToTest.GetHighwayManagerAtLocation(location3), "Factory still registers a highway manager at location3");
        }

        [Test]
        public void Factory_OnGetManagerServingHighwayCalled_ReturnsTheClosestManagerToTheHighway_OrNullIfNoneIsInRange() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);
            
            node1.name = "Node1";
            node2.name = "Node2";
            node3.name = "Node3";
            node4.name = "Node4";
            node5.name = "Node5";

            mapGraph.AddUndirectedEdge(node1, node2);
            mapGraph.AddUndirectedEdge(node2, node3);
            mapGraph.AddUndirectedEdge(node3, node4);
            mapGraph.AddUndirectedEdge(node4, node5);

            var highwayBetween1And2 = BuildMockBlobHighway(node1, node2);
            var highwayBetween2And3 = BuildMockBlobHighway(node2, node3);
            var highwayBetween3And4 = BuildMockBlobHighway(node3, node4);
            var highwayBetween4And5 = BuildMockBlobHighway(node4, node5);

            var managerFactory = BuildHighwayManagerFactory();
            managerFactory.ManagementRadius = 2;
            managerFactory.MapGraph = mapGraph;

            var managerAtNode1 = managerFactory.ConstructHighwayManagerAtLocation(node1);
            var managerAtNode5 = managerFactory.ConstructHighwayManagerAtLocation(node5);

            managerAtNode1.name = "Manager at Node1";
            managerAtNode5.name = "Manager at Node5";

            //Execution and validation
            
            Assert.AreEqual(managerAtNode1, managerFactory.GetManagerServingHighway(highwayBetween1And2),
                "Failed to return the closest manager for the highway between node1 and node2");
            Assert.AreEqual(managerAtNode1, managerFactory.GetManagerServingHighway(highwayBetween2And3),
                "Failed to return the closest manager for the highway between node2 and node3");
            Assert.AreEqual(managerAtNode5, managerFactory.GetManagerServingHighway(highwayBetween3And4),
                "Failed to return the closest manager for the highway between node3 and node4");
            Assert.AreEqual(managerAtNode5, managerFactory.GetManagerServingHighway(highwayBetween4And5),
                "Failed to return the closest manager for the highway between node4 and node5");

            managerFactory.DestroyHighwayManager(managerAtNode5);

            Assert.IsNull(managerFactory.GetManagerServingHighway(highwayBetween4And5),
                "Falsely returned a manager too far away from the highway between node4 and node5");
        }

        [Test]
        public void Factory_SubscribeHighwayIsCalled_GetHighwaysServedByManagerReflectsNewlySubscribedHighway() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);
            
            node1.name = "Node1";
            node2.name = "Node2";
            node3.name = "Node3";
            node4.name = "Node4";
            node5.name = "Node5";

            mapGraph.AddUndirectedEdge(node1, node2);
            mapGraph.AddUndirectedEdge(node2, node3);
            mapGraph.AddUndirectedEdge(node3, node4);
            mapGraph.AddUndirectedEdge(node4, node5);

            var highwayBetween1And2 = BuildMockBlobHighway(node1, node2);
            var highwayBetween2And3 = BuildMockBlobHighway(node2, node3);
            var highwayBetween3And4 = BuildMockBlobHighway(node3, node4);
            var highwayBetween4And5 = BuildMockBlobHighway(node4, node5);

            var managerFactory = BuildHighwayManagerFactory();
            managerFactory.ManagementRadius = 2;
            managerFactory.MapGraph = mapGraph;

            var managerAtNodeOne = managerFactory.ConstructHighwayManagerAtLocation(node1);
            var managerAtNodeTwo = managerFactory.ConstructHighwayManagerAtLocation(node5);

            //Execution
            managerFactory.SubscribeHighway(highwayBetween1And2);
            managerFactory.SubscribeHighway(highwayBetween2And3);
            managerFactory.SubscribeHighway(highwayBetween3And4);
            managerFactory.SubscribeHighway(highwayBetween4And5);

            var highwaysForManagerOne = managerFactory.GetHighwaysServedByManager(managerAtNodeOne);
            var highwaysForManagerTwo = managerFactory.GetHighwaysServedByManager(managerAtNodeTwo);

            //Validation
            Assert.AreEqual(2, highwaysForManagerOne.Count(), "ManagerOne is managing the wrong number of highways");
            Assert.That(highwaysForManagerOne.Contains(highwayBetween1And2), "ManagerOne is not managing highway between node1 and node2");
            Assert.That(highwaysForManagerOne.Contains(highwayBetween2And3), "ManagerOne is not managing highway between node2 and node3");

            Assert.AreEqual(2, highwaysForManagerTwo.Count(), "ManagerTwo is managing the wrong number of highways");
            Assert.That(highwaysForManagerTwo.Contains(highwayBetween3And4), "ManagerTwo is not managing highway between node3 and node4");
            Assert.That(highwaysForManagerTwo.Contains(highwayBetween4And5), "ManagerTwo is not managing highway between node4 and node5");
        }

        [Test]
        public void Factory_UnsubscribeHighwayIsCalled_GetHighwaysServedByManagerReflectsNewlyUnsubscribedHighway() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);
            
            node1.name = "Node1";
            node2.name = "Node2";
            node3.name = "Node3";
            node4.name = "Node4";
            node5.name = "Node5";

            mapGraph.AddUndirectedEdge(node1, node2);
            mapGraph.AddUndirectedEdge(node2, node3);
            mapGraph.AddUndirectedEdge(node3, node4);
            mapGraph.AddUndirectedEdge(node4, node5);

            var highwayBetween1And2 = BuildMockBlobHighway(node1, node2);
            var highwayBetween2And3 = BuildMockBlobHighway(node2, node3);
            var highwayBetween3And4 = BuildMockBlobHighway(node3, node4);
            var highwayBetween4And5 = BuildMockBlobHighway(node4, node5);

            var managerFactory = BuildHighwayManagerFactory();
            managerFactory.ManagementRadius = 2;
            managerFactory.MapGraph = mapGraph;

            var managerAtNodeOne = managerFactory.ConstructHighwayManagerAtLocation(node1);
            var managerAtNodeTwo = managerFactory.ConstructHighwayManagerAtLocation(node5);

            //Execution
            managerFactory.SubscribeHighway(highwayBetween1And2);
            managerFactory.SubscribeHighway(highwayBetween2And3);
            managerFactory.SubscribeHighway(highwayBetween3And4);
            managerFactory.SubscribeHighway(highwayBetween4And5);

            managerFactory.UnsubscribeHighway(highwayBetween1And2);
            managerFactory.UnsubscribeHighway(highwayBetween2And3);
            managerFactory.UnsubscribeHighway(highwayBetween3And4);

            var highwaysForManagerOne = managerFactory.GetHighwaysServedByManager(managerAtNodeOne);
            var highwaysForManagerTwo = managerFactory.GetHighwaysServedByManager(managerAtNodeTwo);

            //Validation
            Assert.AreEqual(0, highwaysForManagerOne.Count(), "ManagerOne is managing the wrong number of highways");

            Assert.AreEqual(1, highwaysForManagerTwo.Count(), "ManagerTwo is managing the wrong number of highways");
            Assert.That(highwaysForManagerTwo.Contains(highwayBetween4And5), "ManagerTwo is not managing highway between node4 and node5");
        }

        [Test]
        public void Factory_WhenANewHighwayIsConstructed_ItIsAutomaticallySubscribed() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);
            
            node1.name = "Node1";
            node2.name = "Node2";
            node3.name = "Node3";
            node4.name = "Node4";
            node5.name = "Node5";

            mapGraph.AddUndirectedEdge(node1, node2);
            mapGraph.AddUndirectedEdge(node2, node3);
            mapGraph.AddUndirectedEdge(node3, node4);
            mapGraph.AddUndirectedEdge(node4, node5);

            var highwayFactory = BuildMockHighwayFactory();

            var managerFactory = BuildHighwayManagerFactory();
            managerFactory.ManagementRadius = 2;
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;

            var managerAtNodeOne = managerFactory.ConstructHighwayManagerAtLocation(node1);
            var managerAtNodeTwo = managerFactory.ConstructHighwayManagerAtLocation(node5);

            //Execution
            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween2And3 = highwayFactory.ConstructHighwayBetween(node2, node3);
            var highwayBetween3And4 = highwayFactory.ConstructHighwayBetween(node3, node4);
            var highwayBetween4And5 = highwayFactory.ConstructHighwayBetween(node4, node5);

            var highwaysForManagerOne = managerFactory.GetHighwaysServedByManager(managerAtNodeOne);
            var highwaysForManagerTwo = managerFactory.GetHighwaysServedByManager(managerAtNodeTwo);

            //Validation
            Assert.AreEqual(2, highwaysForManagerOne.Count(), "ManagerOne is managing the wrong number of highways");
            Assert.That(highwaysForManagerOne.Contains(highwayBetween1And2), "ManagerOne is not managing highway between node1 and node2");
            Assert.That(highwaysForManagerOne.Contains(highwayBetween2And3), "ManagerOne is not managing highway between node2 and node3");

            Assert.AreEqual(2, highwaysForManagerTwo.Count(), "ManagerTwo is managing the wrong number of highways");
            Assert.That(highwaysForManagerTwo.Contains(highwayBetween3And4), "ManagerTwo is not managing highway between node3 and node4");
            Assert.That(highwaysForManagerTwo.Contains(highwayBetween4And5), "ManagerTwo is not managing highway between node4 and node5");
        }

        [Test]
        public void Factory_CanConstructHighwayManagerAtLocation_ReturnsFalseIfGetHighwayManagerAtLocationReturnsANonNullValue_AndTrueOtherwise() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();
            var location2 = BuildMockMapNode();
            var location3 = BuildMockMapNode();

            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);
            var manager2 = factoryToTest.ConstructHighwayManagerAtLocation(location2);

            //Execution and validation
            Assert.IsFalse(factoryToTest.CanConstructHighwayManagerAtLocation(location1), "Falsely permits a HighwayManager at location1");
            Assert.IsFalse(factoryToTest.CanConstructHighwayManagerAtLocation(location2), "Falsely permits a HighwayManager at location2");
            Assert.IsTrue(factoryToTest.CanConstructHighwayManagerAtLocation(location3), "Fails to permit a HighwayManager at location3");
        }

        [Test]
        public void Factory_CanConstructManagerAtLocationReturnsFalse_AndConstructManagerCalled_ThrowsHighwayManagerException() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();
            var location1 = BuildMockMapNode();

            var manager1 = factoryToTest.ConstructHighwayManagerAtLocation(location1);

            //Execution and Validation
            Assert.Throws<HighwayManagerException>(delegate() {
                factoryToTest.ConstructHighwayManagerAtLocation(location1);
            });
        }

        [Test]
        public void Factory_AnyNullArgumentPassedIntoAnyMethod_ThrowsAppropriateArgumentNullException() {
            //Setup
            var factoryToTest = BuildHighwayManagerFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetHighwayManagerAtLocation(null);
            }, "No exception on GetHighwayManagerAtLocation()");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetManagerServingHighway(null);
            }, "No exception on GetManagerServingHighway()");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetHighwaysServedByManager(null);
            }, "No exception on GetHighwaysServedByManager()");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructHighwayManagerAtLocation(null);
            }, "No exception on CanConstructHighwayManagerAtLocation()");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructHighwayManagerAtLocation(null);
            }, "No exception on ConstructHighwayManagerAtLocation()");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.DestroyHighwayManager(null);
            }, "No exception on DestroyHighwayManager()");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.UnsubscribeHighwayManager(null);
            }, "No exception on UnsubscribeHighwayManager()");
        }

        #endregion

        #region for HighwayManager

        [Test]
        public void OnTickConsumptionIsCalled_ConsumptionPerformedAtSpecifiedRate_WithHighwaysGottenFromParentFactory_AndLastCalculatedUpkeepIsSetProperly() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);

            node1.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            node1.BlobSite.SetCapacityForResourceType(ResourceType.Food, 100);
            node1.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            node1.BlobSite.SetCapacityForResourceType(ResourceType.Yellow, 100);
            node1.BlobSite.TotalCapacity = 100;
            for(int i = 0; i < 3; ++i) {
                node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Food));
            }
            for(int i = 0; i < 4; ++i) {
                node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Yellow));
            }

            mapGraph.AddUndirectedEdge(node1, node2);
            mapGraph.AddUndirectedEdge(node2, node3);
            mapGraph.AddUndirectedEdge(node3, node4);

            var highwayFactory = BuildMockHighwayFactory();

            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween2And3 = highwayFactory.ConstructHighwayBetween(node2, node3);
            var highwayBetween3And4 = highwayFactory.ConstructHighwayBetween(node3, node4);


            var testProfile = BuildHighwayProfile(ResourceSummary.BuildResourceSummary(
                highwayBetween1And2.gameObject, 
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)
            ));

            highwayBetween1And2.Profile = testProfile;
            highwayBetween2And3.Profile = testProfile;
            highwayBetween3And4.Profile = testProfile;

            var managerFactory = BuildHighwayManagerFactory();
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;
            managerFactory.ManagementRadius = 2;
            managerFactory.SecondsForManagerToPerformConsumption = 1f;
            managerFactory.NeedStockpileCoefficient = 5;
            managerFactory.BlobFactory = BuildMockBlobFactory();

            managerFactory.SubscribeHighway(highwayBetween1And2);
            managerFactory.SubscribeHighway(highwayBetween2And3);
            managerFactory.SubscribeHighway(highwayBetween3And4);

            var managerToTest = managerFactory.ConstructHighwayManagerAtLocation(node1);

            //Execution
            managerToTest.TickConsumption(0.5f);
            managerToTest.TickConsumption(0.5f);
            managerToTest.TickConsumption(0.5f);

            //Validation
            Assert.AreEqual(0, node1.BlobSite.GetCountOfContentsOfType(ResourceType.Food),   "BlobSite was left with an incorrect amount of food");
            Assert.AreEqual(1, node1.BlobSite.GetCountOfContentsOfType(ResourceType.Yellow), "BlobSite was left with an incorrect amount of yellow");

            Assert.AreEqual(3, managerToTest.LastCalculatedUpkeep[ResourceType.Food],   "LastCalculatedUpkeep has the wrong food value");
            Assert.AreEqual(3, managerToTest.LastCalculatedUpkeep[ResourceType.Yellow], "LastCalculatedUpkeep has the wrong yellow value");
        }

        [Test]
        public void OnTickConsumptionIsCalled_AndConsumptionPerformed_EfficienciesOfAllParticipantsAreSetToThePercentageOfNeedsConsumed() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);

            node1.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            node1.BlobSite.SetCapacityForResourceType(ResourceType.Food, 100);
            node1.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            node1.BlobSite.SetCapacityForResourceType(ResourceType.Yellow, 100);
            node1.BlobSite.TotalCapacity = 100;
            node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Food));
            node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Yellow));

            mapGraph.AddUndirectedEdge(node1, node2);
            mapGraph.AddUndirectedEdge(node2, node3);

            var highwayFactory = BuildMockHighwayFactory();

            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween2And3 = highwayFactory.ConstructHighwayBetween(node2, node3);

            var testProfile = BuildHighwayProfile(ResourceSummary.BuildResourceSummary(
                highwayBetween1And2.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)
            ));

            highwayBetween1And2.Profile = testProfile;
            highwayBetween2And3.Profile = testProfile;

            var managerFactory = BuildHighwayManagerFactory();
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;
            managerFactory.ManagementRadius = 2;
            managerFactory.SecondsForManagerToPerformConsumption = 1f;
            managerFactory.NeedStockpileCoefficient = 5;
            managerFactory.BlobFactory = BuildMockBlobFactory();

            managerFactory.SubscribeHighway(highwayBetween1And2);
            managerFactory.SubscribeHighway(highwayBetween2And3);

            var managerToTest = managerFactory.ConstructHighwayManagerAtLocation(node1);

            //Execution
            managerToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(0.5f, managerToTest.LastCalculatedEfficiency, "The HighwayManager has the wrong LastCalculatedEfficiency");
            Assert.AreEqual(0.5f, highwayBetween1And2.Efficiency, "The highway between node1 and node2 has the wrong efficiency");
            Assert.AreEqual(0.5f, highwayBetween2And3.Efficiency, "The highway between node2 and node3 has the wrong efficiency");
        }

        #endregion

        #endregion

        #region utilities

        private HighwayManagerFactory BuildHighwayManagerFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayManagerFactory>();
        }

        private HighwayManager BuildHighwayManager() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayManager>();
        }

        private MockMapNode BuildMockMapNode() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockMapNode>();
        }

        private MockBlobHighway BuildMockBlobHighway(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint) {
            var hostingObject = new GameObject();
            var newHighway = hostingObject.AddComponent<MockBlobHighway>();

            newHighway.SetFirstEndpoint(firstEndpoint);
            newHighway.SetSecondEndpoint(secondEndpoint);

            return newHighway;
        }

        private MockMapGraph BuildMockMapGraph() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockMapGraph>();
        }

        private MockResourceBlob BuildMockResourceBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<MockResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        private BlobHighwayProfile BuildHighwayProfile(ResourceSummary upkeep) {
            var hostingObject = new GameObject();
            var newProfile = hostingObject.AddComponent<BlobHighwayProfile>();
            newProfile.SetUpkeep(upkeep);
            return newProfile;
        }

        private MockBlobHighwayFactory BuildMockHighwayFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobHighwayFactory>();
        }

        private MockResourceBlobFactory BuildMockBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        #endregion

        #endregion

    }

}


