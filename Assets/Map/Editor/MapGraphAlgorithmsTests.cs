using System;
using System.Linq;

using UnityEngine;

using NUnit.Framework;

using Assets.Map.ForTesting;

namespace Assets.Map.Editor {

    public class GraphAlgorithmsTestsMap {

        #region instance methods

        #region tests

        [Test]
        public void WhenGetDistanceBetweenNodesIsCalled_ReturnsShortestDistanceBetweenNodes() {
            //Setup
            var setToTest = BuildAlgorithmSet();
            var mapGraph = BuildMapGraph();

            var nodeOne   = mapGraph.BuildNode(Vector3.zero);
            var nodeTwo   = mapGraph.BuildNode(Vector3.zero);
            var nodeThree = mapGraph.BuildNode(Vector3.zero);
            var nodeFour  = mapGraph.BuildNode(Vector3.zero);
            var nodeFive  = mapGraph.BuildNode(Vector3.zero);
            var nodeSix   = mapGraph.BuildNode(Vector3.zero);
            var nodeSeven = mapGraph.BuildNode(Vector3.zero);
            var nodeEight = mapGraph.BuildNode(Vector3.zero);
            var nodeNine  = mapGraph.BuildNode(Vector3.zero);

            mapGraph.BuildMapEdge(nodeOne,   nodeTwo  );
            mapGraph.BuildMapEdge(nodeOne,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeFour );
            mapGraph.BuildMapEdge(nodeTwo,   nodeFive );
            mapGraph.BuildMapEdge(nodeTwo,   nodeSix  );
            mapGraph.BuildMapEdge(nodeThree, nodeFour );
            mapGraph.BuildMapEdge(nodeFour,  nodeFive );
            mapGraph.BuildMapEdge(nodeFive,  nodeSix  );
            mapGraph.BuildMapEdge(nodeFive,  nodeSeven);
            mapGraph.BuildMapEdge(nodeFive,  nodeEight);
            mapGraph.BuildMapEdge(nodeSix,   nodeSeven);
            mapGraph.BuildMapEdge(nodeSeven, nodeEight);
            mapGraph.BuildMapEdge(nodeEight, nodeNine );

            //Execution
            var distanceFromOneToNine = setToTest.GetDistanceBetweenNodes(nodeOne, nodeNine, mapGraph.Nodes);
            var distanceFromNineToOne = setToTest.GetDistanceBetweenNodes(nodeNine, nodeOne, mapGraph.Nodes);

            var distanceFromOneToSix = setToTest.GetDistanceBetweenNodes(nodeOne, nodeSix, mapGraph.Nodes);
            var distanceFromSixToOne = setToTest.GetDistanceBetweenNodes(nodeSix, nodeOne, mapGraph.Nodes);

            var distanceFromThreeToSix = setToTest.GetDistanceBetweenNodes(nodeThree, nodeSix, mapGraph.Nodes);
            var distanceFromSixToThree = setToTest.GetDistanceBetweenNodes(nodeSix, nodeThree, mapGraph.Nodes);

            var distanceFromFourToSix = setToTest.GetDistanceBetweenNodes(nodeFour, nodeSix, mapGraph.Nodes);
            var distanceFromSixToFour = setToTest.GetDistanceBetweenNodes(nodeSix, nodeFour, mapGraph.Nodes);

            var distanceFromFiveToFive = setToTest.GetDistanceBetweenNodes(nodeFive, nodeFive, mapGraph.Nodes);
            var distanceFromTwoToTwo = setToTest.GetDistanceBetweenNodes(nodeTwo, nodeTwo, mapGraph.Nodes);

            //Validation
            Assert.AreEqual(4, distanceFromOneToNine, "Incorrect distance from nodeOne to nodeNine");
            Assert.AreEqual(4, distanceFromNineToOne, "Incorrect distance from nodeNine to nodeOne");

            Assert.AreEqual(2, distanceFromOneToSix, "Incorrect distance from nodeOne to nodeSix");
            Assert.AreEqual(2, distanceFromSixToOne, "Incorrect distance from nodeSix to nodeOne");

            Assert.AreEqual(2, distanceFromThreeToSix, "Incorrect distance from nodeThree to nodeSix");
            Assert.AreEqual(2, distanceFromSixToThree, "Incorrect distance from nodeSix to nodeThree");

            Assert.AreEqual(2, distanceFromFourToSix, "Incorrect distance from nodeFour to nodeSix");
            Assert.AreEqual(2, distanceFromSixToFour, "Incorrect distance from nodeSix to nodeFour");

            Assert.AreEqual(0, distanceFromFiveToFive, "Incorrect distance from nodeFive to nodeFive");
            Assert.AreEqual(0, distanceFromTwoToTwo, "Incorrect distance from nodeTwo to nodeTwo");
        }

