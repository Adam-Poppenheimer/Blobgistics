using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;
using Assets.Societies.ForTesting;


namespace Assets.Societies.Editor {

    public class SocietyFactoryUnitTests {

        #region instance methods

        #region tests

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
        public void Factory_OnConstructSocietyAtCalled_SocietyIsAlsoSubscribed() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            var locationToPlace = BuildMapNode();

            //Execution
            var societyConstructed = factoryToTest.ConstructSocietyAt(locationToPlace, factoryToTest.StandardComplexityLadder,
                factoryToTest.DefaultComplexityDefinition);

            //Validation
            Assert.Contains(societyConstructed, factoryToTest.Societies);
        }

        [Test]
        public void Factory_OnSubscribeSocietyIsCalled_SocietyAppearsInSocietiesCollection() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var societyToSubscribe = BuildSociety(BuildComplexityDefinition());

            //Execution
            factoryToTest.SubscribeSociety(societyToSubscribe);

            //Validation
            Assert.Contains(societyToSubscribe, factoryToTest.Societies);
        }

        [Test]
        public void Factory_OnSubscribeSocietyIsCalled_RaisesSocietySubscribedEvent_WithAppropriateSociety() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            SocietyBase societyReturnedByEvent = null;
            factoryToTest.SocietySubscribed += delegate(object sender, SocietyEventArgs e) {
                societyReturnedByEvent = e.Society;
            };

            var societyToSubscribe = BuildSociety(BuildComplexityDefinition());

            //Execution
            factoryToTest.SubscribeSociety(societyToSubscribe);

            //Validation
            Assert.AreEqual(societyToSubscribe, societyReturnedByEvent);
        }

        [Test]
        public void Factory_OnUnsubscribeSocietyIsCalled_SocietyNoLongerAppearsInSocietiesCollection() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            SocietyBase societyReturnedByEvent = null;
            factoryToTest.SocietyUnsubscribed += delegate(object sender, SocietyEventArgs e) {
                societyReturnedByEvent = e.Society;
            };

            var societyToUnsubscribe = BuildSociety(BuildComplexityDefinition());
            factoryToTest.SubscribeSociety(societyToUnsubscribe);

            //Execution
            factoryToTest.UnsubscribeSociety(societyToUnsubscribe);

            //Validation
            Assert.AreEqual(societyToUnsubscribe, societyReturnedByEvent);
        }

        [Test]
        public void Factory_OnUnsubscribeSocietyIsCalled_RaisesSocietyUnsubscribedEvent_WithAppropriateSociety() {
            //Setup
            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());

            var societyToUnsubscribe = BuildSociety(BuildComplexityDefinition());
            factoryToTest.SubscribeSociety(societyToUnsubscribe);

            //Execution
            factoryToTest.UnsubscribeSociety(societyToUnsubscribe);

            //Validation
            Assert.That(!factoryToTest.Societies.Contains(societyToUnsubscribe));
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
        public void Factory_OnGetComplexityDefinitionOfNameCalled_ReturnsAnAppropriateDefinition_OrNullIfNoneExists() {
            //Setup
            var definitionOne = BuildComplexityDefinition();
            definitionOne.name = "DefinitionOne";

            var definitionTwo = BuildComplexityDefinition();
            definitionTwo.name = "DefinitionTwo";

            var definitionThree = BuildComplexityDefinition();
            definitionThree.name = "DefinitionThree";

            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            factoryToTest.SetComplexityDefinitions(new List<ComplexityDefinitionBase>() {
                definitionOne, definitionTwo, definitionThree
            });

            //Execution and Validation
            Assert.AreEqual(definitionOne,   factoryToTest.GetComplexityDefinitionOfName(definitionOne.name),
                "definitionOne.name failed to return definitionOne");
            Assert.AreEqual(definitionTwo,   factoryToTest.GetComplexityDefinitionOfName(definitionTwo.name),
                "definitionTwo.name failed to return definitionTwo");
            Assert.AreEqual(definitionThree, factoryToTest.GetComplexityDefinitionOfName(definitionThree.name),
                "definitionThree.name failed to return definitionThree");
        }

        [Test]
        public void Factory_OnGetComplexityLadderOfNameCalled_ReturnsAnAppropriateLadder_OrNullIfNoneExists() {
            //Setup
            var ladderOne = BuildComplexityLadder();
            ladderOne.name = "LadderOne";

            var ladderTwo = BuildComplexityLadder();
            ladderTwo.name = "LadderTwo";

            var ladderThree = BuildComplexityLadder();
            ladderThree.name = "LadderThree";

            var factoryToTest = BuildSocietyFactory(BuildBlobFactory());
            factoryToTest.SetComplexityLadders(new List<ComplexityLadderBase>() {
                ladderOne, ladderTwo, ladderThree
            });

            //Execution and Validation
            Assert.AreEqual(ladderOne,   factoryToTest.GetComplexityLadderOfName(ladderOne.name  ), "ladderOne.name failed to return ladderOne"    );
            Assert.AreEqual(ladderTwo,   factoryToTest.GetComplexityLadderOfName(ladderTwo.name  ), "ladderTwo.name failed to return ladderTwo"    );
            Assert.AreEqual(ladderThree, factoryToTest.GetComplexityLadderOfName(ladderThree.name), "ladderThree.name failed to return ladderThree");
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

        #region utilities

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
            var blobFactory = BuildBlobFactory();
            var privateData = BuildPrivateData(activeLadder, blobFactory, location, BuildSocietyFactory(blobFactory));
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
