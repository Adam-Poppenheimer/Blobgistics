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
            Assert.That(GameObject.Find(depotName) == null, "There still exists a GameObject with the destroyed depot's name");
            Assert.Null(controlToTest.ResourceDepotFactory.GetDepotOfID(depotID), "DepotFactory still recognizes the destroyed depot");
        }

        [Test]
        public void OnAnyMethodIsCalledOnAnInvalidID_AllMethodsDisplayAnError_ButDoNotThrow() {
            //Setup
            var controlToTest = BuildResourceDepotControl();

            var defaultLogHandler = Debug.logger.logHandler;
            var insertionHandler = new ListInsertionLogHandler();
            Debug.logger.logHandler = insertionHandler;

            //Execution and Validation
            DebugMessageData lastMessage;

            Assert.DoesNotThrow(delegate() {
                controlToTest.DestroyResourceDepotOfID(42);
            }, "DestroyResourceDepotOfID threw an exception");

            lastMessage = insertionHandler.StoredMessages.LastOrDefault();
            Assert.NotNull(lastMessage, "DestroyResourceDepotOfID did not display an error");
            insertionHandler.StoredMessages.Clear();
            lastMessage = null;

            //Cleanup
            Debug.logger.logHandler = defaultLogHandler;
        }

        #endregion

        #region utilities

        private ResourceDepotControl BuildResourceDepotControl() {
            var newControl = (new GameObject()).AddComponent<ResourceDepotControl>();
            newControl.ResourceDepotFactory = BuildMockDepotFactory();

            return newControl;
        }

        private MockResourceDepotFactory BuildMockDepotFactory() {
            return (new GameObject()).AddComponent<MockResourceDepotFactory>();
        }

        private MockMapNode BuildMockMapNode() {
            return (new GameObject()).AddComponent<MockMapNode>();
        }

        #endregion

        #endregion

    }

}
