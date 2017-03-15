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

            var blobToAdd = BuildResourceBlob();

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

            var blobToAdd = BuildResourceBlob();

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

            var blobToAdd = BuildResourceBlob();

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

            var blobToAdd = BuildResourceBlob();

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

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            ResourceBlob[] blobsToAdd = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
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
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            ResourceBlob[] blobsToAdd = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
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

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);
            var blobToAdd = BuildResourceBlob(ResourceType.Red);

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

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var greenBlob = BuildResourceBlob(ResourceType.Green);
            var blueBlob  = BuildResourceBlob(ResourceType.Blue );

            //Execution
            var canPushGreen = tubeToTest.CanPushBlobInto(greenBlob);
            var canPushBlue = tubeToTest.CanPushBlobInto(blueBlob);

            //Validation
            Assert.That(!canPushGreen && !canPushBlue);
        }

        [Test]
        public void OnBlobPushedIntoTube_BlobIsNowAtSourceLocationOfTube() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 1;
            
            tubeToTest.SetEndpoints(new Vector3(-10f, 0f, 0f), new Vector3(10f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var blobToAdd = BuildResourceBlob(ResourceType.Red);

            //Execute
            tubeToTest.PushBlobInto(blobToAdd);

            //Validate
            Assert.That(blobToAdd.transform.position.Equals(tubeToTest.SourceLocation));
        }

        [Test]
        public void OnBlobTubeMovementTicked_AllBlobsInTubeAreMoved() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(10f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            ResourceBlob[] blobsToAdd = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
            };

            //Execution
            for(int i = 0; i < blobsToAdd.Length; ++i) {
                tubeToTest.PushBlobInto(blobsToAdd[i]);
            }
            tubeToTest.TickMovement(1f);

            //Validate
            foreach(var blob in blobsToAdd) {
                Assert.That(Mathf.Approximately(blob.transform.position.x, 1f));
            }
        }

        [Test]
        public void OnBlobInTube_AndMovementSpeedNonzero_BlobWillEventuallyBecomePullable() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 0.1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(10f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var movingBlob = BuildResourceBlob(ResourceType.Red);

            //Execution
            bool hasReachedEndOfTube = false;
            tubeToTest.PushBlobInto(movingBlob);
            for(int i = 0; i < 100000; ++i) {
                tubeToTest.TickMovement(0.01f);
                if(tubeToTest.CanPullBlobFrom(movingBlob)) {
                    hasReachedEndOfTube = true;
                    break;
                }
            }

            //Validation
            Assert.That(hasReachedEndOfTube);
        }

        [Test]
        public void WhenBlobIsVeryCloseToEndOfTube_BlobCanBePulledFromTube() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var blobToManipulate = BuildResourceBlob(ResourceType.Red);

            //Execution
            tubeToTest.PushBlobInto(blobToManipulate);
            tubeToTest.TickMovement(1f);
            var canBePulled = tubeToTest.CanPullBlobFrom(blobToManipulate);

            //Validation
            Assert.That(canBePulled);
        }
    
        [Test]
        public void BlobIsInTubeButNotAtItsEnd_BlobCannotBePulledFromTube() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var blobToManipulate = BuildResourceBlob(ResourceType.Red);

            //Execution
            tubeToTest.PushBlobInto(blobToManipulate);
            var canBePulled = tubeToTest.CanPullBlobFrom(blobToManipulate);

            //Validation
            Assert.That(!canBePulled);
        }

        [Test]
        public void BlobInTubeHasReachedEnd_FurtherMovementTicksDoNotChangeBlobsPositionOrPullability() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var blobToManipulate = BuildResourceBlob(ResourceType.Red);

            //Execution
            tubeToTest.PushBlobInto(blobToManipulate);
            tubeToTest.TickMovement(1f);
            var canBePulled = tubeToTest.CanPullBlobFrom(blobToManipulate);
            var blobPositionX = blobToManipulate.transform.position.x;

            //Validation
            Assert.That(canBePulled, "Blob cannot be pulled to begin with");
            for(int i = 1; i < 11; ++i) {
                tubeToTest.TickMovement(1f);
                Assert.That(tubeToTest.CanPullBlobFrom(blobToManipulate),
                    string.Format("Blob lost the ability to be pulled on cycle {0}", i));
                Assert.That(Mathf.Approximately(blobPositionX, tubeToTest.TargetLocation.x),
                    string.Format("Blob wandered away from target location on cycle {0}", i));
            }
        }

        [Test]
        public void OnBlobTubeMovementTicked_BlobsReachTheEndInTheOrderTheyWerePushed() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(5f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            ResourceBlob[] blobsToManipulate = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
            };

            //Execution
            foreach(var blob in blobsToManipulate) {
                tubeToTest.PushBlobInto(blob);
                tubeToTest.TickMovement(1f);
            }

            //Validation
            for(int removalIndex = 0; removalIndex < 5; ++removalIndex) {
                for(int blobIndex = 0; blobIndex < blobsToManipulate.Length; ++blobIndex) {
                    var blobBeingLookedAt = blobsToManipulate[blobIndex];
                    if(!tubeToTest.Contents.Contains(blobBeingLookedAt)) {
                        continue;
                    }
                    if(blobIndex <= removalIndex) {
                        Assert.That(tubeToTest.CanPullBlobFrom(blobBeingLookedAt));
                        tubeToTest.PullBlobFrom(blobBeingLookedAt);
                    }else {
                        Assert.That(!tubeToTest.CanPullBlobFrom(blobBeingLookedAt));
                    }
                }
                tubeToTest.TickMovement(1f);
            }
        }

        [Test]
        public void OnResourcePulledFrom_BlobTubeDoesNotContainPulledBlob() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var blobToManipulate = BuildResourceBlob(ResourceType.Red);

            //Execute
            tubeToTest.PushBlobInto(blobToManipulate);
            tubeToTest.PullBlobFrom(blobToManipulate);
            var blobIsInTube = tubeToTest.Contents.Contains(blobToManipulate);

            //Validate
            Assert.That(!blobIsInTube);
        }

        [Test]
        public void WhenMultipleBlobsPresent_OnlyEarliestBlobInsertedCanBePulled() {
            //Setup
            var privateData = BuildPrivateData();

            var tubeToTest = BuildBlobTube(privateData);
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;
            
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            ResourceBlob[] blobsToManipulate = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
            };

            //Execution
            foreach(var blob in blobsToManipulate) {
                tubeToTest.PushBlobInto(blob);
            }

            //Validation
            for(int outerBlobIndex = 0; outerBlobIndex < blobsToManipulate.Length; ++outerBlobIndex) {
                for(int innerBlobIndex = outerBlobIndex + 1; innerBlobIndex < blobsToManipulate.Length; ++innerBlobIndex) {
                    var blobNotToPull = blobsToManipulate[innerBlobIndex];
                    Assert.That(
                        !tubeToTest.CanPullBlobFrom(blobNotToPull),
                        string.Format("Blob {0} is not leading the list and should not have be pullable, but was", innerBlobIndex)
                    );
                }
                var blobToPull = blobsToManipulate[outerBlobIndex];
                Assert.That(
                    tubeToTest.CanPullBlobFrom(blobToPull), 
                    string.Format("Blob {0} is at the front of the list and should've been pullable, but was not", outerBlobIndex)
                );
                tubeToTest.PullBlobFrom(blobToPull);
            }
        }

        [Test]
        public void OnRemoveBlobFromCalled_BlobNoLongerPresentInBlobsWithin_ButRemainingBlobsMaintainOrder() {
            //Setup
            var tubeToTest = BuildBlobTube(BuildPrivateData());
            tubeToTest.Capacity = 10;
            tubeToTest.TransportSpeedPerSecond = 1f;

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Green, true);

            ResourceBlob[] blobsToManipulate = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Green),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
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

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Green, true);

            ResourceBlob[] blobsToManipulate = new ResourceBlob[] {
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Green),
                BuildResourceBlob(ResourceType.Red),
                BuildResourceBlob(ResourceType.Red),
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
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Green, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Blue, true);
            tubeToTest.SetPermissionForResourceType(ResourceType.Blue, false);

            //Validation
            Assert.That(tubeToTest.GetPermissionForResourceType(ResourceType.Red),   "Red is not permitted");
            Assert.That(tubeToTest.GetPermissionForResourceType(ResourceType.Green), "Green is not permitted");
            Assert.False(tubeToTest.GetPermissionForResourceType(ResourceType.Blue), "Blue is falsely permitted");
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

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, false);

            var blobToPush = BuildResourceBlob(ResourceType.Red);

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

            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);
            tubeToTest.SetEndpoints(Vector3.zero, new Vector3(10f, 0f, 0f));

            var blobToPull = BuildResourceBlob(ResourceType.Red);

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

            var blobToPull = BuildResourceBlob(ResourceType.Red);

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

            var blobToPull = BuildResourceBlob(ResourceType.Red);

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

        private ResourceBlob BuildResourceBlob(ResourceType typeOfBlob = ResourceType.Red) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfBlob;
            return newBlob;
        }

        private BlobTubeFactory BuildTubeFactory() {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobTubeFactory>();
            newFactory.BlobFactory = hostingObject.AddComponent<MockResourceBlobFactory>();
            return newFactory;
        }

        #endregion

        #endregion

    }

}


