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
            throw new NotImplementedException();
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
            ConstructionProjectBase project;
            factoryToUse.TryGetProjectOfName("Resource Depot", out project);
            factoryToUse.BuildConstructionZone(mapNode, project);

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
            ConstructionProjectBase project;
            factoryToUse.TryGetProjectOfName("Resource Depot", out project);
            for(; i < 50; ++i) {
                zoneList.Add(factoryToUse.BuildConstructionZone(mapNodeList[i], project));
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
                zoneList.Add(factoryToUse.BuildConstructionZone(mapNodeList[i], project));
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

            ConstructionProjectBase project;
            factoryToTest.TryGetProjectOfName("Resource Depot", out project);

            factoryToTest.BuildConstructionZone(mapNodes[0], project);
            factoryToTest.BuildConstructionZone(mapNodes[1], project);
            factoryToTest.BuildConstructionZone(mapNodes[4], project);

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

            ConstructionProjectBase project;
            factoryToTest.TryGetProjectOfName("Resource Depot", out project);

            var zoneOnNode0 = factoryToTest.BuildConstructionZone(mapNodes[0], project);
            var zoneOnNode1 = factoryToTest.BuildConstructionZone(mapNodes[1], project);
            var zoneOnNode2 = factoryToTest.BuildConstructionZone(mapNodes[2], project);

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

            ConstructionProjectBase project;
            factoryToTest.TryGetProjectOfName("Resource Depot", out project);

            var zones = new List<ConstructionZoneBase>() {
                factoryToTest.BuildConstructionZone(mapNodes[0], project),
                factoryToTest.BuildConstructionZone(mapNodes[1], project),
                factoryToTest.BuildConstructionZone(mapNodes[2], project),
                factoryToTest.BuildConstructionZone(mapNodes[3], project),
                factoryToTest.BuildConstructionZone(mapNodes[4], project),
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
            throw new NotImplementedException();
        }

        [Test]
        public void OnUnderlyingBlobSiteMeetsConstructionConditions_ConstructionActionIsCalledCorrectly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnUnderlyingBlobSiteMeetsConstructionConditions_BlobSiteIsClearedOfContents() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnUnderlyingBlobSiteMeetsConstructionConditions_BlobSiteIsClearedOfPermissionsAndCapacities() {
            throw new NotImplementedException();
        }

        #endregion

        #region error checking

        [Test]
        public void OnFactorysBuildConstructionSitePassedNullMapNode_ThrowsArgumentNullException() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();

            //Execution and Validation
            
            ConstructionProjectBase project;
            factoryToUse.TryGetProjectOfName("Resource Depot", out project);
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToUse.BuildConstructionZone(null, project);
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

            ConstructionProjectBase project;
            factoryToUse.TryGetProjectOfName("Resource Depot", out project);
            var zoneToTest = factoryToUse.BuildConstructionZone(newLocation, project);

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
        public void Factory_OnHasConstructionZoneAtLocationIsTrue_AndBuildConstructionZoneCalledAtLocation_ThrowsConstrutionZoneException() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var location = BuildMapNode();

            ConstructionProjectBase project;
            factoryToTest.TryGetProjectOfName("Resource Depot", out project);
            factoryToTest.BuildConstructionZone(location, project);

            //Execution and Validation
            Assert.Throws<ConstructionZoneException>(delegate() {
                factoryToTest.BuildConstructionZone(location, project);
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


