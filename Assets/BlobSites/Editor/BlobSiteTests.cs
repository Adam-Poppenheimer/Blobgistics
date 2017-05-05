using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;
using Assets.BlobSites.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobSites.Editor {

    public class BlobSiteTests {

        #region instance fields and properties

        private BlobSitePrivateDataBase PrivateData {
            get {
                if(_privateData == null) {
                    var hostingObject = new GameObject();
                    _privateData = hostingObject.AddComponent<MockBlobSitePrivateData>();
                    var newBlobFactory = hostingObject.AddComponent<MockResourceBlobFactory>();
                    _privateData.SetBlobFactory(newBlobFactory);
                }
                return _privateData;
            }
        }
        private MockBlobSitePrivateData _privateData;

        #endregion

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnInitialization_GetPlacementPermissionForResourceTypeIsFalseForAllResourceTypes() {
            //Setup
            var factoryToUse = BuildFactory(PrivateData);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(new GameObject());

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(!siteToTest.GetPlacementPermissionForResourceType(resourceType),
                    "BlobSite is permitting resource type " + resourceType);
            }
        }

        [Test]
        public void OnInitialization_GetExtractionPermissionForResourceTypeIsFalseForAllResourceTypes() {
            //Setup
            var factoryToUse = BuildFactory(PrivateData);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(new GameObject());

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(!siteToTest.GetExtractionPermissionForResourceType(resourceType),
                    "BlobSite is permitting resource type " + resourceType);
            }
        }

        [Test]
        public void OnInitialization_GetCapacityForResourceTypeIsZeroForAllResourceTypes() {
            //Setup
            var factoryToUse = BuildFactory(PrivateData);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(new GameObject());

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(resourceType),
                    "Capacity is nonzero for resource type " + resourceType);
            }
        }

        [Test]
        public void OnGetPointOfConnectionFacingPointCalled_ValueReturnedLiesOnTheCorrectConnectionCircle() {
            //Setup
            var privateData = BuildBlobSitePrivateData();
            privateData.SetConnectionCircleRadius(1f);

            var siteToTest = BuildBlobSite(privateData);

            var shortUpVector    = Vector3.up    * 2;
            var shortDownVector  = Vector3.down  * 2;
            var shortLeftVector  = Vector3.left  * 2;
            var shortRightVector = Vector3.right * 2;

            var longUpVector    = Vector3.up    * 6;
            var longDownVector  = Vector3.down  * 6;
            var longLeftVector  = Vector3.left  * 6;
            var longRightVector = Vector3.right * 6;

            //Execution
            var connectionToShortUp    = siteToTest.GetPointOfConnectionFacingPoint(shortUpVector);
            var connectionToShortDown  = siteToTest.GetPointOfConnectionFacingPoint(shortDownVector);
            var connectionToShortLeft  = siteToTest.GetPointOfConnectionFacingPoint(shortLeftVector);
            var connectionToShortRight = siteToTest.GetPointOfConnectionFacingPoint(shortRightVector);

            var connectionToLongUp    = siteToTest.GetPointOfConnectionFacingPoint(longUpVector);
            var connectionToLongDown  = siteToTest.GetPointOfConnectionFacingPoint(longDownVector);
            var connectionToLongLeft  = siteToTest.GetPointOfConnectionFacingPoint(longLeftVector);
            var connectionToLongRight = siteToTest.GetPointOfConnectionFacingPoint(longRightVector);

            //Validation
            Assert.That((Vector3.up   ).Equals(connectionToShortUp),    "Incorrect connection point to shortUpVector");
            Assert.That((Vector3.down ).Equals(connectionToShortDown),  "Incorrect connection point to shortDownVector");
            Assert.That((Vector3.left ).Equals(connectionToShortLeft),  "Incorrect connection point to shortLeftVector");
            Assert.That((Vector3.right).Equals(connectionToShortRight), "Incorrect connection point to shortRightVector");

            Assert.That((Vector3.up   ).Equals(connectionToLongUp),    "Incorrect connection point to longUpVector");
            Assert.That((Vector3.down ).Equals(connectionToLongDown),  "Incorrect connection point to longDownVector");
            Assert.That((Vector3.left ).Equals(connectionToLongLeft),  "Incorrect connection point to longLeftVector");
            Assert.That((Vector3.right).Equals(connectionToLongRight), "Incorrect connection point to longRightVector");
        }
        
        [Test]
        public void OnPlacementPermissionSetForResourceType_GetPlacementPermissionForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);

            //Validation
            Assert.That(siteToTest.GetPlacementPermissionForResourceType(ResourceType.Food),   "ResourceType.Food does not register its permission");
            Assert.That(siteToTest.GetPlacementPermissionForResourceType(ResourceType.Textiles), "ResourceType.Textiles does not register its permission");
            Assert.That(!siteToTest.GetPlacementPermissionForResourceType(ResourceType.ServiceGoods), "ResourceType.ServiceGoods falsely registers that it has permission");
        }

        [Test]
        public void OnExtractionPermissionSetForResourceType_GetExtractionPermissionForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);

            //Validation
            Assert.That(siteToTest.GetExtractionPermissionForResourceType(ResourceType.Food),   "ResourceType.Food does not register its permission");
            Assert.That(siteToTest.GetExtractionPermissionForResourceType(ResourceType.Textiles), "ResourceType.Textiles does not register its permission");
            Assert.That(!siteToTest.GetExtractionPermissionForResourceType(ResourceType.ServiceGoods), "ResourceType.ServiceGoods falsely registers that it has permission");
        }

        [Test]
        public void OnCapacitySetForResourceType_GetCapacityForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.SetCapacityForResourceType(ResourceType.Textiles, 2);

            //Validation
            Assert.AreEqual(1, siteToTest.GetCapacityForResourceType(ResourceType.Food),   "ResourceType.Food has the wrong capacity");
            Assert.AreEqual(2, siteToTest.GetCapacityForResourceType(ResourceType.Textiles), "ResourceType.Textiles has the wrong capacity");
            Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(ResourceType.ServiceGoods),  "ResourceType.ServiceGoods has the wrong capacity");
        }

        [Test]
        public void OnSetPermissionAndCapacityCalled_PermissionsAndCapacitiesAreCorrectlyAssigned() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var placementPermissionSummary = IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 4),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 0)
            );

            //Execution
            siteToTest.SetPlacementPermissionsAndCapacity(placementPermissionSummary);

            //Validation
            Assert.That(siteToTest.GetPlacementPermissionForResourceType(ResourceType.Food), "Placement of Food is not permitted");
            Assert.AreEqual(2, siteToTest.GetCapacityForResourceType(ResourceType.Food), "Food has the wrong capacity");

            Assert.That(siteToTest.GetPlacementPermissionForResourceType (ResourceType.Textiles), "Placement of Textiles is not permitted" );
            Assert.AreEqual(4, siteToTest.GetCapacityForResourceType(ResourceType.Textiles), "Textiles has the wrong capacity");

            Assert.IsFalse(siteToTest.GetPlacementPermissionForResourceType (ResourceType.ServiceGoods), "Placement of ServiceGoods is falsely permitted");
            Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(ResourceType.ServiceGoods), "ServiceGoods has the wrong capacity");

            Assert.AreEqual(6, siteToTest.TotalCapacity, "TotalCapacity is incorrect");
        }

        [Test]
        public void OnPermissionForResourceTypeSetToTrue_AndCapacityIsZero_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Food));
        }

        [Test]
        public void OnPermissionForResourceTypeSetToTrue_AndCapacityIsZero_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnPermissionForResourceTypeIsFalse_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, false);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Food));
        }

        [Test]
        public void OnPermissionForResourceTypeIsFalse_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, false);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnPermissionForResourceTypeGiven_AndCapacitySetAboveZero_AndTotalCapacityIsZero_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Food));
        }

        [Test]
        public void OnPermissionForResourceTypeGiven_AndCapacitySetAboveZero_AndTotalCapacityIsZero_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnClearPermissionsAndCapacitiesCalled_AllPermissionsAndCapacitiesReturnedToDefaultValues() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                siteToTest.SetPlacementPermissionForResourceType(resourceType, true);
                siteToTest.SetExtractionPermissionForResourceType(resourceType, true);
                siteToTest.SetCapacityForResourceType(resourceType, 100);
            }

            //Execution
            siteToTest.ClearPermissionsAndCapacity();

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(!siteToTest.GetPlacementPermissionForResourceType(resourceType),
                    "Failed to clear placement permission for resourceType " + resourceType);
                Assert.That(!siteToTest.GetExtractionPermissionForResourceType(resourceType),
                    "Failed to clear extraction permission for resourceType " + resourceType);
                Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(resourceType),
                    "Failed to clear capacity for resourceType " + resourceType);
            }
        }

        [Test]
        public void IfResourceTypePermitted_AndResourceCapacitySetAboveZero_AndTotalCapacitySetAboveZero_CanPlaceBlobOfTypeIntoReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Validation
            Assert.That(siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Food));
        }

        [Test]
        public void IfResourceTypePermitted_AndResourceCapacitySetAboveZero_AndTotalCapacitySetAboveZero_CanPlaceBlobIntoOfThatTypeReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Validation
            Assert.That(siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_BlobAppearsInContents() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.Contents.Contains(blobToInsert));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_TotalSpaceLeftDecreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 2);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.AreEqual(1, siteToTest.TotalSpaceLeft);
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_AndExtractionPermissionGiven_GetExtractableTypesContainsTypeOfPlacedBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.TotalCapacity = 1;

            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.GetExtractableTypes().Contains(blobToInsert.BlobType));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetContentsOfTypeOnTypeOfPlacedBlobContainsPlacedBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.GetContentsOfType(blobToInsert.BlobType).Contains(blobToInsert));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetCountOfContentsOfTypeIncreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.AreEqual(1, siteToTest.GetCountOfContentsOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetSpaceLeftOfTypeDecreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 2);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.AreEqual(1, siteToTest.GetSpaceLeftOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_CanPlaceBlobIntoOnPlacedBlobReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 2);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnManyResourceTypesPlacedInto_GetExtractableTypesInformedByExtractionPermissions() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)
            ));
            
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));

            //Execution and Validation
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Food), "Falsely acknowledges Food as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Textiles), "Falsely acknowledges Textiles as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.ServiceGoods), "Falsely acknowledges ServiceGoods as extractable");

            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);

            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Food), "Fails to acknowledge Food as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Textiles), "Falsely acknowledges Textiles as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.ServiceGoods), "Falsely acknowledges ServiceGoods as extractable");

            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);

            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Food), "Fails to acknowledge Food as extractable");
            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Textiles), "Fails to acknowledges Textiles as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.ServiceGoods), "Falsely acknowledges ServiceGoods as extractable");

            siteToTest.SetExtractionPermissionForResourceType(ResourceType.ServiceGoods, true);

            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Food), "Fails to acknowledge Food as extractable");
            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Textiles), "Fails to acknowledges Textiles as extractable");
            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.ServiceGoods), "Fails to acknowledges ServiceGoods as extractable");

        }

        [Test]
        public void OnBlobPlacedInto_AndExtractionPermissionGiven_CanExtractAnyBlobReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Execution
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.CanExtractAnyBlob());
        }

        [Test]
        public void OnBlobPlacedInto_AndExtractionPermissionNotGiven_CanExtractAnyBlobReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, false);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Execution
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(!siteToTest.CanExtractAnyBlob());
        }

        [Test]
        public void OnBlobPlacedInto_AndExtractionPermissionGiven_CanExtractBlobOfTypeOnTypeOfPlacedBlobReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Execution
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.CanExtractBlobOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnBlobPlacedInto_AndExtractionPermissionNotGiven_CanExtractBlobOfTypeOnTypeOfPlacedBlobReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, false);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Execution
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(!siteToTest.CanExtractBlobOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnBlobPlacedInto_AndExtractionPermissionForBlobsTypeGiven_CanExtractBlobOfTypeOnADifferentTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            //Execution
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(!siteToTest.CanExtractBlobOfType(ResourceType.Textiles));
        }

        [Test]
        public void OnBlobPlacedInto_BlobPlacedIntoEventFiresWithCorrectBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;

            bool eventHasFired = false;
            ResourceBlobBase blobReturnedByEvent = null;
            siteToTest.BlobPlacedInto += delegate(object sender, BlobEventArgs e) {
                eventHasFired = true;
                blobReturnedByEvent = e.Blob;
            };

            //Execution
            
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(eventHasFired, "Event did not fire");
            Assert.AreEqual(blobToInsert, blobReturnedByEvent, "Blob broadcast by the event is not the blob that was placed");
        }

        [Test]
        public void OnBlobExtractedFrom_ReturnsTheExpectedBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var redBlob = BuildBlob(ResourceType.Food);
            var blueBlob = BuildBlob(ResourceType.ServiceGoods);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.ServiceGoods, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.ServiceGoods, true);
            siteToTest.SetCapacityForResourceType(ResourceType.ServiceGoods, 1);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(redBlob);
            siteToTest.PlaceBlobInto(blueBlob);

            //Execution
            var extractedBlob = siteToTest.ExtractBlobOfType(redBlob.BlobType);

            //Validation
            Assert.AreEqual(extractedBlob, redBlob);
        }

        [Test]
        public void OnBlobExtractedFrom_ContentsNoLongerContainsBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            siteToTest.ExtractBlobOfType(blobToInsert.BlobType);

            //Validation
            Assert.That(!siteToTest.Contents.Contains(blobToInsert));
        }

        [Test]
        public void OnBlobExtractedFrom_TotalSpaceLeftIncreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            siteToTest.ExtractBlobOfType(blobToInsert.BlobType);

            //Validation
            Assert.AreEqual(1, siteToTest.TotalSpaceLeft);
        }

        [Test]
        public void OnBlobExtractedFrom_GetContentsOfTypeOfExtractedBlobNoLongerContainsExtractedBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            
            siteToTest.ExtractBlobOfType(blobToInsert.BlobType);

            //Validation
            Assert.That(!siteToTest.GetContentsOfType(blobToInsert.BlobType).Contains(blobToInsert));
        }

        [Test]
        public void OnBlobExtractedFrom_GetCountOfContentsOfTypeOnTypeOfExtractedBlobDecreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            
            siteToTest.ExtractBlobOfType(blobToInsert.BlobType);

            //Validation
            Assert.AreEqual(0, siteToTest.GetCountOfContentsOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnBlobExtractedFrom_BlobExtractedFromFiresWithCorrectBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            bool eventHasFired = false;
            ResourceBlobBase blobExtracted = null;
            siteToTest.BlobExtractedFrom += delegate(object sender, BlobEventArgs e) {
                eventHasFired = true;
                blobExtracted = e.Blob;
            };

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            siteToTest.ExtractBlobOfType(blobToInsert.BlobType);

            //Validation
            Assert.That(eventHasFired, "Event did not fire");
            Assert.AreEqual(blobExtracted, blobToInsert, "Blob broadcast as extracted is not the blob that was extracted");
        }

        [Test]
        public void OnPlacementPermissionSetToFalse_BlobExtractionIsUnaffected() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Food);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Food, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Food, false);

            //Valdiation
            Assert.That(siteToTest.CanExtractBlobOfType(ResourceType.Food));
        }

        [Test]
        public void OnBlobExtractedOfType_BlobReturnedIsOfCorrectType() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            var redBlob   = BuildBlob(ResourceType.Food);
            var greenBlob = BuildBlob(ResourceType.Textiles);
            var blueBlob  = BuildBlob(ResourceType.ServiceGoods);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)
            ));
            siteToTest.TotalCapacity = 3;
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food,   true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.ServiceGoods,  true);

            siteToTest.PlaceBlobInto(redBlob);
            siteToTest.PlaceBlobInto(greenBlob);
            siteToTest.PlaceBlobInto(blueBlob);

            //Execution
            var blueExtractedBlob  = siteToTest.ExtractBlobOfType(ResourceType.ServiceGoods);
            var redExtractedBlob   = siteToTest.ExtractBlobOfType(ResourceType.Food);
            var greenExtractedBlob = siteToTest.ExtractBlobOfType(ResourceType.Textiles);

            //Validation
            Assert.AreEqual(ResourceType.Food,   redExtractedBlob.BlobType,   "redExtractedBlob has the wrong BlobType"  );
            Assert.AreEqual(ResourceType.Textiles, greenExtractedBlob.BlobType, "greenExtractedBlob has the wrong BlobType");
            Assert.AreEqual(ResourceType.ServiceGoods,  blueExtractedBlob.BlobType,  "blueExtractedBlob has the wrong BlobType" );
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndExtractionPermittedForAllTypes_GetExtractableTypesReturnsAllTypesOfBlobsWithin() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food,   true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.ServiceGoods,  true);

            //Execution
            for(int i = 0; i < 5; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }

            var extractableTypes = siteToTest.GetExtractableTypes();

            //Validation
            Assert.That(extractableTypes.Contains (ResourceType.Food  ), "extractableTypes does not contain Food");
            Assert.That(extractableTypes.Contains (ResourceType.Textiles), "extractableTypes does not contain Textiles");
            Assert.That(!extractableTypes.Contains(ResourceType.ServiceGoods ), "extractableTypes falsely contains ServiceGoods");
        }

        [Test]
        public void OnNumberOfBlobsPlacedIntoEqualToTotalCapacity_IsAtCapacityBecomesTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 5),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 5),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 5)
            ));
            siteToTest.TotalCapacity = 10;

            //Execution
            for(int i = 0; i < 5; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 5; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }

            //Validation
            Assert.That(siteToTest.IsAtCapacity);
        }

        [Test]
        public void OnBlobsPlacedIntoAndExtractedFrom_GetExtractableTypesAlwaysReadsTheCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            var redBlob   = BuildBlob(ResourceType.Food);
            var greenBlob = BuildBlob(ResourceType.Textiles);
            var blueBlob  = BuildBlob(ResourceType.ServiceGoods);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            siteToTest.TotalCapacity = 3;
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Food,   true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.ServiceGoods,  true);

            IEnumerable<ResourceType> extractableTypes;

            //Execution and Validation
            siteToTest.PlaceBlobInto(redBlob);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains (ResourceType.Food  ), "Food insertion does not add red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Textiles), "Food insertion adds green as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.ServiceGoods ), "Food insertion adds blue as an extractable type");

            siteToTest.PlaceBlobInto(greenBlob);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains (ResourceType.Food  ), "Textiles insertion removes red as an extractable type");
            Assert.That(extractableTypes.Contains (ResourceType.Textiles), "Textiles insertion does not add green as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.ServiceGoods ), "Textiles insertion adds blue as an extractable type");

            siteToTest.PlaceBlobInto(blueBlob);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains(ResourceType.Food  ), "ServiceGoods insertion removes red as an extractable type");
            Assert.That(extractableTypes.Contains(ResourceType.Textiles), "ServiceGoods insertion removes green as an extractable type");
            Assert.That(extractableTypes.Contains(ResourceType.ServiceGoods ), "ServiceGoods insertion does not add blue as an extractable type");

            siteToTest.ExtractBlobOfType(ResourceType.Textiles);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains (ResourceType.Food  ), "Textiles extraction removes red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Textiles), "Textiles extraction does not remove green as an extractable type");
            Assert.That(extractableTypes.Contains (ResourceType.ServiceGoods ), "Textiles extraction removes blue as an extractable type");

            siteToTest.ExtractBlobOfType(ResourceType.Food);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(!extractableTypes.Contains(ResourceType.Food  ), "Food extraction does not remove red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Textiles), "Food extraction adds green as an extractable type");
            Assert.That(extractableTypes.Contains (ResourceType.ServiceGoods ), "Food extraction removes blue as an extractable type");

            siteToTest.ExtractBlobOfType(ResourceType.ServiceGoods);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(!extractableTypes.Contains(ResourceType.Food  ), "ServiceGoods extraction adds red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Textiles), "ServiceGoods extraction adds green as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.ServiceGoods ), "ServiceGoods extraction does not remove blue as an extractable type");
        }

        [Test]
        public void OnSeveralResourceAtCapacity_ButOneIsNot_AndSpaceLeftIsNonZero_IsAtCapacityIsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            siteToTest.TotalCapacity = 3;

            //Execution
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            //Validation
            Assert.That(!siteToTest.IsAtCapacity);
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_ContentsIsEmpty() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));
            }

            siteToTest.ClearContents();

            //Validation
            Assert.IsEmpty(siteToTest.Contents);
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_TotalSpaceLeftEqualsTotalCapacity() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));
            }

            siteToTest.ClearContents();

            //Validation
            Assert.AreEqual(siteToTest.TotalCapacity, siteToTest.TotalSpaceLeft);
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_GetContentsOfTypeForAllTypesIsEmpty() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));
            }

            siteToTest.ClearContents();

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.IsEmpty(siteToTest.GetContentsOfType(resourceType), "contents are not empty for resource type " + resourceType);
            }
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_GetCountOfContentsofTypeForAllTypesIsZero() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));
            }

            siteToTest.ClearContents();

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.AreEqual(0, siteToTest.GetCountOfContentsOfType(resourceType), "Count is nonzero for resource type " + resourceType);
            }
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_ThenForAllTypes_GetSpaceLeftOfTypeEqualsGetCapacityForType() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));
            }

            siteToTest.ClearContents();

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.AreEqual(siteToTest.GetCapacityForResourceType(resourceType), siteToTest.GetSpaceLeftOfType(resourceType),
                    string.Format("for resource type {0}, space left does not equal capacity", resourceType));
            }
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_AllBlobsClearedEventIsCalled() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 10)
            ));
            siteToTest.TotalCapacity = 30;

            bool siteWasCleared = false;
            siteToTest.AllBlobsCleared += delegate(object sender, EventArgs e) {
                siteWasCleared = true;
            };

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));
            }

            siteToTest.ClearContents();

            //Validation
            Assert.That(siteWasCleared);
        }

        [Test]
        public void OnAllBlobTypesFilledToCapacity_ButTotalCapacityNotReached_TotalSpaceLeftIsNonzero_AndIsAtCapacityIsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            siteToTest.TotalCapacity = 4;

            //Execution
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Food));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Textiles));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.ServiceGoods));

            //Validation
            Assert.That(!siteToTest.IsAtCapacity);
        }

        [Test]
        public void OnResourceBlobPlacedInto_BlobSiteIsMadeNewParentOfBlob_AndLocationDoesntChange() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));

            var blobToPlace = BuildBlob(ResourceType.Food);
            var blobStartingPosition = new Vector3(11f, 22f, 33f);
            blobToPlace.transform.position = blobStartingPosition;

            //Execution
            siteToTest.PlaceBlobInto(blobToPlace);

            //Validation
            Assert.AreEqual(siteToTest.transform, blobToPlace.transform.parent, "BlobToPlace has an incorrect parent");
            Assert.AreEqual(blobStartingPosition, blobToPlace.transform.position, "BlobToPlace was moved during placement");
        }

        [Test]
        public void OnResourceBlobPlacedInto_AlignmentStrategyIsCalledOnAllContents() {
            //Setup
            var hostingObject = new GameObject();
            var newPrivateData = hostingObject.AddComponent<MockBlobSitePrivateData>();

            var alignmentStrategy = hostingObject.AddComponent<MockBlobAlignmentStrategy>();

            newPrivateData.SetAlignmentStrategy(alignmentStrategy);

            var siteToTest = BuildBlobSite(newPrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(IntResourceSummary.BuildSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 5)
            ));

            var blob1 = BuildBlob(ResourceType.Food);
            var blob2 = BuildBlob(ResourceType.Food);
            var blob3 = BuildBlob(ResourceType.Food);

            //Execution and Validation
            siteToTest.PlaceBlobInto(blob1);
            Assert.AreEqual(1, alignmentStrategy.LastAlignmentRequest.Count, "Placement of blob1 led to an incorrect number of blobs being aligned");
            Assert.Contains(blob1, alignmentStrategy.LastAlignmentRequest, "Blob1 not requested for alignment after placement of blob1");

            siteToTest.PlaceBlobInto(blob2);
            Assert.AreEqual(2, alignmentStrategy.LastAlignmentRequest.Count, "Placement of blob2 led to an incorrect number of blobs being aligned");
            Assert.Contains(blob1, alignmentStrategy.LastAlignmentRequest, "Blob1 not requested for alignment after placement of blob2");
            Assert.Contains(blob2, alignmentStrategy.LastAlignmentRequest, "Blob2 not requested for alignment after placement of blob2");

            siteToTest.PlaceBlobInto(blob3);
            Assert.AreEqual(3, alignmentStrategy.LastAlignmentRequest.Count, "Placement of blob3 led to an incorrect number of blobs being aligned");
            Assert.Contains(blob1, alignmentStrategy.LastAlignmentRequest, "Blob1 not requested for alignment after placement of blob3");
            Assert.Contains(blob2, alignmentStrategy.LastAlignmentRequest, "Blob2 not requested for alignment after placement of blob3");
            Assert.Contains(blob3, alignmentStrategy.LastAlignmentRequest, "Blob3 not requested for alignment after placement of blob3");
        }

        #endregion

        #region error handling

        [Test]
        public void CanPlaceBlobInto_IsPassedNullValue_ThrowsArgumentNullException() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                siteToTest.CanPlaceBlobInto(null);
            });
        }

        [Test]
        public void CanPlaceBlobIntoReturnsFalse_AndPlaceBlobIntoIsCalled_ThrowsBlobSiteException() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            var blobToAdd = BuildBlob(ResourceType.Food);

            //Execution and Validation
            Assert.Throws<BlobSiteException>(delegate() {
                siteToTest.PlaceBlobInto(blobToAdd);
            });
        }

        [Test]
        public void CanExtractAnyBlobReturnsFalse_AndExtractAnyBlobIsCalled_ThrowsBlobSiteException() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution and Validation
            Assert.Throws<BlobSiteException>(delegate() {
                siteToTest.ExtractAnyBlob();
            });
        }

        [Test]
        public void CanExtractBlobOfTypeReturnsFalse_AndExtractBlobOfTypeIsCalled_ThrowsBlobSiteException() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution and Validation
            Assert.Throws<BlobSiteException>(delegate() {
                siteToTest.ExtractBlobOfType(ResourceType.Textiles);
            });
        }

        [Test]
        public void SetPermissionsAndCapacity_IsPassedNullValue_ThrowsArgumentNullException() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                siteToTest.SetPlacementPermissionsAndCapacity(null);
            });
        }

        #endregion

        #endregion

        #region utilities

        private BlobSite BuildBlobSite(BlobSitePrivateDataBase privateData) {
            var hostingObject = new GameObject();
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            newBlobSite.PrivateData = privateData;
            return newBlobSite;
        }

        private BlobSiteFactory BuildFactory(BlobSitePrivateDataBase privateData) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobSiteFactory>();
            newFactory.BlobSitePrivateData = privateData;
            return newFactory;
        }

        private ResourceBlobBase BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        private BlobSitePrivateData BuildBlobSitePrivateData() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<BlobSitePrivateData>();
        }

        #endregion

        #endregion

    }

}


