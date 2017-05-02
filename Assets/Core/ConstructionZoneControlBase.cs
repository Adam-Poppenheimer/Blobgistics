using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;

namespace Assets.Core {

    public abstract class ConstructionZoneControlBase : MonoBehaviour {

        #region instance methods

        public abstract IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID);

        public abstract bool CanCreateConstructionZoneOnNode(int nodeID, string buildingName);
        public abstract void CreateConstructionZoneOnNode   (int nodeID, string buildingName);

        public abstract void DestroyConstructionZone(int zoneID);

        #endregion

    }

}
