using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.BlobEngine;

public class BlobTubeTests {

    #region instance fields and properties

    private BlobTubePrivateData TubePrivateData {
        get {
            if(_tubePrivateData == null) {
                var objectWithData = GameObject.Find("FactoryBuilder/ForBlobTube");
                _tubePrivateData = objectWithData.GetComponent<BlobTubePrivateData>();
            }
            return _tubePrivateData;
        }
    }
    private BlobTubePrivateData _tubePrivateData;

    #endregion

    #region instance methods

    #region environment testing

    [Test]
    public void SceneContainsFactoryBuilderStructure() {
        var factoryBuilderObject = GameObject.Find("FactoryBuilder");
        Assert.IsNotNull(factoryBuilderObject, "Did not find a GameObject named FactoryBuilder in the scene hierarchy");
    }

    [Test]
    public void SceneContainsProperPrivateData() {
        Assert.IsNotNull(TubePrivateData, "Could not find the BlobTubePrivateData");
    }

    #endregion

    #region basic functionality testing

    [Test]
    public void OnEndpointsSet_BlobTubeHasAppropriateSourceAndTarget() {
        throw new NotImplementedException();
    }

    [Test]
	public void OnResourceBlobPushedInto_BlobTubeContainsPushedBlob(){
		throw new NotImplementedException();
	}

    [Test]
    public void OnResourcePulledFrom_BlobTubeDoesNotContainPulledBlob() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnManyBlobsPushedInto_BlobTubeMaintainsTheirOrderInternally() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnBlobTubeMovementTicked_AllBlobsInTubeAreModified() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnBlobTubeMovementTicked_NoRemovedBlobsAreModified() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnBlobTubeMovementTicked_BlobsThatHaveReachedTheEndOfTheTubeArePulledFromIt() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnBlobTubeMovementTicked_BlobsReachTheEndInTheOrderTheyWerePushed() {
        throw new NotImplementedException();
    }

    [Test]
    public void WhenBlobIsAtEndOfTube_BlobCanBePulledFromTube() {
        throw new NotImplementedException();
    }
    
    [Test]
    public void BlobIsInTubeButNotAtItsEnd_BlobCannotBePulledFromTube() {
        throw new NotImplementedException();
    }

    [Test]
    public void WhenMultipleBlobsPresent_OnlyBlobInsertedEarliestCanBePulled() {
        throw new NotImplementedException();
    }

    #endregion

    #region error testing

    [Test]
    public void OnNullBlobPushed_ThrowsArgumentNullException() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnNullBlobTestedForPushability_ThrowsArgumentNullException() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnCanPushBlobIntoReturnsFalse_PushingBlobIntoThrowsBlobTubeException() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnCanPullBlobFromReturnsFalse_PullingBlobFromThrowsBlobTubeException() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnBlobNotInTheTubeIsCheckedForPullability_ThrowsInvalidArgumentException() {
        throw new NotImplementedException();
    }

    [Test]
    public void OnBlobNotInTheTubeIsPulled_ThrowBlobTubeException() {
        throw new NotImplementedException();
    }

    #endregion

    #endregion

}
