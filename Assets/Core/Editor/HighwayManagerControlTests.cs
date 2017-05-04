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
            //Setup
            var controlToTest = BuildHighwayManagerControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution and Validation
            DebugMessageData lastMessage;

            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyHighwayManagerOfID(42);
            }, "DestroyHighwayManagerOfID threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "DestroyHighwayManagerOfID did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;
        }

        #endregion

        #region utilities

        private HighwayManagerControl BuildHighwayManagerControl() {
            var newControl = (new GameObject()).AddComponent<HighwayManagerControl>();
            newControl.HighwayManagerFactory = newControl.gameObject.AddComponent<MockHighwayManagerFactory>();
            return newControl;
        }

        private MockMapNode BuildMockMapNode() {
            return (new GameObject()).AddComponent<MockMapNode>();
        }

        #endregion

        #endregion

    }

}
