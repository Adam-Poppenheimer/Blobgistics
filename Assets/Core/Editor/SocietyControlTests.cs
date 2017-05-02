using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.Blobs;
using Assets.Societies;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class SocietyControlTests {

        #region instance methods

        #region tests

        [Test]
        public void OnCanDestroySocietyIsCalled_ReturnValueProperlyRepresentsInternalState() {
            //Setup
            var controlToTest = BuildSocietyControl();

            var mapGraph = controlToTest.MapGraph;
            var societyFactory = controlToTest.SocietyFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);
            var rightNode  = mapGraph.BuildNode(Vector3.right);

            var simpleDefinition = societyFactory.StandardComplexityLadder.GetStartingComplexity();
            var complexifiedDefinition = societyFactory.StandardComplexityLadder.GetAscentTransition(simpleDefinition);

            var simpleSociety = societyFactory.ConstructSocietyAt(middleNode, societyFactory.StandardComplexityLadder);
            var complexifiedSociety = societyFactory.ConstructSocietyAt(rightNode, societyFactory.StandardComplexityLadder, complexifiedDefinition);

            //Execution

            //Validation
            Assert.IsTrue(controlToTest.CanDestroySociety(simpleSociety.ID),
                "Is not permitted to destroy the simple society");

            Assert.IsFalse(controlToTest.CanDestroySociety(complexifiedSociety.ID), 
                "Is falsely permitted to destroy the complexified society");
        }

        [Test]
        public void OnDestroySocietyIsCalled_SpecifiedSocietyObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildSocietyControl();

            var mapGraph = controlToTest.MapGraph;
            var societyFactory = controlToTest.SocietyFactory;

            var middleNode = mapGraph.BuildNode(Vector3.zero);

            var societyToRemove = societyFactory.ConstructSocietyAt(middleNode, societyFactory.StandardComplexityLadder);
            societyToRemove.name = "SimulationControl Integration Tests' Society";

            //Execution
            controlToTest.DestroySociety(societyToRemove.ID);

            //Validation
            Assert.IsFalse(societyFactory.HasSocietyAtLocation(middleNode),
                "SocietyFactory still registers the removed middleNode");
            Assert.IsNull(GameObject.Find("SimulationControl Integration Tests' Society"),
                "Destroyed Society's GameObject still exists in the GameObject hierarchy");
        }

        [Test]
        public void OnSetAscensionPermissionForSocietyIsCalled_SpecifiedSocietyHasItsAscensionIsPermittedFieldChanged() {
            //Setup
            var controlToTest = BuildSocietyControl();

            var mapGraph = controlToTest.MapGraph;
            var societyFactory = controlToTest.SocietyFactory;

            var node1 = mapGraph.BuildNode(Vector3.zero);
            var node2   = mapGraph.BuildNode(Vector3.zero);
            var node3  = mapGraph.BuildNode(Vector3.zero);

            var society1 = societyFactory.ConstructSocietyAt(node1, societyFactory.StandardComplexityLadder);
            var society2 = societyFactory.ConstructSocietyAt(node2, societyFactory.StandardComplexityLadder);
            var society3 = societyFactory.ConstructSocietyAt(node3, societyFactory.StandardComplexityLadder);

            //Execution
            controlToTest.SetAscensionPermissionForSociety(society1.ID, false);
            controlToTest.SetAscensionPermissionForSociety(society2.ID, false);
            controlToTest.SetAscensionPermissionForSociety(society2.ID, true);
            controlToTest.SetAscensionPermissionForSociety(society3.ID, true);

            //Validation
            Assert.IsFalse(society1.AscensionIsPermitted, "Society1 falsely permits ascension");
            Assert.IsTrue (society2.AscensionIsPermitted, "Society2 does not permit ascension");
            Assert.IsTrue (society3.AscensionIsPermitted, "Society3 does not permit ascension");
        }

        [Test]
        public void OnMethodsCalledWithInvalidID_AllMethodsDisplayAnError_ButDoNoThrow() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private SocietyControl BuildSocietyControl() {
            throw new NotImplementedException();
        }

        private void CauseSocietyToAscend(SocietyBase society) {
            var currentComplexity = society.CurrentComplexity;

            foreach(var needType in currentComplexity.Needs) {
                for(int i = 0; i < currentComplexity.Needs[needType]; ++i) {
                    society.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(needType));
                }
            }

            foreach(var ascentType in currentComplexity.CostOfAscent) {
                for(int i = 0; i < currentComplexity.CostOfAscent[ascentType]; ++i) {
                    if(society.Location.BlobSite.CanPlaceBlobOfTypeInto(ascentType)) {
                        society.Location.BlobSite.PlaceBlobInto(BuildResourceBlob(ascentType));
                    }
                }
            }
            society.TickConsumption(currentComplexity.SecondsToFullyConsumeNeeds);
        }

        private ResourceBlobBase BuildResourceBlob(ResourceType type) {
            var newBlob = (new GameObject()).AddComponent<MockResourceBlob>();
            newBlob.BlobType = type;
            return newBlob;
        }

        #endregion

        #endregion

    }

}
