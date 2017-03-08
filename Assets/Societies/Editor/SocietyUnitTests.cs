using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Map;
using Assets.Societies.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies.Editor {

    public class SocietyUnitTests {

        #region instance fields and properties

        private MockComplexityDefinition StandardComplexity {
            get {
                if(_standardComplexity == null) {
                    var hostingObject = new GameObject();
                    _standardComplexity = hostingObject.AddComponent<MockComplexityDefinition>();
                    _standardComplexity.SetComplexityDescentDuration(10f);
                    _standardComplexity.SetName("StandardComplexity");
                    _standardComplexity.SetNeeds(new ResourceSummary(
                        new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
                    ));
                    _standardComplexity.SetProduction(new ResourceSummary(
                        new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)
                    ));
                    _standardComplexity.SetWants(new List<ResourceSummary>() {
                        new ResourceSummary(
                            new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)
                        )
                    });
                    _standardComplexity.SetNeedsCapacityCoefficient(5);
                    _standardComplexity.SetProductionCapacityCoefficient(5);
                    _standardComplexity.SetWantsCapacityCoefficient(5);

                    _standardComplexity.SetSecondsToPerformFullProduction(1f);
                    _standardComplexity.SetSecondsToFullyConsumeNeeds(1f);

                    _standardComplexity.SetCostOfAscent(new ResourceSummary(
                        new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)
                    ));
                }
                return _standardComplexity;
            }
        }
        private MockComplexityDefinition _standardComplexity = null;

        private MockResourceBlobFactory StandardBlobFactory {
            get {
                throw new NotImplementedException();
            }
        }

        private MockMapNode StandardLocation {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region instance methods

        #region tests

        [Test]
        public void OnPrivateDataInitialized_NewComplexityIsStartingComplexityDefinedInPrivateData() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();
            var complexityLadder = BuildComplexityLadder(startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, StandardLocation);
            var newSociety = BuildSociety(privateData);

            //Execution
            newSociety.PrivateData = privateData;

            //Validation
            Assert.That(newSociety.CurrentComplexity == startingComplexity);
        }

        [Test]
        public void OnCurrentComplexityChanged_NeedsWantsAndProductionCorrectlyAffectCapacityOfLocationsBlobSite() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();

            startingComplexity.SetNeeds     (new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red,   10)));
            startingComplexity.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue,  30)));
            startingComplexity.SetWants(new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 20))
            });

            startingComplexity.SetNeedsCapacityCoefficient(4);
            startingComplexity.SetWantsCapacityCoefficient(5);
            startingComplexity.SetProductionCapacityCoefficient(6);

            var complexityLadder = BuildComplexityLadder(startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, StandardLocation);
            var newSociety = BuildSociety(privateData); 

            //Execution
            newSociety.PrivateData = privateData;

            //Validation
            Assert.AreEqual(40, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Red),
                "Incorrect Red Capacity");
            Assert.AreEqual(100, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Green),
                "Incorrect Green Capacity");
            Assert.AreEqual(180, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Blue),
                "Incorrect Blue Capacity");
        }

        [Test]
        public void OnCurrentComplexityChanged_AscentCostsOfNextComplexityCorrectlyAffectCapacityOfLocationsBlobSite() {
            throw new NotImplementedException();
        }

        [Test]
        public void OnProductionPerformed_WhenEmptyOfResources_AndTimeTickedIsSufficientForFullProduction_ProductionFoundInLocationsBlobSite() {
            //Setup
            var societyToTest = BuildSociety(StandardComplexity);

            //Execution
            societyToTest.TickProduction(societyToTest.CurrentComplexity.SecondsToPerformFullProduction);

            //Validation
            Assert.That( StandardComplexity.Production.IsContainedWithinBlobSite(societyToTest.Location.BlobSite) );
        }

        [Test]
        public void OnProductionPerformed_AndNotEnoughRoom_ProductionIsInsertedToCapacity_AndNoExceptionIsThrown() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetProductionCapacityCoefficient(1);
            complexityToUse.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 10)));
            complexityToUse.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            Assert.DoesNotThrow(delegate() {
                societyToTest.TickProduction(complexityToUse.SecondsToPerformFullProduction);
            });

            //Validation
            Assert.That(
                societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Red).Count() == 10
            );
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsRemovedFromLocationsBlobSite() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  3)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);

            var societyToTest = BuildSociety(complexityToUse);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));

            //Execution
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Red).Count(),
                "Red blob count is incorrect");
            Assert.AreEqual(2, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Green).Count(),
                "Green blob count is incorrect");
            Assert.AreEqual(3, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Blue).Count(),
                "Blue blob count is incorrect");
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  3)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);

            var societyToTest = BuildSociety(complexityToUse);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));

            //Execution
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.NeedsAreSatisfied);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreUnavailable_NeedsBecomeUnsatisfied() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(!societyToTest.NeedsAreSatisfied);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreUnavailable_SocietyStartsDescentTimer_AndSetsItProperly() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.AreEqual(1f, societyToTest.SecondsOfUnsatisfiedNeeds, float.Epsilon);
            Assert.AreEqual(9f, societyToTest.SecondsUntilComplexityDescent, float.Epsilon);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsWereUnsatisfied_ButNowNeedsCanBeSatisfied_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.NeedsAreSatisfied);
            Assert.That(Mathf.Approximately(societyToTest.SecondsOfUnsatisfiedNeeds, 0f), 
                "SecondsOfUnsatisfiedNeeds is not approximately zero");
            Assert.That(Mathf.Approximately(societyToTest.SecondsUntilComplexityDescent, -1f),
                "SecondsUntilComplexityDescent is not approximately -1");
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsWereUnsatsified_ButNowNeedsCanBeSatisfied_ComplexityDescentTimerResets() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(Mathf.Approximately(societyToTest.SecondsOfUnsatisfiedNeeds, 0f));
            Assert.That(Mathf.Approximately(societyToTest.SecondsUntilComplexityDescent, -1f));
        }

        [Test]
        public void OnConsumptionPerformed_AndAscentConditionsAreSatisfied_SocietyAscendsComplexityLadder() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            var ascentComplexity = BuildComplexityDefinition();

            currentComplexity.SetNeeds(ResourceSummary.Empty);
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);

            ascentComplexity.SetCostOfAscent(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            ));

            var activeLadder = BuildComplexityLadder(currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, StandardLocation);

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickConsumption(1f);
            
            //Validation
            Assert.That(societyToTest.CurrentComplexity == ascentComplexity);
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ExactlyOneWantSummaryIsRemovedFromLocationsblobSite() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red,   1)),
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)),
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)),
            });
            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.TickProduction(1f);

            bool hasRed   = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Red  ).Count() > 0;
            bool hasGreen = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Green).Count() > 0;
            bool hasBlue  = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Blue ).Count() > 0;

            //Validation
            Assert.That(
                (!hasRed && hasGreen && hasBlue) || (hasRed && !hasGreen && hasBlue) || (hasRed && hasGreen && !hasBlue)
            );
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ProductionIsIncreasedByOne() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)),
            });
            currentComplexity.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)));

            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Green) == 2);
        }

        [Test]
        public void OnProductionPerformed_AndContainsOnlyEmptyWantSummaries_ProductionIsNotIncreased() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)));

            currentComplexity.SetWants(new List<ResourceSummary>() {
                ResourceSummary.Empty,
                ResourceSummary.Empty,
                ResourceSummary.Empty
            });
            
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Red));
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_AndOthersAreNot_WantsAreStillConsideredSatisfied() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetNeeds(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)));
            currentComplexity.SetWants(new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red,   1)),
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)),
            });
            currentComplexity.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)));
            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(2, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Blue));
        }

        [Test]
        public void OnComplexityDescentTimerElapsed_SocietyDescendsComplexityLadder() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)));
            currentComplexity.SetName("Current");
            currentComplexity.SetCostOfAscent(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, Int32.MaxValue)));

            var descentComplexity = BuildComplexityDefinition();
            descentComplexity.SetName("Descent");

            var activeLadder = BuildComplexityLadder(descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, StandardLocation);

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validate
            Assert.That(societyToTest.CurrentComplexity == descentComplexity);
        }

        [Test]
        public void OnAscendComplexityLadder_BlobsWithinBecomesEmpty() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            var ascentComplexity = BuildComplexityDefinition();

            currentComplexity.SetNeeds(ResourceSummary.Empty);
            currentComplexity.SetProduction(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)
            ));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);

            var activeLadder = BuildComplexityLadder(currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, StandardLocation);

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.Contents.Count() == 0);
        }

        [Test]
        public void OnDescendComplexityLadder_BlobsWithinBecomesEmpty() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)));
            currentComplexity.SetProduction(new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)
            ));

            var descentComplexity = BuildComplexityDefinition();

            var activeLadder = BuildComplexityLadder(descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, StandardLocation);

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.Contents.Count() == 0);
        }

        [Test]
        public void WhenNeededOrWantedBlobsPlacedInto_ThoseBlobsCannotBeExtracted_ButOthersStillCanBe() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetNeeds(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)));
            currentComplexity.SetWants(new List<ResourceSummary>() { 
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1))
            });
            currentComplexity.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)));
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetWantsCapacityCoefficient(5);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));

            //Validation
            Assert.That(!societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Red  ));
            Assert.That(!societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Green));
            Assert.That( societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Blue ));
        }

        [Test]
        public void OnProductionTicked_WhenAllWantsOnlyPartiallyFulfillable_NoResourcesPartiallyFulfillingWantsConsumed() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<ResourceSummary>() { 
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 2))
            });
            currentComplexity.SetProduction(new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)));
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetWantsCapacityCoefficient(5);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1f, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Green),
                "Partially fulfilled needs were consumed");
        }

        #endregion

        #region utility methods

        private ResourceBlob BuildResourceBlob(ResourceType typeOfBlob) {
            var hostingGameObject = new GameObject();
            var newBlob = hostingGameObject.AddComponent<ResourceBlob>();
            newBlob.BlobType = typeOfBlob;
            return newBlob;
        }

        private MockComplexityDefinition BuildComplexityDefinition() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockComplexityDefinition>();
        }

        private ComplexityLadderBase BuildComplexityLadder(params ComplexityDefinitionBase[] ascentChain) {
            var ascentChainList = new List<ComplexityDefinitionBase>(ascentChain);
            var hostingObject = new GameObject();
            var newLadder = hostingObject.AddComponent<MockComplexityLadder>();
            newLadder.AscentChain = ascentChainList;
            return newLadder;
        }

        private MockSocietyPrivateData BuildPrivateData(ComplexityLadderBase complexityLadder, ResourceBlobFactoryBase blobFactory,
            MapNodeBase location) {
            var hostingObject = new GameObject();
            var newPrivateData = hostingObject.AddComponent<MockSocietyPrivateData>();

            newPrivateData.SetActiveComplexityLadder(complexityLadder);
            newPrivateData.SetBlobFactory(blobFactory);
            newPrivateData.SetLocation(location);

            return newPrivateData;
        }

        private Society BuildSociety(SocietyPrivateDataBase privateData) {
            var hostingObject = new GameObject();
            var newSociety = hostingObject.AddComponent<Society>();
            newSociety.PrivateData = privateData;
            return newSociety;
        }

        private Society BuildSociety(ComplexityDefinitionBase startingDefinition) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}


