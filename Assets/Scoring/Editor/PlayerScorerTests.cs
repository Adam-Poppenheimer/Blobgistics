using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using NUnit.Framework;

using Assets.Scoring;
using Assets.Scoring.ForTesting;

using UnityCustomUtilities.Extensions;

namespace Assets.Scoring.Editor {

    public class PlayerScorerTests {

        #region instance methods

        #region tests

        [Test]
        public void OnSocietyFactoryInitialized_ScoreIsRecalculated_WithTheSocietiesInTheFactory() {
            //Setup
            var complexityOne = BuildMockComplexityDefinition();
            var complexityTwo = BuildMockComplexityDefinition();
            var complexityThree = BuildMockComplexityDefinition();

            complexityOne.score = 1;
            complexityTwo.score = 10;
            complexityThree.score = 100;

            var societyFactory = BuildMockSocietyFactory();
            societyFactory.ConstructSocietyAt(null, null, complexityOne);
            societyFactory.ConstructSocietyAt(null, null, complexityOne);
            societyFactory.ConstructSocietyAt(null, null, complexityOne);

            societyFactory.ConstructSocietyAt(null, null, complexityTwo);
            societyFactory.ConstructSocietyAt(null, null, complexityTwo);

            societyFactory.ConstructSocietyAt(null, null, complexityThree);

            var scorerToTest = BuildPlayerScorer();

            //Execution
            scorerToTest.SocietyFactory = societyFactory;

            //Validation
            Assert.AreEqual(123, scorerToTest.TotalScore);
        }

        [Test]
        public void OnSocietyFactoryRaisesSocietyCreatedEvent_NewSocietyIsScored_AndScoreIsIncreasedAccordingly() {
            //Setup
            var complexityOne = BuildMockComplexityDefinition();
            var complexityTwo = BuildMockComplexityDefinition();
            var complexityThree = BuildMockComplexityDefinition();

            complexityOne.score = 1;
            complexityTwo.score = 10;
            complexityThree.score = 100;

            var societyFactory = BuildMockSocietyFactory();
            societyFactory.ConstructSocietyAt(null, null, complexityOne);
            societyFactory.ConstructSocietyAt(null, null, complexityOne);
            societyFactory.ConstructSocietyAt(null, null, complexityOne);

            societyFactory.ConstructSocietyAt(null, null, complexityTwo);
            societyFactory.ConstructSocietyAt(null, null, complexityTwo);

            societyFactory.ConstructSocietyAt(null, null, complexityThree);

            var scorerToTest = BuildPlayerScorer();
            scorerToTest.SocietyFactory = societyFactory;

            //Execution and validation
            societyFactory.ConstructSocietyAt(null, null, complexityOne);
            Assert.AreEqual(124, scorerToTest.TotalScore, "Scorer produced an incorrect score after SocietyFactory added a society of complexityOne");

            societyFactory.ConstructSocietyAt(null, null, complexityTwo);
            Assert.AreEqual(134, scorerToTest.TotalScore, "Scorer produced an incorrect score after SocietyFactory added a society of complexityTwo");

            societyFactory.ConstructSocietyAt(null, null, complexityThree);
            Assert.AreEqual(234, scorerToTest.TotalScore, "Scorer produced an incorrect score after SocietyFactory added a society of complexityThree");
        }

        [Test]
        public void OnSocietyFactoryRaisesSocietyDestroyedEvent_OldSocietyIsScored_AndScoreIsDecreasedAccordingly() {
            //Setup
            var complexityOne = BuildMockComplexityDefinition();
            var complexityTwo = BuildMockComplexityDefinition();
            var complexityThree = BuildMockComplexityDefinition();

            complexityOne.score = 1;
            complexityTwo.score = 10;
            complexityThree.score = 100;

            var societyFactory = BuildMockSocietyFactory();
            var societyOne = societyFactory.ConstructSocietyAt(null, null, complexityOne);
            var societyTwo = societyFactory.ConstructSocietyAt(null, null, complexityOne);
            var societyThree = societyFactory.ConstructSocietyAt(null, null, complexityOne);

            var societyFour = societyFactory.ConstructSocietyAt(null, null, complexityTwo);
            var societyFive = societyFactory.ConstructSocietyAt(null, null, complexityTwo);

            var societySix = societyFactory.ConstructSocietyAt(null, null, complexityThree);

            var scorerToTest = BuildPlayerScorer();
            scorerToTest.SocietyFactory = societyFactory;

            //Execution and validation
            societyFactory.UnsubscribeSociety(societyOne);
            Assert.AreEqual(122, scorerToTest.TotalScore, "Scorer produced an incorrect score after Society factory unsubscribed society one");

            societyFactory.UnsubscribeSociety(societyFour);
            Assert.AreEqual(112, scorerToTest.TotalScore, "Scorer produced an incorrect score after Society factory unsubscribed society four");

            societyFactory.UnsubscribeSociety(societySix);
            Assert.AreEqual(12, scorerToTest.TotalScore, "Scorer produced an incorrect score after Society factory unsubscribed society six");
        }

        [Test]
        public void OnSocietyInFactoryRaisesCurrentComplexityChangedEvent_SocietyIsRescored_AndScoreIsChangedAccordingly() {
            //Setup
            var complexityOne = BuildMockComplexityDefinition();
            var complexityTwo = BuildMockComplexityDefinition();
            var complexityThree = BuildMockComplexityDefinition();

            complexityOne.score = 1;
            complexityTwo.score = 10;
            complexityThree.score = 100;

            var societyFactory = BuildMockSocietyFactory();
            var changingSociety = societyFactory.ConstructSocietyAt(null, null, complexityOne) as MockSociety;

            var scorerToTest = BuildPlayerScorer();
            scorerToTest.SocietyFactory = societyFactory;

            //Execution and validation
            changingSociety.SetCurrentComplexity(complexityTwo);
            Assert.AreEqual(10, scorerToTest.TotalScore, "Scorer produced an incorrect value when changingSociety changed its complexity to complexityTwo");

            changingSociety.SetCurrentComplexity(complexityThree);
            Assert.AreEqual(100, scorerToTest.TotalScore, "Scorer produced an incorrect value when changingSociety changed its complexity to complexityThree");

            changingSociety.SetCurrentComplexity(complexityOne);
            Assert.AreEqual(1, scorerToTest.TotalScore, "Scorer produced an incorrect value when changingSociety changed its complexity to complexityOne");
        }

        [Test]
        public void OnScoreChanged_RaisesScoreChangedEventWithTheProperNewScore() {
            //Setup
            var complexityOne = BuildMockComplexityDefinition();

            complexityOne.score = 1;

            var societyFactory = BuildMockSocietyFactory();

            var scorerToTest = BuildPlayerScorer();
            scorerToTest.SocietyFactory = societyFactory;

            int newScore = -1;
            scorerToTest.ScoreChanged += delegate(object sender, IntEventArgs e) {
                newScore = e.Value;
            };

            //Execution
            var societyOne = societyFactory.ConstructSocietyAt(null, null, complexityOne);

            //Validation
            Assert.AreEqual(1, newScore);
        }

        #endregion

        #region utilities

        public PlayerScorer BuildPlayerScorer() {
            return (new GameObject()).AddComponent<PlayerScorer>();
        }

        public MockSocietyFactory BuildMockSocietyFactory() {
            return (new GameObject()).AddComponent<MockSocietyFactory>();
        }

        public MockComplexityDefinition BuildMockComplexityDefinition() {
            return (new GameObject()).AddComponent<MockComplexityDefinition>();
        }

        #endregion

        #endregion

    }

}


