using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Map;
using Assets.Highways;
using Assets.Blobs;
using Assets.HighwayUpgraders;
using Assets.BlobDistributors.ForTesting;

namespace Assets.BlobDistributors.Editor {

    public class BlobDistributorTests {

        #region instance methods

        #region tests

        [Test]
        public void OnDistributorIsRepeatedlyTicked_PerformsDistributionAtTheCorrectRate() {
            //Setup
            var mockMapGraph = BuildAccessingCountingMockMapGraph();

            int numberOfTimesNodesWereAccessed = 0;
            mockMapGraph.NodesAccessed += delegate(object sender, EventArgs e) {
                ++numberOfTimesNodesWereAccessed;
            };

            var mockHighwayFactory = BuildMockHighwayFactory();
            
            var distributorToTest = BuildBlobDistributor();
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mockMapGraph;
            distributorToTest.HighwayFactory = mockHighwayFactory;

            //Execution
            for(int i = 0; i < 101; ++i) {
                distributorToTest.Tick(0.25f);
            }

            //Validation
            Assert.AreEqual(25, numberOfTimesNodesWereAccessed, "MapGraph.Nodes was not accessed the expected number of times");
        }

        [Test]
        public void OnDistributionPerformed_HigherPriorityHighwaysAreFilledBeforeLowerPriorityOnesReceiveAny() {
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
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            distributorToTest.Tick(5f);

            int leftCountAfter5Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter5Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter5Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            distributorToTest.Tick(5f);

            int leftCountAfter10Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter10Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter10Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            //Validation
            Assert.AreEqual(5, leftCountAfter5Seconds,  "LeftCountAfter5Seconds is incorrect");
            Assert.AreEqual(0, rightCountAfter5Seconds, "RightCountAfter5Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter5Seconds,    "UpCountAfter5Seconds is incorrect");

            Assert.AreEqual(5, leftCountAfter10Seconds,  "LeftCountAfter10Seconds is incorrect");
            Assert.AreEqual(5, rightCountAfter10Seconds, "RightCountAfter10Seconds is incorrect");
            Assert.AreEqual(0, upCountAfter10Seconds,    "UpCountAfter10Seconds is incorrect");
        }

        [Test]
        public void OnDistributionPerformed_HighwaysWithTheSamePriorityGetServedRoundRobin() {
            //Setup
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
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            distributorToTest.Tick(6f);

            int leftCountAfter6Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter6Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter6Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            distributorToTest.Tick(4f);

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
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            distributorToTest.Tick(3f);

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter3Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter3Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            highwayUp.Priority = 1;
            distributorToTest.Tick(3f);

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
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            distributorToTest.Tick(3f);

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter3Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;

            highwayUp = highwayFactory.ConstructHighwayBetween(centerNode, upNode);
            highwayUp.Profile = new BlobHighwayProfile(1f, 5, ResourceSummary.BuildResourceSummary(highwayUp.gameObject));
            highwayUp.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayUp.Priority = 1;
            distributorToTest.Tick(3f);

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
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;

            //Execution
            distributorToTest.Tick(3f);

            int leftCountAfter3Seconds  = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter3Seconds = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter3Seconds    = highwayUp.ContentsPulledFromFirstEndpoint.Count;

            highwayFactory.DestroyHighway(highwayLeft);
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
        public void OnSomeHighwayHasActiveUpgrader_TheBlobSiteSupportingTheUpgraderIsPrioritizedAboveAllHighways() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 100;
            highwayRight.Priority = 10;
            highwayUp.Priority = 1;

            var upgraderFactory = BuildUpgraderFactory();
            var onlyUpgrader = upgraderFactory.BuildHighwayUpgrader(highwayLeft,
                mapGraph.GetEdge(centerNode, leftNode).BlobSite, upgraderFactory.ProfileToUse);

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;
            distributorToTest.HighwayUpgraderFactory = upgraderFactory;

            //Execution
            distributorToTest.Tick(5f);

            int leftCountAfter5Seconds     = highwayLeft.ContentsPulledFromFirstEndpoint.Count;
            int rightCountAfter5Seconds    = highwayRight.ContentsPulledFromFirstEndpoint.Count;
            int upCountAfter5Seconds       = highwayUp.ContentsPulledFromFirstEndpoint.Count;
            int upgraderCountAfter5Seconds = onlyUpgrader.UnderlyingSite.GetCountOfContentsOfType(ResourceType.Food);

            //Validation
            Assert.AreEqual(0, leftCountAfter5Seconds,     "HighwayLeft has an incorrect pull count" );
            Assert.AreEqual(0, rightCountAfter5Seconds,    "HighwayRight has an incorrect pull count");
            Assert.AreEqual(0, upCountAfter5Seconds,       "HighwayUp has an incorrect pull count"   );
            Assert.AreEqual(5, upgraderCountAfter5Seconds, "OnlyUpgrader has an incorrect pull count");
        }

        [Test]
        public void OnMultipleHighwaysHaveActiveUpgrader_UpgradersAreAllServedRoundRobin() {
            //Setup
            var mapGraph = BuildMapGraph();

            MapNodeBase centerNode, leftNode, rightNode, upNode;
            SetUpMapGraph(mapGraph, out centerNode, out leftNode, out rightNode, out upNode);

            var highwayFactory = BuildMockHighwayFactory();

            BlobHighwayBase highwayLeft, highwayRight, highwayUp;
            SetUpHighwayFactory(highwayFactory, centerNode, leftNode, rightNode, upNode,
                out highwayLeft, out highwayRight, out highwayUp);

            highwayLeft.Priority = 100;
            highwayRight.Priority = 10;
            highwayUp.Priority = 1;

            var upgraderFactory = BuildUpgraderFactory();
            var upgraderLeft = upgraderFactory.BuildHighwayUpgrader(highwayLeft,
                mapGraph.GetEdge(centerNode, leftNode).BlobSite, upgraderFactory.ProfileToUse);

            var upgraderRight = upgraderFactory.BuildHighwayUpgrader(highwayRight,
                mapGraph.GetEdge(centerNode, rightNode).BlobSite, upgraderFactory.ProfileToUse);

            var upgraderUp = upgraderFactory.BuildHighwayUpgrader(highwayUp,
                mapGraph.GetEdge(centerNode, upNode).BlobSite, upgraderFactory.ProfileToUse);

            var distributorToTest = BuildBlobDistributor();
            distributorToTest.SecondsToPerformDistributionTick = 1f;
            distributorToTest.MapGraph = mapGraph;
            distributorToTest.HighwayFactory = highwayFactory;
            distributorToTest.HighwayUpgraderFactory = upgraderFactory;

            //Execution
            distributorToTest.Tick(9f);

            int upgraderLeftCountAfter9Seconds  = upgraderLeft.UnderlyingSite.GetCountOfContentsOfType(ResourceType.Food);
            int upgraderRightCountAfter9Seconds = upgraderRight.UnderlyingSite.GetCountOfContentsOfType(ResourceType.Food);
            int upgraderUpCountAfter9Seconds    = upgraderUp.UnderlyingSite.GetCountOfContentsOfType(ResourceType.Food);

            //Validation
            Assert.AreEqual(3, upgraderLeftCountAfter9Seconds,  "UpgraderLeft has an incorrect pull count");
            Assert.AreEqual(3, upgraderRightCountAfter9Seconds, "UpgraderRight has an incorrect pull count");
            Assert.AreEqual(3, upgraderUpCountAfter9Seconds,    "UpgraderUp has an incorrect pull count");
        }

        #endregion

        #region utilities

        private ComplexMockMapGraph BuildMapGraph() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<ComplexMockMapGraph>();
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
            return hostingObject.AddComponent<MockHighwayFactory>();
        }

