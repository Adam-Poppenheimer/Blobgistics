using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Blobs;
using Assets.ConstructionZones.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.ConstructionZones.Editor {

    public class ConstructionZoneTests {

        #region static fields and properties

        private static readonly string StandardProjectName = "Resource Depot";

        #endregion

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnProjectCompleted_ProjectHasBeenCompletedSetToTrue_AndIsFalseOtherwise() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var locationToUse = BuildMapNode();

            var easyProjectHost = new GameObject();
            var easyProject = easyProjectHost.AddComponent<MockConstructionProject>();
            easyProject.SiteContainsNecessaryResources = true;
            easyProject.name = "Easy Project";

            factoryToUse.AvailableProjects = new List<ConstructionProjectBase>() { easyProject };

            var siteToTest = factoryToUse.BuildConstructionZone(locationToUse, easyProject);

            //Execution and Validation
            Assert.IsFalse(siteToTest.ProjectHasBeenCompleted, "Project falsely registers itself as having been completed");
            locationToUse.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            //Validation
            Assert.That(siteToTest.ProjectHasBeenCompleted, "Project fails to indicate that it has been completed");
        }

        [Test]
        public void OnConstructionZoneBuiltViaFactory_ProjectIsInstructedToSetSite() {
            //Setup
            var hostingObject = new GameObject();
            var factoryToUse = hostingObject.AddComponent<ConstructionZoneFactory>();
            var locationToUse = BuildMapNode();

            var projectToUse = hostingObject.AddComponent<MockConstructionProject>();
            bool siteWasSet = false;
            projectToUse.SiteSet += delegate(object sender, EventArgs e) {
                siteWasSet = true;
            };

            //Execution
            var newConstructionZone = factoryToUse.BuildConstructionZone(locationToUse, projectToUse);

            //Validation
            Assert.That(siteWasSet);
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
            factoryToUse.TryGetProjectOfName(StandardProjectName, out project);
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
            factoryToTest.TryGetProjectOfName(StandardProjectName, out project);

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
            factoryToTest.TryGetProjectOfName(StandardProjectName, out project);

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
            factoryToTest.TryGetProjectOfName(StandardProjectName, out project);

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
        public void OnBlobPushedIntoUnderlyingBlobSite_AndContentsMeetConstructionConditions_ExecuteBuildIsCalledOnProject() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var locationToUse = BuildMapNode();

            var easyProjectHost = new GameObject();
            var easyProject = easyProjectHost.AddComponent<MockConstructionProject>();
            easyProject.SiteContainsNecessaryResources = true;
            easyProject.name = "Easy Project";

            bool executeBuildWasCalled = false;
            easyProject.BuildExecuted += delegate(object sender, EventArgs e) {
                executeBuildWasCalled = true;
            };

            factoryToUse.AvailableProjects = new List<ConstructionProjectBase>() { easyProject };

            var siteToTest = factoryToUse.BuildConstructionZone(locationToUse, easyProject);

            //Execution
            locationToUse.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            //Validation
            Assert.That(executeBuildWasCalled);
        }

        [Test]
        public void OnBlobPushedIntoUnderlyingBlobSite_AndContentsMeetConstructionConditions_BlobSiteIsClearedOfContents() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var locationToUse = BuildMapNode();
            
            bool siteWasCleared = false;
            locationToUse.BlobSite.AllBlobsCleared += delegate(object sender, EventArgs e) {
                siteWasCleared = true;
            };

            var easyProjectHost = new GameObject();
            var easyProject = easyProjectHost.AddComponent<MockConstructionProject>();
            easyProject.SiteContainsNecessaryResources = true;
            easyProject.name = "Easy Project";

            factoryToUse.AvailableProjects = new List<ConstructionProjectBase>() { easyProject };

            var siteToTest = factoryToUse.BuildConstructionZone(locationToUse, easyProject);

            //Execution
            locationToUse.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            //Validation
            Assert.That(siteWasCleared);
        }

        [Test]
        public void OnBlobPushedIntoUnderlyingBlobSite_AndContentsMeetConstructionConditions_BlobSiteIsClearedOfPermissionsAndCapacities() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();
            var locationToUse = BuildMapNode();
            var mockBlobSite = locationToUse.BlobSite as MockBlobSite;

            bool permissionsAndCapacitiesWereCleared = false;
            mockBlobSite.PermissionsAndCapacitiesCleared += delegate(object sender, EventArgs e) {
                permissionsAndCapacitiesWereCleared = true;
            };

            var easyProjectHost = new GameObject();
            var easyProject = easyProjectHost.AddComponent<MockConstructionProject>();
            easyProject.SiteContainsNecessaryResources = true;
            easyProject.name = "Easy Project";

            factoryToUse.AvailableProjects = new List<ConstructionProjectBase>() { easyProject };

            var siteToTest = factoryToUse.BuildConstructionZone(locationToUse, easyProject);

            //Execution
            locationToUse.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            //Validation
            Assert.That(permissionsAndCapacitiesWereCleared);
        }

        [Test]
        public void Factory_OnTryGetProjectOfNameCalled_ProjectSetToCorrectValueAndMethodReturnsTrue_OrProjectSetToNullAndMethodReturnsFalse() {
            //Setup
            var hostingObject = new GameObject();
            var factoryToTest = hostingObject.AddComponent<ConstructionZoneFactory>();
            
            var project1Host = new GameObject();
            var project1 = project1Host.AddComponent<MockConstructionProject>();
            project1.name = "Project 1";

            var project2Host = new GameObject();
            var project2 = project2Host.AddComponent<MockConstructionProject>();
            project2.name = "Project 2";

            var project3Host = new GameObject();
            var project3 = project3Host.AddComponent<MockConstructionProject>();
            project3.name = "Project 3";

            var project4Host = new GameObject();
            var project4 = project4Host.AddComponent<MockConstructionProject>();
            project4.name = "Project 4";

            factoryToTest.AvailableProjects = new List<ConstructionProjectBase>() {
                project1, project2, project3, project4
            };

            //Execution
            ConstructionProjectBase outForProject1, outForProject2, outForProject3, outForProject4, outForArgleBargle;
            bool hasSomeProject1 = factoryToTest.TryGetProjectOfName(project1.name, out outForProject1);
            bool hasSomeProject2 = factoryToTest.TryGetProjectOfName(project2.name, out outForProject2);
            bool hasSomeProject3 = factoryToTest.TryGetProjectOfName(project3.name, out outForProject3);
            bool hasSomeProject4 = factoryToTest.TryGetProjectOfName(project4.name, out outForProject4);
            bool hasSomeProjectArgleBargle = factoryToTest.TryGetProjectOfName("ArgleBargle", out outForArgleBargle);

            //Validation
            Assert.That(hasSomeProject1, "Failed to recognize the existence of project1");
            Assert.That(hasSomeProject2, "Failed to recognize the existence of project2");
            Assert.That(hasSomeProject3, "Failed to recognize the existence of project3");
            Assert.That(hasSomeProject4, "Failed to recognize the existence of project4");
            Assert.IsFalse(hasSomeProjectArgleBargle, "Falsely recognized the existence of ArgleBargle");

            Assert.AreEqual(project1, outForProject1, "OutForProject1 has incorrect value");
            Assert.AreEqual(project2, outForProject2, "OutForProject2 has incorrect value");
            Assert.AreEqual(project3, outForProject3, "OutForProject3 has incorrect value");
            Assert.AreEqual(project4, outForProject4, "OutForProject4 has incorrect value");
            Assert.Null(outForArgleBargle, "OutForArgleBargle is not null");
        }

        [Test]
        public void Factory_OnGetAllProjectsCalled_AllProjectsReturned() {
            //Setup
            var hostingObject = new GameObject();
            var factoryToTest = hostingObject.AddComponent<ConstructionZoneFactory>();
            
            var project1 = hostingObject.AddComponent<MockConstructionProject>();
            project1.name = "Project 1";
            var project2 = hostingObject.AddComponent<MockConstructionProject>();
            project1.name = "Project 2";
            var project3 = hostingObject.AddComponent<MockConstructionProject>();
            project1.name = "Project 3";
            var project4 = hostingObject.AddComponent<MockConstructionProject>();
            project1.name = "Project 4";

            factoryToTest.AvailableProjects = new List<ConstructionProjectBase>() {
                project1, project2, project3, project4
            };

            //Execution
            var allProjectsReturned = factoryToTest.GetAvailableProjects();

            //Validation
            Assert.That(allProjectsReturned.Contains(project1), "AllProjectsReturned does not contain project1");
            Assert.That(allProjectsReturned.Contains(project2), "AllProjectsReturned does not contain project2");
            Assert.That(allProjectsReturned.Contains(project3), "AllProjectsReturned does not contain project3");
            Assert.That(allProjectsReturned.Contains(project4), "AllProjectsReturned does not contain project4");
        }

        [Test]
        public void Factory_OnCanBuildConstructionZoneCalled_ReturnsFalseIfAZoneAlreadyExistsAtThatLocation_AndTrueOtherwise() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var projectToUse = factoryToTest.GetAvailableProjects().First();

            var location1 = BuildMapNode();
            var location2 = BuildMapNode();
            var location3 = BuildMapNode();

            factoryToTest.BuildConstructionZone(location1, projectToUse);

            //Execution and Validation
            Assert.IsFalse(factoryToTest.CanBuildConstructionZone(location1, projectToUse), "Factory falsely permits another zone on location1");
            Assert.IsTrue(factoryToTest.CanBuildConstructionZone(location2, projectToUse), "Factory fails to permit a zone on location2");
            Assert.IsTrue(factoryToTest.CanBuildConstructionZone(location3, projectToUse), "Factory fails to permit a zone on location3");
        }

        #endregion

        #region error checking

        [Test]
        public void Factory_OnBuildConstructionSitePassedNullMapNode_ThrowsArgumentNullException() {
            //Setup
            var factoryToUse = BuildConstructionZoneFactory();

            //Execution and Validation
            
            ConstructionProjectBase project;
            factoryToUse.TryGetProjectOfName(StandardProjectName, out project);
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToUse.BuildConstructionZone(null, project);
            });
        }

        [Test]
        public void Factory_OnBuildConstructionSitePassedNullConstructionProject_ThrowsArgumentNullException() {
            //Setup
            var newLocation = BuildMapNode();
            var factoryToUse = BuildConstructionZoneFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToUse.BuildConstructionZone(newLocation, null);
            });
        }

        [Test]
        public void Factory_OnDestroyConstructionSitePassedNullConstructionSite_ThrowsArgumentNullException() {
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
            factoryToUse.TryGetProjectOfName(StandardProjectName, out project);
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
        public void Factory_OnCanBuildConstructionZoneReturnsFalse_AndBuildConstructionZoneIsCalled_ThrowsConstructionZoneException() {
            //Setup
            var factoryToTest = BuildConstructionZoneFactory();
            var location = BuildMapNode();

            ConstructionProjectBase project;
            factoryToTest.TryGetProjectOfName(StandardProjectName, out project);
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
            var newFactory = hostingObject.AddComponent<ConstructionZoneFactory>();

            var defaultProject = hostingObject.AddComponent<MockConstructionProject>();
            defaultProject.name = StandardProjectName;

            newFactory.AvailableProjects = new List<ConstructionProjectBase>() { defaultProject };

            return newFactory;
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


