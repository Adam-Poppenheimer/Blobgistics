using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;

namespace Assets.Highways.Editor {

    public class BlobHighwayTests {

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnEndpointsSet_FirstAndSecondEndpointAreSetProperly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionForResourceTypeSet_SamePermissionForResourceTypeIsGotten() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionSetForFirstEndpoint_TubePullingFromFirstEndpointAlsoHasNewPermission() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionSetForSecondEndpoint_TubePullingFromSecondEndpointAlsoHasNewPermission() {
            throw new NotImplementedException();
        }

        [Test]
        public void FirstEndpointHasNoExtractableTypes_CanPullFromFirstEndpointReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_AndSecondEndpointCanAcceptPlacementOfOneOfThem_AndHighwayHasPermissions_CanPullFromFirstEndpointReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_AndSecondEndpointCanAcceptPlacementOfOneOfThem_ButHighwayLacksPermissions_CanPullFromFirstEndpointReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_ButSecondEndpointCannotAcceptAnyOfThem_CanPullFromFirstEndpointReturnFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void SecondEndpointHasNoExtractableTypes_CanPullFromSecondEndpointReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_AndFirstEndpointCanAcceptPlacementOfOneOfThem_AndHighwayHasPermissions_CanPullFromSecondEndpointReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_AndFirstEndpointCanAcceptPlacementOfOneOfThem_ButHighwayLacksPermissions_CanPullFromSecondEndpointReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_ButFirstEndpointCannotAcceptAnyOfThem_CanPullFromSecondEndpointReturnFalse() {
            throw new NotImplementedException();
        }
        
        [Test]
        public void OnPullFromFirstEndpointCalled_AndPullFromFirstEndpointReturnsTrue_AResourceIsRemovedFromFirstEndpoint_ThatCanBePlacedIntoSecondEndpoint() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPullFromSecondEndpointCalled_AndCanPullFromSecondEndpointReturnsTrue_AResourceIsRemovedFromSecondEndpoint_ThatCanBePlacedIntoFirstEndpoint() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourcePulledFromFirstEndpoint_ResourceIsPlacedIntoAppropriateTube() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourcePulledFromSecondEndpoint_ResourceIsPlacedIntoAppropriateTube() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnHighwayTicked_TubesWithinHighwayAlsoTicked() {
            throw new NotImplementedException();
        }

        public void OnFirstEndpointLosesPermissionForAResourceHeadingTowardsIt_NoExceptionIsThrownWhenTheResourceReachesTheEndOfItsTube() {
            throw new NotImplementedException();
        }

        public void OnSecondEndpointLosesPermissionForAResourceHeadingTowardsIt_NoExceptionIsThrownWhenTheResourceReachesTheEndOfItsTube() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourcePulledFromFirstEndpoint_AndSecondEndpointLosesPermissionForIt_ResourcePushedBackIntoTubeGoingTowardsFirstEndpoint_OrDestroyedIfNotPermittedTo() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourcePulledFromSecondEndpoint_AndFirstEndpointLosesPermissionForIt_ResourcePushedBackIntoTubeGoingTowardsSecondEndpoint_OrDestroyedIfNotPermittedTo() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnProfileChanged_NewProfileDeterminesTheSpeedAtWhichBlobsMoveThroughTheTubes() {
            throw new NotImplementedException();
        }

        #endregion

        #region error checking

        [Test]
        public void IfCanPullFromFirstEndpointIsFalse_AndPullFromFirstEndpointIsCalled_ThrowsBlobHighwayException() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfCanPullFromSecondEndpointIsFalse_AndPullFromSecondEndpointIsCalled_ThrowsBlobHighwayException() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfProfilePassedNullValue_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region utility

        private BlobHighway BuildHighway() {
            var hostingGameObject = new GameObject();
            var newHighway = hostingGameObject.AddComponent<BlobHighway>();
            return newHighway;
        }

        private ResourceBlob BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        #endregion

        #endregion

    }

}