        [Test]
        public void WhenGetShortestPathBetweenNodesIsCalled_ReturnsSomeValidShortestPathBetweenNodes() {
            //Setup
            var setToTest = BuildAlgorithmSet();
            var mapGraph = BuildMapGraph();

            var nodeOne   = mapGraph.BuildNode(Vector3.zero);
            var nodeTwo   = mapGraph.BuildNode(Vector3.zero);
            var nodeThree = mapGraph.BuildNode(Vector3.zero);
            var nodeFour  = mapGraph.BuildNode(Vector3.zero);
            var nodeFive  = mapGraph.BuildNode(Vector3.zero);
            var nodeSix   = mapGraph.BuildNode(Vector3.zero);
            var nodeSeven = mapGraph.BuildNode(Vector3.zero);
            var nodeEight = mapGraph.BuildNode(Vector3.zero);
            var nodeNine  = mapGraph.BuildNode(Vector3.zero);

            nodeOne.name   = "Node One";
            nodeTwo.name   = "Node Two";
            nodeThree.name = "Node Three";
            nodeFour.name  = "Node Four";
            nodeFive.name  = "Node Five";
            nodeSix.name   = "Node Six";
            nodeSeven.name = "Node Seven";
            nodeEight.name = "Node Eight";
            nodeNine.name  = "Node Nine";

            mapGraph.BuildMapEdge(nodeOne,   nodeTwo  );
            mapGraph.BuildMapEdge(nodeOne,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeFour );
            mapGraph.BuildMapEdge(nodeTwo,   nodeFive );
            mapGraph.BuildMapEdge(nodeTwo,   nodeSix  );
            mapGraph.BuildMapEdge(nodeThree, nodeFour );
            mapGraph.BuildMapEdge(nodeFour,  nodeFive );
            mapGraph.BuildMapEdge(nodeFive,  nodeSix  );
            mapGraph.BuildMapEdge(nodeFive,  nodeSeven);
            mapGraph.BuildMapEdge(nodeFive,  nodeEight);
            mapGraph.BuildMapEdge(nodeSix,   nodeSeven);
            mapGraph.BuildMapEdge(nodeSeven, nodeEight);
            mapGraph.BuildMapEdge(nodeEight, nodeNine );

            //Execution
            var pathFromOneToNine = setToTest.GetShortestPathBetweenNodes(nodeOne, nodeNine, mapGraph.Nodes);

            var pathFromFourToSix = setToTest.GetShortestPathBetweenNodes(nodeFour, nodeSix, mapGraph.Nodes);
            var pathFromSixToFour = setToTest.GetShortestPathBetweenNodes(nodeSix, nodeFour, mapGraph.Nodes);

            var pathFromThreeToSeven = setToTest.GetShortestPathBetweenNodes(nodeThree, nodeSeven, mapGraph.Nodes);
            var pathFromSevenToThree = setToTest.GetShortestPathBetweenNodes(nodeSeven, nodeThree, mapGraph.Nodes);

            var pathFromFiveToFive = setToTest.GetShortestPathBetweenNodes(nodeFive, nodeFive, mapGraph.Nodes);

            //Validation
            Assert.AreEqual(nodeOne, pathFromOneToNine.First(), "pathFromOneToNine has an incorrect first element");
            Assert.AreEqual(nodeNine, pathFromOneToNine.Last(), "pathFromOneToNine has an incorrect last element");
            Assert.AreEqual(5, pathFromOneToNine.Count, "pathFromOneToNine has an incorrect length");
            for(int i = 0; i < pathFromOneToNine.Count - 1; ++i) {
                Assert.NotNull(mapGraph.GetEdge(pathFromOneToNine[i], pathFromOneToNine[i + 1]), 
                    string.Format("While traversing pathFromOneToNine, MapGraph failed to find an edge between " +
                    "supposedly adjacent nodes {0} and {1}", pathFromOneToNine[i], pathFromOneToNine[i + 1]));
            }


            Assert.AreEqual(nodeFour, pathFromFourToSix.First(), "pathFromFourToSix has an incorrect first element");
            Assert.AreEqual(nodeSix, pathFromFourToSix.Last(), "pathFromFourToSix has an incorrect last element");
            Assert.AreEqual(3, pathFromFourToSix.Count, "pathFromFourToSix has an incorrect length");
            for(int i = 0; i < pathFromFourToSix.Count - 1; ++i) {
                Assert.NotNull(mapGraph.GetEdge(pathFromFourToSix[i], pathFromFourToSix[i + 1]), 
                    string.Format("While traversing pathFromFourToSix, MapGraph failed to find an edge between " +
                    "supposedly adjacent nodes {0} and {1}", pathFromFourToSix[i], pathFromFourToSix[i + 1]));
            }

            Assert.AreEqual(nodeSix, pathFromSixToFour.First(), "pathFromSixToFour has an incorrect first element");
            Assert.AreEqual(nodeFour, pathFromSixToFour.Last(), "pathFromSixToFour has an incorrect last element");
            Assert.AreEqual(3, pathFromSixToFour.Count, "pathFromSixToFour has an incorrect length");
            for(int i = 0; i < pathFromSixToFour.Count - 1; ++i) {
                Assert.NotNull(mapGraph.GetEdge(pathFromSixToFour[i], pathFromSixToFour[i + 1]), 
                    string.Format("While traversing pathFromSixToFour, MapGraph failed to find an edge between " +
                    "supposedly adjacent nodes {0} and {1}", pathFromSixToFour[i], pathFromSixToFour[i + 1]));
            }


            Assert.AreEqual(nodeThree, pathFromThreeToSeven.First(), "pathFromThreeToSeven has an incorrect first element");
            Assert.AreEqual(nodeSeven, pathFromThreeToSeven.Last(), "pathFromThreeToSeven has an incorrect last element");
            Assert.AreEqual(4, pathFromThreeToSeven.Count, "pathFromThreeToSeven has an incorrect length");
            for(int i = 0; i < pathFromThreeToSeven.Count - 1; ++i) {
                Assert.NotNull(mapGraph.GetEdge(pathFromThreeToSeven[i], pathFromThreeToSeven[i + 1]), 
                    string.Format("While traversing pathFromThreeToSeven, MapGraph failed to find an edge between " +
                    "supposedly adjacent nodes {0} and {1}", pathFromThreeToSeven[i], pathFromThreeToSeven[i + 1]));
            }

            Assert.AreEqual(nodeSeven, pathFromSevenToThree.First(), "pathFromSevenToThree has an incorrect first element");
            Assert.AreEqual(nodeThree, pathFromSevenToThree.Last(), "pathFromSevenToThree has an incorrect last element");
            Assert.AreEqual(4, pathFromSevenToThree.Count, "pathFromSevenToThree has an incorrect length");
            for(int i = 0; i < pathFromSevenToThree.Count - 1; ++i) {
                Assert.NotNull(mapGraph.GetEdge(pathFromSevenToThree[i], pathFromSevenToThree[i + 1]), 
                    string.Format("While traversing pathFromSevenToThree, MapGraph failed to find an edge between " +
                    "supposedly adjacent nodes {0} and {1}", pathFromSevenToThree[i], pathFromSevenToThree[i + 1]));
            }


            Assert.AreEqual(nodeFive, pathFromFiveToFive.First(), "pathFromFiveToFive has an incorrect first element");
            Assert.AreEqual(1, pathFromFiveToFive.Count, "pathFromFiveToFive has an incorrect length");
        }

