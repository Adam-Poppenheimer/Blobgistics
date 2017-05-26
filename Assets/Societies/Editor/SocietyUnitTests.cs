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
                    _standardComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                        hostingObject.gameObject,
                        new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
                    ));
                    _standardComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                        hostingObject.gameObject,
                        new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1)
                    ));
                    _standardComplexity.SetWants(new List<IntPerResourceDictionary>() {
                        IntPerResourceDictionary.BuildSummary(
                            hostingObject.gameObject,
                            new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
                        )
                    });
                    _standardComplexity.SetNeedsCapacityCoefficient(5);
                    _standardComplexity.SetProductionCapacityCoefficient(5);
                    _standardComplexity.SetWantsCapacityCoefficient(5);

                    _standardComplexity.SetSecondsToPerformFullProduction(1f);
                    _standardComplexity.SetSecondsToFullyConsumeNeeds(1f);

                    _standardComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
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

        [Test]
        public void OnPrivateDataInitialized_NewComplexityIsStartingComplexityDefinedInPrivateData() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();
            var complexityLadder = BuildComplexityLadder(0, startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));
            var newSociety = BuildSociety(privateData, startingComplexity);

            //Execution
            newSociety.PrivateData = privateData;

            //Validation
            Assert.That(newSociety.CurrentComplexity == startingComplexity);
        }

        [Test]
        public void Factory_OnCanConstructSocietyCalled_ReturnsFalseIfTerrainOfLocationIsIncompatibleWithStartingComplexity() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();
            locationToPlace.Terrain = TerrainType.Forest;

            var startingComplexity = BuildComplexityDefinition();
            startingComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var activeLadder = BuildComplexityLadder(0, startingComplexity);

            //Execution
            var canConstruct = factoryToTest.CanConstructSocietyAt(locationToPlace, activeLadder, startingComplexity);

            //Validation
            Assert.False(canConstruct);
        }

        [Test]
        public void Factory_OnConstructSocietyAtCalled_SocietyReturnedHasTheAppropriateLocation() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();

            //Execution
            var societyConstructed = factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);

            //Validation
            Assert.AreEqual(locationToPlace, societyConstructed.Location, "The constructed society was not in the correct location");
        }

        [Test]
        public void Factory_OnConstructSocietyAtCalled_SocietyReturnedHasTheAppropriateComplexityLadder() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();

            //Execution
            var societyConstructed = factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);

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

            var society1 = factoryToTest.ConstructSocietyAt(location1, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            var society2 = factoryToTest.ConstructSocietyAt(location2, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            
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

            factoryToTest.ConstructSocietyAt(location1, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            factoryToTest.ConstructSocietyAt(location2, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            
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

            factoryToTest.ConstructSocietyAt(location1, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            factoryToTest.ConstructSocietyAt(location2, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            factoryToTest.ConstructSocietyAt(location3, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            
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

            factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            
            //Execution


            //Validation
            Assert.IsFalse(factoryToTest.CanConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition),
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
                societyList.Add(factoryToTest.ConstructSocietyAt(locationList[i], factoryToTest.StandardComplexityLadder,
                    factoryToTest.DefaultComplexityDefinition));
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
                societyList.Add(factoryToTest.ConstructSocietyAt(locationList[i], factoryToTest.StandardComplexityLadder,
                    factoryToTest.DefaultComplexityDefinition));
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

            startingComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                startingComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   10)
            ));
            startingComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                startingComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  30)
            ));
            startingComplexity.SetWants(new List<IntPerResourceDictionary>() {
                IntPerResourceDictionary.BuildSummary(
                    startingComplexity.gameObject,
                    new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 20)
                )
            });

            startingComplexity.SetNeedsCapacityCoefficient(4);
            startingComplexity.SetWantsCapacityCoefficient(5);
            startingComplexity.SetProductionCapacityCoefficient(6);

            var complexityLadder = BuildComplexityLadder(0, startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            //Execution
            var newSociety = BuildSociety(privateData, startingComplexity);

            //Validation
            Assert.AreEqual(40, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Food),
                "Incorrect Food Capacity");
            Assert.AreEqual(100, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.Textiles),
                "Incorrect Textiles Capacity");
            Assert.AreEqual(180, newSociety.Location.BlobSite.GetSpaceLeftOfType(ResourceType.ServiceGoods),
                "Incorrect ServiceGoods Capacity");
        }

        [Test]
        public void OnCurrentComplexityChanged_ProductionsAreForbiddenPlacementIfNotAlsoNeeds_OrWants() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();
            
            startingComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                startingComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            startingComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                startingComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            startingComplexity.SetWants(new List<IntPerResourceDictionary>() {
                IntPerResourceDictionary.BuildSummary(
                    startingComplexity.gameObject,
                    new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1)
                )
            });

            startingComplexity.SetNeedsCapacityCoefficient(4);
            startingComplexity.SetWantsCapacityCoefficient(5);
            startingComplexity.SetProductionCapacityCoefficient(6);

            var complexityLadder = BuildComplexityLadder(0, startingComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            //Execution
            var newSociety = BuildSociety(privateData, startingComplexity);

            //Validation
            Assert.IsTrue(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Food),
                "Incorrect placement permission for Food");
            Assert.IsTrue(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Textiles),
                "Incorrect placement permission for Textiles");
            Assert.IsFalse(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.ServiceGoods),
                "Incorrect placement permission for ServiceGoods");
        }

        [Test]
        public void WhenAscensionIsPermitted_UnderlyingBlobSitePermitsPlacement_AndRefusesExtraction_OfAnyCostsOfAscent() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();
            startingComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
               startingComplexity.gameObject,
               new KeyValuePair<ResourceType, int>(ResourceType.Food, 2),
               new KeyValuePair<ResourceType, int>(ResourceType.Wood, 2),
               new KeyValuePair<ResourceType, int>(ResourceType.Ore, 2)
            ));
            startingComplexity.SetProductionCapacityCoefficient(5);

            var ascentComplexity = BuildComplexityDefinition();
            ascentComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Wood, 10)
            ));
            ascentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var complexityLadder = BuildComplexityLadder(0, startingComplexity, ascentComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            //Execution
            var newSociety = BuildSociety(privateData, startingComplexity);
            newSociety.AscensionIsPermitted = true;

            //Validation
            Assert.IsTrue(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Food),
                "BlobSite fails to permit placement of food");
            Assert.IsFalse(newSociety.Location.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Food),
                "BlobSite falsely permits extraction of food");

            Assert.IsTrue(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Wood),
                "BlobSite fails to permit placement of wood");
            Assert.IsFalse(newSociety.Location.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Wood),
                "BlobSite falsely permits extraction of wood");

            Assert.IsTrue(newSociety.Location.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Ore),
                "BlobSite fails to permit extraction of ore");
        }

        [Test]
        public void WhenAscensionIsForbidden_UnderlyingBlobSiteIsNotAffectedByAnyCostsOfAscent() {
            //Setup
            var startingComplexity = BuildComplexityDefinition();
            startingComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
               startingComplexity.gameObject,
               new KeyValuePair<ResourceType, int>(ResourceType.Food, 2),
               new KeyValuePair<ResourceType, int>(ResourceType.Wood, 2),
               new KeyValuePair<ResourceType, int>(ResourceType.Ore, 2)
            ));
            startingComplexity.SetProductionCapacityCoefficient(5);

            var ascentComplexity = BuildComplexityDefinition();
            ascentComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10),
                new KeyValuePair<ResourceType, int>(ResourceType.Wood, 10)
            ));

            var complexityLadder = BuildComplexityLadder(0, startingComplexity, ascentComplexity);
            var privateData = BuildPrivateData(complexityLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            //Execution
            var newSociety = BuildSociety(privateData, startingComplexity);
            newSociety.AscensionIsPermitted = false;

            //Validation
            Assert.IsFalse(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Food),
                "BlobSite falsely permits placement of food");
            Assert.IsTrue(newSociety.Location.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Food),
                "BlobSite fails to permit extraction of food");

            Assert.IsFalse(newSociety.Location.BlobSite.GetPlacementPermissionForResourceType(ResourceType.Wood),
                "BlobSite falsely permits placement of wood");
            Assert.IsTrue(newSociety.Location.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Wood),
                "BlobSite fails to permit extraction of wood");

            Assert.IsTrue(newSociety.Location.BlobSite.GetExtractionPermissionForResourceType(ResourceType.Ore),
                "BlobSite fails to permit extraction of ore");
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
            complexityToUse.SetProduction(IntPerResourceDictionary.BuildSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 10)
            ));
            complexityToUse.SetSecondsToPerformFullProduction(1f);

            var location = BuildMapNode();
            location.BlobSite.SetPlacementPermissionsAndCapacity(complexityToUse.Production);
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));

            var societyToTest = BuildSociety(complexityToUse, location);

            //Execution
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
            complexityToUse.SetNeeds(IntPerResourceDictionary.BuildSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  3)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);

            var societyToTest = BuildSociety(complexityToUse);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));

            //Execution
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.AreEqual(1, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Food).Count(),
                "Food blob count is incorrect");
            Assert.AreEqual(2, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Textiles).Count(),
                "Textiles blob count is incorrect");
            Assert.AreEqual(3, societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.ServiceGoods).Count(),
                "ServiceGoods blob count is incorrect");
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsAreAvailable_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(IntPerResourceDictionary.BuildSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 2),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  3)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);

            var societyToTest = BuildSociety(complexityToUse);
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));

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
            complexityToUse.SetNeeds(IntPerResourceDictionary.BuildSummary(
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
            complexityToUse.SetNeeds(IntPerResourceDictionary.BuildSummary(
                complexityToUse.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            complexityToUse.SetSecondsToFullyConsumeNeeds(1f);
            complexityToUse.SetComplexityDescentDuration(10f);

            var societyToTest = BuildSociety(complexityToUse);

            //Execution
            societyToTest.TickConsumption(2.5f);

            //Validation
            Assert.AreEqual(2.5f, societyToTest.SecondsOfUnsatisfiedNeeds, float.Epsilon);
            Assert.AreEqual(7.5f, societyToTest.SecondsUntilComplexityDescent, float.Epsilon);
        }

        [Test]
        public void OnConsumptionPerformed_AndNeedsWereUnsatisfied_ButNowNeedsCanBeSatisfied_NeedsBecomeSatisfied() {
            //Setup
            var complexityToUse = BuildComplexityDefinition();
            complexityToUse.SetNeedsCapacityCoefficient(5);
            complexityToUse.SetNeeds(IntPerResourceDictionary.BuildSummary(
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
            complexityToUse.SetNeeds(IntPerResourceDictionary.BuildSummary(
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

            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);
            currentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            ascentComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            ascentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var activeLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            societyToTest.TickConsumption(1f);
            
            //Validation
            Assert.That(societyToTest.CurrentComplexity == ascentComplexity);
        }

        [Test]
        public void OnConsumptionPerformed_ButNoAscentOptionIsCompatibleWithTerrainOnLocation_SocietyWillNotAscend() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            var ascentComplexity = BuildComplexityDefinition();

            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);

            ascentComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            ascentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Mountains });

            var activeLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            societyToTest.Location.BlobSite.SetCapacityForResourceType(ResourceType.Food, 10);
            societyToTest.Location.BlobSite.TotalCapacity += 1;
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            societyToTest.TickConsumption(1f);
            
            //Validation
            Assert.AreEqual(currentComplexity, societyToTest.CurrentComplexity);
        }

        [Test]
        public void OnProductionPerformed_AndSomeWantSummaryIsSatisfiable_ExactlyOneWantSummaryIsRemovedFromLocationsblobSite() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<IntPerResourceDictionary>() {
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Food,   1)),
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1)),
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)),
            });
            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));
            societyToTest.TickProduction(1f);

            bool hasRed   = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Food  ).Count() > 0;
            bool hasGreen = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.Textiles).Count() > 0;
            bool hasBlue  = societyToTest.Location.BlobSite.GetContentsOfType(ResourceType.ServiceGoods ).Count() > 0;

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
            currentComplexity.SetWants(new List<IntPerResourceDictionary>() {
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)),
            });
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1)
            ));

            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Textiles) == 2);
        }

        [Test]
        public void OnProductionPerformed_AndContainsOnlyEmptyWantSummaries_ProductionIsNotIncreased() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));

            currentComplexity.SetWants(new List<IntPerResourceDictionary>() {
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject),
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject),
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject)
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
            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)
            ));
            currentComplexity.SetWants(new List<IntPerResourceDictionary>() {
                IntPerResourceDictionary.BuildSummary(new GameObject(), new KeyValuePair<ResourceType, int>(ResourceType.Food,   1)),
                IntPerResourceDictionary.BuildSummary(new GameObject(), new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1)),
            });
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            currentComplexity.SetWantsCapacityCoefficient(5);
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetSecondsToPerformFullProduction(1f);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(2, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.ServiceGoods));
        }

        [Test]
        public void OnComplexityDescentTimerElapsed_SocietyDescendsComplexityLadder() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetName("Current");
            currentComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, Int32.MaxValue)
            ));
            currentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var descentComplexity = BuildComplexityDefinition();
            descentComplexity.SetName("Descent");
            descentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var activeLadder = BuildComplexityLadder(1, descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);

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

            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject));
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)
            ));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);
            currentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            ascentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var location = BuildMapNode();
            location.BlobSite.SetPlacementPermissionsAndCapacity(currentComplexity.Production);
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));

            var activeLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, location,
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);

            //Execution
            
            societyToTest.TickConsumption(1f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.Contents.Count() == 0);
        }

        [Test]
        public void OnDescendComplexityLadder_BlobsWithinBecomesEmpty() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)
            ));
            currentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var descentComplexity = BuildComplexityDefinition();
            descentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var location = BuildMapNode();
            location.BlobSite.SetPlacementPermissionsAndCapacity(currentComplexity.Production);
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));

            var activeLadder = BuildComplexityLadder(1, descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, location,
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);

            //Execution
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(societyToTest.Location.BlobSite.Contents.Count() == 0);
        }

        [Test]
        public void OnDescendComplexityLadder_AndSocietyLacksAnEligibleComplexityToDescendTo_SocietyIsDestroyed(){
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetComplexityDescentDuration(1f);
            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food,   1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods,  1)
            ));
            currentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });

            var descentComplexity = BuildComplexityDefinition();
            descentComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Forest });

            var location = BuildMapNode();
            location.BlobSite.SetPlacementPermissionsAndCapacity(currentComplexity.Production);
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));

            var activeLadder = BuildComplexityLadder(1, descentComplexity, currentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, location,
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);
            societyToTest.name = "Society that cannot descend";

            //Execution
            societyToTest.TickConsumption(2f);

            //Validation
            Assert.That(GameObject.Find("Society that cannot descend") == null);
        }

        [Test]
        public void WhenNeededOrWantedBlobsPlacedInto_ThoseBlobsCannotBeExtracted_ButOthersStillCanBe() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));
            currentComplexity.SetWants(new List<IntPerResourceDictionary>() { 
                IntPerResourceDictionary.BuildSummary(new GameObject(), new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1))
            });
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            currentComplexity.SetProductionCapacityCoefficient(5);
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetWantsCapacityCoefficient(5);

            var location = BuildMapNode();
            location.BlobSite.SetPlacementPermissionsAndCapacity(IntPerResourceDictionary.BuildSummary(
                new GameObject(),
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 1),
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food  ));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.ServiceGoods ));

            var societyToTest = BuildSociety(currentComplexity, location);

            //Execution

            //Validation
            Assert.That(!societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Food  ));
            Assert.That(!societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.Textiles));
            Assert.That( societyToTest.Location.BlobSite.CanExtractBlobOfType(ResourceType.ServiceGoods ));
        }

        [Test]
        public void OnProductionTicked_WhenAllWantsOnlyPartiallyFulfillable_NoResourcesPartiallyFulfillingWantsConsumed() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            currentComplexity.SetWants(new List<IntPerResourceDictionary>() { 
                IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject, new KeyValuePair<ResourceType, int>(ResourceType.Textiles, 2))
            });
            currentComplexity.SetProduction(IntPerResourceDictionary.BuildSummary(
                currentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.ServiceGoods, 1)
            ));
            currentComplexity.SetNeedsCapacityCoefficient(5);
            currentComplexity.SetWantsCapacityCoefficient(5);

            var societyToTest = BuildSociety(currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Textiles));
            societyToTest.TickProduction(1f);

            //Validation
            Assert.AreEqual(1f, societyToTest.Location.BlobSite.GetCountOfContentsOfType(ResourceType.Textiles),
                "Partially fulfilled needs were consumed");
        }

        [Test]
        public void WhenCurrentComplexityDoesNotPermitAscension_SocietyWillNeverAscend() {
            //Setup
            var currentComplexity = BuildComplexityDefinition();
            var ascentComplexity = BuildComplexityDefinition();

            currentComplexity.SetNeeds(IntPerResourceDictionary.BuildSummary(currentComplexity.gameObject));
            currentComplexity.SetSecondsToFullyConsumeNeeds(1f);

            ascentComplexity.SetCostOfAscent(IntPerResourceDictionary.BuildSummary(
                ascentComplexity.gameObject,
                new KeyValuePair<ResourceType, int>(ResourceType.Food, 1)
            ));

            var activeLadder = BuildComplexityLadder(0, currentComplexity, ascentComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, BuildMapNode(),
                BuildSocietyFactory(StandardBlobFactory));

            var societyToTest = BuildSociety(privateData, currentComplexity);

            //Execution
            societyToTest.Location.BlobSite.SetPlacementPermissionForResourceType(ResourceType.Food, true);
            societyToTest.Location.BlobSite.SetCapacityForResourceType(ResourceType.Food, 10);
            societyToTest.Location.BlobSite.TotalCapacity += 10;
            societyToTest.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ResourceType.Food));
            societyToTest.AscensionIsPermitted = false;
            societyToTest.TickConsumption(100f);
            
            //Validation
            Assert.AreNotEqual(ascentComplexity, societyToTest.CurrentComplexity);
        }

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
        public void Factory_OnCanConstructSocietyAtPassedAnyNullArgument_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var location = BuildMapNode();
            var complexityDefinition = BuildComplexityDefinition();
            var complexityLadder = BuildComplexityLadder(0, complexityDefinition);

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructSocietyAt(null, complexityLadder, complexityDefinition);
            }, "Does not throw on parameter location");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructSocietyAt(location, null, complexityDefinition);
            }, "Does not throw on parameter ladder");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.CanConstructSocietyAt(location, complexityLadder, null);
            }, "Does not throw on parameter startingComplexity");
        }

        [Test]
        public void Factory_OnConstructSocietyAtPassedNullArguments_ThrowsArgumentNullException() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            //Execution and Validation
            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructSocietyAt(null, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);
            }, "Does not throw on parameter location");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructSocietyAt(BuildMapNode(), null,
                factoryToTest.DefaultComplexityDefinition);
            }, "Does not throw on parameter ladder");

            Assert.Throws<ArgumentNullException>(delegate() {
                factoryToTest.ConstructSocietyAt(BuildMapNode(), factoryToTest.StandardComplexityLadder, null);
            }, "Does not throw on parameter startingComplexity");
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

        #region utility methods

        private ResourceBlobBase BuildResourceBlob(ResourceType typeOfBlob) {
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
            MapNodeBase location, SocietyFactoryBase parentFactory) {
            var hostingObject = new GameObject();
            var newPrivateData = hostingObject.AddComponent<MockSocietyPrivateData>();

            newPrivateData.SetActiveComplexityLadder(complexityLadder);
            newPrivateData.SetBlobFactory(blobFactory);
            newPrivateData.SetLocation(location);
            newPrivateData.SetParentFactory(parentFactory);

            return newPrivateData;
        }

        private Society BuildSociety(SocietyPrivateDataBase privateData, ComplexityDefinitionBase startingComplexity) {
            var hostingObject = new GameObject();
            var newSociety = hostingObject.AddComponent<Society>();

            newSociety.PrivateData = privateData;
            newSociety.SetCurrentComplexity(startingComplexity);
            newSociety.AscensionIsPermitted = true;

            return newSociety;
        }

        private Society BuildSociety(ComplexityDefinitionBase startingComplexity) {
            return BuildSociety(startingComplexity, BuildMapNode());
        }

        private Society BuildSociety(ComplexityDefinitionBase startingComplexity, MapNodeBase location) {
            var activeLadder = BuildComplexityLadder(0, startingComplexity);
            var privateData = BuildPrivateData(activeLadder, StandardBlobFactory, location, BuildSocietyFactory(StandardBlobFactory));
            return BuildSociety(privateData, startingComplexity);
        }

        private BlobSiteBase BuildBlobSite() {
            var hostingObject = new GameObject();
            var newBlobSite = hostingObject.AddComponent<BlobSite>();
            var privateData = hostingObject.AddComponent<MockBlobSitePrivateData>();
            privateData.SetBlobFactory(hostingObject.AddComponent<MockResourceBlobFactory>());

            newBlobSite.Configuration = privateData;
            return newBlobSite;
        }

        private SocietyFactory BuildSocietyFactory(ResourceBlobFactoryBase blobFactory) {
            var hostingObject = new GameObject();
            var newFactory = hostingObject.AddComponent<SocietyFactory>();

            newFactory.BlobFactory = blobFactory;

            var startingComplexity = BuildComplexityDefinition();
            var ascentComplexity = BuildComplexityDefinition();

            startingComplexity.SetPermittedTerrains(new List<TerrainType>() { TerrainType.Grassland });
            ascentComplexity.SetPermittedTerrains  (new List<TerrainType>() { TerrainType.Grassland });

            newFactory.SetStandardComplexityLadder(BuildComplexityLadder(0, startingComplexity, ascentComplexity));
            newFactory.SetDefaultComplexityDefinition(startingComplexity);

            return newFactory;
        }

        private ResourceBlobFactoryBase BuildBlobFactory() {
            var hostingObject = new GameObject();
            return hostingObject.AddComponent<MockResourceBlobFactory>();
        }

        private MockMapNode BuildMapNode() {
            var hostingObject = new GameObject();
            var newLocation = hostingObject.AddComponent<MockMapNode>();
            newLocation.SetBlobSite(BuildBlobSite());
            newLocation.Terrain = TerrainType.Grassland;
            return newLocation;
        }

        #endregion

        #endregion

    }

}


