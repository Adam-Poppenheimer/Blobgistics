using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Highways;
using Assets.HighwayUpgrade.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayUpgrade.Editor {

    public class HighwayUpgraderTests {

        #region instance fields and properties

        private ResourceSummary PlaceholderCost {
            get {
                return new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, Int32.MaxValue));
            }
        }

        #endregion

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnHighwayUpgraderBuiltViaFactory_UpgraderHasCorrectLocation_Profile_AndTargetedHighway() {
            //Setup
            var highway = BuildBlobHighway();
            var site = BuildBlobSite();

            var factoryToTest = BuildUpgraderFactory();

            var profile = new BlobHighwayProfile(24f, 100, PlaceholderCost);

            //Execution
            var upgraderToTest = factoryToTest.BuildHighwayUpgrader(highway, site, profile);

            //Validation
            Assert.AreEqual(highway, upgraderToTest.TargetedHighway, "Upgrader has incorrect TargetedHighway");
            Assert.AreEqual(site, upgraderToTest.UnderlyingSite, "Upgrader has incorrect UnderlyingSite");
            Assert.AreEqual(profile, upgraderToTest.ProfileToInsert, "Upgrader has incorrect ProfileToInsert");
        }

        [Test]
        public void OnHighwayUpgraderBuiltViaFactory_UnderlyingBlobSiteHasCorrectCapacitiesAndPermissions() {
            //Setup
            var highway = BuildBlobHighway();
            var site = BuildBlobSite();

            var factoryToTest = BuildUpgraderFactory();

            var testProfile = new BlobHighwayProfile(24f, 100, new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 15),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 12)
            ));

            //Execution
            var upgraderToTest = factoryToTest.BuildHighwayUpgrader(highway, site, testProfile);

            //Validation
            int intendedTotalCapacity = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                int costCountOfType = upgraderToTest.ProfileToInsert.Cost.GetCountOfResourceType(resourceType);
                if(costCountOfType > 0) {
                    intendedTotalCapacity += costCountOfType;
                    Assert.That(site.GetPlacementPermissionForResourceType(resourceType),
                        "UnderlyingSite fails to give placement permission to resourceType " + resourceType);
                    Assert.AreEqual(costCountOfType, site.GetCapacityForResourceType(resourceType),
                        "UnderlyingSite has incorrect capacity for resourceType " + resourceType);
                }else {
                    Assert.IsFalse(site.GetPlacementPermissionForResourceType(resourceType),
                        "UnderlyingSite falsely gives placement permission to resourceType " + resourceType);
                }
                Assert.IsFalse(site.GetExtractionPermissionForResourceType(resourceType),
                    "UnderlyingSite falsely gives extraction permission to resourceType " + resourceType);
            }

            Assert.AreEqual(intendedTotalCapacity, site.TotalCapacity, "UnderlyingSite has incorrect TotalCapacity");
        }

        [Test]
        public void OnHighwayUpgraderBuiltViaFactory_UnderlyingBlobSiteIsCleared() {
            //Setup
            var highway = BuildBlobHighway();
            var site = BuildBlobSite();

            bool hasBeenCleared = false;
            site.AllBlobsCleared += delegate(object sender, EventArgs e) {
                hasBeenCleared = true;
            };

            var factoryToTest = BuildUpgraderFactory();

            var testProfile = new BlobHighwayProfile(24f, 100, PlaceholderCost);

            //Execution
            var upgraderToTest = factoryToTest.BuildHighwayUpgrader(highway, site, testProfile);

            //Validation
            Assert.That(hasBeenCleared);
        }

        [Test]
        public void OnBlobsPlacedWithinUnderlyingBlobSite_GetResourcesNeededToUpgradeIsAlwaysCorrect() {
            //Setup
            var blobSite = BuildBlobSite();
            var blobHighway = BuildBlobHighway();
            var cost = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10)
            );

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(new BlobHighwayProfile(1f, 20, cost));

            var upgraderToTest = BuildHighwayUpgrader(privateData);

            //Execution and Validation
            for(int redCount = 0; redCount < 9; ++redCount) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
                Assert.AreEqual(cost[ResourceType.Red] - redCount - 1, upgraderToTest.GetResourcesNeededToUpgrade()[ResourceType.Red],
                    "Incorrect resources needed in Red placement iteration " + redCount);
            }
            for(int greenCount = 0; greenCount < 9; ++greenCount) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Green));
                Assert.AreEqual(cost[ResourceType.Green] - greenCount - 1, upgraderToTest.GetResourcesNeededToUpgrade()[ResourceType.Green],
                    "Incorrect resources needed in Green placement iteration " + greenCount);
            }
        }

        [Test]
        public void OnBlobsWithinContainsResourcesDefinedByCost_TargetedHighwayIsUpgradedWithNewProfile() {
            //Setup
            var blobSite = BuildBlobSite();
            var blobHighway = BuildBlobHighway();
            var cost = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)
            );
            var newProfile = new BlobHighwayProfile(1f, 20, cost);

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(newProfile);


            var upgraderToTest = BuildHighwayUpgrader(privateData);

            //Execution
            for(int i = 0; i < 10; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }

            //Validation
            Assert.AreEqual(blobHighway.Profile, newProfile);
        }

        [Test]
        public void OnBlobSiteContainsResourcesDefinedByCost_BlobSiteIsClearedOfAllCapacitiesAndPermissions() {
            //Setup
            var blobSite = BuildBlobSite();

            var blobHighway = BuildBlobHighway();
            var cost = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)
            );
            var newProfile = new BlobHighwayProfile(1f, 20, cost);

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(newProfile);


            var upgraderToTest = BuildHighwayUpgrader(privateData);

            //Execution
            for(int i = 0; i < 10; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.IsFalse(blobSite.GetPlacementPermissionForResourceType(resourceType), 
                    "UnderlyingSite falsely permits placement of resourceType " + resourceType);
                Assert.IsFalse(blobSite.GetExtractionPermissionForResourceType(resourceType),
                    "UnderlyingSite falsely permits extraction of resourceType " + resourceType);
                Assert.AreEqual(0, blobSite.GetCapacityForResourceType(resourceType),
                    "UnderlyingSite has an incorrect capacity for resourceType " + resourceType);
            }
            Assert.AreEqual(0, blobSite.TotalCapacity, "UnderlyingSite has an incorrect TotalCapcity");
        }

        [Test]
        public void OnBlobSiteContainsResourcesDefinedByCost_BlobSiteIsClearedOfAllContents() {
            //Setup
            var blobSite = BuildBlobSite();

            var blobHighway = BuildBlobHighway();
            var cost = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)
            );
            var newProfile = new BlobHighwayProfile(1f, 20, cost);

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(newProfile);


            var upgraderToTest = BuildHighwayUpgrader(privateData);

            bool hasBeenCleared = false;
            blobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                hasBeenCleared = true;
            };

            //Execution
            for(int i = 0; i < 10; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }

            //Validation
            Assert.That(hasBeenCleared);
        }

        #endregion

        #endregion

        #region utilities

        private HighwayUpgraderFactoryBase BuildUpgraderFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<HighwayUpgraderFactory>();
        }

        private HighwayUpgraderPrivateData BuildUpgraderPrivateData() {
            var hostingObject = new GameObject();
            var newData = hostingObject.AddComponent<HighwayUpgraderPrivateData>();
            newData.SetSourceFactory(BuildUpgraderFactory());
            return newData;
        }

        private HighwayUpgrader BuildHighwayUpgrader(HighwayUpgraderPrivateDataBase privateData) {
            var hostingObject = new GameObject();
            var newUpgrader = hostingObject.AddComponent<HighwayUpgrader>();
            newUpgrader.PrivateData = privateData;
            return newUpgrader;
        }

        private BlobHighwayBase BuildBlobHighway() {
            var hostingObject = new GameObject();
            var newHighway = hostingObject.AddComponent<MockBlobHighway>();
            return newHighway;
        }

        private BlobSiteBase BuildBlobSite() {
            var hostingObject = new GameObject();
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            return newBlobSite;
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


