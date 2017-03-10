using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.Depots.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Depots.Editor {

    public class ResourceDepotTests {

        #region instance methods

        #region tests

        [Test]
        public void OnResouceDepotConstructedViaFactory_LocationIsInitializedProperly() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            //Execution
            var depotToTest = factoryToUse.ConstructDepot(location);

            //Validation
            Assert.AreEqual(location, depotToTest.Location);
        }

        [Test]
        public void OnResourceDepotConstructedViaFactory_UnderlyingBlobSiteHasProperDefaultPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            //Execution
            var depotToTest = factoryToUse.ConstructDepot(location);

            //Validation
            var blobSite = depotToTest.Location.BlobSite;

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(blobSite.GetPlacementPermissionForResourceType(resourceType),
                    "blobSite lacks placement permission for resourceType " + resourceType);

                Assert.That(blobSite.GetExtractionPermissionForResourceType(resourceType),
                    "blobSite lacks extraction permission for resourceType " + resourceType);

                Assert.AreEqual(0, blobSite.GetCapacityForResourceType(resourceType),
                    "blobSite has incorrect capacity for resourceType " + resourceType);
            }

            Assert.AreEqual(0, blobSite.TotalCapacity, "blobSite has incorrect TotalCapacity");
        }

        [Test]
        public void OnResourceDepotConstructedViaFactory_UnderlyingBlobSiteIsClearedOfResources() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            bool clearedOfAllBlobs = false;
            location.BlobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                clearedOfAllBlobs = true;
            };

            //Execution
            var depotToTest = factoryToUse.ConstructDepot(location);

            //Validation
            Assert.That(clearedOfAllBlobs);
        }

        [Test]
        public void OnResourceDepotConstructedViaFactory_ResourceDepotHasEmptyProfile() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();

            //Execution
            var depotToTest = factoryToUse.ConstructDepot(location);

            //Validation
            Assert.AreEqual(ResourceDepotProfile.Empty, depotToTest.Profile);
        }

        [Test]
        public void OnProfileSet_UnderlyingBlobSiteHasProperPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildFactory();
            var location = BuildMapNode();
            var depotToTest = factoryToUse.ConstructDepot(location);

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
            var depotToTest = factoryToUse.ConstructDepot(location);

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