        private void SetUpMapGraph(MapGraphBase mapGraph,out MapNodeBase centerNode,
            out MapNodeBase leftNode, out MapNodeBase rightNode, out MapNodeBase upNode) {
            centerNode = mapGraph.BuildNode(Vector3.zero);
            leftNode   = mapGraph.BuildNode(Vector3.left);
            rightNode  = mapGraph.BuildNode(Vector3.right);
            upNode     = mapGraph.BuildNode(Vector3.up);
            
            centerNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            centerNode.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            centerNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            centerNode.BlobSite.TotalCapacity = 100;
            for(int i = 0; i < 30; ++i) {
                centerNode.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            }

            leftNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            leftNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            leftNode.BlobSite.TotalCapacity = 100;

            rightNode.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            rightNode.BlobSite.SetCapacityForResourceType(ResourceType.Food, 30);
            rightNode.BlobSite.TotalCapacity = 100;

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

            var highwayProfile = new BlobHighwayProfile(1f, 5, ResourceSummary.BuildResourceSummary(new GameObject()));

            highwayLeft.Profile = highwayProfile;
            highwayLeft.SetPullingPermissionForFirstEndpoint (ResourceType.Food, true);

            highwayRight.Profile = highwayProfile;
            highwayRight.SetPullingPermissionForFirstEndpoint (ResourceType.Food, true);

            highwayUp.Profile = highwayProfile;
            highwayUp.SetPullingPermissionForFirstEndpoint (ResourceType.Food, true);
        }

        private MockHighwayUpgraderFactory BuildUpgraderFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockHighwayUpgraderFactory>();
        }

        #endregion

        #endregion

    }

}


