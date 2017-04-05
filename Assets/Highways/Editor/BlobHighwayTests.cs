using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.Highways.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways.Editor {

    public class BlobHighwayTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnPrivateDataInitialized_FirstAndSecondEndpointAreSetProperly() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var privateData = BuildHighwayPrivateData();
            privateData.SetFirstEndpoint(firstEndpoint);
            privateData.SetSecondEndpoint(secondEndpoint);
            privateData.SetTubePullingFromFirstEndpoint(BuildBlobTube());
            privateData.SetTubePullingFromSecondEndpoint(BuildBlobTube());

            //Execution
            var highwayToTest = BuildHighway(privateData);

            //Validation
            Assert.AreEqual(firstEndpoint, highwayToTest.FirstEndpoint, "First endpoint is incorrect");
            Assert.AreEqual(secondEndpoint, highwayToTest.SecondEndpoint, "Second endpoint is incorrect");
        }

        [Test]
        public void OnPrivateDataInitialized_TubesWithinHaveCorrectEndpoints() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            firstEndpoint.transform.position = Vector3.zero;
            secondEndpoint.transform.position = new Vector3(10, 0f, 0f);

            //Execution
            BuildHighway(highwayData);

            //Validation
            Assert.AreEqual(new Vector3(1f, BlobHighway.TubeCenterOffset, 0f), tubePullingFromFirst.SourceLocation,
                "tubePullingFromFirst has incorrect SourceLocation");

            Assert.AreEqual(new Vector3(9f, BlobHighway.TubeCenterOffset, 0f), tubePullingFromFirst.TargetLocation,
                "tubePullingFromFirst has incorrect TargetLocation");

            Assert.AreEqual(new Vector3(9f, -BlobHighway.TubeCenterOffset, 0f), tubePullingFromSecond.SourceLocation,
                "tubePullingFromSecond has incorrect SourceLocation");

            Assert.AreEqual(new Vector3(1f, -BlobHighway.TubeCenterOffset, 0f), tubePullingFromSecond.TargetLocation,
                "tubePullingFromSecond has incorrect TargetLocation");
        }

        [Test]
        public void OnHighwayInitialized_AllPermissionsDefaultToFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var privateData = BuildHighwayPrivateData();
            privateData.SetFirstEndpoint(firstEndpoint);
            privateData.SetSecondEndpoint(secondEndpoint);
            privateData.SetTubePullingFromFirstEndpoint(BuildBlobTube());
            privateData.SetTubePullingFromSecondEndpoint(BuildBlobTube());

            //Execution
            var highwayToTest = BuildHighway(privateData);

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(!highwayToTest.GetPullingPermissionForFirstEndpoint(resourceType),
                    "FirstEndpoint is given permission to pull resource " + resourceType);

                Assert.That(!highwayToTest.GetPullingPermissionForSecondEndpoint(resourceType),
                    "SecondEndpoint is given permission to pull resource " + resourceType);
            }
        }

        [Test]
        public void OnHighwayInitialized_PriorityDefaultsToMaxValue() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var privateData = BuildHighwayPrivateData();
            privateData.SetFirstEndpoint(firstEndpoint);
            privateData.SetSecondEndpoint(secondEndpoint);
            privateData.SetTubePullingFromFirstEndpoint(BuildBlobTube());
            privateData.SetTubePullingFromSecondEndpoint(BuildBlobTube());

            //Execution
            var highwayToTest = BuildHighway(privateData);

            //Validation
            Assert.AreEqual(Int32.MaxValue, highwayToTest.Priority);
        }

        [Test]
        public void Factory_OnManyHighwaysCreatedAndDestroyed_NoTwoActiveFactoriesEverHaveTheSameID() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var nodeList = new List<KeyValuePair<MapNodeBase, MapNodeBase>>();
            for(int nodeIndex = 0; nodeIndex < 60; ++nodeIndex) {
                var firstNode = mapGraph.BuildNode(Vector3.zero);
                var secondNode = mapGraph.BuildNode(Vector3.left);
                mapGraph.AddUndirectedEdge(firstNode, secondNode);
                nodeList.Add(new KeyValuePair<MapNodeBase, MapNodeBase>(firstNode, secondNode));
            }

            var highwayList = new List<BlobHighwayBase>();

            //Execution and Validation
            int i = 0;
            for(; i < 50; ++i) {
                highwayList.Add(factoryToTest.ConstructHighwayBetween(nodeList[i].Key, nodeList[i].Value));
                foreach(var outerHighway in highwayList) {
                    foreach(var innerHighway in highwayList) {
                        if(innerHighway != outerHighway) {
                            Assert.AreNotEqual(innerHighway.ID, outerHighway.ID, "Duplicate IDs on first creation cycle on index " + i);
                        }
                    }
                }
            }
            for(i = 34; i >= 10; --i) {
                var highwayToDestroy = highwayList[i];
                highwayList.Remove(highwayToDestroy);
                factoryToTest.DestroyHighway(highwayToDestroy);
            }
            for(i = 10; i < 35; ++i) {
                highwayList.Add(factoryToTest.ConstructHighwayBetween(nodeList[i].Key, nodeList[i].Value));
                foreach(var outerHighway in highwayList) {
                    foreach(var innerHighway in highwayList) {
                        if(innerHighway != outerHighway) {
                            Assert.AreNotEqual(innerHighway.ID, outerHighway.ID, "Duplicate IDs on second creation cycle on index " + i);
                        }
                    }
                }
            }
        }

        [Test]
        public void Factory_OnGetHighwayOfIDCalled_HighwayOfThatIDIsReturned_OrNullIfNoneExists() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            var highways = new List<BlobHighwayBase>() {
                factoryToTest.ConstructHighwayBetween(middleNode, leftNode),
                factoryToTest.ConstructHighwayBetween(middleNode, rightNode),
                factoryToTest.ConstructHighwayBetween(middleNode, upNode),
            };

            //Execution


            //Validation
            Assert.AreEqual(highways[0], factoryToTest.GetHighwayOfID(highways[0].ID), "Did not return highways[0] when passed its ID");
            Assert.AreEqual(highways[1], factoryToTest.GetHighwayOfID(highways[1].ID), "Did not return highways[1] when passed its ID");
            Assert.AreEqual(highways[2], factoryToTest.GetHighwayOfID(highways[2].ID), "Did not return highways[2] when passed its ID");
            Assert.IsNull(factoryToTest.GetHighwayOfID(Int32.MaxValue), "An expected invalid ID did not return a null value");
        }

        [Test]
        public void Factory_OnHasHighwayBetweenCalled_ReturnsTrueIfAndOnlyIfSomeHighwayConnectsThoseMapNodes() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            factoryToTest.ConstructHighwayBetween(middleNode, leftNode);
            factoryToTest.ConstructHighwayBetween(middleNode, rightNode);

            //Execution


            //Validation
            Assert.IsTrue(factoryToTest.HasHighwayBetween(middleNode, leftNode),  "Does not register highway between middleNode and leftNode");
            Assert.IsTrue(factoryToTest.HasHighwayBetween(middleNode, rightNode), "Does not register highway between middleNode and rightNode");
            Assert.IsFalse(factoryToTest.HasHighwayBetween(middleNode, upNode), "Falsely registers highway between middleNode and upNode");
        }

        [Test]
        public void Factory_OnGetHighwayBetweenCalled_ReturnedHighwayHasSameEndpointsAsArgumentsPassed() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            var highwayToLeft = factoryToTest.ConstructHighwayBetween(middleNode, leftNode);
            var highwayToRight = factoryToTest.ConstructHighwayBetween(middleNode, rightNode);
            var highwayToUp = factoryToTest.ConstructHighwayBetween(middleNode, upNode);

            //Execution


            //Validation
            Assert.AreEqual(highwayToLeft, factoryToTest.GetHighwayBetween(middleNode, leftNode),   "Factory retrieves the wrong highway between middleNode and leftNode");
            Assert.AreEqual(highwayToRight, factoryToTest.GetHighwayBetween(middleNode, rightNode), "Factory retrieves the wrong highway between middleNode and rightNode");
            Assert.AreEqual(highwayToUp, factoryToTest.GetHighwayBetween(middleNode, upNode),       "Factory retrieves the wrong highway between middleNode and upNode");
        }

        [Test]
        public void Factory_OnCanConstructHighwayBetweenCalled_ReturnsFalseIfHasHighwayBetweenIsTrue() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            factoryToTest.ConstructHighwayBetween(middleNode, leftNode);

            //Execution


            //Validation
            Assert.IsFalse(factoryToTest.CanConstructHighwayBetween(middleNode, leftNode), "Factory falsely permits construction between middleNode and leftNode");
        }

        [Test]
        public void Factory_OnConstructHighwayBetweenCalled_NewHighwayHasCorrectEndpoints() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.AddUndirectedEdge(middleNode, leftNode);
            mapGraph.AddUndirectedEdge(middleNode, rightNode);
            mapGraph.AddUndirectedEdge(middleNode, upNode);

            //Execution
            var highwayToLeft = factoryToTest.ConstructHighwayBetween(middleNode, leftNode);

            //Validation
            Assert.That(
                (highwayToLeft.FirstEndpoint == middleNode && highwayToLeft.SecondEndpoint == leftNode  ) ||
                (highwayToLeft.FirstEndpoint == leftNode   && highwayToLeft.SecondEndpoint == middleNode)
            );
        }

        [Test]
        public void OnPermissionForResourceTypeSet_SamePermissionForResourceTypeIsGotten() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var privateData = BuildHighwayPrivateData();
            privateData.SetFirstEndpoint(firstEndpoint);
            privateData.SetSecondEndpoint(secondEndpoint);
            privateData.SetTubePullingFromFirstEndpoint(BuildBlobTube());
            privateData.SetTubePullingFromSecondEndpoint(BuildBlobTube());

            var highwayToTest = BuildHighway(privateData);

            //Execution
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Yellow, true);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, false);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Yellow, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.White, true);

            //Validation
            Assert.That(highwayToTest.GetPullingPermissionForFirstEndpoint(ResourceType.Food),   "For FirstEndpoint, Food is not permitted");
            Assert.That(highwayToTest.GetPullingPermissionForFirstEndpoint(ResourceType.Yellow), "For FirstEndpoint, Yellow is not permitted");
            Assert.That(!highwayToTest.GetPullingPermissionForFirstEndpoint(ResourceType.White), "For FirstEndpoint, White is falsely permitted");

            Assert.That(!highwayToTest.GetPullingPermissionForSecondEndpoint(ResourceType.Food),  "For SecondEndpoint, Food is falsely permitted");
            Assert.That(highwayToTest.GetPullingPermissionForSecondEndpoint(ResourceType.Yellow), "For SecondEndpoint, Yellow is not permitted");
            Assert.That(highwayToTest.GetPullingPermissionForSecondEndpoint(ResourceType.White),  "For SecondEndpoint, White is not permitted");
        }

        [Test]
        public void OnPermissionSetForFirstEndpoint_TubePullingFromFirstEndpointAlsoHasNewPermission() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            //Execution
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Yellow, false);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.White, true);

            //Validation
            Assert.That(tubePullingFromFirst.GetPermissionForResourceType(ResourceType.Food),    "Tube does not give Food permission");
            Assert.That(!tubePullingFromFirst.GetPermissionForResourceType(ResourceType.Yellow), "Tube does falsely gives Yellow permission");
            Assert.That(tubePullingFromFirst.GetPermissionForResourceType(ResourceType.White),   "Tube does not give White permission");
        }

        [Test]
        public void OnPermissionSetForSecondEndpoint_TubePullingFromSecondEndpointAlsoHasNewPermission() {
            //Setup
            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);
            highwayData.SetFirstEndpoint(BuildMapNode());
            highwayData.SetSecondEndpoint(BuildMapNode());

            var highwayToTest = BuildHighway(highwayData);

            //Execution
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Yellow, false);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.White, true);

            //Validation
            Assert.That(tubePullingFromSecond.GetPermissionForResourceType(ResourceType.Food),    "Tube does not give Food permission");
            Assert.That(!tubePullingFromSecond.GetPermissionForResourceType(ResourceType.Yellow), "Tube does falsely gives Yellow permission");
            Assert.That(tubePullingFromSecond.GetPermissionForResourceType(ResourceType.White),   "Tube does not give White permission");
        }

        [Test]
        public void FirstEndpointHasNoExtractableTypes_CanPullFromFirstEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            Assert.AreEqual(0, firstEndpoint.BlobSite.GetExtractableTypes().Count(), "FirstEndpoint has extractable types");

            //Execution
            

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromFirstEndpoint(), "Can still pull from FirstEndpoint");
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_AndSecondEndpointCanAcceptPlacementOfOneOfThem_AndHighwayHasPermissions_CanPullFromFirstEndpointReturnsTrue() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, false);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Yellow, true);

            //Validation
            Assert.That(highwayToTest.CanPullFromFirstEndpoint());
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_AndSecondEndpointCanAcceptPlacementOfOneOfThem_ButHighwayLacksPermissions_CanPullFromFirstEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, false);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromFirstEndpoint());
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_ButSecondEndpointCannotAcceptAnyOfThem_CanPullFromFirstEndpointReturnFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, false);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Yellow, true);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromFirstEndpoint());
        }

        [Test]
        public void SecondEndpointHasNoExtractableTypes_CanPullFromSecondEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            Assert.AreEqual(0, secondEndpoint.BlobSite.GetExtractableTypes().Count(), "SecondEndpoint has extractable types");

            //Execution
            

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromSecondEndpoint(), "Can still pull from SecondEndpoint");
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_AndFirstEndpointCanAcceptPlacementOfOneOfThem_AndHighwayHasPermissions_CanPullFromSecondEndpointReturnsTrue() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Yellow, true);

            //Validation
            Assert.That(highwayToTest.CanPullFromSecondEndpoint());
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_AndFirstEndpointCanAcceptPlacementOfOneOfThem_ButHighwayLacksPermissions_CanPullFromSecondEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, false);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromSecondEndpoint());
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_ButFirstEndpointCannotAcceptAnyOfThem_CanPullFromSecondEndpointReturnFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, false);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Yellow, true);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromSecondEndpoint());
        }
        
        [Test]
        public void OnPullFromFirstEndpointCalled_AndPullFromFirstEndpointReturnsTrue_AResourceIsRemovedFromFirstEndpoint_ThatCanBePlacedIntoSecondEndpoint() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirstEndpoint = BuildBlobTube();
            var tubePullingFromSecondEndpoint = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirstEndpoint);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecondEndpoint);

            var highwayToTest = BuildHighway(highwayData);

            var redBlob = BuildBlob(ResourceType.Food);
            var greenBlob = BuildBlob(ResourceType.Yellow);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(redBlob);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.PlaceBlobInto(greenBlob);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Yellow, true);

            //Execution
            highwayToTest.PullFromFirstEndpoint();

            //Validation
            Assert.That(firstEndpoint.BlobSite.Contents.Contains(redBlob),    "Food blob has been removed from FirstEndpoint");
            Assert.That(!firstEndpoint.BlobSite.Contents.Contains(greenBlob), "Yellow blob has not been removed from FirstEndpoint");
        }

        [Test]
        public void OnPullFromSecondEndpointCalled_AndCanPullFromSecondEndpointReturnsTrue_AResourceIsRemovedFromSecondEndpoint_ThatCanBePlacedIntoFirstEndpoint() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(BuildBlobTube());
            highwayData.SetTubePullingFromSecondEndpoint(BuildBlobTube());

            var highwayToTest = BuildHighway(highwayData);

            var redBlob = BuildBlob(ResourceType.Food);
            var greenBlob = BuildBlob(ResourceType.Yellow);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(redBlob);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.PlaceBlobInto(greenBlob);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Yellow, true);

            //Execution
            highwayToTest.PullFromSecondEndpoint();

            //Validation
            Assert.That(secondEndpoint.BlobSite.Contents.Contains(redBlob),    "Food blob has been removed from SecondEndpoint");
            Assert.That(!secondEndpoint.BlobSite.Contents.Contains(greenBlob), "Yellow blob has not been removed from SecondEndpoint");
        }

        [Test]
        public void OnResourcePulledFromFirstEndpoint_ResourceIsPlacedIntoAppropriateTube() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            var redBlob = BuildBlob(ResourceType.Food);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(redBlob);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);

            //Execution
            highwayToTest.PullFromFirstEndpoint();

            //Validation
            Assert.Contains(redBlob, tubePullingFromFirst.Contents);
        }

        [Test]
        public void OnResourcePulledFromSecondEndpoint_ResourceIsPlacedIntoAppropriateTube() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            var redBlob = BuildBlob(ResourceType.Food);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(redBlob);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);

            //Execution
            highwayToTest.PullFromSecondEndpoint();

            //Validation
            Assert.Contains(redBlob, tubePullingFromSecond.Contents);
        }

        [Test]
        public void OnProfileChanged_BlobSpeedPerSecondChangesForBothTubes() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            //Execution
            highwayToTest.Profile = new BlobHighwayProfile(5, 20, ResourceSummary.BuildResourceSummary(highwayToTest.gameObject));

            //Validation
            Assert.AreEqual(5, tubePullingFromFirst.TransportSpeedPerSecond, "tubePullingFromFirst has incorrect BlobSpeedPerSecond");
            Assert.AreEqual(5, tubePullingFromSecond.TransportSpeedPerSecond, "tubePullingFromSecond has incorrect BlobSpeedPerSecond");

            Assert.AreEqual(20, tubePullingFromFirst.Capacity, "tubePullingFromFirst has incorrect Capacity");
            Assert.AreEqual(20, tubePullingFromSecond.Capacity, "tubePullingFromSecond has incorrect Capacity");
        }

        [Test]
        public void OnClearCalled_AllTubesAreClearedAsWell() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Yellow, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Yellow));

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Yellow, true);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Yellow, true);

            highwayToTest.PullFromFirstEndpoint();
            highwayToTest.PullFromFirstEndpoint();

            highwayToTest.PullFromSecondEndpoint();
            highwayToTest.PullFromSecondEndpoint();

            //Execute
            highwayToTest.Clear();

            //Validation
            Assert.AreEqual(0, tubePullingFromFirst.Contents.Count, "tubePullingFromFirst still has blobs within it");
            Assert.AreEqual(0, tubePullingFromSecond.Contents.Count, "tubePullingFromSecond still has blobs within it");
        }

        #endregion

        #region error checking

        [Test]
        public void IfCanPullFromFirstEndpointIsFalse_AndPullFromFirstEndpointIsCalled_ThrowsBlobHighwayException() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            //Execution and Validation
            Assert.Throws<BlobHighwayException>(delegate() {
                highwayToTest.PullFromFirstEndpoint();
            });
        }

        [Test]
        public void IfCanPullFromSecondEndpointIsFalse_AndPullFromSecondEndpointIsCalled_ThrowsBlobHighwayException() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayData = BuildHighwayPrivateData();
            highwayData.SetFirstEndpoint(firstEndpoint);
            highwayData.SetSecondEndpoint(secondEndpoint);
            highwayData.SetTubePullingFromFirstEndpoint(tubePullingFromFirst);
            highwayData.SetTubePullingFromSecondEndpoint(tubePullingFromSecond);

            var highwayToTest = BuildHighway(highwayData);

            //Execution and Validation
            Assert.Throws<BlobHighwayException>(delegate() {
                highwayToTest.PullFromSecondEndpoint();
            });
        }

        [Test]
        public void Factory_IfHasHighwayBetweenCalledWithNullArguments_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.HasHighwayBetween(null, secondEndpoint);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.HasHighwayBetween(firstEndpoint, null);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.HasHighwayBetween(null, null);
            });
        }

        [Test]
        public void Factory_IfGetHighwayBetweenCalledWithNullArguments_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetHighwayBetween(null, secondEndpoint);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetHighwayBetween(firstEndpoint, null);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetHighwayBetween(null, null);
            });
        }

        [Test]
        public void Factory_IfGetHighwayBetweenCalled_AndHasHighwayBetweenReturnsFalse_ThrowsBlobHighwayException() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            //Execution and Validation
            Assert.Throws<BlobHighwayException>(delegate() {
                factoryToTest.GetHighwayBetween(firstEndpoint, secondEndpoint);
            });
        }

        [Test]
        public void Factory_IfConstructHighwayBetweenCalled_AndCanConstructHighwayBetweenReturnsFalse_ThrowsBlobHighwayException() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var firstEndpoint = mapGraph.BuildNode(Vector3.zero);
            var secondEndpoint = mapGraph.BuildNode(Vector3.left);

            mapGraph.AddUndirectedEdge(firstEndpoint, secondEndpoint);

            factoryToTest.ConstructHighwayBetween(firstEndpoint, secondEndpoint);

            //Execution and Validation
            Assert.Throws<BlobHighwayException>(delegate() {
                factoryToTest.ConstructHighwayBetween(firstEndpoint, secondEndpoint);
            });
        }

        [Test]
        public void Factory_IfCanConstructHighwayBetweenCalledWithNullArguments_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructHighwayBetween(null, secondEndpoint);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructHighwayBetween(firstEndpoint, null);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructHighwayBetween(null, null);
            });
        }

        [Test]
        public void Factory_IfConstructHighwayBetweenCalledWithNullArguments_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructHighwayBetween(null, secondEndpoint);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructHighwayBetween(firstEndpoint, null);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructHighwayBetween(null, null);
            });
        }

        #endregion

        #endregion

        #region utility

        private MockMapNode BuildMapNode() {
            var hostingObject = new GameObject();

            var newBlobSiteData = hostingObject.AddComponent<MockBlobSitePrivateData>();
            newBlobSiteData.SetNorthConnectionOffset(Vector3.up);
            newBlobSiteData.SetSouthConnectionOffset(Vector3.down);
            newBlobSiteData.SetEastConnectionOffset (Vector3.right);
            newBlobSiteData.SetWestConnectionOffset (Vector3.left);

            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.PrivateData = newBlobSiteData;
            newBlobSite.TotalCapacity = Int32.MaxValue;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                newBlobSite.SetCapacityForResourceType(resourceType, Int32.MaxValue);
            }

            var newMapNode = hostingObject.AddComponent<MockMapNode>();
            newMapNode.SetBlobSite(newBlobSite);

            return newMapNode;
        }

        private MockBlobHighwayPrivateData BuildHighwayPrivateData() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobHighwayPrivateData>();
        }

        private BlobHighway BuildHighway(BlobHighwayPrivateDataBase privateData) {
            var hostingGameObject = new GameObject();
            var newHighway = hostingGameObject.AddComponent<BlobHighway>();
            newHighway.PrivateData = privateData;
            newHighway.Profile = new BlobHighwayProfile(1f, 10, ResourceSummary.BuildResourceSummary(newHighway.gameObject));
            return newHighway;
        }

        private ResourceBlobBase BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        private MockBlobTube BuildBlobTube() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobTube>();
        }

        private BlobHighwayFactory BuildHighwayFactory() {
            var hostingObject = new GameObject();

            var newBlobSiteFactory = hostingObject.AddComponent<BlobSiteFactory>();
            newBlobSiteFactory.BlobSitePrivateData = hostingObject.AddComponent<BlobSitePrivateData>();

            var newMapGraph = hostingObject.AddComponent<MapGraph>();
            newMapGraph.BlobSiteFactory = newBlobSiteFactory;

            var newTubeFactory = hostingObject.AddComponent<BlobTubeFactory>();
            var newTubePrivateData = hostingObject.AddComponent<BlobTubePrivateData>();
            newTubePrivateData.SetBlobFactory(hostingObject.AddComponent<MockResourceBlobFactory>());
            newTubeFactory.TubePrivateData = newTubePrivateData;

            var newHighwayFactory = hostingObject.AddComponent<BlobHighwayFactory>();
            newHighwayFactory.MapGraph = newMapGraph;
            newHighwayFactory.BlobTubeFactory = newTubeFactory;
            newHighwayFactory.StartingProfile = new BlobHighwayProfile(1, 10, ResourceSummary.BuildResourceSummary(newHighwayFactory.gameObject));

            return newHighwayFactory;
        }

        #endregion

        #endregion

    }

}
