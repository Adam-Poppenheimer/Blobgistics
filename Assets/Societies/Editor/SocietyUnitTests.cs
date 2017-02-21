using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.BlobEngine;

namespace Assets.Societies.Editor {

    public class SocietyUnitTests {

        #region instance methods

        [Test]
        public void OnProductionPerformed_WhenEmptyOfResources_ProductionDefinedByCurrentComplexityFoundInBlobsWithin() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnProductionPerformed_BlobsWithinCapacityIsNotExceeded_AndNoExceptionIsThrown() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnConsumptionPerformed_ConsumptionDefinedByCurrentComplexityRemovedFromBlobsWithin() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreUnavailable_SocietyStartsDescentTimer() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_SocietyStopsDescentTimer() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnConsumptionPerformed_AndAscentConditionsAreSatisfied_SocietyAscendsComplexityLadder() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreUnavailable_NeedsAreSatisfiedIsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsAreSatisfiedIsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyConsumptionsPerformed_AndNeedsAreUnavailable_DescentTimerGoesDownAtASpecifiedRate() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnProductionPerformed_AndWantsAreAvailable_WantsAreRemovedFromBlobsWithin() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnProductionPerformed_AndWantsAreAvailable_ProductionIsIncreasedByOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnComplexityDescentTimerElapsed_SocietyDescendsComplexityLadder() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnAscendComplexityLadder_CurrentComplexityIsRetrievedFromComplexityLadderProperly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnAscendComplexityLadder_BlobsWithinBecomesEmpty() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnDescendComplexityLadder_CurrentComplexityIsRetrievedFromComplexityLadderProperly() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnDescendComplexityLadder_BlobsWithinBecomesEmpty() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenNeededOrWantedBlobsPlacedInto_ThoseBlobsCannotBeExtracted() {
            throw new NotImplementedException();
        }

        #endregion

    }

}


