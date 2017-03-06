using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;

namespace Assets.Core {

    public class SimulationControl : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private BlobHighwayFactoryBase BlobHighwayFactory;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            IncrementTime(Time.deltaTime);
        }

        #endregion

        public void SetHighwayPriority(int highwayID, int newPriority) {
            var highwayToChange = BlobHighwayFactory.GetHighwayOfID(highwayID);
            highwayToChange.Priority = newPriority;
        }

        public void IncrementTime(float secondsPassed) {
            BlobHighwayFactory.TickHighways(secondsPassed);
        }

        #endregion

    }

}
