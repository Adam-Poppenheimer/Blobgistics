using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;

namespace Assets.Highways.Editor {

    public class BlobTubeTests {

        #region instance methods

        #region basic functionality testing

        [Test]
        public void OnEndpointsSet_BlobTubeHasAppropriateSourceAndTarget() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            var hostingGameObject = new GameObject();
            var tubeToTest = hostingGameObject.AddComponent<BlobTube>();
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

            var blobToAdd = BuildResourceBlob();

            //Execution
            var canAddBlob = tubeToTest.CanPushBlobInto(blobToAdd);

            //Validation
            Assert.That(!canAddBlob);
        }

        [Test]
	    public void OnAttemptingToPushPermittedBlobIntoTube_BlobTubeContainsPushedBlob(){
		    //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

            var blobToAdd = BuildResourceBlob();

            //Execution
            tubeToTest.SetPermissionForResourceType(blobToAdd.BlobType, true);
            tubeToTest.PushBlobInto(blobToAdd);

            //Validation
            Assert.That(tubeToTest.BlobsWithin.Contains(blobToAdd));
	    }

        [Test]
        public void OnManyBlobsPushedInto_BlobTubeHasAllBlobsAndMaintainsTheirOrderInternally() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
                Assert.That(tubeToTest.BlobsWithin[i] == blobsToAdd[i],
                    string.Format("ResourceBlob {0} was not in its expected place", i));
            }
        }

        [Test]
        public void OnBlobCapacityReached_CanPushBlobIntoReturnsFalse() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 5;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 0.1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
                    if(!tubeToTest.BlobsWithin.Contains(blobBeingLookedAt)) {
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
            tubeToTest.SetEndpoints(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            tubeToTest.SetPermissionForResourceType(ResourceType.Red, true);

            var blobToManipulate = BuildResourceBlob(ResourceType.Red);

            //Execute
            tubeToTest.PushBlobInto(blobToManipulate);
            tubeToTest.PullBlobFrom(blobToManipulate);
            var blobIsInTube = tubeToTest.BlobsWithin.Contains(blobToManipulate);

            //Validate
            Assert.That(!blobIsInTube);
        }

        [Test]
        public void WhenMultipleBlobsPresent_OnlyEarliestBlobInsertedCanBePulled() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 10;
            privateData.TransportSpeedPerSecond = 1f;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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

        #endregion

        #region error testing

        [Test]
        public void OnNullBlobPushed_ThrowsArgumentNullException() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                tubeToTest.PushBlobInto(null);
            });
        }

        [Test]
        public void OnNullBlobTestedForPushability_ThrowsArgumentNullException() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                tubeToTest.CanPushBlobInto(null);
            });
        }

        [Test]
        public void OnCanPushBlobIntoReturnsFalse_PushingBlobIntoThrowsBlobTubeException() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;
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
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

            var blobToPull = BuildResourceBlob(ResourceType.Red);

            //Execution and Validation
            Assert.Throws<BlobTubeException>(delegate() {
                tubeToTest.CanPullBlobFrom(blobToPull);
            });
        }

        [Test]
        public void OnBlobNotInTheTubeIsPulled_ThrowBlobTubeException() {
            //Setup
            var privateData = new MockBlobTubePrivateData();
            privateData.Capacity = 1;

            var tubeGameObject = new GameObject();
            var tubeToTest = tubeGameObject.AddComponent<BlobTube>();
            tubeToTest.PrivateData = privateData;

            var blobToPull = BuildResourceBlob(ResourceType.Red);

            //Execution and Validation
            Assert.Throws<BlobTubeException>(delegate() {
                tubeToTest.PullBlobFrom(blobToPull);
            });
        }

        #endregion

        #region utility methods

        private ResourceBlob BuildResourceBlob(ResourceType typeOfBlob = ResourceType.Red) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfBlob;
            return newBlob;
        }

        #endregion

        #endregion

    }

}


