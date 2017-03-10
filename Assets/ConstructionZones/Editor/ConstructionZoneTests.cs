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
            var zoneToTest = factoryToUse.BuildConstructionZone(mapNode, factoryToUse.ResourceDepotProject);

            //Validation
            var projectCost = factoryToUse.ResourceDepotProject.Cost;
            foreach(var resourceType in projectCost) {
                Assert.That(mapNode.BlobSite.GetPlacementPermissionForResourceType(resourceType),
                    "BlobSite does not have placement permission for resourceType " + resourceType);

                Assert.IsFalse(mapNode.BlobSite.GetExtractionPermissionForResourceType(resourceType),
                    "BlobSite falsely has extraction permission for resourceType " + resourceType);

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
            var zoneToTest = factoryToUse.BuildConstructionZone(mapNode, factoryToUse.ResourceDepotProject);

            //Validation
            Assert.That(hasBeenCleared);
        }

        [Test]
        public void OnCurrentProjectChanged_UnderlyingBlobSiteGainsCorrectPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var mapNode = BuildMapNode();

            var zoneToTest = factoryToUse.BuildConstructionZone(mapNode, factoryToUse.ResourceDepotProject);

            var alternateProject = new MockConstructionProject();
            alternateProject.SetCost(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 40),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 80)
            ));

            //Execution
            zoneToTest.CurrentProject = alternateProject;

            //Validation
            Assert.AreEqual(40, mapNode.BlobSite.GetCapacityForResourceType(ResourceType.Red), "Incorrect capacity for Red");
            Assert.AreEqual(0, mapNode.BlobSite.GetCapacityForResourceType(ResourceType.Green), "Incorrect capacity for Green");
            Assert.AreEqual(80, mapNode.BlobSite.GetCapacityForResourceType(ResourceType.Blue), "Incorrect capacity for Blue");

            Assert.That(mapNode.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Red), "Red lacks placement permission");
            Assert.IsFalse(mapNode.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Green), "Green falsely has placement permission");
            Assert.That(mapNode.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Blue), "Blue lacks placement permission");

            Assert.IsFalse(mapNode.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Red), "Red falsely has extraction permission");
            Assert.IsFalse(mapNode.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Blue), "Blue falsely has extraction permission");
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
            for(int i = 1; i < depotCost[ResourceType.Red]; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
                Assert.AreEqual(depotCost[ResourceType.Red] - i, zoneToTest.GetResourcesNeededToFinish()[ResourceType.Red],
                    "Incorrect Red needed to finish on iteration " + i);
            }
            for(int i = 1; i < depotCost[ResourceType.Green]; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Green));
                Assert.AreEqual(depotCost[ResourceType.Green] - i, zoneToTest.GetResourcesNeededToFinish()[ResourceType.Green],
                    "Incorrect Red needed to finish on iteration " + i);
            }
            for(int i = 1; i < depotCost[ResourceType.Blue]; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Blue));
                Assert.AreEqual(depotCost[ResourceType.Blue] - i, zoneToTest.GetResourcesNeededToFinish()[ResourceType.Blue],
                    "Incorrect Red needed to finish on iteration " + i);
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
            projectToComplete.SetCost(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)));

            MapNodeBase locationPlacedIntoBuildAction = null;
            projectToComplete.SetBuildAction(delegate(MapNodeBase location) {
                locationPlacedIntoBuildAction = location;
            });

            var zoneToTest = factoryToUse.BuildConstructionZone(newLocation, projectToComplete);

            var depotCost = factoryToUse.ResourceDepotProject.Cost;
            var blobSite = newLocation.BlobSite;

            //Execution
            for(int i = 0; i < 10; ++i) {
                newLocation.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
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
            projectToComplete.SetCost(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)));

            var zoneToTest = factoryToUse.BuildConstructionZone(newLocation, projectToComplete);

            var depotCost = factoryToUse.ResourceDepotProject.Cost;
            var blobSite = newLocation.BlobSite;

            //Execution
            for(int i = 0; i < 10; ++i) {
                newLocation.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
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
            projectToComplete.SetCost(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)));

            var zoneToTest = factoryToUse.BuildConstructionZone(newLocation, projectToComplete);

            var depotCost = factoryToUse.ResourceDepotProject.Cost;
            var blobSite = newLocation.BlobSite;

            //Execution
            for(int i = 0; i < 10; ++i) {
                newLocation.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
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


