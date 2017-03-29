using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Map;
using Assets.BlobSites;
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
                    _standardComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(
                        hostingObject.gameObject,
                        new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
                    ));
                    _standardComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                        hostingObject.gameObject,
                        new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)
                    ));
                    _standardComplexity.SetWants(new List<ResourceSummary>() {
                        ResourceSummary.BuildResourceSummary(
                            hostingObject.gameObject,
                            new KeyValuePair<ResourceType, int>(ResourceType.White, 1)
                        )
                    });
                    _standardComplexity.SetNeedsCapacityCoefficient(5);
                    _standardComplexity.SetProductionCapacityCoefficient(5);
                    _standardComplexity.SetWantsCapacityCoefficient(5);

                    _standardComplexity.SetSecondsToPerformFullProduction(1f);
                    _standardComplexity.SetSecondsToFullyConsumeNeeds(1f);

                    _standardComplexity.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                        hostingObject.gameObject,
                        new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
                    ));
                }
                return _standardComplexity;
            }
        }
        private MockComplexityDefinition _standardComplexity = null;

        private MockResourceBlobFactory StandardBlobFactory {
            get {
                if(_standardBlobFactory == null) {
                    var hostingObject = new GameObject();
                    _standardBlobFactory = hostingObject.AddComponent<MockResourceBlobFactory>();
                }
                return _standardBlobFactory;
            }
        }
        private MockResourceBlobFactory _standardBlobFactory = null;

        #endregion

        #region instance methods

        #region tests

        #region functionality

        [Test]
        public void OnPrivateDataInitialized_NewComplexityIsStartingComplexityDefinedInPrivateData() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();
            var complexityLadder = BuildComplexityLadder(0, startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode());
            var newSociety = BuildSociety(privateData);

            //Execution
            newSociety.PrivateData = privateData;

            //Validation
            Assert.That(newSociety.CurrentComplexity == startingComplexity);
        }

        [Test]
        public void Factory_OnConstructSocietyAtCalled_SocietyReturnedHasTheAppropriateLocation() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();

            //Execution
            var societyConstructed = factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder);

            //Validation
            Assert.AreEqual(locationToPlace, societyConstructed.Location, "The constructed society was not in the correct location");
        }

        [Test]
        public void Factory_OnConstructSocietyAtCalled_SocietyReturnedHasTheAppropriateComplexityLadder() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();

            //Execution
            var societyConstructed = factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder);

            //Validation
            Assert.AreEqual(factoryToTest.StandardComplexityLadder, societyConstructed.ActiveComplexityLadder, 
                "The constructed society has the wrong ActiveComplexityLadder");
        }

        [Test]
        public void Factory_OnGetSocietyOfIDCalled_ReturnsASocietyWithTheSpecifiedID_OrNullIfNoneExists() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var location1 = BuildMapNode();
            var location2 = BuildMapNode();

            var society1 = factoryToTest.ConstructSocietyAt(location1, factoryToTest.StandardComplexityLadder);
            var society2 = factoryToTest.ConstructSocietyAt(location2, factoryToTest.StandardComplexityLadder);
            
            //Execution


            //Validation
            Assert.AreEqual(society1, factoryToTest.GetSocietyOfID(society1.ID), "Does not return society1 when its ID is given");
            Assert.AreEqual(society2, factoryToTest.GetSocietyOfID(society2.ID), "Does not return society2 when its ID is given");
            Assert.IsNull(factoryToTest.GetSocietyOfID(Int32.MaxValue), "Does not return null on an ID not expected to exist");
        }

        [Test]
        public void Factory_OnHasSocietyAtLocationCalled_ReturnsTrueIfThereExistsASocietyWithThatLocation() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var location1 = BuildMapNode();
            var location2 = BuildMapNode();
            var location3 = BuildMapNode();

            factoryToTest.ConstructSocietyAt(location1, factoryToTest.StandardComplexityLadder);
            factoryToTest.ConstructSocietyAt(location2, factoryToTest.StandardComplexityLadder);
            
            //Execution


            //Validation
            Assert.IsTrue(factoryToTest.HasSocietyAtLocation(location1), "Factory does not register society at location1");
            Assert.IsTrue(factoryToTest.HasSocietyAtLocation(location2), "Factory does not register society at location2");
            Assert.IsFalse(factoryToTest.HasSocietyAtLocation(location3), "Factory falsely registers a society at location3");
        }

        [Test]
        public void Factory_OnGetSocietyAtLocationCalled_SocietyReturnedHasTheCorrectLocation() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var location1 = BuildMapNode();
            var location2 = BuildMapNode();
            var location3 = BuildMapNode();

            factoryToTest.ConstructSocietyAt(location1, factoryToTest.StandardComplexityLadder);
            factoryToTest.ConstructSocietyAt(location2, factoryToTest.StandardComplexityLadder);
            factoryToTest.ConstructSocietyAt(location3, factoryToTest.StandardComplexityLadder);
            
            //Execution


            //Validation
            Assert.AreEqual(location1, factoryToTest.GetSocietyAtLocation(location1).Location, "The society registered at location1 had an incorrect location");
            Assert.AreEqual(location2, factoryToTest.GetSocietyAtLocation(location2).Location, "The society registered at location2 had an incorrect location");
            Assert.AreEqual(location3, factoryToTest.GetSocietyAtLocation(location3).Location, "The society registered at location3 had an incorrect location");
        }

        [Test]
        public void Factory_OnHasSocietyAtLocationReturnsTrue_CanConstructSocietyAtSameLocationReturnsFalse() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();

            factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder);
            
            //Execution


            //Validation
            Assert.IsFalse(factoryToTest.CanConstructSocietyAt(locationToPlace),
                "Factory falsely registers the ability to construct a society at locationToPlace");
        }

        [Test]
        public void Factory_OnManySocietiesCreatedAndDestroyed_NoTwoActiveSocietiesEverHaveTheSameID() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var locationList = new List<MapNodeBase>();
            for(int locationIndex = 0; locationIndex < 60; ++locationIndex) {
                locationList.Add(BuildMapNode());
            }

            var societyList = new List<SocietyBase>();

            //Execution and Validation
            int i = 0;
            for(; i < 50; ++i) {
                societyList.Add(factoryToTest.ConstructSocietyAt(locationList[i], factoryToTest.StandardComplexityLadder));
                foreach(var outerSociety in societyList) {
                    foreach(var innerSociety in societyList) {
                        if(innerSociety != outerSociety) {
                            Assert.AreNotEqual(innerSociety.ID, outerSociety.ID, "Duplicate IDs on first creation cycle on index " + i);
                        }
                    }
                }
            }
            for(i = 34; i >= 10; --i) {
                var societyToDestroy = societyList[i];
                societyList.Remove(societyToDestroy);
                factoryToTest.DestroySociety(societyToDestroy);
            }
            for(i = 10; i < 35; ++i) {
                societyList.Add(factoryToTest.ConstructSocietyAt(locationList[i], factoryToTest.StandardComplexityLadder));
                foreach(var outerSociety in societyList) {
                    foreach(var innerSociety in societyList) {
                        if(innerSociety != outerSociety) {
                            Assert.AreNotEqual(innerSociety.ID, outerSociety.ID, "Duplicate IDs on second creation cycle on index " + i);
                        }
                    }
                }
            }
        }

        [Test]
        public void OnCurrentComplexityChanged_NeedsWantsAndProductionCorrectlyAffectCapacityOfLocationsBlobSite() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();

            startingComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(
                startingComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   10)
            ));
            startingComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                startingComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.White,  30)
            ));
            startingComplexity.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(
                    startingComplexity.gameObject,
                    new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 20)
                )
            });

            startingComplexity.SetNeedsCapacityCoefficient(4);
            startingComplexity.SetWantsCapacityCoefficient(5);
            startingComplexity.SetProductionCapacityCoefficient(6);

            var complexityLadder = BuildComplexityLadder(0, startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode());

            //Execution
            var newSociety = BuildSociety(privateData);

            //Validation
            Assert.AreEqual(40, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Food),
                "Incorrect Food Capacity");
            Assert.AreEqual(100, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Yellow),
                "Incorrect Yellow Capacity");
            Assert.AreEqual(180, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.White),
                "Incorrect White Capacity");
        }

        [Test]
        public void OnCurrentComplexityChanged_AscentCostsOfNextComplexityCorrectlyAffectCapacityOfLocationsBlobSite() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            var ascentComplexity = BuildComplexityDefinition();

            ascentComplexity.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));

            var currentLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(currentLadder, StandardBlobFactory, BuildMapNode());

            //Execution
            var societyToTest = BuildSociety(privateData);

            //Validation
            Assert.AreEqual(10, societyToTest.Location.BlobSite.GetCapacityForResourceType(ResourceType.Food));
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
            complexityToUse.SetProduction(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));
            complexityToUse.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            Assert.DoesNotThrow(delegate() {
                societyToTest.TickProduction(complexityToUse.SecondsToPerformFullProduction);
            });

            //Validation
            Assert.That(
                societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Food).Count() == 10
            );
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsRemovedFromLocationsBlobSite() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.White,  3)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);

            var societyToTest = BuildSociety(complexityToUse);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));

            //Execution
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Food).Count(),
                "Food blob count is incorrect");
            Assert.AreEqual(2, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Yellow).Count(),
                "Yellow blob count is incorrect");
            Assert.AreEqual(3, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.White).Count(),
                "White blob count is incorrect");
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.White,  3)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);

            var societyToTest = BuildSociety(complexityToUse);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));

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
            complexityToUse.SetNeeds(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
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
            complexityToUse.SetNeeds(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
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
            complexityToUse.SetNeeds(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
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
            complexityToUse.SetNeeds(ResourceSummary.BuildResourceSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2f);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
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

            currentComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(currentComplexity.gameObject));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);

            ascentComplexity.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));

            var activeLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode());

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            societyToTest.TickConsumption(1f);
            
            //Validation
            Assert.That(societyToTest.CurrentComplexity == ascentComplexity);
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ExactlyOneWantSummaryIsRemovedFromLocationsblobSite() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Food,   1)),
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)),
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.White,  1)),
            });
            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.TickProduction(1f);

            bool hasRed   = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Food  ).Count() > 0;
            bool hasGreen = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Yellow).Count() > 0;
            bool hasBlue  = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.White ).Count() > 0;

            //Validation
            if(!hasRed && hasGreen && hasBlue) {
                Assert.Pass("BlobSite lacks red, but still has green and blue");
            }else if((hasRed && !hasGreen && hasBlue)) {
                Assert.Pass("BlobSite lacks green, but still had red and blue");
            }else if((hasRed && hasGreen && !hasBlue)) {
                Assert.Pass("BlobSite lacks blue, but still had red and green");
            }else {
                Assert.Fail("BlobSite recorded hasRed = {0}, hasGreen = {1}, and hasBlue = {2}, " +
                    "which is not a valid configuration", hasRed, hasGreen, hasBlue);
            }
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ProductionIsIncreasedByOne() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)),
            });
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)
            ));

            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Yellow) == 2);
        }

        [Test]
        public void OnProductionPerformed_AndContainsOnlyEmptyWantSummaries_ProductionIsNotIncreased() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));

            currentComplexity.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject),
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject),
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject)
            });
            
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Food));
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_AndOthersAreNot_WantsAreStillConsideredSatisfied() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.White,  1)
            ));
            currentComplexity.SetWants(new List<ResourceSummary>() {
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Food,   1)),
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1)),
            });
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.White, 1)
            ));
            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(2, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.White));
        }

        [Test]
        public void OnComplexityDescentTimerElapsed_SocietyDescendsComplexityLadder() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetName("Current");
            currentComplexity.SetCostOfAscent(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, Int32.MaxValue)
            ));

            var descentComplexity = BuildComplexityDefinition();
            descentComplexity.SetName("Descent");

            var activeLadder = BuildComplexityLadder(1, descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode());

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

            currentComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(currentComplexity.gameObject));
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.White,  1)
            ));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);

            var activeLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode());

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.Contents.Count() == 0);
        }

        [Test]
        public void OnDescendComplexityLadder_BlobsWithinBecomesEmpty() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.White,  1)
            ));

            var descentComplexity = BuildComplexityDefinition();

            var activeLadder = BuildComplexityLadder(1, descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode());

            var societyToTest = BuildSociety(privateData);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.Contents.Count() == 0);
        }

        [Test]
        public void WhenNeededOrWantedBlobsPlacedInto_ThoseBlobsCannotBeExtracted_ButOthersStillCanBe() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetNeeds(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetWants(new List<ResourceSummary>() { 
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 1))
            });
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.White, 1)
            ));
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetWantsCapacityCoefficient(5);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.White ));

            //Validation
            Assert.That(!societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Food  ));
            Assert.That(!societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Yellow));
            Assert.That( societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.White ));
        }

        [Test]
        public void OnProductionTicked_WhenAllWantsOnlyPartiallyFulfillable_NoResourcesPartiallyFulfillingWantsConsumed() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<ResourceSummary>() { 
                ResourceSummary.BuildResourceSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Yellow, 2))
            });
            currentComplexity.SetProduction(ResourceSummary.BuildResourceSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.White, 1)
            ));
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetWantsCapacityCoefficient(5);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Yellow));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1f, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Yellow),
                "Partially fulfilled needs were consumed");
        }

        #endregion

        #region error handling

        [Test]
        public void Factory_OnHasSocietyAtLocationReturnsFalse_AndGetSocietyAtLocationIsCalled_ThrowsSocietyException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<SocietyException>(delegate() {
                factoryToTest.GetSocietyAtLocation(BuildMapNode());
            });
        }

        [Test]
        public void Factory_OnHasSocietyAtLocationPassedNullLocation_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.HasSocietyAtLocation(null);
            });
        }

        [Test]
        public void Factory_OnGetSocietyAtLocationPassedNullLocation_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.GetSocietyAtLocation(null);
            });
        }

        [Test]
        public void Factory_OnCanConstructSocietyAtPassedNullLocation_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructSocietyAt(null);
            });
        }

        [Test]
        public void Factory_OnConstructSocietyAtPassedNullArguments_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructSocietyAt(null, factoryToTest.StandardComplexityLadder);
            });
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructSocietyAt(BuildMapNode(), null);
            });
        }

        [Test]
        public void Factory_OnDestroySocietyPassedNullSociety_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.DestroySociety(null);
            });
        }

        #endregion

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

        private ComplexityLadderBase BuildComplexityLadder(int startingIndex = 0, params ComplexityDefinitionBase[] ascentChain) {
            var ascentChainList = new List<ComplexityDefinitionBase>(ascentChain);
            var hostingObject = new GameObject();
            var newLadder = hostingObject.AddComponent<MockComplexityLadder>();
            newLadder.AscentChain = ascentChainList;
            newLadder.StartingIndex = startingIndex;
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
            var activeLadder = BuildComplexityLadder(0, startingDefinition);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode());
            return BuildSociety(privateData);
        }

        private BlobSiteBase BuildBlobSite() {
            var hostingObject = new GameObject();
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            var privateData = hostingObject.AddComponent<MockBlobSitePrivateData>();

            newBlobSite.PrivateData = privateData;
            return newBlobSite;
        }

        private SocietyFactory BuildSocietyFactory(ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<SocietyFactory>();
            newFactory.BlobFactory = blobFactory;
            newFactory.SetStandardComplexityLadder(BuildComplexityLadder(0, BuildComplexityDefinition(), BuildComplexityDefinition()));
            return newFactory;
        }

        private ResourceBlobFactoryBase BuildBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        private MapNodeBase BuildMapNode() {
            var hostingObject = new GameObject();
            var newLocation = hostingObject.AddComponent<MockMapNode>();
            newLocation.SetBlobSite(BuildBlobSite());
            return newLocation;
        }

        #endregion

        #endregion

    }

}


