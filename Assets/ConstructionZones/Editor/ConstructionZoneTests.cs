using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.ConstructionZones.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.ConstructionZones.Editor {

    public class ConstructionZoneTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnConstructionZoneBuiltViaFactory_UnderlyingBlobSiteHasCorrectPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var mapNode = BuildMapNode();

            //Execution
            factoryToUse.BuildConstructionZone(mapNode, factoryToUse.ResourceDepotProject);

            //Validation
            var projectCost = factoryToUse.ResourceDepotProject.Cost;
            foreach(var resourceType in projectCost) {
                if(projectCost[resourceType] > 0) {
                    Assert.That(mapNode.BlobSite.GetPlacementPermissionForResourceType(resourceType),
                        "BlobSite does not have placement permission for resourceType " + resourceType);

                    Assert.IsFalse(mapNode.BlobSite.GetExtractionPermissionForResourceType(resourceType),
                        "BlobSite falsely has extraction permission for resourceType " + resourceType);
                }

                Assert.AreEqual(projectCost[resourceType], mapNode.BlobSite.GetCapacityForResourceType(resourceType),
                    "BlobSite has the incorrect capacity for resourceType "+ resourceType);
            }
            Assert.AreEqual(projectCost.GetTotalResourceCount(), mapNode.BlobSite.TotalCapacity,
                "BlobSite has the incorrect TotalCapacity");
        }

        [Test]
        public void OnConstructionZoneBuildViaFactory_UnderlyingBlobSiteIsCleared() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var mapNode = BuildMapNode();

            bool hasBeenCleared = false;
            mapNode.BlobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                hasBeenCleared = true;
            };

            //Execution
            factoryToUse.BuildConstructionZone(mapNode, factoryToUse.ResourceDepotProject);

            //Validation
            Assert.That(hasBeenCleared);
        }

        [Test]
        public void Factory_OnManyConstructionZonesCreatedAndDestroyed_NoTwoActiveZonesEverHaveTheSameID() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();

            var mapNodeList = new List<MapNodeBase>();
            for(int nodeCreateIndex = 0; nodeCreateIndex < 100; ++nodeCreateIndex) {
                mapNodeList.Add(BuildMapNode());
            }

            var zoneList = new List<ConstructionZoneBase>();

            //Execution and Validation
            int i = 0;
            for(; i < 50; ++i) {
                zoneList.Add(factoryToUse.BuildConstructionZone(mapNodeList[i], factoryToUse.ResourceDepotProject));
                foreach(var outerZone in zoneList) {
                    foreach(var innerZone in zoneList) {
                        if(outerZone != innerZone) {
                            Assert.AreNotEqual(outerZone.ID, innerZone.ID, "Duplicate IDs on first creation cycle on index " + i);
                        }
                    }
                }
            }
            for(i = 34; i >= 10; --i) {
                var zoneToDestroy = zoneList[i];
                zoneList.Remove(zoneToDestroy);
                factoryToUse.DestroyConstructionZone(zoneToDestroy);
            }
            for(i = 10; i < 35; ++i) {
                zoneList.Add(factoryToUse.BuildConstructionZone(mapNodeList[i], factoryToUse.ResourceDepotProject));
                foreach(var outerZone in zoneList) {
                    foreach(var innerZone in zoneList) {
                        if(outerZone != innerZone) {
                            Assert.AreNotEqual(outerZone.ID, innerZone.ID, "Duplicate IDs on second creation cycle on index " + i);
                        }
                    }
                }
            }
        }

        [Test]
        public void Factory_OnHasConstructionZoneAtLocationCalled_ReturnsTrueIfThereExistsAConstructionZoneWithThatLocation() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var mapNodes = new List<MapNodeBase>() {
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
            };

            factoryToTest.BuildConstructionZone(mapNodes[0], factoryToTest.ResourceDepotProject);
            factoryToTest.BuildConstructionZone(mapNodes[1], factoryToTest.ResourceDepotProject);
            factoryToTest.BuildConstructionZone(mapNodes[4], factoryToTest.ResourceDepotProject);

            //Execution

            //Validation
            Assert.IsTrue(factoryToTest.HasConstructionZoneAtLocation(mapNodes[0]), "Does not register constructionZone at location 0");
            Assert.IsTrue(factoryToTest.HasConstructionZoneAtLocation(mapNodes[1]), "Does not register constructionZone at location 1");
            Assert.IsFalse(factoryToTest.HasConstructionZoneAtLocation(mapNodes[2]), "Falsely registers constructionZone at location 2");
            Assert.IsFalse(factoryToTest.HasConstructionZoneAtLocation(mapNodes[3]), "Falsely registers constructionZone at location 3");
            Assert.IsTrue(factoryToTest.HasConstructionZoneAtLocation(mapNodes[4]), "Does not register constructionZone at location 4");
        }

        [Test]
        public void Factory_OnGetConstructionZoneAtLocationCalled_ConstructionZoneReturnedHasCorrectLocation() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var mapNodes = new List<MapNodeBase>() {
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
            };

            var zoneOnNode0 = factoryToTest.BuildConstructionZone(mapNodes[0], factoryToTest.ResourceDepotProject);
            var zoneOnNode1 = factoryToTest.BuildConstructionZone(mapNodes[1], factoryToTest.ResourceDepotProject);
            var zoneOnNode2 = factoryToTest.BuildConstructionZone(mapNodes[2], factoryToTest.ResourceDepotProject);

            //Execution

            //Validation
            Assert.AreEqual(zoneOnNode0, factoryToTest.GetConstructionZoneAtLocation(mapNodes[0]), "Incorrect zone retrieved from node 0");
            Assert.AreEqual(zoneOnNode1, factoryToTest.GetConstructionZoneAtLocation(mapNodes[1]), "Incorrect zone retrieved from node 1");
            Assert.AreEqual(zoneOnNode2, factoryToTest.GetConstructionZoneAtLocation(mapNodes[2]), "Incorrect zone retrieved from node 2");
        }

        [Test]
        public void Factory_OnGetConstructionZoneOfIDCalled_ConstructionZoneReturnedHasThatID_OrNullIfNoSuchZoneExists() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var mapNodes = new List<MapNodeBase>() {
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
                BuildMapNode(),
            };

            var zones = new List<ConstructionZoneBase>() {
                factoryToTest.BuildConstructionZone(mapNodes[0], factoryToTest.ResourceDepotProject),
                factoryToTest.BuildConstructionZone(mapNodes[1], factoryToTest.ResourceDepotProject),
                factoryToTest.BuildConstructionZone(mapNodes[2], factoryToTest.ResourceDepotProject),
                factoryToTest.BuildConstructionZone(mapNodes[3], factoryToTest.ResourceDepotProject),
                factoryToTest.BuildConstructionZone(mapNodes[4], factoryToTest.ResourceDepotProject),
            };

            //Execution


            //Validation
            Assert.AreEqual(zones[0], factoryToTest.GetConstructionZoneOfID(zones[0].ID), "Did not return Zone[0] when passed its ID");
            Assert.AreEqual(zones[1], factoryToTest.GetConstructionZoneOfID(zones[1].ID), "Did not return Zone[1] when passed its ID");
            Assert.AreEqual(zones[2], factoryToTest.GetConstructionZoneOfID(zones[2].ID), "Did not return Zone[2] when passed its ID");
            Assert.AreEqual(zones[3], factoryToTest.GetConstructionZoneOfID(zones[3].ID), "Did not return Zone[3] when passed its ID");
            Assert.AreEqual(zones[4], factoryToTest.GetConstructionZoneOfID(zones[4].ID), "Did not return Zone[4] when passed its ID");
            Assert.IsNull(factoryToTest.GetConstructionZoneOfID(Int32.MaxValue), "ID of Int32.MaxValue did not return null");
        }

        [Test]
        public void OnCurrentProjectChanged_UnderlyingBlobSiteGainsCorrectPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var mapNode = BuildMapNode();

            var zoneToTest = factoryToUse.BuildConstructionZone(mapNode, factoryToUse.ResourceDepotProject);

            var alternateProject = new MockConstructionProject();
            alternateProject.SetCost(ResourceSummary.BuildResourceSummary(
                factoryToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 40),
                new KeyValuePair<ResourceType, int>(ResourceType.White, 80)
            ));

            //Execution
            zoneToTest.CurrentProject = alternateProject;

            //Validation
            Assert.AreEqual(40, mapNode.BlobSite.GetCapacityForResourceType(ResourceType.Food), "Incorrect capacity for Food");
            Assert.AreEqual(0, mapNode.BlobSite.GetCapacityForResourceType(ResourceType.Yellow), "Incorrect capacity for Yellow");
            Assert.AreEqual(80, mapNode.BlobSite.GetCapacityForResourceType(ResourceType.White), "Incorrect capacity for White");

            Assert.That(mapNode.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Food), "Food lacks placement permission");
            Assert.IsFalse(mapNode.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Yellow), "Yellow falsely has placement permission");
            Assert.That(mapNode.BlobSite.GetPlacementPermissionForResourceType(ResourceType.White), "White lacks placement permission");

            Assert.IsFalse(mapNode.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Food), "Food falsely has extraction permission");
            Assert.IsFalse(mapNode.BlobSite.GetExtractionPermissionForResourceType(ResourceType.White), "White falsely has extraction permission");
        }

        [Test]
        public void OnBlobsAddedToAndRemovedFromUnderlyingSite_GetResourcesNeededToFinishIsAlwaysCorrect() {
            //Setup
            var newLocation = BuildMapNode();
            var factoryToUse = BuildConstructionZoneFactory();

            var zoneToTest = factoryToUse.BuildConstructionZone(newLocation, factoryToUse.ResourceDepotProject);

            var depotCost = factoryToUse.ResourceDepotProject.Cost;
            var blobSite = newLocation.BlobSite;

            //Execution and Validation
            for(int i = 1; i < depotCost[ResourceType.Food]; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
                Assert.AreEqual(depotCost[ResourceType.Food] - i, zoneToTest.GetResourcesNeededToFinish()[ResourceType.Food],
                    "Incorrect Food needed to finish on iteration " + i);
            }
            for(int i = 1; i < depotCost[ResourceType.Yellow]; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));
                Assert.AreEqual(depotCost[ResourceType.Yellow] - i, zoneToTest.GetResourcesNeededToFinish()[ResourceType.Yellow],
                    "Incorrect Food needed to finish on iteration " + i);
            }
            for(int i = 1; i < depotCost[ResourceType.White]; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.White));
                Assert.AreEqual(depotCost[ResourceType.White] - i, zoneToTest.GetResourcesNeededToFinish()[ResourceType.White],
                    "Incorrect Food needed to finish on iteration " + i);
            }

            blobSite.ClearContents();
            var resourcesNeeded = zoneToTest.GetResourcesNeededToFinish();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.AreEqual(depotCost[resourceType], resourcesNeeded[resourceType],
                    "Incorrect post-clear resources needed for resourceType " + resourceType);
            }
        }

        [Test]
        public void OnUnderlyingBlobSiteMeetsConstructionConditions_ConstructionActionIsCalledCorrectly() {
            //Setup
            var newLocation = BuildMapNode();
            var factoryToUse = BuildConstructionZoneFactory();

            var projectToComplete = new MockConstructionProject();
            projectToComplete.SetCost(ResourceSummary.BuildResourceSummary(
                factoryToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            MapNodeBase locationPlacedIntoBuildAction = null;
            projectToComplete.SetBuildAction(delegate(MapNodeBase location) {
                locationPlacedIntoBuildAction = location;
            });

            factoryToUse.BuildConstructionZone(newLocation, projectToComplete);

            //Execution
            for(int i = 0; i < 10; ++i) {
                newLocation.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }

            //Validation
            Assert.AreEqual(newLocation, locationPlacedIntoBuildAction, "Delegate not called on correct location");
        }

        [Test]
        public void OnUnderlyingBlobSiteMeetsConstructionConditions_BlobSiteIsClearedOfContents() {
            //Setup
            var newLocation = BuildMapNode();

            bool siteWasCleared = false;
            newLocation.BlobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                siteWasCleared = true;
            };

            var factoryToUse = BuildConstructionZoneFactory();
            var projectToComplete = new MockConstructionProject();
            projectToComplete.SetCost(ResourceSummary.BuildResourceSummary(
                factoryToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            factoryToUse.BuildConstructionZone(newLocation, projectToComplete);

            //Execution
            for(int i = 0; i < 10; ++i) {
                newLocation.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }

            //Validation
            Assert.That(siteWasCleared);
        }

        [Test]
        public void OnUnderlyingBlobSiteMeetsConstructionConditions_BlobSiteIsClearedOfPermissionsAndCapacities() {
            //Setup
            var newLocation = BuildMapNode();

            var factoryToUse = BuildConstructionZoneFactory();
            var projectToComplete = new MockConstructionProject();
            projectToComplete.SetCost(ResourceSummary.BuildResourceSummary(
                factoryToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            factoryToUse.BuildConstructionZone(newLocation, projectToComplete);

            var blobSite = newLocation.BlobSite;

            //Execution
            for(int i = 0; i < 10; ++i) {
                newLocation.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.IsFalse(blobSite.GetPlacementPermissionForResourceType(resourceType),
                    "blobSite falsely has placement permission for resourceType " + resourceType);
                Assert.IsFalse(blobSite.GetExtractionPermissionForResourceType(resourceType),
                    "blobSite falsely has extraction permission for resourceType " + resourceType);
                Assert.AreEqual(0, blobSite.GetCapacityForResourceType(resourceType), 
                    "blobSite has incorrect capacity for resourceType " + resourceType);
            }
            Assert.AreEqual(0, blobSite.TotalCapacity, "blobSite has incorrect TotalCapacity");
        }

        #endregion

        #region error checking

        [Test]
        public void OnFactorysBuildConstructionSitePassedNullMapNode_ThrowsArgumentNullException() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToUse.BuildConstructionZone(null, factoryToUse.ResourceDepotProject);
            });
        }

        [Test]
        public void OnFactorysBuildConstructionSitePassedNullConstructionProject_ThrowsArgumentNullException() {
            //Setup
            var newLocation = BuildMapNode();
            var factoryToUse = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToUse.BuildConstructionZone(newLocation, null);
            });
        }

        [Test]
        public void OnFactorysDestroyConstructionSitePassedNullConstructionSite_ThrowsArgumentNullException() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToUse.DestroyConstructionZone(null);
            });
        }

        [Test]
        public void OnConstructionSitePassedNullCurrentProject_ThrowsArgumentNullException() {
            //Setup
            var newLocation = BuildMapNode();
            var factoryToUse = BuildConstructionZoneFactory();

            var zoneToTest = factoryToUse.BuildConstructionZone(newLocation, factoryToUse.ResourceDepotProject);

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                zoneToTest.CurrentProject = null;
            });
        }

        [Test]
        public void Factory_OnGetConstructionZoneAtLocationCalled_WhenHasConstructionZoneAtLocationIsFalse_ThrowsConstructionZoneException() {
            //Setup
            var newLocation = BuildMapNode();
            var factoryToTest = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ConstructionZoneException>(delegate() {
                factoryToTest.GetConstructionZoneAtLocation(newLocation);
            });
        }

        [Test]
        public void Factory_OnHasConstructionZoneAtLocationPassedNullLocation_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.HasConstructionZoneAtLocation(null);
            });
        }

        [Test]
        public void Factory_OnGetConstructionZoneAtLocationPassedNullLocation_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetConstructionZoneAtLocation(null);
            });
        }

        [Test]
        public void Factory_OnHasMapNodeAtLocationIsTrue_AndBuildConstructionZoneCalledAtLocation_ThrowsConstrutionZoneException() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var location = BuildMapNode();

            factoryToTest.BuildConstructionZone(location, factoryToTest.ResourceDepotProject);

            //Execution and Validation
            Assert.Throws<ConstructionZoneException>(delegate() {
                factoryToTest.BuildConstructionZone(location, factoryToTest.ResourceDepotProject);
            });
        }

        #endregion

        #endregion

        #region utilities

        private ConstructionZoneFactory BuildConstructionZoneFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ConstructionZoneFactory>();
        }

        private MockMapNode BuildMapNode() {
            var hostingObject = new GameObject();
            var newNode = hostingObject.AddComponent<MockMapNode>();
            newNode.SetBlobSite(hostingObject.AddComponent<MockBlobSite>());
            return newNode;
        }

        private MockResourceDepotFactory BuildResourceDepotFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceDepotFactory>();
        }

        private ResourceBlob BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        #endregion

        #endregion

    }

}


