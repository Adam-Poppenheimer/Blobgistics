using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.ConstructionZones {

    public abstract class ConstructionZoneFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<ConstructionZoneBase> ConstructionZones { get; }

        #endregion

        #region instance methods

        public abstract ConstructionZoneBase GetConstructionZoneOfID(int id);

        public abstract bool                 HasConstructionZoneAtLocation(MapNodeBase location);
        public abstract ConstructionZoneBase GetConstructionZoneAtLocation(MapNodeBase location);

        public abstract bool                 CanBuildConstructionZone(MapNodeBase location, ConstructionProjectBase project);
        public abstract ConstructionZoneBase BuildConstructionZone   (MapNodeBase location, ConstructionProjectBase project);

        public abstract void DestroyConstructionZone(ConstructionZoneBase constructionZone);
        public abstract void UnsubsribeConstructionZone(ConstructionZoneBase constructionZone);

        public abstract bool TryGetProjectOfName(string projectName, out ConstructionProjectBase project);

        public abstract IEnumerable<ConstructionProjectBase> GetAvailableProjects();

        #endregion

    }

}
