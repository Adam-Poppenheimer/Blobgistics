using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NUnit.Framework;

using Assets.HighwayManager;

using Assets.Core.ForTesting;

namespace Assets.Core.Editor {

    public class HighwayManagerControlTests {

        #region instance methods

        #region tests

        [Test]
        public void OnDestroyHighwayManagerIsCalled_SpecifiedManagerObjectIsRemovedFromHierarchyAndAllRecords() {
            //Setup
            var controlToTest = BuildHighwayManagerControl();
            var nodeToPlaceUpon = BuildMockMapNode();

            var newManager = controlToTest.HighwayManagerFactory.ConstructHighwayManagerAtLocation(nodeToPlaceUpon);
            var managerName = "SimulationControlTest's Destroyed HighwayManager";
            var managerID = newManager.ID;
            newManager.name = managerName;


            //Execution
            controlToTest.DestroyHighwayManagerOfID(newManager.ID);

            //Validation
            Assert.Null(GameObject.Find(managerName), "There still exists a GameObject with the destroyed manager's name");
            Assert.Null(controlToTest.HighwayManagerFactory.GetHighwayManagerOfID(managerID), "HighwayManagerFactory still recognizes the destroyed manager");
        }

        [Test]
        public void OnMethodsAreCalledWithInvalidIDs_AllMethodsDisplayAnError_ButDoNotThrow() {
            throw new NotImplementedException();
        }

        #endregion

        #region utilities

        private HighwayManagerControl BuildHighwayManagerControl() {
            throw new NotImplementedException();
        }

        private MockMapNode BuildMockMapNode() {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
