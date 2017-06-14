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

    public class BlobHighwayFactoryTests {

        #region instance methods

        #region tests

        [Test]
        public void Factory_OnManyHighwaysCreatedAndDestroyed_NoTwoHighwaysHaveTheSameID() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph;

            var nodeList = new List<KeyValuePair<MapNodeBase, MapNodeBase>>();
            for(int nodeIndex = 0; nodeIndex < 60; ++nodeIndex) {
                var firstNode = mapGraph.BuildNode(Vector3.zero);
                var secondNode = mapGraph.BuildNode(Vector3.left);
                mapGraph.BuildMapEdge(firstNode, secondNode);
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

            mapGraph.BuildMapEdge(middleNode, leftNode);
            mapGraph.BuildMapEdge(middleNode, rightNode);
            mapGraph.BuildMapEdge(middleNode, upNode);

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

            mapGraph.BuildMapEdge(middleNode, leftNode);
            mapGraph.BuildMapEdge(middleNode, rightNode);
            mapGraph.BuildMapEdge(middleNode, upNode);

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

            mapGraph.BuildMapEdge(middleNode, leftNode);
            mapGraph.BuildMapEdge(middleNode, rightNode);
            mapGraph.BuildMapEdge(middleNode, upNode);

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

            mapGraph.BuildMapEdge(middleNode, leftNode);
            mapGraph.BuildMapEdge(middleNode, rightNode);
            mapGraph.BuildMapEdge(middleNode, upNode);

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

            mapGraph.BuildMapEdge(middleNode, leftNode);
            mapGraph.BuildMapEdge(middleNode, rightNode);
            mapGraph.BuildMapEdge(middleNode, upNode);

            //Execution
            var highwayToLeft = factoryToTest.ConstructHighwayBetween(middleNode, leftNode);

            //Validation
            Assert.That(
                (highwayToLeft.FirstEndpoint == middleNode && highwayToLeft.SecondEndpoint == leftNode  ) ||
                (highwayToLeft.FirstEndpoint == leftNode   && highwayToLeft.SecondEndpoint == middleNode)
            );
        }

        [Test]
        public void Factory_OnMapGraphRaisesMapNodeUnsubscribedEvent_HighwaysAttachedToNodeAreDestroyed() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph as MockMapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            mapGraph.BuildMapEdge(middleNode, leftNode);
            mapGraph.BuildMapEdge(middleNode, rightNode);
            mapGraph.BuildMapEdge(middleNode, upNode);

            var highwayToLeft  = factoryToTest.ConstructHighwayBetween(middleNode, leftNode);
            var highwayToRight = factoryToTest.ConstructHighwayBetween(middleNode, rightNode);
            var highwayToUp    = factoryToTest.ConstructHighwayBetween(middleNode, upNode);

            //Execution and validation
            mapGraph.UnsubscribeNode(leftNode);
            Assert.That(highwayToLeft == null, "HighwayToLeft was not destroyed when LeftNode was unsubscribed");

            mapGraph.UnsubscribeNode(middleNode);
            Assert.AreEqual(0, factoryToTest.Highways.Count, "Factory still has highways even after MiddleNode was destroyed");
        }

        [Test]
        public void Factory_OnMapGraphRaisesMapEdgeUnsubscribedEvent_HighwayOnEdgeIsDestroyed() {
            //Setup
            var factoryToTest = BuildHighwayFactory();
            var mapGraph = factoryToTest.MapGraph as MockMapGraph;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var leftNode   = mapGraph.BuildNode(Vector3.left);
            var rightNode  = mapGraph.BuildNode(Vector3.right);
            var upNode     = mapGraph.BuildNode(Vector3.up);

            var edgeToLeft  = mapGraph.BuildMapEdge(middleNode, leftNode);
            var edgeToRight = mapGraph.BuildMapEdge(middleNode, rightNode);
            var edgeToUp    = mapGraph.BuildMapEdge(middleNode, upNode);

            var highwayToLeft  = factoryToTest.ConstructHighwayBetween(middleNode, leftNode);
            var highwayToRight = factoryToTest.ConstructHighwayBetween(middleNode, rightNode);
            var highwayToUp    = factoryToTest.ConstructHighwayBetween(middleNode, upNode);

            //Execution and validation
            mapGraph.UnsubscribeMapEdge(edgeToLeft);
            Assert.That(highwayToLeft == null, "HighwayToLeft was not destroyed when EdgeToLeft was unsubscribed");

            mapGraph.UnsubscribeMapEdge(edgeToRight);
            Assert.That(highwayToRight == null, "HighwayToRight was not destroyed when EdgeToRight was unsubscribed");

            mapGraph.UnsubscribeMapEdge(edgeToUp);
            Assert.That(highwayToUp == null, "HighwayToUp was not destroyed when EdgeToUp was unsubscribed");
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

            mapGraph.BuildMapEdge(firstEndpoint, secondEndpoint);

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

        #region utilities

        private BlobHighwayFactory BuildHighwayFactory() {
            var hostingObject = new GameObject();

            var newBlobFactory = hostingObject.AddComponent<MockResourceBlobFactory>();

            var newMapGraph = hostingObject.AddComponent<MockMapGraph>();

            var newTubeFactory = hostingObject.AddComponent<BlobTubeFactory>();
            var newTubePrivateData = hostingObject.AddComponent<BlobTubePrivateData>();
            newTubePrivateData.SetBlobFactory(newBlobFactory);
            newTubeFactory.TubePrivateData = newTubePrivateData;

            var newHighwayFactory = hostingObject.AddComponent<BlobHighwayFactory>();
            newHighwayFactory.MapGraph = newMapGraph;
            newHighwayFactory.BlobTubeFactory = newTubeFactory;
            newHighwayFactory.HighwayProfile = BuildBlobHighwayProfile(1, 10, 1f);
            newHighwayFactory.BlobFactory = newBlobFactory;

            return newHighwayFactory;
        }

        private MockMapNode BuildMapNode() {
            var hostingObject = new GameObject();

            var newBlobSiteData = hostingObject.AddComponent<MockBlobSiteConfiguration>();
            newBlobSiteData.SetConnectionCircleRadius(1f);

            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.Configuration = newBlobSiteData;
            newBlobSite.TotalCapacity = Int32.MaxValue;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                newBlobSite.SetCapacityForResourceType(resourceType, Int32.MaxValue);
            }

            var newMapNode = hostingObject.AddComponent<MockMapNode>();
            newMapNode.blobSite = newBlobSite;

            return newMapNode;
        }

        private BlobHighwayProfile BuildBlobHighwayProfile(float blobSpeedPerSecond, int capacity, float BlobPullCooldownInSeconds) {
            var newProfile = new BlobHighwayProfile();

            newProfile.BlobSpeedPerSecond = blobSpeedPerSecond;
            newProfile.Capacity = capacity;
            newProfile.BlobPullCooldownInSeconds = BlobPullCooldownInSeconds;

            return newProfile;
        }

        #endregion

        #endregion

    }

}
