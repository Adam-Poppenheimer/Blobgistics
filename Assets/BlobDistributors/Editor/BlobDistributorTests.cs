using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;
using Assets.BlobDistributors.ForTesting;

namespace Assets.BlobDistributors.Editor {

    public class BlobDistributorTests {

        #region instance methods

        #region tests

        [Test]
        public void OnDistributionIsPerformed_HigherPriorityHighwaysReceiveBlobsBeforeLowerPriorityOnes() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 1;
            highwayRight.Priority = 10;
            highwayUp.Priority = 100;

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            for(int i = 0; i < 6; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter6Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(6, leftCountAfter6Seconds,  "LeftCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter6Seconds, "RightCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter6Seconds,    "UpCountAfter6Seconds is incorrect");
        }

        [Test]
        public void OnDistributionIsPerformed_AndAHigherPriorityHighwayIsStillOnCooldown_LowerPriorityHighwaysCanReceiveBlobs() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 1;
            highwayRight.Priority = 10;
            highwayUp.Priority = 100;

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            for(int i = 0; i < 6; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
                Assert.AreEqual(i + 1, highwayLeft.ContentsPulledFromFirstEndpoint.Count, "Incorrect highwayLeft");
                Assert.AreEqual(i + 1, highwayRight.ContentsPulledFromFirstEndpoint.Count, "Incorrect highwayRight");
            }

            int leftCountAfter6Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(6, leftCountAfter6Seconds,  "LeftCountAfter6Seconds is incorrect");
            Assert.AreEqual(6, rightCountAfter6Seconds, "RightCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter6Seconds,    "UpCountAfter6Seconds is incorrect");
        }

