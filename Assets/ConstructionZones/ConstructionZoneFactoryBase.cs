using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.ConstructionZones {

    public abstract class ConstructionZoneFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ConstructionProjectBase ResourceDepotProject { get; }
        public abstract ConstructionProjectBase FarmlandProject      { get; }
        public abstract ConstructionProjectBase VillageProject       { get; }

        #endregion

        #region instance methods

        public abstract ConstructionZoneBase GetConstructionZoneOfID(int id);

        public abstract bool HasConstructionZoneAtLocation(MapNodeBase location);
        public abstract ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location);

        public abstract ConstructionZoneBase BuildConstructionZone(MapNodeBase location, ConstructionProjectBase project);

        public abstract void DestroyConstructionZone(ConstructionZoneBase constructionZone);

        #endregion

    }

}
