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
            var hostingObject = new GameObject();
            hostingObject.transform.position = new Vector3(1f, 1f, 1f);

            //Execution
            var siteToTest = factoryToUse.ConstructBlobSite(hostingObject);

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
        public void OnPlacementPermissionSetForResourceType_GetPlacementPermissionForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Green, true);

            //Validation
            Assert.That(siteToTest.GetPlacementPermissionForResourceType(ResourceType.Red),   "ResourceType.Red does not register its permission");
            Assert.That(siteToTest.GetPlacementPermissionForResourceType(ResourceType.Green), "ResourceType.Green does not register its permission");
            Assert.That(!siteToTest.GetPlacementPermissionForResourceType(ResourceType.Blue), "ResourceType.Blue falsely registers that it has permission");
        }

        [Test]
        public void OnExtractionPermissionSetForResourceType_GetExtractionPermissionForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Green, true);

            //Validation
            Assert.That(siteToTest.GetExtractionPermissionForResourceType(ResourceType.Red),   "ResourceType.Red does not register its permission");
            Assert.That(siteToTest.GetExtractionPermissionForResourceType(ResourceType.Green), "ResourceType.Green does not register its permission");
            Assert.That(!siteToTest.GetExtractionPermissionForResourceType(ResourceType.Blue), "ResourceType.Blue falsely registers that it has permission");
        }

        [Test]
        public void OnCapacitySetForResourceType_GetCapacityForResourceTypeReturnsCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.SetCapacityForResourceType(ResourceType.Green, 2);

            //Validation
            Assert.AreEqual(1, siteToTest.GetCapacityForResourceType(ResourceType.Red),   "ResourceType.Red has the wrong capacity");
            Assert.AreEqual(2, siteToTest.GetCapacityForResourceType(ResourceType.Green), "ResourceType.Green has the wrong capacity");
            Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(ResourceType.Blue),  "ResourceType.Blue has the wrong capacity");
        }

        [Test]
        public void OnSetPermissionAndCapacityCalled_PermissionsAndCapacitiesAreCorrectlyAssigned() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var placementPermissionSummary = ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 4),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 0)
            );

            //Execution
            siteToTest.SetPlacementPermissionsAndCapacity(placementPermissionSummary);

            //Validation
            Assert.That(siteToTest.GetPlacementPermissionForResourceType(ResourceType.Red), "Placement of Red is not permitted");
            Assert.AreEqual(2, siteToTest.GetCapacityForResourceType(ResourceType.Red), "Red has the wrong capacity");

            Assert.That(siteToTest.GetPlacementPermissionForResourceType (ResourceType.Green), "Placement of Green is not permitted" );
            Assert.AreEqual(4, siteToTest.GetCapacityForResourceType(ResourceType.Green), "Green has the wrong capacity");

            Assert.IsFalse(siteToTest.GetPlacementPermissionForResourceType (ResourceType.Blue), "Placement of Blue is falsely permitted");
            Assert.AreEqual(0, siteToTest.GetCapacityForResourceType(ResourceType.Blue), "Blue has the wrong capacity");

            Assert.AreEqual(6, siteToTest.TotalCapacity, "TotalCapacity is incorrect");
        }

        [Test]
        public void OnPermissionForResourceTypeSetToTrue_AndCapacityIsZero_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Red));
        }

        [Test]
        public void OnPermissionForResourceTypeSetToTrue_AndCapacityIsZero_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnPermissionForResourceTypeIsFalse_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, false);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Red));
        }

        [Test]
        public void OnPermissionForResourceTypeIsFalse_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, false);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnPermissionForResourceTypeGiven_AndCapacitySetAboveZero_AndTotalCapacityIsZero_CanPlaceBlobOfTypeIntoForThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Red));
        }

        [Test]
        public void OnPermissionForResourceTypeGiven_AndCapacitySetAboveZero_AndTotalCapacityIsZero_CanPlaceBlobIntoOfThatTypeReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);

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
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;

            //Validation
            Assert.That(siteToTest.CanPlaceBlobOfTypeInto(ResourceType.Red));
        }

        [Test]
        public void IfResourceTypePermitted_AndResourceCapacitySetAboveZero_AndTotalCapacitySetAboveZero_CanPlaceBlobIntoOfThatTypeReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;

            //Validation
            Assert.That(siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_BlobAppearsInContents() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.Contents.Contains(blobToInsert));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_TotalSpaceLeftDecreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 2);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.AreEqual(1, siteToTest.TotalSpaceLeft);
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_AndExtractionPermissionGiven_GetExtractableTypesContainsTypeOfPlacedBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.TotalCapacity = 1;

            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.GetExtractableTypes().Contains(blobToInsert.BlobType));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetContentsOfTypeOnTypeOfPlacedBlobContainsPlacedBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(siteToTest.GetContentsOfType(blobToInsert.BlobType).Contains(blobToInsert));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetCountOfContentsOfTypeIncreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.AreEqual(1, siteToTest.GetCountOfContentsOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_GetSpaceLeftOfTypeDecreasesByOne() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 2);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.AreEqual(1, siteToTest.GetSpaceLeftOfType(blobToInsert.BlobType));
        }

        [Test]
        public void OnResourceBlobCanBePlaced_AndBlobPlacedInto_CanPlaceBlobIntoOnPlacedBlobReturnsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 2);
            siteToTest.TotalCapacity = 2;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(!siteToTest.CanPlaceBlobInto(blobToInsert));
        }

        [Test]
        public void OnManyResourceTypesPlacedInto_GetExtractableTypesInformedByExtractionPermissions() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)
            ));
            
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));

            //Execution and Validation
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Red), "Falsely acknowledges Red as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Green), "Falsely acknowledges Green as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Blue), "Falsely acknowledges Blue as extractable");

            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);

            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Red), "Fails to acknowledge Red as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Green), "Falsely acknowledges Green as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Blue), "Falsely acknowledges Blue as extractable");

            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Green, true);

            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Red), "Fails to acknowledge Red as extractable");
            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Green), "Fails to acknowledges Green as extractable");
            Assert.That(!siteToTest.GetExtractableTypes().Contains(ResourceType.Blue), "Falsely acknowledges Blue as extractable");

            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Blue, true);

            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Red), "Fails to acknowledge Red as extractable");
            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Green), "Fails to acknowledges Green as extractable");
            Assert.That(siteToTest.GetExtractableTypes().Contains(ResourceType.Blue), "Fails to acknowledges Blue as extractable");

        }

        [Test]
        public void OnBlobPlacedInto_AndExtractionPermissionGiven_CanExtractAnyBlobReturnsTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, false);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, false);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;

            //Execution
            siteToTest.PlaceBlobInto(blobToInsert);

            //Validation
            Assert.That(!siteToTest.CanExtractBlobOfType(ResourceType.Green));
        }

        [Test]
        public void OnBlobPlacedInto_BlobPlacedIntoEventFiresWithCorrectBlob() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;

            bool eventHasFired = false;
            ResourceBlob blobReturnedByEvent = null;
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
            var redBlob = BuildBlob(ResourceType.Red);
            var blueBlob = BuildBlob(ResourceType.Blue);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Blue, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Blue, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Blue, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            bool eventHasFired = false;
            ResourceBlob blobExtracted = null;
            siteToTest.BlobExtractedFrom += delegate(object sender, BlobEventArgs e) {
                eventHasFired = true;
                blobExtracted = e.Blob;
            };

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
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
            var blobToInsert = BuildBlob(ResourceType.Red);

            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red, true);
            siteToTest.SetCapacityForResourceType(ResourceType.Red, 1);
            siteToTest.TotalCapacity = 1;
            siteToTest.PlaceBlobInto(blobToInsert);

            //Execution
            siteToTest.SetPlacementPermissionForResourceType(ResourceType.Red, false);

            //Valdiation
            Assert.That(siteToTest.CanExtractBlobOfType(ResourceType.Red));
        }

        [Test]
        public void OnBlobExtractedOfType_BlobReturnedIsOfCorrectType() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            var redBlob   = BuildBlob(ResourceType.Red);
            var greenBlob = BuildBlob(ResourceType.Green);
            var blueBlob  = BuildBlob(ResourceType.Blue);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)
            ));
            siteToTest.TotalCapacity = 3;
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red,   true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Green, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Blue,  true);

            siteToTest.PlaceBlobInto(redBlob);
            siteToTest.PlaceBlobInto(greenBlob);
            siteToTest.PlaceBlobInto(blueBlob);

            //Execution
            var blueExtractedBlob  = siteToTest.ExtractBlobOfType(ResourceType.Blue);
            var redExtractedBlob   = siteToTest.ExtractBlobOfType(ResourceType.Red);
            var greenExtractedBlob = siteToTest.ExtractBlobOfType(ResourceType.Green);

            //Validation
            Assert.AreEqual(ResourceType.Red,   redExtractedBlob.BlobType,   "redExtractedBlob has the wrong BlobType"  );
            Assert.AreEqual(ResourceType.Green, greenExtractedBlob.BlobType, "greenExtractedBlob has the wrong BlobType");
            Assert.AreEqual(ResourceType.Blue,  blueExtractedBlob.BlobType,  "blueExtractedBlob has the wrong BlobType" );
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndExtractionPermittedForAllTypes_GetExtractableTypesReturnsAllTypesOfBlobsWithin() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red,   true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Green, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Blue,  true);

            //Execution
            for(int i = 0; i < 5; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }

            var extractableTypes = siteToTest.GetExtractableTypes();

            //Validation
            Assert.That(extractableTypes.Contains (ResourceType.Red  ), "extractableTypes does not contain Red");
            Assert.That(extractableTypes.Contains (ResourceType.Green), "extractableTypes does not contain Green");
            Assert.That(!extractableTypes.Contains(ResourceType.Blue ), "extractableTypes falsely contains Blue");
        }

        [Test]
        public void OnNumberOfBlobsPlacedIntoEqualToTotalCapacity_IsAtCapacityBecomesTrue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 5),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 5),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 5)
            ));
            siteToTest.TotalCapacity = 10;

            //Execution
            for(int i = 0; i < 5; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 5; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }

            //Validation
            Assert.That(siteToTest.IsAtCapacity);
        }

        [Test]
        public void OnBlobsPlacedIntoAndExtractedFrom_GetExtractableTypesAlwaysReadsTheCorrectValue() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            var redBlob   = BuildBlob(ResourceType.Red);
            var greenBlob = BuildBlob(ResourceType.Green);
            var blueBlob  = BuildBlob(ResourceType.Blue);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)
            ));
            siteToTest.TotalCapacity = 3;
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Red,   true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Green, true);
            siteToTest.SetExtractionPermissionForResourceType(ResourceType.Blue,  true);

            IEnumerable<ResourceType> extractableTypes;

            //Execution and Validation
            siteToTest.PlaceBlobInto(redBlob);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains (ResourceType.Red  ), "Red insertion does not add red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Green), "Red insertion adds green as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Blue ), "Red insertion adds blue as an extractable type");

            siteToTest.PlaceBlobInto(greenBlob);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains (ResourceType.Red  ), "Green insertion removes red as an extractable type");
            Assert.That(extractableTypes.Contains (ResourceType.Green), "Green insertion does not add green as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Blue ), "Green insertion adds blue as an extractable type");

            siteToTest.PlaceBlobInto(blueBlob);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains(ResourceType.Red  ), "Blue insertion removes red as an extractable type");
            Assert.That(extractableTypes.Contains(ResourceType.Green), "Blue insertion removes green as an extractable type");
            Assert.That(extractableTypes.Contains(ResourceType.Blue ), "Blue insertion does not add blue as an extractable type");

            siteToTest.ExtractBlobOfType(ResourceType.Green);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(extractableTypes.Contains (ResourceType.Red  ), "Green extraction removes red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Green), "Green extraction does not remove green as an extractable type");
            Assert.That(extractableTypes.Contains (ResourceType.Blue ), "Green extraction removes blue as an extractable type");

            siteToTest.ExtractBlobOfType(ResourceType.Red);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(!extractableTypes.Contains(ResourceType.Red  ), "Red extraction does not remove red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Green), "Red extraction adds green as an extractable type");
            Assert.That(extractableTypes.Contains (ResourceType.Blue ), "Red extraction removes blue as an extractable type");

            siteToTest.ExtractBlobOfType(ResourceType.Blue);
            extractableTypes = siteToTest.GetExtractableTypes();
            Assert.That(!extractableTypes.Contains(ResourceType.Red  ), "Blue extraction adds red as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Green), "Blue extraction adds green as an extractable type");
            Assert.That(!extractableTypes.Contains(ResourceType.Blue ), "Blue extraction does not remove blue as an extractable type");
        }

        [Test]
        public void OnSeveralResourceAtCapacity_ButOneIsNot_AndSpaceLeftIsNonZero_IsAtCapacityIsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)
            ));
            siteToTest.TotalCapacity = 3;

            //Execution
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));

            //Validation
            Assert.That(!siteToTest.IsAtCapacity);
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_ContentsIsEmpty() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));
            }

            siteToTest.ClearContents();

            //Validation
            Assert.IsEmpty(siteToTest.Contents);
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_TotalSpaceLeftEqualsTotalCapacity() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));
            }

            siteToTest.ClearContents();

            //Validation
            Assert.AreEqual(siteToTest.TotalCapacity, siteToTest.TotalSpaceLeft);
        }

        [Test]
        public void OnManyBlobsPlacedInto_AndThenClearIsCalled_GetContentsOfTypeForAllTypesIsEmpty() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));
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

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));
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

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));
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

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 10)
            ));
            siteToTest.TotalCapacity = 30;

            bool siteWasCleared = false;
            siteToTest.AllBlobsCleared += delegate(object sender, EventArgs e) {
                siteWasCleared = true;
            };

            //Execution
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            }
            for(int i = 0; i < 10; ++i) {
                siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));
            }

            siteToTest.ClearContents();

            //Validation
            Assert.That(siteWasCleared);
        }

        [Test]
        public void OnAllBlobTypesFilledToCapacity_ButTotalCapacityNotReached_TotalSpaceLeftIsNonzero_AndIsAtCapacityIsFalse() {
            //Setup
            var siteToTest = BuildBlobSite(PrivateData);

            siteToTest.SetPlacementPermissionsAndCapacity(ResourceSummary.BuildResourceSummary(
                siteToTest.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)
            ));
            siteToTest.TotalCapacity = 4;

            //Execution
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Red));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Green));
            siteToTest.PlaceBlobInto(BuildBlob(ResourceType.Blue));

            //Validation
            Assert.That(!siteToTest.IsAtCapacity);
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

            var blobToAdd = BuildBlob(ResourceType.Red);

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
                siteToTest.ExtractBlobOfType(ResourceType.Green);
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
            return hostingObject.AddComponent<BlobSite>();
        }

        private BlobSiteFactory BuildFactory(BlobSitePrivateDataBase privateData) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<BlobSiteFactory>();
            newFactory.BlobSitePrivateData = privateData;
            return newFactory;
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


