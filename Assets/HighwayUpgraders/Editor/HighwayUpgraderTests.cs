using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Highways;
using Assets.HighwayUpgraders.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.HighwayUpgraders.Editor {

    public class HighwayUpgraderTests {

        #region instance fields and properties

        private ResourceSummary PlaceholderCost {
            get {
                return ResourceSummary.BuildResourceSummary(
                    new GameObject(),
                    new KeyValuePair<ResourceType, int>(ResourceType.Food, Int32.MaxValue)
                );
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

            var profile = new BlobHighwayProfile(24f, 100, PlaceholderCost, 1f);

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

            var testProfile = new BlobHighwayProfile(24f, 100, ResourceSummary.BuildResourceSummary(
                factoryToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 15),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 12)
            ), 1f);

            //Execution
            var upgraderToTest = factoryToTest.BuildHighwayUpgrader(highway, site, testProfile);

            //Validation
            int intendedTotalCapacity = 0;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                int costCountOfType = upgraderToTest.ProfileToInsert.Cost[resourceType];
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

            var testProfile = new BlobHighwayProfile(24f, 100, PlaceholderCost, 1f);

            //Execution
            factoryToTest.BuildHighwayUpgrader(highway, site, testProfile);

            //Validation
            Assert.That(hasBeenCleared);
        }

        [Test]
        public void Factory_OnManyUpgradersCreatedAndDestroyed_NoTwoActiveUpgradersEverHaveTheSameID() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var blobSite = BuildBlobSite();
            var profile = new BlobHighwayProfile(0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f);

            var highwayList = new List<BlobHighwayBase>();
            for(int highwayCreateIndex = 0; highwayCreateIndex < 100; ++highwayCreateIndex) {
                highwayList.Add(BuildBlobHighway());
            }

            var upgraderList = new List<HighwayUpgraderBase>();

            //Execution and Validation
            int i = 0;
            for(; i < 50; ++i) {
                upgraderList.Add(factoryToTest.BuildHighwayUpgrader(highwayList[i], blobSite, profile));
                foreach(var outerUpgrader in upgraderList) {
                    foreach(var innerUpgrader in upgraderList) {
                        if(outerUpgrader != innerUpgrader) {
                            Assert.AreNotEqual(outerUpgrader.ID, innerUpgrader.ID, "Duplicate IDs on first creation cycle on index " + i);
                        }
                    }
                }
            }
            for(i = 34; i >= 10; --i) {
                var upgraderToDestroy = upgraderList[i];
                upgraderList.Remove(upgraderToDestroy);
                factoryToTest.DestroyHighwayUpgrader(upgraderToDestroy);
            }
            for(i = 10; i < 35; ++i) {
                upgraderList.Add(factoryToTest.BuildHighwayUpgrader(highwayList[i], blobSite, profile));
                foreach(var outerUpgrader in upgraderList) {
                    foreach(var innerUpgrader in upgraderList) {
                        if(outerUpgrader != innerUpgrader) {
                            Assert.AreNotEqual(outerUpgrader.ID, innerUpgrader.ID, "Duplicate IDs on second creation cycle on index " + i);
                        }
                    }
                }
            }
        }

        [Test]
        public void Factory_OnGetFactoryUpgraderOfIDCalled_UpgraderReturnedHasTheCorrectID_OrNullIfNoneExists() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var blobSite = BuildBlobSite();
            var blobHighways = new List<BlobHighwayBase>() {
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
            };

            var upgraders = new List<HighwayUpgraderBase>() {
                factoryToTest.BuildHighwayUpgrader(blobHighways[0], blobSite, new BlobHighwayProfile(
                    0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f)),
                factoryToTest.BuildHighwayUpgrader(blobHighways[1], blobSite, new BlobHighwayProfile(
                    0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f)),
                factoryToTest.BuildHighwayUpgrader(blobHighways[2], blobSite, new BlobHighwayProfile(
                    0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f)),
            };

            //Execution


            //Validation
            Assert.AreEqual(upgraders[0], factoryToTest.GetHighwayUpgraderOfID(upgraders[0].ID), "Did not return upgraders[0] when passed its ID");
            Assert.AreEqual(upgraders[1], factoryToTest.GetHighwayUpgraderOfID(upgraders[1].ID), "Did not return upgraders[1] when passed its ID");
            Assert.AreEqual(upgraders[2], factoryToTest.GetHighwayUpgraderOfID(upgraders[2].ID), "Did not return upgraders[2] when passed its ID");
            Assert.IsNull(factoryToTest.GetHighwayUpgraderOfID(Int32.MaxValue), "An expected invalid ID did not return a null value");
        }

        [Test]
        public void Factory_OnHasFactoryUpgraderTargetingHighwayCalled_ReturnsTrueOnlyIfSomeUpgraderTargetsThatHighway() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var blobSite = BuildBlobSite();
            var blobHighways = new List<BlobHighwayBase>() {
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
            };

            factoryToTest.BuildHighwayUpgrader(blobHighways[0], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            factoryToTest.BuildHighwayUpgrader(blobHighways[1], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            factoryToTest.BuildHighwayUpgrader(blobHighways[4], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));

            //Execution


            //Validation
            Assert.IsTrue(factoryToTest.HasUpgraderTargetingHighway(blobHighways[0]), "Did not register the existence of an upgrader on blobsHighways[0]");
            Assert.IsTrue(factoryToTest.HasUpgraderTargetingHighway(blobHighways[1]), "Did not register the existence of an upgrader on blobsHighways[1]");
            Assert.IsFalse(factoryToTest.HasUpgraderTargetingHighway(blobHighways[2]), "Falsely registered the existence of an upgrader on blobsHighways[2]");
            Assert.IsFalse(factoryToTest.HasUpgraderTargetingHighway(blobHighways[3]), "Falsely registered the existence of an upgrader on blobsHighways[3]");
            Assert.IsTrue(factoryToTest.HasUpgraderTargetingHighway(blobHighways[4]), "Did not register the existence of an upgrader on blobsHighways[4]");
        }

        [Test]
        public void Factory_OnGetFactoryUpgraderOnHighwayCalled_TheReturnedUpgraderTargetsTheSameHighway() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var blobSite = BuildBlobSite();
            var blobHighways = new List<BlobHighwayBase>() {
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
                BuildBlobHighway(),
            };

            factoryToTest.BuildHighwayUpgrader(blobHighways[0], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            factoryToTest.BuildHighwayUpgrader(blobHighways[1], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            factoryToTest.BuildHighwayUpgrader(blobHighways[2], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            factoryToTest.BuildHighwayUpgrader(blobHighways[3], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            factoryToTest.BuildHighwayUpgrader(blobHighways[4], blobSite, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));

            //Execution


            //Validation
            Assert.AreEqual(blobHighways[0], factoryToTest.GetUpgraderTargetingHighway(blobHighways[0]).TargetedHighway, "Incorrect TargetedHighway for blobHighways[0]");
            Assert.AreEqual(blobHighways[1], factoryToTest.GetUpgraderTargetingHighway(blobHighways[1]).TargetedHighway, "Incorrect TargetedHighway for blobHighways[1]");
            Assert.AreEqual(blobHighways[2], factoryToTest.GetUpgraderTargetingHighway(blobHighways[2]).TargetedHighway, "Incorrect TargetedHighway for blobHighways[2]");
            Assert.AreEqual(blobHighways[3], factoryToTest.GetUpgraderTargetingHighway(blobHighways[3]).TargetedHighway, "Incorrect TargetedHighway for blobHighways[3]");
            Assert.AreEqual(blobHighways[4], factoryToTest.GetUpgraderTargetingHighway(blobHighways[4]).TargetedHighway, "Incorrect TargetedHighway for blobHighways[4]");
        }

        [Test]
        public void OnBlobsPlacedWithinUnderlyingBlobSite_GetResourcesNeededToUpgradeIsAlwaysCorrect() {
            //Setup
            var blobSite = BuildBlobSite();
            var blobHighway = BuildBlobHighway();
            var cost = ResourceSummary.BuildResourceSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 10)
            );

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(new BlobHighwayProfile(1f, 20, cost, 1f));

            var upgraderToTest = BuildHighwayUpgrader(privateData);

            //Execution and Validation
            for(int redCount = 0; redCount < 9; ++redCount) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
                Assert.AreEqual(cost[ResourceType.Food] - redCount - 1, upgraderToTest.GetResourcesNeededToUpgrade()[ResourceType.Food],
                    "Incorrect resources needed in Food placement iteration " + redCount);
            }
            for(int greenCount = 0; greenCount < 9; ++greenCount) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));
                Assert.AreEqual(cost[ResourceType.Yellow] - greenCount - 1, upgraderToTest.GetResourcesNeededToUpgrade()[ResourceType.Yellow],
                    "Incorrect resources needed in Yellow placement iteration " + greenCount);
            }
        }

        [Test]
        public void OnBlobsWithinContainsResourcesDefinedByCost_TargetedHighwayIsUpgradedWithNewProfile() {
            //Setup
            var blobSite = BuildBlobSite();
            var blobHighway = BuildBlobHighway();
            var cost = ResourceSummary.BuildResourceSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            );
            var newProfile = new BlobHighwayProfile(1f, 20, cost, 1f);

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(newProfile);


            BuildHighwayUpgrader(privateData);

            //Execution
            for(int i = 0; i < 10; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }

            //Validation
            Assert.AreEqual(blobHighway.Profile, newProfile);
        }

        [Test]
        public void OnBlobSiteContainsResourcesDefinedByCost_BlobSiteIsClearedOfAllCapacitiesAndPermissions() {
            //Setup
            var blobSite = BuildBlobSite();

            var blobHighway = BuildBlobHighway();
            var cost = ResourceSummary.BuildResourceSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            );
            var newProfile = new BlobHighwayProfile(1f, 20, cost, 1f);

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(newProfile);


            BuildHighwayUpgrader(privateData);

            //Execution
            for(int i = 0; i < 10; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
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
            var cost = ResourceSummary.BuildResourceSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            );
            var newProfile = new BlobHighwayProfile(1f, 20, cost, 1f);

            var privateData = BuildUpgraderPrivateData();
            privateData.SetUnderlyingSite(blobSite);
            privateData.SetTargetedHighway(blobHighway);
            privateData.SetProfileToInsert(newProfile);


            BuildHighwayUpgrader(privateData);

            bool hasBeenCleared = false;
            blobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                hasBeenCleared = true;
            };

            //Execution
            for(int i = 0; i < 10; ++i) {
                blobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }

            //Validation
            Assert.That(hasBeenCleared);
        }

        #endregion

        #region error handling

        [Test]
        public void Factory_HasUpgraderTargetingHighwayPassedNullHighway_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.HasUpgraderTargetingHighway(null);
            });
        }

        [Test]
        public void Factory_GetUpgraderTargetingHighwayPassedNullHighway_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetUpgraderTargetingHighway(null);
            });
        }

        [Test]
        public void Factory_GetUpgraderTargetingHighwayCalledOnHighwayLackingUpgrader_ThrowsHighwayUpgraderException() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var highwayWithoutUpgrader = BuildBlobHighway();

            //Execution and Validation
            Assert.Throws<HighwayUpgraderException>(delegate() {
                factoryToTest.GetUpgraderTargetingHighway(highwayWithoutUpgrader);
            });
        }

        [Test]
        public void Factory_BuildHighwayUpgraderPassedNullValues_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var highway = BuildBlobHighway();
            var blobSite = BuildBlobSite();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.BuildHighwayUpgrader(null, blobSite, new BlobHighwayProfile(
                    0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.BuildHighwayUpgrader(highway, null, new BlobHighwayProfile(
                    0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            });
        }

        [Test]
        public void Factory_DestroyHighwayUpgraderPassedNullValue_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.DestroyHighwayUpgrader(null);
            });
        }

        [Test]
        public void Factory_OnHasUpgraderOnHighwayReturnsTrue_BuildHighwayUpgraderWithSameTargetedHighwayThrowsHighwayUpgraderException() {
            //Setup
            var factoryToTest = BuildUpgraderFactory();
            var highway = BuildBlobHighway();
            var blobSite1 = BuildBlobSite();
            var blobSite2 = BuildBlobSite();

            factoryToTest.BuildHighwayUpgrader(highway, blobSite1, new BlobHighwayProfile(
                0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));

            //Execution and Validation
            Assert.Throws<HighwayUpgraderException>(delegate() {
                factoryToTest.BuildHighwayUpgrader(highway, blobSite2, new BlobHighwayProfile(
                    0f, 0, ResourceSummary.BuildResourceSummary(factoryToTest.gameObject), 1f));
            });
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
            var newPrivateData = hostingObject.AddComponent<MockBlobSitePrivateData>();
            newPrivateData.SetBlobFactory(hostingObject.AddComponent<MockResourceBlobFactory>());

            newBlobSite.PrivateData = newPrivateData;

            return newBlobSite;
        }

        private ResourceBlobBase BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        #endregion

        #endregion

    }

}


