using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.ResourceDepots.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.ResourceDepots.Editor {

    public class ResourceDepotTests {

        #region instance methods

        #region tests

        [Test]
        public void Factory_OnResouceDepotConstructed_LocationIsInitializedProperly() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            //Execution
            var depotToTest = factoryToUse.ConstructDepotAt(location);

            //Validation
            Assert.AreEqual(location, depotToTest.Location);
        }

        [Test]
        public void Factory_OnResourceDepotConstructed_UnderlyingBlobSiteHasProperDefaultPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            //Execution
            var depotToTest = factoryToUse.ConstructDepotAt(location);

            //Validation
            var blobSite = depotToTest.Location.BlobSite;

            int expectedTotalCapacity = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(blobSite.GetPlacementPermissionForResourceType(resourceType),
                    "blobSite lacks placement permission for resourceType " + resourceType);

                Assert.That(blobSite.GetExtractionPermissionForResourceType(resourceType),
                    "blobSite lacks extraction permission for resourceType " + resourceType);

                Assert.AreEqual(0, blobSite.GetCapacityForResourceType(resourceType),
                    "blobSite has incorrect capacity for resourceType " + resourceType);

                expectedTotalCapacity += depotToTest.Profile.PerResourceCapacity;
            }

            Assert.AreEqual(expectedTotalCapacity, blobSite.TotalCapacity, "blobSite has incorrect TotalCapacity");
        }

        [Test]
        public void Factory_OnResourceDepotConstructed_UnderlyingBlobSiteIsClearedOfResources() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            bool clearedOfAllBlobs = false;
            location.BlobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                clearedOfAllBlobs = true;
            };

            //Execution
            factoryToUse.ConstructDepotAt(location);

            //Validation
            Assert.That(clearedOfAllBlobs);
        }

        [Test]
        public void Factory_OnResourceDepotConstructed_ResourceDepotHasEmptyProfile() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            //Execution
            var depotToTest = factoryToUse.ConstructDepotAt(location);

            //Validation
            Assert.AreEqual(ResourceDepotProfile.Empty, depotToTest.Profile);
        }

        [Test]
        public void Factory_OnGetDepotOfIDCalled_ReturnsTheDepotWithTheAppropriateID_OrNullIfNoneExists() {
            //Setup
            var factoryToTest = BuildFactory();
            var location1 = BuildMapNode();
            var location2 = BuildMapNode();
            var location3 = BuildMapNode();

            var depot1 = factoryToTest.ConstructDepotAt(location1);
            var depot2 = factoryToTest.ConstructDepotAt(location2);
            var depot3 = factoryToTest.ConstructDepotAt(location3);

            //Execution
            var depotRetrievedFromID1 = factoryToTest.GetDepotOfID(depot1.ID);
            var depotRetrievedFromID2 = factoryToTest.GetDepotOfID(depot2.ID);
            var depotRetrievedFromID3 = factoryToTest.GetDepotOfID(depot3.ID);
            var depotRetrievedFrom42 = factoryToTest.GetDepotOfID(42);

            //Validation
            Assert.AreEqual(depot1, depotRetrievedFromID1, "Depot1 was not returned when its ID was provided");
            Assert.AreEqual(depot2, depotRetrievedFromID2, "Depot2 was not returned when its ID was provided");
            Assert.AreEqual(depot3, depotRetrievedFromID3, "Depot3 was not returned when its ID was provided");
            Assert.Null(depotRetrievedFrom42, "ID 42 falsely returned a ResourceDepot");
        }

        [Test]
        public void OnProfileSet_UnderlyingBlobSiteHasProperPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();
            var depotToTest = factoryToUse.ConstructDepotAt(location);

            var newProfile = new ResourceDepotProfile(20);

            //Execution
            depotToTest.Profile = newProfile;

            //Validation
            var blobSite = location.BlobSite;
            int expectedTotalCapacity = 0;

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.AreEqual(20, blobSite.GetCapacityForResourceType(resourceType),
                    "blobSite has incorrect capacity for resourceType " + resourceType);
                expectedTotalCapacity += 20;
            }

            Assert.AreEqual(expectedTotalCapacity, blobSite.TotalCapacity, "blobSite has incorrect TotalCapacity");
        }

        [Test]
        public void OnClearCalled_UnderlyingBlobSiteIsCleared() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();
            var depotToTest = factoryToUse.ConstructDepotAt(location);

            bool clearedOfAllBlobs = false;
            location.BlobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                clearedOfAllBlobs = true;
            };

            //Execution
            depotToTest.Clear();

            //Validation
            Assert.That(clearedOfAllBlobs);
        }

        #endregion

        #region utilities

        private ResourceDepotFactory BuildFactory() {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<ResourceDepotFactory>();
            return newFactory;
        }

        private MapNodeBase BuildMapNode() {
            var hostingObject = new GameObject();
            var newMapNode = hostingObject.AddComponent<MockMapNode>();
            newMapNode.SetBlobSite(hostingObject.AddComponent<MockBlobSite>());
            return newMapNode;
        }

        #endregion

        #endregion

    }

}


