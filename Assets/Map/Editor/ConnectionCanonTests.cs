using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;

namespace Assets.Map.Editor {

    /*public class ConnectionCanonIntegrationTests {

        #region instance methods

        #region Creation and destruction

        [Test]
        public void OnHighwayBuiltBetweenBlobSites_HasConnectionBetweenReturnsTrue_ForBothConfigurations() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);

            //Validation
            Assert.That(canonToTest.HasHighwayBetween(site1, site2), "does not have a highway between site1 and site2");
            Assert.That(canonToTest.HasHighwayBetween(site2, site1), "does not have a highway between site2 and site1");
        }

        [Test]
        public void OnHighwayBuiltBetweenBlobSites_HighwayWithProperConnectionPointsIsConstructed() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);
            var highwayBetween = canonToTest.GetHighwayBetween(site1, site2);

            //Validation
            Assert.NotNull(highwayBetween, "The retrieved highway is null");
            Assert.AreEqual(site1, highwayBetween.FirstEndpoint, "The highway has the incorrect FirstEndpoint");
            Assert.AreEqual(site2, highwayBetween.SecondEndpoint, "The highway has the incorrect SecondEndpoint");
        }

        [Test]
        public void OnHighwayBuiltBetweenBlobSites_ASecondHighwayCannotBeBuiltBetweenThem() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);

            //Validation
            Assert.That(!canonToTest.CanBuildHighwayBetween(site1, site2), "Is permitting a highway between site1 and site2");
            Assert.That(!canonToTest.CanBuildHighwayBetween(site2, site1), "Is permitting a highway between site2 and site1");
        }

        [Test]
        public void OnHighwayDestroyedBetweenBlobSites_HasConnectionBetweenReturnsFalse() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);
            canonToTest.DestroyHighwayBetween(site1, site2);

            //Validation
            Assert.That(!canonToTest.HasHighwayBetween(site1, site2), "Is recording a highway between site1 and site2");
            Assert.That(!canonToTest.HasHighwayBetween(site2, site1), "Is recording a highway between site2 and site1");
        }

        [Test]
        public void OnHighwayDestroyedBetweenBlobSites_ANewHighwayCanBeBuiltBetweenThem() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);
            canonToTest.DestroyHighwayBetween(site1, site2);

            //Validate
            Assert.That(canonToTest.CanBuildHighwayBetween(site1, site2), "Cannot construct a highway between site1 and site2");
            Assert.That(canonToTest.CanBuildHighwayBetween(site2, site1), "Cannot construct a highway between site2 and site1");
            Assert.DoesNotThrow(delegate() {
                canonToTest.BuildHighwayBetween(site1, site2);
            }, "BuildHighwayBetween failed");
        }

        [Test]
        public void OnNewHighwayBuiltBetweenBlobSites_HighwaysConnectedToBlobSiteContainsThatHighway() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            var highwayBetween = canonToTest.BuildHighwayBetween(site1, site2);

            //Validation
            Assert.That(canonToTest.GetAllHighwaysConnectedToSite(site1).Contains(highwayBetween),
                "Site1 does not register new highway");

            Assert.That(canonToTest.GetAllHighwaysConnectedToSite(site2).Contains(highwayBetween),
                "Site2 does not register new highway");
        }

        [Test]
        public void OnHighwayDestroyedBetweenBlobSites_HighwaysConnectedToBlobSitesDoNotContainThatHighway() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();

            //Execution
            var highwayBetween = canonToTest.BuildHighwayBetween(site1, site2);
            canonToTest.DestroyHighwayBetween(site1, site2);

            //Validation
            Assert.That(!canonToTest.GetAllHighwaysConnectedToSite(site1).Contains(highwayBetween),
                "Site1 still registers the highway");

            Assert.That(!canonToTest.GetAllHighwaysConnectedToSite(site2).Contains(highwayBetween),
                "Site2 still registers the highway");
        }

        [Test]
        public void OnAllHighwaysDestroyed_NoBlobSiteHasAnyHighwayConnectingIt() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();
            var site3 = new MockBlobSite();
            var site4 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);
            canonToTest.BuildHighwayBetween(site1, site3);
            canonToTest.BuildHighwayBetween(site1, site4);

            canonToTest.BuildHighwayBetween(site2, site3);
            canonToTest.BuildHighwayBetween(site2, site4);

            canonToTest.BuildHighwayBetween(site3, site4);

            canonToTest.DestroyAllHighways();

            //Validation
            Assert.That(!canonToTest.HasHighwayBetween(site1, site2), "Connection between site1 and site2 still exists");
            Assert.That(!canonToTest.HasHighwayBetween(site1, site3), "Connection between site1 and site3 still exists");
            Assert.That(!canonToTest.HasHighwayBetween(site1, site4), "Connection between site1 and site4 still exists");

            Assert.That(!canonToTest.HasHighwayBetween(site2, site3), "Connection between site2 and site3 still exists");
            Assert.That(!canonToTest.HasHighwayBetween(site2, site4), "Connection between site2 and site4 still exists");

            Assert.That(!canonToTest.HasHighwayBetween(site3, site4), "Connection between site3 and site4 still exists");
        }

        [Test]
        public void OnAllHighwaysConnectedToBlobSiteDestroyed_BlobSiteHasNoHighwayConnectingToIt() {
            //Setup
            var canonToTest = new ConnectionCanon(1f);
            var site1 = new MockBlobSite();
            var site2 = new MockBlobSite();
            var site3 = new MockBlobSite();
            var site4 = new MockBlobSite();

            //Execution
            canonToTest.BuildHighwayBetween(site1, site2);
            canonToTest.BuildHighwayBetween(site1, site3);
            canonToTest.BuildHighwayBetween(site1, site4);

            canonToTest.DestroyAllHighwaysConnectedTo(site1);

            //Validation
            Assert.That(!canonToTest.HasHighwayBetween(site1, site2), "Connection between site1 and site2 still exists");
            Assert.That(!canonToTest.HasHighwayBetween(site1, site3), "Connection between site1 and site3 still exists");
            Assert.That(!canonToTest.HasHighwayBetween(site1, site4), "Connection between site1 and site4 still exists");
        }

        #endregion

        #region extraction and placement

        [Test]
        public void IfABlobSiteHasAHighway_AndBothElementsPermitBlobsToBeExtracted_ThoseBlobsWillBeExtracted() {
            //Setup
            var sendingSite = new MockBlobSite();
            var receivingSite = new MockBlobSite();

            sendingSite.AcceptsExtraction = true;
            sendingSite.SetExtractionPermission(ResourceType.Food, true);
            sendingSite.SetPlacementPermission(ResourceType.Food, true);

            receivingSite.AcceptsPlacement = true;
            receivingSite.SetPlacementPermission(ResourceType.Food, true);

            var canonToTest = new ConnectionCanon(1f);

            //Execution
            var constructedHighway = canonToTest.BuildHighwayBetween(sendingSite, receivingSite);
            constructedHighway.TubePullingFromFirstEndpoint.SetPermissionForResourceType(ResourceType.Food, true);

            var blobToExtract = BuildBlob(ResourceType.Food);
            sendingSite.PlaceBlobInto(blobToExtract);
            canonToTest.TickExtraction(1f);

            //Validation
            Assert.That(!sendingSite.ReadOnlyBlobsWithin.Contents.Contains(blobToExtract),
                "SendingSite still contains blobToExtract");
            Assert.That(constructedHighway.Contents.Contains(blobToExtract),
                "ConstructedHighway does not contain blobToExtract");
        }

        [Test]
        public void OnTickExtraction_WithTheHighwayRefusingAnExtraction_NoBlobsWillBeExtracted_AndNoExceptionThrown() {
            //Setup
            var sendingSite = new MockBlobSite();
            var receivingSite = new MockBlobSite();

            sendingSite.AcceptsExtraction = true;
            sendingSite.SetExtractionPermission(ResourceType.Food, true);
            sendingSite.SetPlacementPermission(ResourceType.Food, true);

            receivingSite.AcceptsPlacement = true;
            receivingSite.SetPlacementPermission(ResourceType.Food, true);

            var canonToTest = new ConnectionCanon(1f);
            var refusingHighway = canonToTest.BuildHighwayBetween(sendingSite, receivingSite);
            refusingHighway.TubePullingFromFirstEndpoint.SetPermissionForResourceType(ResourceType.Food, false);

            var blobToRefuse = BuildBlob(ResourceType.Food);
            sendingSite.PlaceBlobInto(blobToRefuse);

            //Execution
            canonToTest.TickExtraction(1f);

            //Validation
            Assert.That(sendingSite.ReadOnlyBlobsWithin.Contents.Contains(blobToRefuse),
                "SendingSite no longer contains blobToRefuse");
        }

        [Test]
        public void OnTickExtraction_WithTheExtractedSiteRefusingExtraction_NoBlobsWillBeExtracted_AndNoExceptionThrown() {
            //Setup
            var sendingSite = new MockBlobSite();
            var receivingSite = new MockBlobSite();

            sendingSite.AcceptsExtraction = true;
            sendingSite.SetExtractionPermission(ResourceType.Food, false);
            sendingSite.SetPlacementPermission(ResourceType.Food, true);

            receivingSite.AcceptsPlacement = true;
            receivingSite.SetPlacementPermission(ResourceType.Food, true);

            var canonToTest = new ConnectionCanon(1f);
            var refusingHighway = canonToTest.BuildHighwayBetween(sendingSite, receivingSite);
            refusingHighway.TubePullingFromFirstEndpoint.SetPermissionForResourceType(ResourceType.Food, true);

            var blobToRefuse = BuildBlob(ResourceType.Food);
            sendingSite.PlaceBlobInto(blobToRefuse);

            //Execution

        }

        [Test]
        public void IfABlobSiteHasAHighway_OnlyPermittedBlobsWillBeExtracted() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfABlobSiteHasAHighway_AndBothElementsPermitBlobsToBePlaced_ThoseBlobsWillBePlaced() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfABlobSiteHasAHighway_ButTheBlobSiteRefusesPlacement_NoBlobsWillBePlaced() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfAHighwayConnectsTwoBlobSites_AndOneSidePermitsExtraction_AndTheOtherPlacement_BlobsWillBeTransportedInThatDirection() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfAHighwayConnectsTwoBlobSites_AndOneSidePermitsExtraction_AndTheOtherForbidsPlacement_BlobsWillNotBeTransportedInThatDirection() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfAHighwayConnectsTwoBlobSites_AndOneSideForbidsExtraction_AndTheOtherPermitsPlacement_BlobsWillNotBeTransportedInThatDirection() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfAHighwayHasAValidTranporationConfiguration_BlobsWillMoveFromOneBlobSiteToAnother() {
            throw new NotImplementedException();
        }

        #endregion

        #region priorities

        [Test]
        public void IfABlobSiteHasManyHighwaysAcceptingABlob_AndOnlyOneBlob_TheHighestPriorityHighwayTransportsTheBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfABlobSiteHasManyHighwaysAcceptingABlobType_AndSeveralBlobs_BlobsAreDistributedToHighwaysFromHighestToLowestPriority() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfABlobSiteHasManyHighwaysAcceptingABlobType_AllOfTheSamePriority_BlobsAreDistributedRoundRobinAsTheyAreMadeAvailable() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfASiteHasManyHighways_AllOfDifferentPriority_HighPriorityHighwaysAreStuffedToCapcityBeforeLowerPriorityTubesAreGivenAny() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfASiteHasSomeHighwaysOfTheSamePriority_RoundRobinAppliesIfHigherCapacityHighwaysAreStuffed() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenThereAreManyBlobSites_ConnectedByManyTubes_AllBlobExtractionIsResolvedInOneSiteBeforeMovingOntoADifferentOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenThereAreManyBlobSites_ConnectedByManyTubes_CanonEstablishesAStableOrderInWhichBlobSitesAreAddressed() {
            throw new NotImplementedException();
        }

        #endregion

        #region error checking

        [Test]
        public void WhenTickExtraction_IsPassedNegativeValue_ThrowsArgumentOutOfRangeException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenHasHighwayBetween_IsPassedNullParameters_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenCanBuildHighwayBetween_IsPassedNullParameters_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenBuildHighwayBetween_IsPassedNullParemeters_ThrowArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenDestroyHighwayBetween_IsPassedNullParameters_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenGetAllHighwaysConnectedToSite_IsPassedNullParameter_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenDestroyAllHighwaysConnectedTo_IsPassedNullParameter_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void WhenCanBuildHighwayBetweenIsFalse_AndBuildHighwayBetweenIsCalled_ThrowsInvalidOperationException() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private ResourceBlob BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        #endregion

        #endregion

    }*/

}


