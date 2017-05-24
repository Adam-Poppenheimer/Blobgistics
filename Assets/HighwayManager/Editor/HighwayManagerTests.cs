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

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayManager.Editor {

    public class HighwayManagerTests {

        #region instance methods

        #region tests

        #region for HighwayManagerFactory

        [Test]
        public void Factory_OnHighwayManagerConstructed_ManagerHasAUniqueID() {
            //Setup
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();
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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();
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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();
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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();
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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();

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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();
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

            mapGraph.BuildMapEdge(node1, node2);
            mapGraph.BuildMapEdge(node2, node3);
            mapGraph.BuildMapEdge(node3, node4);
            mapGraph.BuildMapEdge(node4, node5);

            var highwayFactory = BuildMockHighwayFactory();

            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween2And3 = highwayFactory.ConstructHighwayBetween(node2, node3);
            var highwayBetween3And4 = highwayFactory.ConstructHighwayBetween(node3, node4);
            var highwayBetween4And5 = highwayFactory.ConstructHighwayBetween(node4, node5);

            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var managerFactory = BuildHighwayManagerFactory(privateData);
            managerFactory.HighwayFactory = highwayFactory;
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
        public void Factory_OnHighwayFactoryConstructsNewHighways_GetHighwaysServedByManagerReflectsNewlySubscribedHighway() {
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

            mapGraph.BuildMapEdge(node1, node2);
            mapGraph.BuildMapEdge(node2, node3);
            mapGraph.BuildMapEdge(node3, node4);
            mapGraph.BuildMapEdge(node4, node5);

            var highwayFactory = BuildMockHighwayFactory();

            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var managerFactory = BuildHighwayManagerFactory(privateData);
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
        public void Factory_OnHighwayFactoryDestroysExistingHighways_GetHighwaysServedByManagerReflectsNewlyDestroyedHighway() {
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

            mapGraph.BuildMapEdge(node1, node2);
            mapGraph.BuildMapEdge(node2, node3);
            mapGraph.BuildMapEdge(node3, node4);
            mapGraph.BuildMapEdge(node4, node5);

            var highwayFactory = BuildMockHighwayFactory();

            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween2And3 = highwayFactory.ConstructHighwayBetween(node2, node3);
            var highwayBetween3And4 = highwayFactory.ConstructHighwayBetween(node3, node4);
            var highwayBetween4And5 = highwayFactory.ConstructHighwayBetween(node4, node5);

            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var managerFactory = BuildHighwayManagerFactory(privateData);
            managerFactory.ManagementRadius = 2;
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;

            var managerAtNodeOne = managerFactory.ConstructHighwayManagerAtLocation(node1);
            var managerAtNodeTwo = managerFactory.ConstructHighwayManagerAtLocation(node5);

            //Execution
            highwayFactory.DestroyHighway(highwayBetween1And2);
            highwayFactory.DestroyHighway(highwayBetween2And3);
            highwayFactory.DestroyHighway(highwayBetween3And4);

            var highwaysForManagerOne = managerFactory.GetHighwaysServedByManager(managerAtNodeOne);
            var highwaysForManagerTwo = managerFactory.GetHighwaysServedByManager(managerAtNodeTwo);

            //Validation
            Assert.AreEqual(0, highwaysForManagerOne.Count(), "ManagerOne is managing the wrong number of highways");

            Assert.AreEqual(1, highwaysForManagerTwo.Count(), "ManagerTwo is managing the wrong number of highways");
            Assert.That(highwaysForManagerTwo.Contains(highwayBetween4And5), "ManagerTwo is not managing highway between node4 and node5");
        }

        [Test]
        public void Factory_CanConstructHighwayManagerAtLocation_ReturnsFalseIfGetHighwayManagerAtLocationReturnsANonNullValue_AndTrueOtherwise() {
            //Setup
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();
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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);
            factoryToTest.HighwayFactory = BuildMockHighwayFactory();

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
            var privateData = BuildManagerPrivateData();
            privateData.SetBlobFactory(BuildMockBlobFactory());

            var factoryToTest = BuildHighwayManagerFactory(privateData);

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
        public void OnTickConsumptionIsCalled_AndConsumptionPerformed_HighwaysAreGottenFromFactory_WithUpkeepDeterminedByTheirUpkeepRequests() {
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

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                node1.BlobSite.SetPlacementPermissionForResourceType(resourceType, true);
                node1.BlobSite.SetCapacityForResourceType(resourceType, 10);
                node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(resourceType));
            }
            node1.BlobSite.TotalCapacity = 100;

            mapGraph.BuildMapEdge(node1, node2);
            mapGraph.BuildMapEdge(node2, node3);
            mapGraph.BuildMapEdge(node3, node4);
            mapGraph.BuildMapEdge(node4, node5);

            var highwayFactory = BuildMockHighwayFactory();

            var privateData = BuildManagerPrivateData();
            privateData.SetSecondsToPerformConsumption(1f);
            privateData.SetBlobFactory(BuildMockBlobFactory());
            privateData.SetEfficiencyGainFromResource(IntResourceSummary.BuildSummary(new GameObject()));

            var managerFactory = BuildHighwayManagerFactory(privateData);
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;
            managerFactory.ManagementRadius = 4;

            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween2And3 = highwayFactory.ConstructHighwayBetween(node2, node3);
            var highwayBetween3And4 = highwayFactory.ConstructHighwayBetween(node3, node4);
            var highwayBetween4And5 = highwayFactory.ConstructHighwayBetween(node4, node5);

            highwayBetween1And2.SetUpkeepRequestedForResource(ResourceType.Food,         true);
            highwayBetween2And3.SetUpkeepRequestedForResource(ResourceType.Textiles,     true);
            highwayBetween3And4.SetUpkeepRequestedForResource(ResourceType.ServiceGoods, true);
            highwayBetween4And5.SetUpkeepRequestedForResource(ResourceType.HiTechGoods,  true);

            var managerToTest = managerFactory.ConstructHighwayManagerAtLocation(node1);

            //Execution
            managerToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(1, managerToTest.LastCalculatedUpkeep[ResourceType.Food        ], "Manager records an incorrect Food upkeep"        );
            Assert.AreEqual(1, managerToTest.LastCalculatedUpkeep[ResourceType.Textiles    ], "Manager records an incorrect Textiles upkeep"    );
            Assert.AreEqual(1, managerToTest.LastCalculatedUpkeep[ResourceType.ServiceGoods], "Manager records an incorrect ServiceGoods upkeep");
            Assert.AreEqual(1, managerToTest.LastCalculatedUpkeep[ResourceType.HiTechGoods ], "Manager records an incorrect HiTechGoods upkeep" );
        }

        [Test]
        public void OnTickConsumptionIsCalled_ConsumptionRateDeterminedBySecondsToPerformConsumption() {
            //Setup
            var mapGraph = BuildMockMapGraph();
            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2 = mapGraph.BuildNode(Vector3.right * 4);
            var node3 = mapGraph.BuildNode(Vector3.right * 8);
            var node4 = mapGraph.BuildNode(Vector3.right * 12);
            var node5 = mapGraph.BuildNode(Vector3.right * 16);

            node1.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            node1.BlobSite.SetCapacityForResourceType(ResourceType.Food, 100);
            node1.BlobSite.TotalCapacity = 100;

            node2.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            node2.BlobSite.SetCapacityForResourceType(ResourceType.Food, 100);
            node2.BlobSite.TotalCapacity = 100;

            for(int i = 0; i < 100; ++i) {
                node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Food));
                node5.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Food));
            }
            
            node1.name = "Node1";
            node2.name = "Node2";
            node3.name = "Node3";
            node4.name = "Node4";
            node5.name = "Node5";

            mapGraph.BuildMapEdge(node1, node2);
            mapGraph.BuildMapEdge(node2, node3);
            mapGraph.BuildMapEdge(node3, node4);
            mapGraph.BuildMapEdge(node4, node5);

            var highwayFactory = BuildMockHighwayFactory();

            var privateData = BuildManagerPrivateData();
            privateData.SetSecondsToPerformConsumption(1f);
            privateData.SetBlobFactory(BuildMockBlobFactory());
            privateData.SetEfficiencyGainFromResource(IntResourceSummary.BuildSummary(new GameObject()));

            var managerFactory = BuildHighwayManagerFactory(privateData);
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;
            managerFactory.ManagementRadius = 2;

            var highwayBetween1And2 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highwayBetween4And5 = highwayFactory.ConstructHighwayBetween(node4, node5);

            highwayBetween1And2.SetUpkeepRequestedForResource(ResourceType.Food, true);
            highwayBetween4And5.SetUpkeepRequestedForResource(ResourceType.Food, true);

            var managerAtNode1 = managerFactory.ConstructHighwayManagerAtLocation(node1);
            var managerAtNode2 = managerFactory.ConstructHighwayManagerAtLocation(node5);

            //Execution
            managerAtNode1.TickConsumption(10f);
            privateData.SetSecondsToPerformConsumption(2f);
            managerAtNode2.TickConsumption(10f);

            //Validation
            Assert.AreEqual(90, node1.BlobSite.GetCountOfContentsOfType(ResourceType.Food), "Node1 has an incorrect amount of food remaining");
            Assert.AreEqual(95, node5.BlobSite.GetCountOfContentsOfType(ResourceType.Food), "Node5 has an incorrect amount of food remaining");
        }

        [Test]
        public void OnConsumptionPerformed_HighwaysAreGivenEfficienciesBasedOnWhatUpkeepIsConsumed_AndAreAddressedInPriorityOrder() {
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

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                node1.BlobSite.SetPlacementPermissionForResourceType(resourceType, true);
                node1.BlobSite.SetCapacityForResourceType(resourceType, 10);
                node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(resourceType));
            }
            node1.BlobSite.TotalCapacity = 100;
            node1.BlobSite.PlaceBlobInto(BuildMockResourceBlob(ResourceType.Food));

            mapGraph.BuildMapEdge(node1, node2);
            mapGraph.BuildMapEdge(node2, node3);
            mapGraph.BuildMapEdge(node3, node4);
            mapGraph.BuildMapEdge(node4, node5);
            mapGraph.BuildMapEdge(node5, node1);

            var highwayFactory = BuildMockHighwayFactory();

            var privateData = BuildManagerPrivateData();
            privateData.SetSecondsToPerformConsumption(1f);
            privateData.SetBlobFactory(BuildMockBlobFactory());
            privateData.SetEfficiencyGainFromResource(IntResourceSummary.BuildSummary(new GameObject(), new Dictionary<ResourceType, int>() {
                { ResourceType.Food,         3  },
                { ResourceType.Textiles,     5  },
                { ResourceType.ServiceGoods, 8  },
                { ResourceType.HiTechGoods,  13 },
            }));

            var managerFactory = BuildHighwayManagerFactory(privateData);
            managerFactory.MapGraph = mapGraph;
            managerFactory.HighwayFactory = highwayFactory;
            managerFactory.ManagementRadius = 4;

            var highway1 = highwayFactory.ConstructHighwayBetween(node1, node2);
            var highway2 = highwayFactory.ConstructHighwayBetween(node2, node3);
            var highway3 = highwayFactory.ConstructHighwayBetween(node3, node4);
            var highway4 = highwayFactory.ConstructHighwayBetween(node4, node5);
            var highway5 = highwayFactory.ConstructHighwayBetween(node5, node1);

            highway1.SetUpkeepRequestedForResource(ResourceType.Food,         true);
            highway2.SetUpkeepRequestedForResource(ResourceType.Textiles,     true);
            highway3.SetUpkeepRequestedForResource(ResourceType.ServiceGoods, true);
            highway4.SetUpkeepRequestedForResource(ResourceType.HiTechGoods,  true);

            highway5.SetUpkeepRequestedForResource(ResourceType.Food,         true);
            highway5.SetUpkeepRequestedForResource(ResourceType.Textiles,     true);
            highway5.SetUpkeepRequestedForResource(ResourceType.ServiceGoods, true);
            highway5.SetUpkeepRequestedForResource(ResourceType.HiTechGoods,  true); 

            highway1.Priority = 0;
            highway2.Priority = 0;
            highway3.Priority = 0;
            highway4.Priority = 0;
            highway5.Priority = 1;

            var managerToTest = managerFactory.ConstructHighwayManagerAtLocation(node1);

            //Execution
            managerToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(1 + privateData.EfficiencyGainFromResource[ResourceType.Food        ], highway1.Efficiency, "Highway1 has an incorrect efficiency");
            Assert.AreEqual(1 + privateData.EfficiencyGainFromResource[ResourceType.Textiles    ], highway2.Efficiency, "Highway2 has an incorrect efficiency");
            Assert.AreEqual(1 + privateData.EfficiencyGainFromResource[ResourceType.ServiceGoods], highway3.Efficiency, "Highway3 has an incorrect efficiency");
            Assert.AreEqual(1 + privateData.EfficiencyGainFromResource[ResourceType.HiTechGoods ], highway4.Efficiency, "Highway4 has an incorrect efficiency");
            Assert.AreEqual(1 + privateData.EfficiencyGainFromResource[ResourceType.Food        ], highway5.Efficiency, "Highway5 has an incorrect efficiency");
        }

        #endregion

        #endregion

        #region utilities

        private HighwayManagerFactory BuildHighwayManagerFactory(HighwayManagerPrivateData privateData) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<HighwayManagerFactory>();

            newFactory.ManagerPrivateData = privateData;
            privateData.SetParentFactory(newFactory);

            return newFactory;
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
            var newGraph = hostingObject.AddComponent<MockMapGraph>();
            newGraph.AlgorithmSet = hostingObject.AddComponent<MapGraphAlgorithmSet>();
            return newGraph;
        }

        private MockResourceBlob BuildMockResourceBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<MockResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        private BlobHighwayProfile BuildHighwayProfile() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<BlobHighwayProfile>();
        }

        private MockBlobHighwayFactory BuildMockHighwayFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobHighwayFactory>();
        }

        private MockResourceBlobFactory BuildMockBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        private HighwayManagerPrivateData BuildManagerPrivateData() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayManagerPrivateData>();
        }

        #endregion

        #endregion

    }

}


