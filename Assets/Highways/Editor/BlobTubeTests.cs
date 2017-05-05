using System;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;

using Assets.Highways.ForTesting;

namespace Assets.Highways.Editor {

    public class BlobTubeTests {

        #region instance methods

        #region testing

        #region functionality

        [Test]
        public void Factory_OnConstructTube_ConstructedTubeHasCorrectEndpoints() {
            //Setup
            var factoryToTest = BuildTubeFactory();

            //Execution
            var constructedTube = factoryToTest.ConstructTube(Vector3.left, Vector3.right);

            //Validation
            Assert.AreEqual(Vector3.left, constructedTube.SourceLocation, "ConstructedTube has incorrect SourceLocation");
            Assert.AreEqual(Vector3.right, constructedTube.TargetLocation, "ConstructedTube has incorrect TargetLocation");
        }

        [Test]
        public void OnEndpointsSet_BlobTubeHasAppropriateSourceAndTarget() {
            //Setup
            var tubeToTest = BuildBlobTube(BuildPrivateData());
            var source = new Vector3(-10f, 0, 0);
            var target = new Vector3(10f, 0, 0);

            //Execution
            tubeToTest.SetEndpoints(source, target);

            //Validation
            Assert.That(
                tubeToTest.SourceLocation.Equals(source) &&
                tubeToTest.TargetLocation.Equals(target)
            );
        }

        [Test]
        public void OnBlobTubePermittedToReceiveATypeOfBlob_BlobTubeCanReceivePushedBlobOfThatType() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            var blobToAdd = BuildRealResourceBlob();

            //Execution
            tubeToTest.SetPermissionForResourceType(blobToAdd.BlobType, true);
            var canAddBlob = tubeToTest.CanPushBlobInto(blobToAdd);

            //Validation
            Assert.That(canAddBlob);
        }

        [Test]
        public void OnBlobTubeForbiddenFromReceivingATypeOfBlob_BlobTubeCannotReceivePushedBlobOfThatType() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            var blobToAdd = BuildRealResourceBlob();

            //Execution
            tubeToTest.SetPermissionForResourceType(blobToAdd.BlobType, false);
            var canAddBlob = tubeToTest.CanPushBlobInto(blobToAdd);