        [Test]
        public void WhenGetNearestNodeToEdgeWhereIsCalled_ReturnsNearestNodeWithinRangeOfEdgeEndpointsThatMeetsConditions_OrNullIfNoneExist() {
            //Setup
            var setToTest = BuildAlgorithmSet();
            var mapGraph = BuildMapGraph();

            var nodeOne   = mapGraph.BuildNode(Vector3.zero);
            var nodeTwo   = mapGraph.BuildNode(Vector3.zero);
            var nodeThree = mapGraph.BuildNode(Vector3.zero);
            var nodeFour  = mapGraph.BuildNode(Vector3.zero);
            var nodeFive  = mapGraph.BuildNode(Vector3.zero);
            var nodeSix   = mapGraph.BuildNode(Vector3.zero);
            var nodeSeven = mapGraph.BuildNode(Vector3.zero);
            var nodeEight = mapGraph.BuildNode(Vector3.zero);
            var nodeNine  = mapGraph.BuildNode(Vector3.zero);

            nodeOne.name   = "Node One";
            nodeTwo.name   = "Node Two";
            nodeThree.name = "Node Three";
            nodeFour.name  = "Node Four";
            nodeFive.name  = "Node Five";
            nodeSix.name   = "Node Six";
            nodeSeven.name = "Node Seven";
            nodeEight.name = "Node Eight";
            nodeNine.name  = "Node Nine";

            mapGraph.BuildMapEdge(nodeOne,   nodeTwo  );
            mapGraph.BuildMapEdge(nodeOne,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeFour );
            var edgeTwoFive    = mapGraph.BuildMapEdge(nodeTwo,   nodeFive );
            mapGraph.BuildMapEdge(nodeTwo,   nodeSix  );
            mapGraph.BuildMapEdge(nodeThree, nodeFour );
            mapGraph.BuildMapEdge(nodeFour,  nodeFive );
            var edgeFiveSix    = mapGraph.BuildMapEdge(nodeFive,  nodeSix  );
            mapGraph.BuildMapEdge(nodeFive,  nodeSeven);
            mapGraph.BuildMapEdge(nodeFive,  nodeEight);
            mapGraph.BuildMapEdge(nodeSix,   nodeSeven);
            mapGraph.BuildMapEdge(nodeSeven, nodeEight);
            var edgeEightNine  = mapGraph.BuildMapEdge(nodeEight, nodeNine );

            //Execution
            var nodeOneWithinDistanceThreeOfEdgeEightNine = setToTest.GetNearestNodeToEdgeWhere(edgeEightNine,
                node => node == nodeOne, 3);
            var nodeOneWithinDistanceTwoOfEdgeEightNine = setToTest.GetNearestNodeToEdgeWhere(edgeEightNine,
                node => node == nodeOne, 2);

            var nodeSixWithinDistanceOneOfEdgeTwoFive = setToTest.GetNearestNodeToEdgeWhere(edgeTwoFive,
                node => node == nodeSix, 1);
            var nodeSixWithinDistanceZeroOfEdgeTwoFive = setToTest.GetNearestNodeToEdgeWhere(edgeTwoFive,
                node => node == nodeSix, 0);

            var nodeSixWithinDistanceZeroOfEdgeFiveSix = setToTest.GetNearestNodeToEdgeWhere(edgeFiveSix,
                node => node == nodeSix, 0);

            var impossibleNodeWithinDistanceFiveOfEdgeTwoFive = setToTest.GetNearestNodeToEdgeWhere(edgeTwoFive,
                node => false, 5);

            //Validation
            Assert.AreEqual(nodeOne, nodeOneWithinDistanceThreeOfEdgeEightNine.Node, "nodeOneWithinDistanceThreeOfEdgeEightNine has an incorrect node");
            Assert.AreEqual(3, nodeOneWithinDistanceThreeOfEdgeEightNine.Distance,   "nodeOneWithinDistanceThreeOfEdgeEightNine has an incorrect distance");
            Assert.AreEqual(null, nodeOneWithinDistanceTwoOfEdgeEightNine,"nodeOneWithinDistanceTwoOfEdgeEightNine has an incorrect value");

            Assert.AreEqual(nodeSix, nodeSixWithinDistanceOneOfEdgeTwoFive.Node, "nodeSixWithinDistanceOneOfEdgeTwoFive has an incorrect node");
            Assert.AreEqual(1, nodeSixWithinDistanceOneOfEdgeTwoFive.Distance,   "nodeOneWithinDistanceThreeOfEdgeEightNine has an incorrect distance");
            Assert.AreEqual(null, nodeSixWithinDistanceZeroOfEdgeTwoFive, "nodeSixWithinDistanceZeroOfEdgeTwoFive has an incorrect value");

            Assert.AreEqual(nodeSix, nodeSixWithinDistanceZeroOfEdgeFiveSix.Node, "nodeSixWithinDistanceZeroOfEdgeFiveSix has an incorrect node");
            Assert.AreEqual(0, nodeSixWithinDistanceZeroOfEdgeFiveSix.Distance, "nodeSixWithinDistanceZeroOfEdgeFiveSix has an incorrect node");

            Assert.AreEqual(null, impossibleNodeWithinDistanceFiveOfEdgeTwoFive, "impossibleNodeWithinDistanceFiveOfEdgeTwoFive has an incorrect distance");
        }

