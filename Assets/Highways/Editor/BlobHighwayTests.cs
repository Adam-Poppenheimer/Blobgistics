using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.Highways.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways.Editor {

    public class BlobHighwayTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSetEndpointsCalled_EndpointsAreSetCorrectly_AndTubesWithinHaveCorrectEndpoints() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            firstEndpoint.transform.position = Vector3.zero;
            secondEndpoint.transform.position = new Vector3(10, 0f, 0f);

            var highwayToTest = BuildHighway();
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            //Execution
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);

            //Validation
            Assert.AreEqual(firstEndpoint, highwayToTest.FirstEndpoint, "First endpoint is incorrect");
            Assert.AreEqual(secondEndpoint, highwayToTest.SecondEndpoint, "Second endpoint is incorrect");

            Assert.AreEqual(new Vector3(1f, BlobHighway.TubeCenterOffset, 0f), tubePullingFromFirst.SourceLocation,
                "tubePullingFromFirst has incorrect SourceLocation");

            Assert.AreEqual(new Vector3(9f, BlobHighway.TubeCenterOffset, 0f), tubePullingFromFirst.TargetLocation,
                "tubePullingFromFirst has incorrect TargetLocation");

            Assert.AreEqual(new Vector3(9f, -BlobHighway.TubeCenterOffset, 0f), tubePullingFromSecond.SourceLocation,
                "tubePullingFromSecond has incorrect SourceLocation");

            Assert.AreEqual(new Vector3(1f, -BlobHighway.TubeCenterOffset, 0f), tubePullingFromSecond.TargetLocation,
                "tubePullingFromSecond has incorrect TargetLocation");
        }

        [Test]
        public void OnHighwayInitialized_AllPermissionsDefaultToFalse() {
            //Setup
            var highwayToTest = BuildHighway();
            highwayToTest.TubePullingFromFirstEndpoint = BuildBlobTube();
            highwayToTest.TubePullingFromSecondEndpoint = BuildBlobTube();

            //Execution
            

            //Validation
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                Assert.That(!highwayToTest.GetPullingPermissionForFirstEndpoint(resourceType),
                    "FirstEndpoint is given permission to pull resource " + resourceType);

                Assert.That(!highwayToTest.GetPullingPermissionForSecondEndpoint(resourceType),
                    "SecondEndpoint is given permission to pull resource " + resourceType);
            }
        }

        [Test]
        public void OnPermissionForResourceTypeSet_SamePermissionForResourceTypeIsGotten() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = BuildBlobTube();
            highwayToTest.TubePullingFromSecondEndpoint = BuildBlobTube();

            //Execution
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Textiles, true);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, false);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Textiles, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.ServiceGoods, true);

            //Validation
            Assert.That(highwayToTest.GetPullingPermissionForFirstEndpoint(ResourceType.Food),   "For FirstEndpoint, Food is not permitted");
            Assert.That(highwayToTest.GetPullingPermissionForFirstEndpoint(ResourceType.Textiles), "For FirstEndpoint, Textiles is not permitted");
            Assert.That(!highwayToTest.GetPullingPermissionForFirstEndpoint(ResourceType.ServiceGoods), "For FirstEndpoint, ServiceGoods is falsely permitted");

            Assert.That(!highwayToTest.GetPullingPermissionForSecondEndpoint(ResourceType.Food),  "For SecondEndpoint, Food is falsely permitted");
            Assert.That(highwayToTest.GetPullingPermissionForSecondEndpoint(ResourceType.Textiles), "For SecondEndpoint, Textiles is not permitted");
            Assert.That(highwayToTest.GetPullingPermissionForSecondEndpoint(ResourceType.ServiceGoods),  "For SecondEndpoint, ServiceGoods is not permitted");
        }

        [Test]
        public void OnPermissionSetForFirstEndpoint_TubePullingFromFirstEndpointAlsoHasNewPermission() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            //Execution
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Textiles, false);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.ServiceGoods, true);

            //Validation
            Assert.That(tubePullingFromFirst.GetPermissionForResourceType(ResourceType.Food),    "Tube does not give Food permission");
            Assert.That(!tubePullingFromFirst.GetPermissionForResourceType(ResourceType.Textiles), "Tube does falsely gives Textiles permission");
            Assert.That(tubePullingFromFirst.GetPermissionForResourceType(ResourceType.ServiceGoods),   "Tube does not give ServiceGoods permission");
        }

        [Test]
        public void OnPermissionSetForSecondEndpoint_TubePullingFromSecondEndpointAlsoHasNewPermission() {
            //Setup
            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(BuildMapNode(), BuildMapNode());
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            //Execution
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Textiles, false);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.ServiceGoods, true);

            //Validation
            Assert.That(tubePullingFromSecond.GetPermissionForResourceType(ResourceType.Food),    "Tube does not give Food permission");
            Assert.That(!tubePullingFromSecond.GetPermissionForResourceType(ResourceType.Textiles), "Tube does falsely gives Textiles permission");
            Assert.That(tubePullingFromSecond.GetPermissionForResourceType(ResourceType.ServiceGoods),   "Tube does not give ServiceGoods permission");
        }

        [Test]
        public void FirstEndpointHasNoExtractableTypes_CanPullFromFirstEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            Assert.AreEqual(0, firstEndpoint.BlobSite.GetExtractableTypes().Count(), "FirstEndpoint has extractable types");

            //Execution
            

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromFirstEndpoint(), "Can still pull from FirstEndpoint");
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_AndSecondEndpointCanAcceptPlacementOfOneOfThem_AndHighwayHasPermissions_CanPullFromFirstEndpointReturnsTrue() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, false);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Textiles, true);

            //Validation
            Assert.That(highwayToTest.CanPullFromFirstEndpoint());
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_AndSecondEndpointCanAcceptPlacementOfOneOfThem_ButHighwayLacksPermissions_CanPullFromFirstEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, false);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromFirstEndpoint());
        }

        [Test]
        public void FirstEndpointHasExtractableTypes_ButSecondEndpointCannotAcceptAnyOfThem_CanPullFromFirstEndpointReturnFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, false);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Textiles, true);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromFirstEndpoint());
        }

        [Test]
        public void SecondEndpointHasNoExtractableTypes_CanPullFromSecondEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            Assert.AreEqual(0, secondEndpoint.BlobSite.GetExtractableTypes().Count(), "SecondEndpoint has extractable types");

            //Execution
            

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromSecondEndpoint(), "Can still pull from SecondEndpoint");
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_AndFirstEndpointCanAcceptPlacementOfOneOfThem_AndHighwayHasPermissions_CanPullFromSecondEndpointReturnsTrue() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Textiles, true);

            //Validation
            Assert.That(highwayToTest.CanPullFromSecondEndpoint());
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_AndFirstEndpointCanAcceptPlacementOfOneOfThem_ButHighwayLacksPermissions_CanPullFromSecondEndpointReturnsFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, false);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromSecondEndpoint());
        }

        [Test]
        public void SecondEndpointHasExtractableTypes_ButFirstEndpointCannotAcceptAnyOfThem_CanPullFromSecondEndpointReturnFalse() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            
            //Execution
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, false);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Textiles, true);

            //Validation
            Assert.IsFalse(highwayToTest.CanPullFromSecondEndpoint());
        }
        
        [Test]
        public void OnPullFromFirstEndpointCalled_AndPullFromFirstEndpointReturnsTrue_AResourceIsRemovedFromFirstEndpoint_ThatCanBePlacedIntoSecondEndpoint() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirstEndpoint = BuildBlobTube();
            var tubePullingFromSecondEndpoint = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirstEndpoint;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecondEndpoint;

            var redBlob = BuildBlob(ResourceType.Food);
            var greenBlob = BuildBlob(ResourceType.Textiles);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(redBlob);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.PlaceBlobInto(greenBlob);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Textiles, true);

            //Execution
            highwayToTest.PullFromFirstEndpoint();

            //Validation
            Assert.That(firstEndpoint.BlobSite.Contents.Contains(redBlob),    "Food blob has been removed from FirstEndpoint");
            Assert.That(!firstEndpoint.BlobSite.Contents.Contains(greenBlob), "Textiles blob has not been removed from FirstEndpoint");
        }

        [Test]
        public void OnPullFromSecondEndpointCalled_AndCanPullFromSecondEndpointReturnsTrue_AResourceIsRemovedFromSecondEndpoint_ThatCanBePlacedIntoFirstEndpoint() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = BuildBlobTube();
            highwayToTest.TubePullingFromSecondEndpoint = BuildBlobTube();

            var redBlob = BuildBlob(ResourceType.Food);
            var greenBlob = BuildBlob(ResourceType.Textiles);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, false);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(redBlob);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.PlaceBlobInto(greenBlob);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Textiles, true);

            //Execution
            highwayToTest.PullFromSecondEndpoint();

            //Validation
            Assert.That(secondEndpoint.BlobSite.Contents.Contains(redBlob),    "Food blob has been removed from SecondEndpoint");
            Assert.That(!secondEndpoint.BlobSite.Contents.Contains(greenBlob), "Textiles blob has not been removed from SecondEndpoint");
        }

        [Test]
        public void OnResourcePulledFromFirstEndpoint_ResourceIsPlacedIntoAppropriateTube() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            var redBlob = BuildBlob(ResourceType.Food);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.PlaceBlobInto(redBlob);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);

            //Execution
            highwayToTest.PullFromFirstEndpoint();

            //Validation
            Assert.Contains(redBlob, tubePullingFromFirst.Contents);
        }

        [Test]
        public void OnResourcePulledFromSecondEndpoint_ResourceIsPlacedIntoAppropriateTube() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            var redBlob = BuildBlob(ResourceType.Food);

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.PlaceBlobInto(redBlob);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);

            //Execution
            highwayToTest.PullFromSecondEndpoint();

            //Validation
            Assert.Contains(redBlob, tubePullingFromSecond.Contents);
        }

        [Test]
        public void OnClearCalled_AllTubesAreClearedAsWell() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            firstEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            firstEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Food, true);
            secondEndpoint.BlobSite.SetExtractionPermissionForResourceType(ResourceType.Textiles, true);
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Food));
            secondEndpoint.BlobSite.PlaceBlobInto(BuildBlob(ResourceType.Textiles));

            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForFirstEndpoint(ResourceType.Textiles, true);

            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Food, true);
            highwayToTest.SetPullingPermissionForSecondEndpoint(ResourceType.Textiles, true);

            highwayToTest.PullFromFirstEndpoint();
            highwayToTest.PullFromFirstEndpoint();

            highwayToTest.PullFromSecondEndpoint();
            highwayToTest.PullFromSecondEndpoint();

            //Execute
            highwayToTest.Clear();

            //Validation
            Assert.AreEqual(0, tubePullingFromFirst.Contents.Count, "tubePullingFromFirst still has blobs within it");
            Assert.AreEqual(0, tubePullingFromSecond.Contents.Count, "tubePullingFromSecond still has blobs within it");
        }

        [Test]
        public void OnEfficiencyChanged_BlobTubeSpeedIsAffectedMultiplicatively() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;
            highwayToTest.Profile = BuildBlobHighwayProfile(2f, 10, 1f);

            //Execution
            highwayToTest.Efficiency = 10f;

            //Validation
            Assert.That(Mathf.Approximately(20f, tubePullingFromFirst.TransportSpeedPerSecond),
                "TubePullingFromFirst has an incorrect TransportSpeedPerSecond");
            Assert.That(Mathf.Approximately(20f, tubePullingFromSecond.TransportSpeedPerSecond),
                "TubePullingFromFirst has an incorrect TransportSpeedPerSecond");
        }

        [Test]
        public void IfCanPullFromFirstEndpointIsFalse_AndPullFromFirstEndpointIsCalled_ThrowsBlobHighwayException() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            //Execution and Validation
            Assert.Throws<BlobHighwayException>(delegate() {
                highwayToTest.PullFromFirstEndpoint();
            });
        }

        [Test]
        public void IfCanPullFromSecondEndpointIsFalse_AndPullFromSecondEndpointIsCalled_ThrowsBlobHighwayException() {
            //Setup
            var firstEndpoint = BuildMapNode();
            var secondEndpoint = BuildMapNode();

            var tubePullingFromFirst = BuildBlobTube();
            var tubePullingFromSecond = BuildBlobTube();

            var highwayToTest = BuildHighway();
            highwayToTest.SetEndpoints(firstEndpoint, secondEndpoint);
            highwayToTest.TubePullingFromFirstEndpoint = tubePullingFromFirst;
            highwayToTest.TubePullingFromSecondEndpoint = tubePullingFromSecond;

            //Execution and Validation
            Assert.Throws<BlobHighwayException>(delegate() {
                highwayToTest.PullFromSecondEndpoint();
            });
        }

        #endregion

        #region utility

        private MockMapNode BuildMapNode() {
            var hostingObject = new GameObject();

            var newBlobSiteData = hostingObject.AddComponent<MockBlobSiteConfiguration>();
            newBlobSiteData.SetConnectionCircleRadius(1f);

            var newBlobSite = hostingObject.AddComponent<MockBlobSite>();
            newBlobSite.Configuration = newBlobSiteData;
            newBlobSite.TotalCapacity = Int32.MaxValue;
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                newBlobSite.SetCapacityForResourceType(resourceType, Int32.MaxValue);
            }

            var newMapNode = hostingObject.AddComponent<MockMapNode>();
            newMapNode.blobSite = newBlobSite;

            return newMapNode;
        }

        private BlobHighway BuildHighway() {
            var hostingGameObject = new GameObject();
            var newHighway = hostingGameObject.AddComponent<BlobHighway>();
            newHighway.Profile = BuildBlobHighwayProfile(1f, 10, 0.5f);
            return newHighway;
        }

        private ResourceBlobBase BuildBlob(ResourceType type) {
            var hostingObject = new GameObject();
            var newBlob = hostingObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        private MockBlobTube BuildBlobTube() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockBlobTube>();
        }

        private BlobHighwayProfile BuildBlobHighwayProfile(float blobSpeedPerSecond, int capacity, float BlobPullCooldownInSeconds) {
            var newProfile = new BlobHighwayProfile();

            newProfile.BlobSpeedPerSecond = blobSpeedPerSecond;
            newProfile.Capacity = capacity;
            newProfile.BlobPullCooldownInSeconds = BlobPullCooldownInSeconds;

            return newProfile;
        }

        #endregion

        #endregion

    }

}
