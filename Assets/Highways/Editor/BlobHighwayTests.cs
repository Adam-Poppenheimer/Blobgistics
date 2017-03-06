using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using UnityCustomUtilities.Extensions;

namespace Assets.Highways.Editor {

    /*public class BlobHighwayTests {

        #region instance methods

        #region tests

        [Test]
        public void OnEndpointsSet_FirstAndSecondEndpointAreSetProperly() {
            //Setup
            var highwayToTest = BuildHighway();
            var firstSite = new MockBlobSite();
            var secondSite = new MockBlobSite();

            var privateData = new BlobHighwayPrivateData();
            privateData.TubeFactory = new MockBlobTubeFactory();
            highwayToTest.PrivateData = privateData;

            //Execution
            highwayToTest.SetEndpoints(firstSite, secondSite);

            //Validate
            Assert.AreEqual(firstSite, highwayToTest.FirstEndpoint, "FirstEndpoint is incorrect");
            Assert.AreEqual(secondSite, highwayToTest.SecondEndpoint, "Second endpoint is incorrect");
        }

        [Test]
        public void OnEndpointsSet_TubeEndpointsSetToHighwayEndpoints() {
            //Setup
            var highwayToTest = BuildHighway();
            var firstSite = new MockBlobSite();
            var secondSite = new MockBlobSite();

            var privateData = new BlobHighwayPrivateData();
            privateData.TubeFactory = new MockBlobTubeFactory();
            highwayToTest.PrivateData = privateData;

            var directionFromFirstToSecond = firstSite.transform.GetDominantManhattanDirectionTo(secondSite.transform);
            var directionFromSecondToFirst = secondSite.transform.GetDominantManhattanDirectionTo(firstSite.transform);

            //Execution
            highwayToTest.SetEndpoints(firstSite, secondSite);

            //Validation
            Assert.AreEqual(
                highwayToTest.TubePullingFromFirstEndpoint.SourceLocation,
                firstSite.GetConnectionPointInDirection(directionFromFirstToSecond),
                "Tube pulling from FirstEndpoint has incorrect SourceLocation"
            );
            Assert.AreEqual(
                highwayToTest.TubePullingFromFirstEndpoint.TargetLocation,
                secondSite.GetConnectionPointInDirection(directionFromSecondToFirst),
                "Tube pulling from FirstEndpoint has incorrect TargetLocation"
            );

            Assert.AreEqual(
                highwayToTest.TubePullingFromSecondEndpoint.SourceLocation,
                secondSite.GetConnectionPointInDirection(directionFromSecondToFirst),
                "Tube pulling from FirstEndpoint has incorrect SourceLocation"
            );
            Assert.AreEqual(
                highwayToTest.TubePullingFromSecondEndpoint.TargetLocation,
                firstSite.GetConnectionPointInDirection(directionFromFirstToSecond),
                "Tube pulling from FirstEndpoint has incorrect TargetLocation"
            );
        }

        [Test]
        public void OnPriorityForResourceTypeSet_SamePriorityForResourceTypeIsGotten() {
            //Setup
            var highwayToTest = BuildHighway();

            //Execution
            highwayToTest.SetPriorityForResourceType(ResourceType.Red, 1);
            highwayToTest.SetPriorityForResourceType(ResourceType.Green, 2);
            highwayToTest.SetPriorityForResourceType(ResourceType.Blue, 3);

            //Validation
            Assert.AreEqual(1, highwayToTest.GetPriorityForResourceType(ResourceType.Red),
                "Priority for Red is incorrect");
            Assert.AreEqual(2, highwayToTest.GetPriorityForResourceType(ResourceType.Green),
                "Priority for Green is incorrect");
            Assert.AreEqual(3, highwayToTest.GetPriorityForResourceType(ResourceType.Blue),
                "Priority for Blue is incorrect");
        }

        [Test]
        public void OnBlobPushedIntoFirstTube_HighwayBlobsWithinContainedInsertedBlob() {
            //Setup
            var highwayToTest = BuildHighway();

            var privateData = new BlobHighwayPrivateData();
            privateData.TubeFactory = new MockBlobTubeFactory();
            highwayToTest.PrivateData = privateData;

            highwayToTest.TubePullingFromFirstEndpoint.SetPermissionForResourceType(ResourceType.Red, true);
            var blobToFind = BuildBlob(ResourceType.Red);

            //Execution
            highwayToTest.TubePullingFromFirstEndpoint.PushBlobInto(blobToFind);

            //Validate
            Assert.That(highwayToTest.BlobsWithin.Contains(blobToFind), "Highway does not contain blobToFind");
        }

        [Test]
        public void OnBlobPushedIntoSecondTube_HighwayBlobsWithinContainedInsertedBlob() {
            //Setup
            var highwayToTest = BuildHighway();

            var privateData = new BlobHighwayPrivateData();
            privateData.TubeFactory = new MockBlobTubeFactory();
            highwayToTest.PrivateData = privateData;

            highwayToTest.TubePullingFromSecondEndpoint.SetPermissionForResourceType(ResourceType.Red, true);
            var blobToFind = BuildBlob(ResourceType.Red);

            //Execution
            highwayToTest.TubePullingFromSecondEndpoint.PushBlobInto(blobToFind);

            //Validate
            Assert.That(highwayToTest.BlobsWithin.Contains(blobToFind), "Highway does not contain blobToFind");
        }

        [Test]
        public void OnBlobPulledFromFirstTube_HighwayBlobsWithinDoesNotContainPulledBlob() {
            //Setup
            var highwayToTest = BuildHighway();

            var privateData = new BlobHighwayPrivateData();
            privateData.TubeFactory = new MockBlobTubeFactory();
            highwayToTest.PrivateData = privateData;

            highwayToTest.TubePullingFromFirstEndpoint.SetPermissionForResourceType(ResourceType.Red, true);
            var blobToFind = BuildBlob(ResourceType.Red);

            //Execution
            highwayToTest.TubePullingFromFirstEndpoint.PushBlobInto(blobToFind);
            highwayToTest.TubePullingFromFirstEndpoint.PullBlobFrom(blobToFind);

            //Validate
            Assert.That(!highwayToTest.BlobsWithin.Contains(blobToFind), "Highway does contain blobToFind");
        }

        [Test]
        public void OnBlobPulledFromSecondTube_HighwayBlobsWithinDoesNotContainPulledBlob() {
            //Setup
            var highwayToTest = BuildHighway();

            var privateData = new BlobHighwayPrivateData();
            privateData.TubeFactory = new MockBlobTubeFactory();
            highwayToTest.PrivateData = privateData;

            highwayToTest.TubePullingFromSecondEndpoint.SetPermissionForResourceType(ResourceType.Red, true);
            var blobToFind = BuildBlob(ResourceType.Red);

            //Execution
            highwayToTest.TubePullingFromSecondEndpoint.PushBlobInto(blobToFind);
            highwayToTest.TubePullingFromSecondEndpoint.PullBlobFrom(blobToFind);

            //Validate
            Assert.That(!highwayToTest.BlobsWithin.Contains(blobToFind), "Highway does contain blobToFind");
        }

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

    }*/

}
