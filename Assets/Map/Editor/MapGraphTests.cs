using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Map.ForTesting;

namespace Assets.Map.Editor {

    public class MapGraphTests {

        #region instance methods

        #region tests

        [Test]
        public void OnBuildNodeCalled_NodeHasCorrectLocation_IsAChildOfTheMapGraph_AndIsSubscribed() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            //Execution
            var newNode = graphToTest.BuildNode(Vector3.zero);

            //Validation
            Assert.AreEqual(Vector3.zero, newNode.transform.localPosition, "Node has an incorrect localPosition");
            Assert.AreEqual(graphToTest.transform, newNode.transform.parent, "Node does not have the MapGraph's transform as its parent");
            Assert.Contains(newNode, graphToTest.Nodes, "The Nodes collection does not contain the node");
        }

        [Test]
        public void DestroyNodeCalled_NodeIsUnsubscribed_AndRemovedFromGameObjectHierarchy() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeToDestroy = graphToTest.BuildNode(Vector3.zero);
            nodeToDestroy.name = "Test Node To Destroy";

            //Execution
            graphToTest.DestroyNode(nodeToDestroy);

            //Validation
            Assert.AreEqual(0, graphToTest.Nodes.Count, "MapGraph still recognizes the existence of a node in its Nodes collection");
            Assert.Null(GameObject.Find("Test Node To Destroy"), "Destroyed node still appears in the game object hierarchy");
        }

        [Test]
        public void OnSubscribeNodeCalled_NodeHasItsParentGraphSetProperly_AndAppearsInTheNodesCollectionOfThatGraph() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();
            graphToTest.BlobSiteConfiguration = BuildMockBlobSiteConfiguration();

            //Execution
            var newNode = BuildMapNode();
            graphToTest.SubscribeNode(newNode);

            //Validation
            Assert.Contains(newNode, graphToTest.Nodes);
        }

        [Test]
        public void OnUnsubscribeNodeCalled_NodeIsRemovedFromNodesProperty_LosesItsParentGraph_AndLosesAllNeighborRelations() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var newNode = graphToTest.BuildNode(Vector3.zero);

            //Execution
            graphToTest.UnsubscribeNode(newNode);

            //Validation
            Assert.IsFalse(graphToTest.Nodes.Contains(newNode), "MapGraph.Nodes collection still contains the unsubscribed node");
            Assert.Null(newNode.ParentGraph, "newNode.ParentGraph has not been reset to null");
            Assert.IsEmpty(newNode.Neighbors, "newNode.Neighbors is not empty");
        }

        [Test]
        public void OnUnsubscribeNodeCalled_NodeUnsubscribedEventIsCalled() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            MapNodeBase nodeReturnedByEvent = null;
            graphToTest.MapNodeUnsubscribed += delegate(object sender, MapNodeEventArgs e) {
                nodeReturnedByEvent = e.Node;
            };

            var nodeToUnsubscribe = graphToTest.BuildNode(Vector3.zero);

            //Execution
            graphToTest.UnsubscribeNode(nodeToUnsubscribe);

            //Validation
            Assert.AreEqual(nodeToUnsubscribe, nodeReturnedByEvent, "GraphToTest either did not fire the event or performed it on the wrong node");
        }

        [Test]
        public void OnBuildUndirectedEdgeCalled_ReturnedEdgeHasCorrectEndpoints_AndAppearsInEdgesProperty() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);

            //Execution
            var edgeToBuild = graphToTest.BuildMapEdge(nodeOne, nodeTwo);

            //Validation
            Assert.AreEqual(nodeOne, edgeToBuild.FirstNode,  "Edge has an incorrect FirstNode");
            Assert.AreEqual(nodeTwo, edgeToBuild.SecondNode, "Edge has an incorrect SecondNode");
            Assert.Contains(edgeToBuild, graphToTest.Edges, "Edge  does not appear in MapGraph.Edges collection");
        }

        [Test]
        public void OnBuildUndirectedEdgeCalled_ButAnEdgeAlreadyExistsBetweenTheSpecifiedEndpoints_ThrowsMapGraphException() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.UIControl = BuildMockUIControl();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);

            graphToTest.BuildMapEdge(nodeOne, nodeTwo);

            //Execution and validation
            Assert.Throws<MapGraphException>(delegate() {
                graphToTest.BuildMapEdge(nodeOne, nodeTwo);
            });
        }

        [Test]
        public void OnDestroyUndirectedEdgeCalled_EdgeIsUnsubscribed_AndRemovedFromGameObjectHierarchy() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);

            var edgeToDestroy = graphToTest.BuildMapEdge(nodeOne, nodeTwo);
            edgeToDestroy.name = "Test Edge To Destroy";

            //Execution
            graphToTest.DestroyMapEdge(edgeToDestroy);

            //Validation
            Assert.Null(edgeToDestroy.ParentGraph, "Edge.ParentGraph has not been reset to null");
            Assert.IsFalse(graphToTest.Edges.Contains(edgeToDestroy), "MapGraph.Edges still contains the destroyed edge");
            Assert.Null(GameObject.Find("Test Edge To Destroy"), "The edge's GameObject still exists in the object hierarchy");
        }

        [Test]
        public void OnDestroyUndirectedEdgeCalledWithEndpoints_CorrectlyDestroysTheEdgeWithBothEndpoints_AndDoesNotThrowIfNoneExists() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);
            var nodeThree = graphToTest.BuildNode(Vector3.up);

            graphToTest.BuildMapEdge(nodeOne, nodeTwo  );
            graphToTest.BuildMapEdge(nodeTwo, nodeThree);

            //Execution
            graphToTest.DestroyMapEdge(nodeOne, nodeTwo);

            //Validation
            Assert.Null(graphToTest.GetEdge(nodeOne, nodeTwo), "MapGraph still recognizes an edge between node1 and node2");
            Assert.NotNull(graphToTest.GetEdge(nodeTwo, nodeThree), "MapGraph fails to recognize an edge between node2 and node3");
            Assert.DoesNotThrow(delegate() {
                graphToTest.DestroyMapEdge(nodeOne, nodeTwo);
            }, "Attempting to remove a non-existent edge between nodeOne and nodeTwo throws an exception");
        }

        [Test]
        public void OnSubscribeUndirectedEdgeCalled_EdgeHasItsParentGraphSetCorrectly_AndAppearsInEdgesPropertyOfTheGraph() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);

            var edgeToSubscribe = BuildMapEdge(nodeOne, nodeTwo);

            //Execution
            graphToTest.SubscribeMapEdge(edgeToSubscribe);

            //Validation
            Assert.Contains(edgeToSubscribe, graphToTest.Edges, "Subscribed edge fails to appear in MapGraph's Edges collection");
            Assert.AreEqual(graphToTest, edgeToSubscribe.ParentGraph, "Subscribed edge has an incorrect ParentGraph");
        }

        [Test]
        public void OnUnsubscribeEdgeCalled_EdgeIsRemovedFromEdgesProperty_AndHasItsParentGraphResetToNull() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);

            var edgeToUnsubscribe = graphToTest.BuildMapEdge(nodeOne, nodeTwo);
            
            //Execution
            graphToTest.UnsubscribeMapEdge(edgeToUnsubscribe);

            //Validation
            Assert.IsFalse(graphToTest.Edges.Contains(edgeToUnsubscribe), "EdgeToUnsubscribe is still in the factory's Edges collection");
            Assert.Null(edgeToUnsubscribe.ParentGraph, "EdgeToUnsubscribe still has GraphToTest as its parent graph");
        }

        [Test]
        public void OnUnsubscribeEdgeCalled_EdgeUnsubscribedEventIsInvoked() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            MapEdgeBase edgeReturnedByEvent = null;
            graphToTest.MapEdgeUnsubscribed += delegate(object sender, MapEdgeEventArgs e) {
                edgeReturnedByEvent = e.Edge;
            };

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);

            var edgeToUnsubscribe = graphToTest.BuildMapEdge(nodeOne, nodeTwo);
            
            //Execution
            graphToTest.UnsubscribeMapEdge(edgeToUnsubscribe);

            //Validation
            Assert.AreEqual(edgeToUnsubscribe, edgeReturnedByEvent, "GraphToTest either did not fire the event or fired it on the wrong MapEdge");
        }

        [Test]
        public void OnGetNodeOfIDCalled_ReturnsNodeWithTheRequestedID_OrNullIfNoneExists() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);
            var nodeThree = graphToTest.BuildNode(Vector3.up);

            //Execution and validation
            Assert.AreEqual(nodeOne,   graphToTest.GetNodeOfID(nodeOne.ID  ), "Failed to return nodeOne on its ID"  );
            Assert.AreEqual(nodeTwo,   graphToTest.GetNodeOfID(nodeTwo.ID  ), "Failed to return nodeTwo on its ID"  );
            Assert.AreEqual(nodeThree, graphToTest.GetNodeOfID(nodeThree.ID), "Failed to return nodeThree on its ID");
            Assert.Null(graphToTest.GetNodeOfID(42), "ID of 42 unexpectedly returned a MapNode");
        }

        [Test]
        public void OnGetEdgeCalled_ReturnsAnEdgeWithBothOfTheSpecifiedEndpoints_OrNullIfNoneExist() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var nodeOne = graphToTest.BuildNode(Vector3.zero);
            var nodeTwo = graphToTest.BuildNode(Vector3.one);
            var nodeThree = graphToTest.BuildNode(Vector3.up);

            var edgeBetweenOneAndTwo   = graphToTest.BuildMapEdge(nodeOne, nodeTwo  );
            var edgeBetweenTwoAndThree = graphToTest.BuildMapEdge(nodeTwo, nodeThree);

            //Execution and validation
            Assert.AreEqual(edgeBetweenOneAndTwo, graphToTest.GetEdge(nodeOne, nodeTwo),
                "MapGraph returned the wrong edge between nodeOne and nodeTwo");
            Assert.AreEqual(edgeBetweenTwoAndThree, graphToTest.GetEdge(nodeTwo, nodeThree),
                "MapGraph returned the wrong edge between nodeTwo and nodeThree");
            Assert.Null(graphToTest.GetEdge(nodeThree, nodeOne), "MapGraph falsely recognizes an edge between node3 and node1");
        }

        [Test]
        public void OnGetNeighborsOfNodeCalled_EveryNeighborReturnedAlsoContainsTheCenteredNodeAsOneOfItsNeighbors() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            centerNode.name = "centerNode";
            leftNode.name   = "leftNode";
            rightNode.name  = "rightNode";
            upNode.name     = "upNode";
            downNode.name   = "downNode";

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);
            
            //Execution
            var neighborsOfCenter = graphToTest.GetNeighborsOfNode(centerNode);

            //Validation
            foreach(var neighbor in neighborsOfCenter) {
                Assert.That(graphToTest.GetNeighborsOfNode(neighbor).Contains(centerNode), 
                    string.Format("Neighbor {0} fails to register centerNode as one of its neighbors", neighbor));
            }
            Assert.That(neighborsOfCenter.Contains(leftNode ), "Fails to return leftNode as a neighbor of centerNode" );
            Assert.That(neighborsOfCenter.Contains(rightNode), "Fails to return rightNode as a neighbor of centerNode");
            Assert.That(neighborsOfCenter.Contains(upNode   ), "Fails to return upNode as a neighbor of centerNode"   );
            Assert.That(neighborsOfCenter.Contains(downNode ), "Fails to return leftNode as a neighbor of downNode"   );
        }

        [Test]
        public void OnGetNeighborsOfNodeCalled_NoNodeNotReturnedRecognizesCenteredNodeAsOneOfItsNeighbors() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            centerNode.name = "centerNode";
            leftNode.name   = "leftNode";
            rightNode.name  = "rightNode";
            upNode.name     = "upNode";
            downNode.name   = "downNode";

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);

            //Execution and validation
            foreach(var outerNode in graphToTest.Nodes) {
                var outerNodeNeighbors = graphToTest.GetNeighborsOfNode(outerNode);
                foreach(var innerNode in graphToTest.Nodes) {
                    Assert.AreEqual(
                        outerNodeNeighbors.Contains(innerNode),
                        graphToTest.GetNeighborsOfNode(innerNode).Contains(outerNode),
                        string.Format("Nodes {0} and {1} disagree on their neighbor relationship", outerNode, innerNode)
                    );
                }
            }
        }

        [Test]
        public void OnGetNeighborsOfNodeCalled_GetEdgeBetweenCenteredNodeAndEachOfItsNeighborsDoesNotReturnNull() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            centerNode.name = "centerNode";
            leftNode.name   = "leftNode";
            rightNode.name  = "rightNode";
            upNode.name     = "upNode";
            downNode.name   = "downNode";

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);
            
            //Execution
            var neighborsOfCenter = graphToTest.GetNeighborsOfNode(centerNode);

            //Validation
            foreach(var neighbor in neighborsOfCenter) {
                Assert.NotNull(graphToTest.GetEdge(centerNode, neighbor),
                    string.Format("MapGraph fails to register a MapEdge between two supposed neighbors {0} and {1}", centerNode, neighbor));
            }
        }

        [Test]
        public void OnGetNeighborsOfNodeCalled_GetEdgeBetweenCenteredNodeAndEachNonNeighborReturnsNull() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            centerNode.name = "centerNode";
            leftNode.name   = "leftNode";
            rightNode.name  = "rightNode";
            upNode.name     = "upNode";
            downNode.name   = "downNode";

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);

            //Execution
            var neighborsOfCenter = graphToTest.GetNeighborsOfNode(centerNode);

            //Validation
            foreach(var nonNeighbor in graphToTest.Nodes.Where(node => !neighborsOfCenter.Contains(node))) {
                Assert.Null(graphToTest.GetEdge(centerNode, nonNeighbor),
                    string.Format("Non neighboring nodes {0} and {1} have an edge between them", centerNode, nonNeighbor));
            }
        }

        [Test]
        public void OnGetEdgesAttachedToNodeCalled_EveryEdgeReturnedHasCenteredNodeAsSomeEndpoint() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            centerNode.name = "centerNode";
            leftNode.name   = "leftNode";
            rightNode.name  = "rightNode";
            upNode.name     = "upNode";
            downNode.name   = "downNode";

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);
            graphToTest.BuildMapEdge(leftNode, upNode);
            graphToTest.BuildMapEdge(rightNode, downNode);

            //Execution
            var edgesAttachedToCenter = graphToTest.GetEdgesAttachedToNode(centerNode);

            //Validation
            foreach(var edgeToCenter in edgesAttachedToCenter) {
                Assert.That(edgeToCenter.FirstNode == centerNode || edgeToCenter.SecondNode == centerNode,
                    "An edge claimed to be attached to centerNode lacks it as one of its endpoints");
            }
        }

        [Test]
        public void OnGetEdgesAttachedToNodeCalled_NoEdgeNotReturnedHasCenteredNodeAsSomeEndpoint() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            centerNode.name = "centerNode";
            leftNode.name   = "leftNode";
            rightNode.name  = "rightNode";
            upNode.name     = "upNode";
            downNode.name   = "downNode";

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);
            graphToTest.BuildMapEdge(leftNode, upNode);
            graphToTest.BuildMapEdge(rightNode, downNode);

            //Execution
            var edgesAttachedToCenter = graphToTest.GetEdgesAttachedToNode(centerNode);

            //Validation
            foreach(var nonAttachedEdge in graphToTest.Edges.Where(edge => !edgesAttachedToCenter.Contains(edge))) {
                Assert.IsFalse(nonAttachedEdge.FirstNode == centerNode || nonAttachedEdge.SecondNode == centerNode,
                    "An edge not returned by GetEdgesAttachedToNode(centerNode) registers centerNode as one of its endpoints");
            }
        }

        [Test]
        public void OnGetDistanceBetweenNodesCalled_CorrespondingMethodInMapGraphAlgorithmsIsCalled() {
            //Setup
            var mockAlgorithmSet = BuildMockMapAlgorithmSet();

            MapNodeBase receivedNode1 = null;
            MapNodeBase receivedNode2 = null;
            IEnumerable<MapNodeBase> receivedAllNodes = null;
            mockAlgorithmSet.GetDistanceBetweenNodesCalled += delegate(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes) {
                receivedNode1 = node1;
                receivedNode2 = node2;
                receivedAllNodes = allNodes;
            };

            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();
            graphToTest.AlgorithmSet = mockAlgorithmSet;

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);

            //Execution
            graphToTest.GetDistanceBetweenNodes(centerNode, leftNode);

            //Validation
            Assert.AreEqual(centerNode, receivedNode1, "MapGraph passed incorrect node1 argument");
            Assert.AreEqual(leftNode, receivedNode2, "MapGraph passed incorrect node2 argument");
            Assert.AreEqual(graphToTest.Nodes, receivedAllNodes, "MapGraph passed incorrect allNodes argument");
        }

        [Test]
        public void OnGetShorestPathBetweenNodesCalled_CorrespondingMethodInMapGraphAlgorithmsIsCalled() {
            //Setup
            var mockAlgorithmSet = BuildMockMapAlgorithmSet();

            MapNodeBase receivedNode1 = null;
            MapNodeBase receivedNode2 = null;
            IEnumerable<MapNodeBase> receivedAllNodes = null;
            mockAlgorithmSet.GetShortestPathBetweenNodesCalled += delegate(MapNodeBase node1, MapNodeBase node2, IEnumerable<MapNodeBase> allNodes) {
                receivedNode1 = node1;
                receivedNode2 = node2;
                receivedAllNodes = allNodes;
            };

            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();
            graphToTest.AlgorithmSet = mockAlgorithmSet;

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);

            //Execution
            graphToTest.GetShortestPathBetweenNodes(centerNode, leftNode);

            //Validation
            Assert.AreEqual(centerNode, receivedNode1, "MapGraph passed incorrect node1 argument");
            Assert.AreEqual(leftNode, receivedNode2, "MapGraph passed incorrect node2 argument");
            Assert.AreEqual(graphToTest.Nodes, receivedAllNodes, "MapGraph passed incorrect allNodes argument");
        }

        [Test]
        public void OnGetNearestNodeToEdgeWhereCalled_CorrespondingMethodInMapGraphAlgorithmsIsCalled() {
            //Setup
            var mockAlgorithmSet = BuildMockMapAlgorithmSet();

            MapEdgeBase receivedEdgeOfOrigin = null;
            Predicate<MapNodeBase> receivedCondition = null;
            int receivedMaxDistance = -1;
            mockAlgorithmSet.GetNearestNodeToEdgeWhereCalled += delegate(MapEdgeBase edgeOfOrigin,
                Predicate<MapNodeBase> condition, int maxDistance) {

                receivedEdgeOfOrigin = edgeOfOrigin;
                receivedCondition = condition;
                receivedMaxDistance = maxDistance;
            };

            var graphToTest = BuildMapGraph();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();
            graphToTest.UIControl = BuildMockUIControl();
            graphToTest.AlgorithmSet = mockAlgorithmSet;

            var centerNode = graphToTest.BuildNode(Vector3.zero);
            var leftNode   = graphToTest.BuildNode(Vector3.left);
            var rightNode  = graphToTest.BuildNode(Vector3.right);
            var upNode     = graphToTest.BuildNode(Vector3.up);
            var downNode   = graphToTest.BuildNode(Vector3.down);

            var edgeToTest = graphToTest.BuildMapEdge(centerNode, leftNode);
            graphToTest.BuildMapEdge(centerNode, rightNode);
            graphToTest.BuildMapEdge(centerNode, upNode);
            graphToTest.BuildMapEdge(centerNode, downNode);

            //Execution
            graphToTest.GetNearestNodeToEdgeWhere(edgeToTest, MapNodeMockPredicate, 42);

            //Validation
            Assert.AreEqual(edgeToTest, receivedEdgeOfOrigin, "MapGraph passed incorrect edgeOfOrigin argument");
            Assert.That(receivedCondition == MapNodeMockPredicate, "MapGraph passed incorrect condition argument");
            Assert.AreEqual(42, receivedMaxDistance, "MapGraph passed incorrect maxDistance argument");
        }

        [Test]
        public void OnAnyMethodCalledWithNullArguments_ThrowsArgumentNullException() {
            //Setup
            var graphToTest = BuildMapGraph();
            graphToTest.UIControl = BuildMockUIControl();
            graphToTest.TerrainMaterialRegistry = BuildTerrainMaterialRegistry();

            var node1 = graphToTest.BuildNode(Vector3.zero);
            var node2 = graphToTest.BuildNode(Vector3.one);

            var edge1 = graphToTest.BuildMapEdge(node1, node2);

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.DestroyNode(null);
            }, "DestroyNode() fails to throw on a null 'node' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.SubscribeNode(null);
            }, "SubscribeNode() fails to throw on a null 'node' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.UnsubscribeNode(null);
            }, "UnubscribeNode() fails to throw on a null 'node' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.BuildMapEdge(null, node2);
            }, "BuildUndirectedEdge() fails to throw on a null 'firstEndpoint' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.BuildMapEdge(node1, null);
            }, "BuildUndirectedEdge() fails to throw on a null 'secondEndpoint' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.DestroyMapEdge(null, node2);
            }, "DestroyUndirectedEdge() fails to throw on a null 'firstEndpoint' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.DestroyMapEdge(node1, null);
            }, "DestroyUndirectedEdge() fails to throw on a null 'secondEndpoint' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.DestroyMapEdge(null);
            }, "DestroyUndirectedEdge() fails to throw on a null 'edge' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.SubscribeMapEdge(null);
            }, "SubscribeDirectedEdge() fails to throw on a null 'edge' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.UnsubscribeMapEdge(null);
            }, "UnsubscribeDirectedEdge() fails to throw on a null 'edge' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetEdge(node1, null);
            }, "GetEdge() fails to throw on a null 'endpointOne' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetEdge(null, node2);
            }, "GetEdge() fails to throw on a null 'endpointTwo' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetNeighborsOfNode(null);
            }, "GetNeighborsOfNode() fails to throw on a null 'node' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetEdgesAttachedToNode(null);
            }, "GetEdgesAttachedToNode() fails to throw on a null 'node' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetDistanceBetweenNodes(node1, null);
            }, "GetDistanceBetweenNodes() fails to throw on a null 'nodeOne' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetDistanceBetweenNodes(null, node2);
            }, "GetDistanceBetweenNodes() fails to throw on a null 'nodeTwo' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetShortestPathBetweenNodes(node1, null);
            }, "GetShortestPathBetweenNodes() fails to throw on a null 'nodeOne' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetShortestPathBetweenNodes(null, node2);
            }, "GetShortestPathBetweenNodes() fails to throw on a null 'nodeTwo' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetNearestNodeToEdgeWhere(null, MapNodeMockPredicate, 5);
            }, "GetShortestPathBetweenNodes() fails to throw on a null 'edgeOfOrigin' argument");

            Assert.Throws<ArgumentNullException>(delegate() {
                graphToTest.GetNearestNodeToEdgeWhere(edge1, null, 5);
            }, "GetShortestPathBetweenNodes() fails to throw on a null 'condition' argument");
        }

        #endregion

        #region utilities

        private MapGraph BuildMapGraph() {
            return (new GameObject()).AddComponent<MapGraph>();
        }

        private bool MapNodeMockPredicate(MapNodeBase node) {
            return true;
        }

        private MockUIControl BuildMockUIControl() {
            return (new GameObject()).AddComponent<MockUIControl>();
        }

        private TerrainMaterialRegistry BuildTerrainMaterialRegistry() {
            return (new GameObject()).AddComponent<TerrainMaterialRegistry>();
        }

        private MockBlobSiteConfiguration BuildMockBlobSiteConfiguration() {
            return (new GameObject()).AddComponent<MockBlobSiteConfiguration>();
        }

        private MapNode BuildMapNode() {
            var newNode = (new GameObject()).AddComponent<MapNode>();
            newNode.SetBlobSite(newNode.gameObject.AddComponent<MockBlobSite>());
            return newNode;
        }

        private MapEdge BuildMapEdge(MapNodeBase firstNode, MapNodeBase secondNode) {
            var newEdge = (new GameObject()).AddComponent<MapEdge>();

            newEdge.DisplayComponent = new GameObject().transform;
            newEdge.SetNodes(firstNode, secondNode);

            return newEdge;
        }

        private MockMapAlgorithmSet BuildMockMapAlgorithmSet() {
            return (new GameObject()).AddComponent<MockMapAlgorithmSet>();
        }

        private MapNodeBase GetEquivalentNode(MapNodeBase nodeToTestAgainst, ReadOnlyCollection<MapNodeBase> candidates) {
            return candidates.Where(delegate(MapNodeBase candidate) {
                return (
                    candidate.transform.localPosition.Equals(nodeToTestAgainst.transform.localPosition) &&
                    candidate.Terrain == nodeToTestAgainst.Terrain
                );
            }).FirstOrDefault();
        }

        #endregion

        #endregion

    }

}
