using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.ResourceDepots;
using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class ResourceDepotControlTests {

        #region instance methods

        #region tests

        [Test]
        public void OnDestroyResourceDepotIsCalled_SpecifiedDepotIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildResourceDepotControl();
            var nodeToPlaceUpon = BuildMockMapNode();

            var newDepot = controlToTest.ResourceDepotFactory.ConstructDepotAt(nodeToPlaceUpon);
            var depotName = "SimulationControlTest's Destroyed Depot";
            var depotID = newDepot.ID;
            newDepot.name = depotName;


            //Execution
            controlToTest.DestroyResourceDepotOfID(newDepot.ID);

            //Validation
            Assert.Null(GameObject.Find(depotName), "There still exists a GameObject with the destroyed depot's name");
            Assert.Null(controlToTest.ResourceDepotFactory.GetDepotOfID(depotID), "DepotFactory still recognizes the destroyed depot");
        }

        [Test]
        public void OnAnyMethodIsCalledOnAnInvalidID_AllMethodsDisplayAnError_ButDoNotThrow() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private ResourceDepotControl BuildResourceDepotControl() {
            throw new NotImplementedException();
        }

        private MockMapNode BuildMockMapNode() {
            return (new GameObject()).AddComponent<MockMapNode>();
        }

        #endregion

        #endregion

    }

}