        [Test]
        public void WhenGetNearestNodeToNodeWhereIsCalled_ReturnsNearestNodeWithinRangeThatMeetsConditions_OrNullIfNoneExist() {
            //Setup
            var setToTest = BuildAlgorithmSet();
            var mapGraph = BuildMapGraph();

            var nodeOne   = mapGraph.BuildNode(Vector3.zero);
            var nodeTwo   = mapGraph.BuildNode(Vector3.zero);
            var nodeThree = mapGraph.BuildNode(Vector3.zero);
            var nodeFour  = mapGraph.BuildNode(Vector3.zero);
            var nodeFive  = mapGraph.BuildNode(Vector3.zero);
            var nodeSix   = mapGraph.BuildNode(Vector3.zero);
            var nodeSeven = mapGraph.BuildNode(Vector3.zero);
            var nodeEight = mapGraph.BuildNode(Vector3.zero);
            var nodeNine  = mapGraph.BuildNode(Vector3.zero);

            nodeOne.name   = "Node One";
            nodeTwo.name   = "Node Two";
            nodeThree.name = "Node Three";
            nodeFour.name  = "Node Four";
            nodeFive.name  = "Node Five";
            nodeSix.name   = "Node Six";
            nodeSeven.name = "Node Seven";
            nodeEight.name = "Node Eight";
            nodeNine.name  = "Node Nine";

            mapGraph.BuildMapEdge(nodeOne,   nodeTwo  );
            mapGraph.BuildMapEdge(nodeOne,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeThree);
            mapGraph.BuildMapEdge(nodeTwo,   nodeFour );
            mapGraph.BuildMapEdge(nodeTwo,   nodeFive );
            mapGraph.BuildMapEdge(nodeTwo,   nodeSix  );
            mapGraph.BuildMapEdge(nodeThree, nodeFour );
            mapGraph.BuildMapEdge(nodeFour,  nodeFive );
            mapGraph.BuildMapEdge(nodeFive,  nodeSix  );
            mapGraph.BuildMapEdge(nodeFive,  nodeSeven);
            mapGraph.BuildMapEdge(nodeFive,  nodeEight);
            mapGraph.BuildMapEdge(nodeSix,   nodeSeven);
            mapGraph.BuildMapEdge(nodeSeven, nodeEight);
            mapGraph.BuildMapEdge(nodeEight, nodeNine );

            //Execution
            var nodeOneWithinDistanceFourOfNodeNine = setToTest.GetNearestNodeToNodeWhere(nodeNine, node => node == nodeOne, 4);
            var nodeOneWithinDistanceThreeOfNodeNine = setToTest.GetNearestNodeToNodeWhere(nodeNine, node => node == nodeOne, 3);

            var nodeFiveWithinDistanceZeroOfNodeFive = setToTest.GetNearestNodeToNodeWhere(nodeFive, node => node == nodeFive, 0);

            var impossibleNodeWithinDistanceFiveOfNodeSix = setToTest.GetNearestNodeToNodeWhere(nodeSix, node => false, 5);

            //Validation
            Assert.AreEqual(nodeOne, nodeOneWithinDistanceFourOfNodeNine.Node, "nodeOneWithinDistanceFourOfNodeNine has an incorrect node");
            Assert.AreEqual(4, nodeOneWithinDistanceFourOfNodeNine.Distance, "nodeOneWithinDistanceFourOfNodeNine has an incorrect distance");

            Assert.IsNull(nodeOneWithinDistanceThreeOfNodeNine, "nodeOneWithinDistanceThreeOfNodeNine has an incorrect value");

            Assert.AreEqual(nodeFive, nodeFiveWithinDistanceZeroOfNodeFive.Node, "nodeFiveWithinDistanceZeroOfNodeFive has an incorrect node");
            Assert.AreEqual(0, nodeFiveWithinDistanceZeroOfNodeFive.Distance, "nodeFiveWithinDistanceZeroOfNodeFive has an incorrect distance");

            Assert.IsNull(impossibleNodeWithinDistanceFiveOfNodeSix, "impossibleNodeWithinDistanceFiveOfNodeSix has an incorrect value");
        }

        #endregion

        #region utilities

        private MapGraphAlgorithmSet BuildAlgorithmSet() {
            return (new GameObject()).AddComponent<MapGraphAlgorithmSet>();
        }

        private MapGraph BuildMapGraph() {
            var newGraph = (new GameObject()).AddComponent<MapGraph>();
            
            newGraph.TerrainMaterialRegistry = (new GameObject()).AddComponent<TerrainMaterialRegistry>();
            newGraph.UIControl = (new GameObject()).AddComponent<MockUIControl>();

            return newGraph;
        }

        #endregion

        #endregion

    }

}