        [Test]
        public void OnDistributionPerformed_HighwaysWithTheSamePriorityGetServedRoundRobin() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 1;
            highwayRight.Priority = 1;
            highwayUp.Priority = 10;

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            for(int i = 0; i < 6; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter6Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            for(int i = 0; i < 4; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter10Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter10Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter10Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(3, leftCountAfter6Seconds,  "LeftCountAfter6Seconds is incorrect");
            Assert.AreEqual(3, rightCountAfter6Seconds, "RightCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter6Seconds,    "UpCountAfter6Seconds is incorrect");

            Assert.AreEqual(5, leftCountAfter10Seconds,  "LeftCountAfter10Seconds is incorrect");
            Assert.AreEqual(5, rightCountAfter10Seconds, "RightCountAfter10Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter10Seconds,    "UpCountAfter10Seconds is incorrect");
        }

        [Test]
        public void OnSomeHighwayHasANewPriority_DistributionProperlyHandlesTheNewPriority() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 10;
            highwayRight.Priority = 100;
            highwayUp.Priority = 1000;

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            for(int i = 0; i < 3; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter3Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter3Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            highwayUp.Priority = 1;
            for(int i = 0; i < 3; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter6Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(3, leftCountAfter3Seconds,  "LeftCountAfter3Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter3Seconds, "RightCountAfter3Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter3Seconds,    "UpCountAfter3Seconds is incorrect");

            Assert.AreEqual(3, leftCountAfter6Seconds,  "LeftCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter6Seconds, "RightCountAfter6Seconds is incorrect");
            Assert.AreEqual(3, upCountAfter6Seconds,    "UpCountAfter6Seconds is incorrect");
        }

        [Test]
        public void OnHighwayFactoryCreatesANewHighway_AfterDistributorHasBeenInitialized_ThatHighwayIsConsideredInFutureDistributions() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayFactory.DestroyHighway(highwayUp);

            highwayLeft.Priority = 10;
            highwayRight.Priority = 100;

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            for(int i = 0; i < 3; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter3Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;

            highwayUp = highwayFactory.ConstructHighwayBetween(centerNode, upNode);
            highwayUp.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayUp.Priority = 1;

            for(int i = 0; i < 3; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
                distributorToTest.Tick(1f);
            }

            int leftCountAfter6Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(3, leftCountAfter3Seconds,  "LeftCountAfter3Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter3Seconds, "RightCountAfter3Seconds is incorrect");

            Assert.AreEqual(3, leftCountAfter6Seconds,  "LeftCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter6Seconds, "RightCountAfter6Seconds is incorrect");
            Assert.AreEqual(3, upCountAfter6Seconds,    "UpCountAfter6Seconds is incorrect");
        }

        [Test]
        public void OnHighwayFactoryDestroysAHighway_ThatHighwayIsNoLongerConsideredForDistribution_AndNoErrorIsThrown() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 1;
            highwayRight.Priority = 10;
            highwayUp.Priority = 100;

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            for(int i = 0; i < 3; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            }

            distributorToTest.Tick(3f);

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter3Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter3Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            highwayFactory.DestroyHighway(highwayLeft);
            
            for(int i = 0; i < 3; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            }

            distributorToTest.Tick(3f);

            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(3, leftCountAfter3Seconds,  "LeftCountAfter3Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter3Seconds, "RightCountAfter3Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter3Seconds,    "UpCountAfter3Seconds is incorrect");

            Assert.AreEqual(3, rightCountAfter6Seconds, "RightCountAfter6Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter6Seconds,    "UpCountAfter6Seconds is incorrect");
        }

        [Test]
        public void OnDistributionIsPerformed_HighwayEfficiencyModifiesPullCooldown() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 1;

            highwayRight.SetPullingPermissionForFirstEndpoint (ResourceType.Food, false);
            highwayRight.SetPullingPermissionForSecondEndpoint(ResourceType.Food, false);
            highwayUp.SetPullingPermissionForFirstEndpoint    (ResourceType.Food, false);
            highwayUp.SetPullingPermissionForSecondEndpoint   (ResourceType.Food, false);

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            centerNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            centerNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 50);
            centerNode.BlobSite.TotalCapacity = 100;
            for(int i = 0; i < 50; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            }

            distributorToTest.Tick(3f);

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;

            highwayLeft.Efficiency = 10f;

            distributorToTest.Tick(1.01f);

            int leftCountAfter6Seconds = highwayLeft.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(3,  leftCountAfter3Seconds, "LeftCountAfter3Seconds is incorrect");
            Assert.AreEqual(13, leftCountAfter6Seconds, "LeftCountAfter6Seconds is incorrect");
        }

        #endregion

        #region utilities

        private ComplexMockMapGraph BuildMapGraph() {
            var hostingObject = new GameObject();
            var newMapGraph = hostingObject.AddComponent<ComplexMockMapGraph>();
            newMapGraph.BlobFactory = hostingObject.AddComponent<MockResourceBlobFactory>();
            return newMapGraph;
        }

        private BlobDistributor BuildBlobDistributor() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<BlobDistributor>();
        }

        private ResourceBlobBase BuildResourceBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        private AccessingCountingMockMapGraph BuildAccessingCountingMockMapGraph() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<AccessingCountingMockMapGraph>();
        }

        private MockHighwayFactory BuildMockHighwayFactory() {
            var hostingObject = new GameObject();
            var newHighwayFactory = hostingObject.AddComponent<MockHighwayFactory>();
            newHighwayFactory.StartingProfile = BuildBlobHighwayProfile(1f, 20, 1f);
            return newHighwayFactory;
        }

        private void SetUpMapGraph(MapGraphBase mapGraph,out MapNodeBase centerNode,
            out MapNodeBase leftNode, out MapNodeBase rightNode, out MapNodeBase upNode) {
            centerNode = mapGraph.BuildNode(Vector3.zero);
            leftNode   = mapGraph.BuildNode(Vector3.left);
            rightNode  = mapGraph.BuildNode(Vector3.right);
            upNode     = mapGraph.BuildNode(Vector3.up);
            
            centerNode.name = "Center Node";
            centerNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            centerNode.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            centerNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            centerNode.BlobSite.TotalCapacity = 100;

            leftNode.name = "Left Node";
            leftNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            leftNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            leftNode.BlobSite.TotalCapacity = 100;

            rightNode.name = "Right Node";
            rightNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            rightNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            rightNode.BlobSite.TotalCapacity = 100;

            upNode.name = "Up Node";
            upNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            upNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            upNode.BlobSite.TotalCapacity = 100;

            mapGraph.AddUndirectedEdge(centerNode, leftNode);
            mapGraph.AddUndirectedEdge(centerNode, rightNode);
            mapGraph.AddUndirectedEdge(centerNode, upNode);
        }

        private void SetUpHighwayFactory(BlobHighwayFactoryBase highwayFactory,
            MapNodeBase centerNode, MapNodeBase leftNode, MapNodeBase rightNode, MapNodeBase upNode,
            out BlobHighwayBase highwayLeft, out BlobHighwayBase highwayRight, out BlobHighwayBase highwayUp) {

            highwayLeft  = highwayFactory.ConstructHighwayBetween(centerNode, leftNode);
            highwayRight = highwayFactory.ConstructHighwayBetween(centerNode, rightNode);
            highwayUp    = highwayFactory.ConstructHighwayBetween(centerNode, upNode);

            highwayLeft.SetPullingPermissionForFirstEndpoint (ResourceType.Food, true);
            highwayRight.SetPullingPermissionForFirstEndpoint (ResourceType.Food, true);
            highwayUp.SetPullingPermissionForFirstEndpoint (ResourceType.Food, true);
        }

        private BlobHighwayProfile BuildBlobHighwayProfile(float blobSpeedPerSecond, int capacity, float blobPullCooldownInSeconds) {
            var hostingObject = new GameObject();
            var newProfile = hostingObject.AddComponent<BlobHighwayProfile>();
            newProfile.SetBlobSpeedPerSecond(blobSpeedPerSecond);
            newProfile.SetCapacity(capacity);
            newProfile.SetBlobPullCooldownInSeconds(blobPullCooldownInSeconds);
            return newProfile;
        }

        #endregion

        #endregion

    }

}