            //Validation
            Assert.That(!canAddBlob);
        }

        [Test]
        public void OnBlobTubeNotPermittedToReceiveATypeOfBlob_DefaultBehaviourIsToForbidBlobOfThatType() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            var blobToAdd = BuildRealResourceBlob();

            //Execution
            var canAddBlob = tubeToTest.CanPushBlobInto(blobToAdd);

            //Validation
            Assert.That(!canAddBlob);
        }

        [Test]
	    public void OnAttemptingToPushPermittedBlobIntoTube_BlobTubeContainsPushedBlob(){
		    //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            var blobToAdd = BuildRealResourceBlob();

            //Execution
            tubeToTest.SetPermissionForResourceType(blobToAdd.BlobType, true);
            tubeToTest.PushBlobInto(blobToAdd);

            //Validation
            Assert.That(tubeToTest.Contents.Contains(blobToAdd));
	    }

        [Test]
        public void OnManyBlobsPushedInto_BlobTubeHasAllBlobsAndMaintainsTheirOrderInternally() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            ResourceBlobBase[] blobsToAdd = new ResourceBlobBase[] {
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
            };

            //Execution
            for(int i = 0; i < blobsToAdd.Length; ++i) {
                tubeToTest.PushBlobInto(blobsToAdd[i]);
            }

            //Validation
            for(int i = 0; i < blobsToAdd.Length; ++i) {
                Assert.That(tubeToTest.Contents[i] == blobsToAdd[i],
                    string.Format("ResourceBlob {0} was not in its expected place", i));
            }
        }

        [Test]
        public void OnBlobCapacityReached_CanPushBlobIntoReturnsFalse() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 5;
            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            ResourceBlobBase[] blobsToAdd = new ResourceBlobBase[] {
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
            };

            //Execution
            for(int i = 0; i < blobsToAdd.Length - 1; ++i) {
                tubeToTest.PushBlobInto(blobsToAdd[i]);
            }

            //Validation
            Assert.That(!tubeToTest.CanPushBlobInto(blobsToAdd[blobsToAdd.Length - 1]));
        }

        [Test]
        public void OnBlobAlreadyInTube_CanPushBlobIntoReturnsFalse() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);
            var blobToAdd = BuildRealResourceBlob(ResourceType.Food);

            //Execution
            tubeToTest.PushBlobInto(blobToAdd);

            //Validation
            Assert.That(!tubeToTest.CanPushBlobInto(blobToAdd));
        }

        [Test]
        public void OnOneResourceTypePermittedInTube_CanPushBlobReturnsFalseOnOtherResourceTypes() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            var greenBlob = BuildRealResourceBlob(ResourceType.Textiles);
            var blueBlob  = BuildRealResourceBlob(ResourceType.ServiceGoods );

            //Execution
            var canPushGreen = tubeToTest.CanPushBlobInto(greenBlob);
            var canPushBlue = tubeToTest.CanPushBlobInto(blueBlob);

            //Validation
            Assert.That(!canPushGreen && !canPushBlue);
        }

        [Test]
        public void OnBlobPushedIntoTube_BlobIsGivenGoalToMoveToStartOfTube_ThenEndOfTube() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;
            
            var endpoint1 = new Vector3(-10f, 0f, ResourceBlobBase.DesiredZPositionOfAllBlobs);
            var endpoint2 = new Vector3(10f, 0f, ResourceBlobBase.DesiredZPositionOfAllBlobs);
            tubeToTest.SetEndpoints(endpoint1, endpoint2);
            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            var blobHost = new GameObject();
            var blobToAdd = blobHost.AddComponent<MockResourceBlob>();

            //Execute
            tubeToTest.PushBlobInto(blobToAdd);

            //Validation
            Assert.AreEqual(2, blobToAdd.PushedGoals.Count, "BlobToAdd has an incorrect number of pushed goals");
            Assert.AreEqual(endpoint1, blobToAdd.PushedGoals[0].DesiredLocation, "BlobToAdd's first MovementGoal points to the wrong destination");
            Assert.AreEqual(endpoint2, blobToAdd.PushedGoals[1].DesiredLocation, "BlobToAdd's second MovementGoal points to the wrong destination");
        }

        [Test]
        public void OnBlobInTube_AndMovementSpeedNonzero_BlobWillEventuallyBecomePullable() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 0.1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(10f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            var movingBlob = BuildRealResourceBlob(ResourceType.Food);

            //Execution
            bool hasReachedEndOfTube = false;
            tubeToTest.PushBlobInto(movingBlob);
            for(int i = 0; i < 100000; ++i) {
                movingBlob.Tick(0.01f);
                if(tubeToTest.CanPullBlobFrom(movingBlob)) {
                    hasReachedEndOfTube = true;
                    break;
                }
            }

            //Validation
            Assert.That(hasReachedEndOfTube);
        }
    
        [Test]
        public void BlobIsInTubeButNotAtItsEnd_BlobCannotBePulledFromTube() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            var blobToManipulate = BuildRealResourceBlob(ResourceType.Food);

            //Execution
            tubeToTest.PushBlobInto(blobToManipulate);
            var canBePulled = tubeToTest.CanPullBlobFrom(blobToManipulate);

            //Validation
            Assert.That(!canBePulled);
        }

        [Test]
        public void OnResourcePulledFrom_BlobTubeDoesNotContainPulledBlob() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);

            var blobToManipulate = BuildRealResourceBlob(ResourceType.Food);
            blobToManipulate.transform.position = Vector3.zero;

            //Execute
            tubeToTest.PushBlobInto(blobToManipulate);
            blobToManipulate.Tick(100f);
            tubeToTest.PullBlobFrom(blobToManipulate);
            var blobIsInTube = tubeToTest.Contents.Contains(blobToManipulate);

            //Validate
            Assert.That(!blobIsInTube);
        }

        [Test]
        public void OnRemoveBlobFromCalled_BlobNoLongerPresentInBlobsWithin_ButRemainingBlobsMaintainOrder() {
            //Setup
            var tubeToTest = BuildBlobTube(BuildPrivateData());
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Textiles, true);

            ResourceBlobBase[] blobsToManipulate = new ResourceBlobBase[] {
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Textiles),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
            };

            foreach(var blob in blobsToManipulate) {
                tubeToTest.PushBlobInto(blob);
            }

            //Execution
            tubeToTest.RemoveBlobFrom(blobsToManipulate[2]);

            //Validate
            Assert.AreEqual(blobsToManipulate[0], tubeToTest.Contents[0],    "First blob is not in its intended position");
            Assert.AreEqual(blobsToManipulate[1], tubeToTest.Contents[1],    "Second blob is not in its intended position");

            Assert.That(!tubeToTest.Contents.Contains(blobsToManipulate[2]), "Third blob is still contained in Contents");

            Assert.AreEqual(blobsToManipulate[3], tubeToTest.Contents[2],    "Fourth blob is not in its intended position");
            Assert.AreEqual(blobsToManipulate[4], tubeToTest.Contents[3],    "Fifth blob is not in its intended position");
        }

        [Test]
        public void OnClearCalled_BlobsWithinBecomesEmpty() {
            //Setup
            var tubeToTest = BuildBlobTube(BuildPrivateData());
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Textiles, true);

            ResourceBlobBase[] blobsToManipulate = new ResourceBlobBase[] {
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Textiles),
                BuildRealResourceBlob(ResourceType.Food),
                BuildRealResourceBlob(ResourceType.Food),
            };

            foreach(var blob in blobsToManipulate) {
                tubeToTest.PushBlobInto(blob);
            }

            //Execution
            tubeToTest.Clear();

            //Validation
            Assert.AreEqual(0, tubeToTest.Contents.Count);
        }

        [Test]
        public void OnSetPermissionForResourceTypeCalled_GetPermissionForResourceTypeReturnsCorrectValues() {
            //Setup
            var tubeToTest = BuildBlobTube(BuildPrivateData());

            //Execution
            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Textiles, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.ServiceGoods, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.ServiceGoods, false);

            //Validation
            Assert.That(tubeToTest.GetPermissionForResourceType(ResourceType.Food),   "Food is not permitted");
            Assert.That(tubeToTest.GetPermissionForResourceType(ResourceType.Textiles), "Textiles is not permitted");
            Assert.False(tubeToTest.GetPermissionForResourceType(ResourceType.ServiceGoods), "ServiceGoods is falsely permitted");
        }

        #endregion

        #region error handling

        [Test]
        public void Factory_OnDestroyTubePassedNullArgument_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildTubeFactory();

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.DestroyTube(null);
            });
        }

        [Test]
        public void OnNullBlobPushed_ThrowsArgumentNullException() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                tubeToTest.PushBlobInto(null);
            });
        }

        [Test]
        public void OnNullBlobTestedForPushability_ThrowsArgumentNullException() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                tubeToTest.CanPushBlobInto(null);
            });
        }

        [Test]
        public void OnCanPushBlobIntoReturnsFalse_PushingBlobIntoThrowsBlobTubeException() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, false);

            var blobToPush = BuildRealResourceBlob(ResourceType.Food);

            //Execution and Validation
            Assert.Throws<BlobTubeException>(delegate() {
                if(!tubeToTest.CanPushBlobInto(blobToPush)) {
                    tubeToTest.PushBlobInto(blobToPush);
                }
            });
        }

        [Test]
        public void OnCanPullBlobFromReturnsFalse_PullingBlobFromThrowsBlobTubeException() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            tubeToTest.SetPermissionForResourceType(ResourceType.Food, true);
            tubeToTest.SetEndpoints(Vector3.zero, new Vector3(10f, 0f, 0f));

            var blobToPull = BuildRealResourceBlob(ResourceType.Food);

            //Execution
            tubeToTest.PushBlobInto(blobToPull);

            //Validation
            Assert.Throws<BlobTubeException>(delegate() {
                if(!tubeToTest.CanPullBlobFrom(blobToPull)) {
                    tubeToTest.PullBlobFrom(blobToPull);
                }
            });
        }

        [Test]
        public void OnBlobNotInTheTubeIsCheckedForPullability_ThrowsBlobTubeException() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            var blobToPull = BuildRealResourceBlob(ResourceType.Food);

            //Execution and Validation
            Assert.Throws<BlobTubeException>(delegate() {
                tubeToTest.CanPullBlobFrom(blobToPull);
            });
        }

        [Test]
        public void OnBlobNotInTheTubeIsPulled_ThrowBlobTubeException() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;

            var blobToPull = BuildRealResourceBlob(ResourceType.Food);

            //Execution and Validation
            Assert.Throws<BlobTubeException>(delegate() {
                tubeToTest.PullBlobFrom(blobToPull);
            });
        }

        [Test]
        public void OnNullBlobIsRemoved_ThrowArgumentNullException() {
            //Setup
            var tubeToTest = BuildBlobTube(BuildPrivateData());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                tubeToTest.RemoveBlobFrom(null);
            });
        }

        #endregion

        #endregion

        #region utility methods

        private MockBlobTubePrivateData BuildPrivateData() {
            var hostingObject = new GameObject();
            var newPrivateData = hostingObject.AddComponent<MockBlobTubePrivateData>();
            newPrivateData.SetBlobFactory(hostingObject.AddComponent<MockResourceBlobFactory>());
            return newPrivateData;
        }

        private BlobTube BuildBlobTube(MockBlobTubePrivateData privateData) {
            var newBlobTube = privateData.gameObject.AddComponent<BlobTube>();
            newBlobTube.PrivateData = privateData;
            return newBlobTube;
        }

        private ResourceBlobBase BuildRealResourceBlob(ResourceType typeOfBlob = ResourceType.Food) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfBlob;
            return newBlob;
        }

        private BlobTubeFactory BuildTubeFactory() {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobTubeFactory>();
            var newPrivateData = hostingObject.AddComponent<BlobTubePrivateData>();
            newPrivateData.SetBlobFactory(hostingObject.AddComponent<MockResourceBlobFactory>());
            newFactory.TubePrivateData = newPrivateData;
            return newFactory;
        }

        private void TickBlobsWithin(BlobTube tube, float secondsPassed) {
            foreach(var blob in tube.Contents) {
                blob.Tick(secondsPassed);
            }
        }

        #endregion

        #endregion

    }

}


