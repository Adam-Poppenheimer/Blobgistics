using System;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites.Editor {

    public class BlobSiteTests {

        #region instance fields and properties

        private BlobSitePrivateDataBase PrivateData {
            get {
                if(_privateData == null) {
                    var hostingObject = new GameObject();
                    _privateData = hostingObject.AddComponent<MockBlobSitePrivateData>();
                }
                return _privateData;
            }
        }
        private BlobSitePrivateDataBase _privateData;

        #endregion

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnInitialization_ConnectionPointsReturnCurrentPositionPlusValueInPrivateData() {
            //Setup
            var factoryToUse = BuildFactory(PrivateData);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(new Vector3(1f, 1f, 1f), null);

            //Validation
            Assert.That(Mathf.Approximately(0f, Vector3.Distance(
                siteToTest.NorthConnectionPoint,
                new Vector3(1f, 2f, 1f)
            )), "NorthConnectionPoint is not approximately where it should be");
            Assert.That(Mathf.Approximately(0f, Vector3.Distance(
                siteToTest.SouthConnectionPoint,
                new Vector3(1f, -1f, 1f)
            )), "SouthConnectionPoint is not approximately where it should be");
            Assert.That(Mathf.Approximately(0f, Vector3.Distance(
                siteToTest.EastConnectionPoint,
                new Vector3(4f, 1f, 1f)
            )), "EastConnectionPoint is not approximately where it should be");
            Assert.That(Mathf.Approximately(0f, Vector3.Distance(
                siteToTest.WestConnectionPoint,
                new Vector3(-3f, 1f, 1f)
            )), "WestConnectionPoint is not approximately where it should be");
        }

        [Test]
        public void OnInitialization_GetPermissionForResourceTypeIsFalseForAllResourceTypes() {
            //Setup
            var factoryToUse = BuildFactory(PrivateData);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(Vector3.zero, null);

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(!siteToTest.GetPermissionForResourceType(resourceType),
                    "BlobSite is permitting resource type " + resourceType);
            }
        }

        [Test]
        public void OnInitialization_GetCapacityForResourceTypeIsZeroForAllResourceTypes() {
            //Setup
            var factoryToUse = BuildFactory(PrivateData);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(Vector3.zero, null);

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(resourceType),
                    "Capacity is nonzero for resource type " + resourceType);
            }
        }

        [Test]
        public void OnPermissionSetForResourceType_GetPermissionForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetPermissionForResourceType(ResourceType.Green, true);

            //Validation
            Assert.That(siteToTest.GetPermissionForResourceType(ResourceType.Red),   "ResourceType.Red does not register its permission");
            Assert.That(siteToTest.GetPermissionForResourceType(ResourceType.Green), "ResourceType.Green does not register its permission");
            Assert.That(!siteToTest.GetPermissionForResourceType(ResourceType.Blue), "ResourceType.Blue falsely registers that it has permission");
        }

        [Test]
        public void OnCapacitySetForResourceType_GetCapacityForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.SetCapacityForResourceType(ResourceType.Green, 2);

            //Validation
            Assert.AreEqual(1, siteToTest.GetPermissionForResourceType(ResourceType.Red),   "ResourceType.Red has the wrong capacity");
            Assert.AreEqual(2, siteToTest.GetPermissionForResourceType(ResourceType.Green), "ResourceType.Green has the wrong capacity");
            Assert.AreEqual(0, !siteToTest.GetPermissionForResourceType(ResourceType.Blue), "ResourceType.Blue has the wrong capacity");
        }

        [Test]
        public void OnSetPermissionAndCapacityCalled_PermissionsAndCapacitiesAreCorrectlyAssigned() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionForResourceTypeSetToTrue_CanPlaceBlobOfTypeIntoForThatTypeReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionForResourceTypeSetToTrue_CanPlaceBlobIntoOfThatTypeReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionForResourceTypeSetToFalse_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionForResourceTypeSetToFalse_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnCapacityForResourceTypeSetAboveZero_CanPlaceBlobOfTypeIntoForThatTypeReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnCapacityForResourceTypeSetAboveZero_CanPlaceBlobIntoOfThatTypeReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnCapacityForResourceTypeSetToZero_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnCapacityForResourceTypeSetToOrBelowZero_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfResourceTypePermitted_AndResourceCapacitySetAboveZero_AndTotalCapacitySetAboveZero_CanPlaceBlobOfTypeIntoReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfResourceTypePermitted_AndResourceCapacitySetAboveZero_AndTotalCapacitySetToZero_CanPlaceBlobOfTypeIntoReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_BlobAppearsInContents() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_TotalSpaceLeftDecreasesByOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetExtractableTypesContainsTypeOfPlacedBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetContentsOfTypeOnTypeOfPlacedBlobContainsPlacedBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetCountOfContentsOfTypeIncreasesByOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetSpaceLeftOfTypeDecreasesByOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_CanPlaceBlobIntoOnPlacedBlobReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobPlacedInto_CanExtractAnyBlobReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobPlacedInto_CanExtractBlobOfTypeOnTypeOfPlacedBlobReturnsTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobPlacedInto_CanExtractBlobOfTypeOnADifferentTypeReturnsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobPlacedInto_BlobPlacedIntoEventFiresWithCorrectBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobExtractedFrom_ContentsNoLongerContainsBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobExtractedFrom_TotalSpaceLeftIncreasesByOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobExtractedFrom_GetContentsOfTypeOfExtractedBlobNoLongerContainsExtractedBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobExtractedFrom_GetCountOfContentsOfTypeOnTypeOfExtractedBlobDecreasesByOne() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobExtractedFrom_BlobExtractedFromFiresWithCorrectBlob() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnPermissionSetToFalse_BlobExtractionIsUnaffected() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnBlobExtractedOfType_BlobReturnedIsOfCorrectType() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_GetExtractableTypesReturnsAllTypesOfBlobsWithin() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnNumberOfBlobsPlacedIntoEqualToTotalCapacity_TotalSpaceLeftReachesZero() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnNumberOfBlobsPlacedIntoEqualToTotalCapacity_IsAtCapacityBecomesTrue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedIntoAndExtractedFrom_GetExtractableTypesAlwaysReadsTheCorrectValue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedIntoAndExtractedFrom_GetCountOfContentsOfTypeAlwaysReadsTheCorrectValue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedIntoAndExtractedFrom_GetSpaceLeftOfTypeAlwaysReadsTheCorrectValue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedIntoAndExtractedFrom_TotalSpaceLeftAlwaysReadsTheCorrectValue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedIntoAndExtractedFrom_ContentsAlwaysReadsTheCorrectValue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedIntoAndExtractedFrom_IsAtCapacityAlwaysReadsTheCorrectValue() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_ButTotalSpaceLeftIsNeverZero_IsAtCapacityIsAlwaysFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnOneResourceIsAtCapacity_ButOthersAreNot_IsAtCapacityIsFalse() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_ContentsIsEmpty() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_TotalSpaceLeftEqualsTotalCapacity() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_GetContentsOfTypeForAllTypesIsEmpty() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_GetCountOfContentsofTypeForAllTypesIsZero() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_ThenForAllTypes_GetSpaceLeftOfTypeEqualsGetCapacityForType() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_AllBlobsClearedEventIsCalled() {
            throw new NotImplementedException();
        }

        [Test]
        public void IfIsAtCapacity_ThenGetIsAtCapacityForResourceReturnsTrueForEveryResourceType() {
            throw new NotImplementedException();
        }

        #endregion

        #region error handling

        [Test]
        public void CanPlaceBlobInto_IsPassedNullValue_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        [Test]
        public void CanPlaceBlobIntoReturnsFalse_AndPlaceBlobIntoIsCalled_ThrowsBlobSiteException() {
            throw new NotImplementedException();
        }

        [Test]
        public void CanExtractAnyBlobReturnsFalse_AndExtractAnyBlobIsCalled_ThrowsBlobSiteException() {
            throw new NotImplementedException();
        }

        [Test]
        public void CanExtractBlobOfTypeReturnsFalse_AndExtractBlobOfTypeIsCalled_ThrowsBlobSiteException() {
            throw new NotImplementedException();
        }

        [Test]
        public void SetPermissionsAndCapacity_IsPassedNullValue_ThrowsArgumentNullException() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        private BlobSite BuildBlobSite(BlobSitePrivateDataBase privateData) {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<BlobSite>();
        }

        private BlobSiteFactory BuildFactory(BlobSitePrivateDataBase privateData) {

        }

        #endregion

    }

}


