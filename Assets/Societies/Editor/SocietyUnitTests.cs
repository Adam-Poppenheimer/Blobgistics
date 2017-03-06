using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies.Editor {

    /*public class SocietyUnitTests {

        #region instance fields and properties

        private MockComplexityDefinition TestComplexity1 {
            get {
                if(_complexityTest1 == null) {
                    _complexityTest1 = new MockComplexityDefinition();
                    _complexityTest1.ComplexityDescentDuration = 10f;
                    _complexityTest1.Name = "FirstComplexity";
                    _complexityTest1.Needs = new ResourceSummary(
                        new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
                    );
                    _complexityTest1.Production = new ResourceSummary(
                        new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)
                    );
                    _complexityTest1.Wants = new List<ResourceSummary>() {
                        new ResourceSummary(
                            new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1)
                        )
                    };
                    _complexityTest1.NeedsCapacityCoefficient      = 5;
                    _complexityTest1.ProductionCapacityCoefficient = 5;
                    _complexityTest1.WantsCapacityCoefficient      = 5;

                    _complexityTest1.SecondsToPerformFullProduction = 1f;
                    _complexityTest1.SecondsToFullyConsumeNeeds     = 1f;
                }
                return _complexityTest1;
            }
        }
        private MockComplexityDefinition _complexityTest1 = null;

        #endregion

        #region instance methods

        #region tests

        [Test]
        public void OnPrivateDataInitialized_NewComplexityIsStartingComplexityDefinedInPrivateData() {
            //Setup
            var newSocietyObject = new GameObject();
            var newSociety = newSocietyObject.AddComponent<Society>();
            var privateData = new MockSocietyPrivateData();
            var startingComplexity = new MockComplexityDefinition();
            privateData.StartingComplexity = startingComplexity;

            //Execution
            newSociety.PrivateData = privateData;

            //Validation
            Assert.That(newSociety.CurrentComplexity == startingComplexity);
        }

        [Test]
        public void OnCurrentComplexityChanged_NeedsWantsAndProductionCorrectlyAffectCapacity() {
            //Setup
            var newSocietyObject = new GameObject();
            var newSociety = newSocietyObject.AddComponent<Society>();
            var privateData = new MockSocietyPrivateData();

            var newComplexity = new MockComplexityDefinition();
            newComplexity.Needs      = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red,   10));
            newComplexity.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue,  30));

            newComplexity.Wants = new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 20))
            }; 

            newComplexity.NeedsCapacityCoefficient = 4;
            newComplexity.WantsCapacityCoefficient = 5;
            newComplexity.ProductionCapacityCoefficient = 6;

            privateData.StartingComplexity = newComplexity;

            //Execution
            newSociety.PrivateData = privateData;

            //Validation
            Assert.AreEqual(40, newSociety.ReadOnlyBlobsWithin.GetSpaceLeftForBlobOfType(ResourceType.Red),
                "Incorrect Red Capacity");
            Assert.AreEqual(100, newSociety.ReadOnlyBlobsWithin.GetSpaceLeftForBlobOfType(ResourceType.Green),
                "Incorrect Green Capacity");
            Assert.AreEqual(180, newSociety.ReadOnlyBlobsWithin.GetSpaceLeftForBlobOfType(ResourceType.Blue),
                "Incorrect Blue Capacity");
        }

        [Test]
        public void OnCurrentComplexityChanged_AscentCostsCorrectlyAffectCapacity() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            var ascentComplexity = new MockComplexityDefinition();
            currentComplexity.Name = "Current";
            ascentComplexity.Name = "Ascent";

            var ascentChain = new List<ComplexityDefinitionBase>() { currentComplexity, ascentComplexity };
            var costsToReach = new Dictionary<ComplexityDefinitionBase, ResourceSummary>() {
                { ascentComplexity, new ResourceSummary(
                    new KeyValuePair<ResourceType, int>(ResourceType.Red, 5)
                ) }
            };

            var complexityLadder = new MockComplexityLadder();
            complexityLadder.AscentChain = ascentChain;
            complexityLadder.CostsToReach = costsToReach;

            //Execute
            var societyToTest = BuildSociety(complexityLadder, currentComplexity);

            //Validate
            Assert.AreEqual(5, societyToTest.ReadOnlyBlobsWithin.GetSpaceLeftForBlobOfType(ResourceType.Red),
                "Incorrect Red Capacity");
        }

        [Test]
        public void OnProductionPerformed_WhenEmptyOfResources_AndTimeTickedIsSufficientForFullProduction_ProductionFoundInBlobsWithin() {
            //Setup
            var societyToTest = BuildSociety(null, TestComplexity1);

            //Execution
            societyToTest.TickProduction(societyToTest.CurrentComplexity.SecondsToPerformFullProduction);

            //Validation
            Assert.That(
                societyToTest.ReadOnlyBlobsWithin.ContainsResourceSummary(TestComplexity1.Production)
            );
        }

        [Test]
        public void OnProductionPerformed_AndNotEnoughRoom_ProductionIsInsertedToCapacity_AndNoExceptionIsThrown() {
            //Setup
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.ProductionCapacityCoefficient = 1;
            complexityToUse.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 10));
            complexityToUse.SecondsToPerformFullProduction = 1f;

            var societyToTest = BuildSociety(null, complexityToUse);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            Assert.DoesNotThrow(delegate() {
                societyToTest.TickProduction(complexityToUse.SecondsToPerformFullProduction);
            });

            //Validation
            Assert.That(
                societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Red).Count() == 10
            );
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsRemovedFromBlobsWithin() {
            //Setup
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.NeedsCapacityCoefficient = 5;
            complexityToUse.Needs = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  3)
            );
            complexityToUse.SecondsToFullyConsumeNeeds = 1f;

            var societyToTest = BuildSociety(null, complexityToUse);
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));

            //Execution
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Red).Count(),
                "Red blob count is incorrect");
            Assert.AreEqual(2, societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Green).Count(),
                "Green blob count is incorrect");
            Assert.AreEqual(3, societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Blue).Count(),
                "Blue blob count is incorrect");
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.NeedsCapacityCoefficient = 5;
            complexityToUse.Needs = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  3)
            );
            complexityToUse.SecondsToFullyConsumeNeeds = 1f;

            var societyToTest = BuildSociety(null, complexityToUse);
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));

            //Execution
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.NeedsAreSatisfied);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreUnavailable_NeedsBecomeUnsatisfied() {
            //Setup
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.NeedsCapacityCoefficient = 5;
            complexityToUse.Needs = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            );
            complexityToUse.SecondsToFullyConsumeNeeds = 1f;
            complexityToUse.ComplexityDescentDuration = 10f;

            var societyToTest = BuildSociety(null, complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(!societyToTest.NeedsAreSatisfied);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreUnavailable_SocietyStartsDescentTimer_AndSetsItProperly() {
            //Setup
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.NeedsCapacityCoefficient = 5;
            complexityToUse.Needs = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            );
            complexityToUse.SecondsToFullyConsumeNeeds = 1f;
            complexityToUse.ComplexityDescentDuration = 10f;

            var societyToTest = BuildSociety(null, complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.AreEqual(1f, societyToTest.SecondsOfUnsatisfiedNeeds, float.Epsilon);
            Assert.AreEqual(9f, societyToTest.SecondsUntilComplexityDescent, float.Epsilon);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsWereUnsatisfied_ButNowNeedsCanBeSatisfied_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.NeedsCapacityCoefficient = 5;
            complexityToUse.Needs = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            );
            complexityToUse.SecondsToFullyConsumeNeeds = 1f;
            complexityToUse.ComplexityDescentDuration = 10f;

            var societyToTest = BuildSociety(null, complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
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
            var complexityToUse = new MockComplexityDefinition();
            complexityToUse.NeedsCapacityCoefficient = 5;
            complexityToUse.Needs = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
            );
            complexityToUse.SecondsToFullyConsumeNeeds = 1f;
            complexityToUse.ComplexityDescentDuration = 10f;

            var societyToTest = BuildSociety(null, complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(Mathf.Approximately(societyToTest.SecondsOfUnsatisfiedNeeds, 0f));
            Assert.That(Mathf.Approximately(societyToTest.SecondsUntilComplexityDescent, -1f));
        }

        [Test]
        public void OnConsumptionPerformed_AndAscentConditionsAreSatisfied_SocietyAscendsComplexityLadder() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            var ascentComplexity = new MockComplexityDefinition();

            currentComplexity.Needs = ResourceSummary.Empty;
            currentComplexity.SecondsToFullyConsumeNeeds = 1f;

            var activeLadder = new MockComplexityLadder();
            activeLadder.AscentChain = new List<ComplexityDefinitionBase>() { currentComplexity, ascentComplexity };
            activeLadder.CostsToReach = new Dictionary<ComplexityDefinitionBase, ResourceSummary>() {
                { ascentComplexity, new ResourceSummary(
                    new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)
                ) }
            };

            var societyToTest = BuildSociety(activeLadder, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickConsumption(1f);
            
            //Validation
            Assert.That(societyToTest.CurrentComplexity == ascentComplexity);
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ExactlyOneWantSummaryIsRemovedFromBlobsWithin() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.Wants = new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red,   1)),
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)),
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)),
            };
            currentComplexity.WantsCapacityCoefficient = 5;
            currentComplexity.SecondsToPerformFullProduction = 1f;

            var societyToTest = BuildSociety(null, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.TickProduction(1f);

            bool hasRed   = societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Red  ).Count() > 0;
            bool hasGreen = societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Green).Count() > 0;
            bool hasBlue  = societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Blue ).Count() > 0;

            //Validation
            Assert.That(
                (!hasRed && hasGreen && hasBlue) || (hasRed && !hasGreen && hasBlue) || (hasRed && hasGreen && !hasBlue)
            );
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ProductionIsIncreasedByOne() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.Wants = new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1)),
            };
            currentComplexity.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1));

            currentComplexity.WantsCapacityCoefficient = 5;
            currentComplexity.ProductionCapacityCoefficient = 5;
            currentComplexity.SecondsToPerformFullProduction = 1f;

            var societyToTest = BuildSociety(null, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.That(societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Green).Count() == 2);
        }

        [Test]
        public void OnProductionPerformed_AndContainsOnlyEmptyWantSummaries_ProductionIsNotIncreased() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1));

            currentComplexity.Wants = new List<ResourceSummary>() {
                ResourceSummary.Empty,
                ResourceSummary.Empty,
                ResourceSummary.Empty
            };
            
            currentComplexity.ProductionCapacityCoefficient = 5;
            currentComplexity.SecondsToPerformFullProduction = 1f;

            var societyToTest = BuildSociety(null, currentComplexity);

            //Execution
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Red).Count());
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_AndOthersAreNot_WantsAreStillConsideredSatisfied() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.Needs = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1));
            currentComplexity.Wants = new List<ResourceSummary>() {
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red,   1)),
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1)),
            };
            currentComplexity.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1));
            currentComplexity.WantsCapacityCoefficient = 5;
            currentComplexity.ProductionCapacityCoefficient = 5;
            currentComplexity.NeedsCapacityCoefficient = 5;
            currentComplexity.SecondsToPerformFullProduction = 1f;

            var societyToTest = BuildSociety(null, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(2, societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Blue).Count());
        }

        [Test]
        public void OnComplexityDescentTimerElapsed_SocietyDescendsComplexityLadder() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.ComplexityDescentDuration = 1f;
            currentComplexity.Needs = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1));
            currentComplexity.Name = "Current";

            var descentComplexity = new MockComplexityDefinition();
            descentComplexity.Name = "Descent";

            var activeLadder = new MockComplexityLadder();
            activeLadder.AscentChain = new List<ComplexityDefinitionBase>() { descentComplexity, currentComplexity };
            activeLadder.CostsToReach = new Dictionary<ComplexityDefinitionBase, ResourceSummary>() {
                { currentComplexity, new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, Int32.MaxValue)) }
            };

            var societyToTest = BuildSociety(activeLadder, currentComplexity);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validate
            Assert.That(societyToTest.CurrentComplexity == descentComplexity);
        }

        [Test]
        public void OnAscendComplexityLadder_BlobsWithinBecomesEmpty() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            var ascentComplexity = new MockComplexityDefinition();

            currentComplexity.Needs = ResourceSummary.Empty;
            currentComplexity.Production = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)
            );
            currentComplexity.SecondsToFullyConsumeNeeds = 1f;

            var activeLadder = new MockComplexityLadder();
            activeLadder.AscentChain = new List<ComplexityDefinitionBase>() { currentComplexity, ascentComplexity };
            activeLadder.CostsToReach = new Dictionary<ComplexityDefinitionBase, ResourceSummary>() {
                { ascentComplexity, ResourceSummary.Empty }
            };

            var societyToTest = BuildSociety(activeLadder, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.ReadOnlyBlobsWithin.Contents.Count() == 0);
        }

        [Test]
        public void OnDescendComplexityLadder_BlobsWithinBecomesEmpty() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.ComplexityDescentDuration = 1f;
            currentComplexity.Needs = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1));
            currentComplexity.Production = new ResourceSummary(
                new KeyValuePair<ResourceType, int>(ResourceType.Red,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Green, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Blue,  1)
            );

            var descentComplexity = new MockComplexityDefinition();

            var activeLadder = new MockComplexityLadder();
            activeLadder.AscentChain = new List<ComplexityDefinitionBase>() { descentComplexity, currentComplexity };

            var societyToTest = BuildSociety(activeLadder, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(societyToTest.ReadOnlyBlobsWithin.Contents.Count() == 0);
        }

        [Test]
        public void WhenNeededOrWantedBlobsPlacedInto_ThoseBlobsCannotBeExtracted_ButOthersStillCanBe() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.Needs = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Red, 1));
            currentComplexity.Wants = new List<ResourceSummary>() { 
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 1))
            };
            currentComplexity.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1));
            currentComplexity.NeedsCapacityCoefficient = 5;
            currentComplexity.WantsCapacityCoefficient = 5;

            var societyToTest = BuildSociety(null, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Red  ));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Blue ));

            //Validation
            Assert.That(!societyToTest.CanExtractBlobOfType(ResourceType.Red  ));
            Assert.That(!societyToTest.CanExtractBlobOfType(ResourceType.Green));
            Assert.That( societyToTest.CanExtractBlobOfType(ResourceType.Blue ));
        }

        [Test]
        public void OnProductionTicked_WhenAllWantsOnlyPartiallyFulfillable_NoResourcesPartiallyFulfillingWantsConsumed() {
            //Setup
            var currentComplexity = new MockComplexityDefinition();
            currentComplexity.Wants = new List<ResourceSummary>() { 
                new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Green, 2))
            };
            currentComplexity.Production = new ResourceSummary(new KeyValuePair<ResourceType, int>(ResourceType.Blue, 1));
            currentComplexity.NeedsCapacityCoefficient = 5;
            currentComplexity.WantsCapacityCoefficient = 5;

            var societyToTest = BuildSociety(null, currentComplexity);

            //Execution
            societyToTest.PlaceBlobInto(BuildResourceBlob(ResourceType.Green));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1f, societyToTest.ReadOnlyBlobsWithin.GetAllBlobsOfType(ResourceType.Green).Count(),
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

        private Society BuildSociety(ComplexityLadderBase activeLadder, ComplexityDefinitionBase startingComplexity) {
            var newSocietyObject = new GameObject();
            var newSociety = newSocietyObject.AddComponent<Society>();
            var privateData = new MockSocietyPrivateData();

            if(activeLadder != null) {
                privateData.ActiveComplexityLadder = activeLadder;
            }
            if(startingComplexity != null) {
                privateData.StartingComplexity = startingComplexity;
            }
            newSociety.PrivateData = privateData;
            return newSociety;
        }

        #endregion

        #endregion

    }*/

}


