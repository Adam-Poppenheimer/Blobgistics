using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

namespace Assets.ConstructionZones {

    public class ConstructionZone : ConstructionZoneBase {

        #region instance fields and properties

        #region from ConstructionZoneBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override ConstructionProjectBase CurrentProject {
            get { return _currentProject; }

            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                }else {
                    _currentProject = value;

                    var blobSite = Location.BlobSite;
                    blobSite.ClearPermissionsAndCapacity();
                    blobSite.ClearContents();

                    int runningCapacityTotal = 0;
                    foreach(var resourceType in _currentProject.Cost) {
                        blobSite.SetPlacementPermissionForResourceType(resourceType, true);
                        blobSite.SetExtractionPermissionForResourceType(resourceType, false);
                        blobSite.SetCapacityForResourceType(resourceType, _currentProject.Cost[resourceType]);
                        runningCapacityTotal += _currentProject.Cost[resourceType];
                    }
                    blobSite.TotalCapacity = runningCapacityTotal;

                    blobSite.BlobPlacedInto += BlobSite_BlobPlacedInto;
                }
            }
        }

        [SerializeField] private ConstructionProjectBase _currentProject;

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        [SerializeField, HideInInspector] private MapNodeBase _location;

        #endregion

        public ConstructionZoneFactoryBase ParentFactory {
            get {
                if(_parentFactory == null) {
                    throw new InvalidOperationException("ParentFactory is uninitialized");
                } else {
                    return _parentFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _parentFactory = value;
                }
            }
        }
        [SerializeField, HideInInspector] private ConstructionZoneFactoryBase _parentFactory;

        #endregion

        #region instance methods

        #region from ConstructionZoneBase

        public override ResourceSummary GetResourcesNeededToFinish() {
            var countDict = new Dictionary<ResourceType, int>();
            var blobSite = Location.BlobSite;

            foreach(var resourceType in CurrentProject.Cost) {
                countDict[resourceType] = CurrentProject.Cost[resourceType] - blobSite.GetCountOfContentsOfType(resourceType);
            }

            return new ResourceSummary(countDict);
        }

        #endregion

        private void BlobSite_BlobPlacedInto(object sender, BlobEventArgs e) {
            if(CurrentProject.Cost.IsContainedWithinBlobSite(Location.BlobSite)) {
                Location.BlobSite.ClearContents();
                Location.BlobSite.ClearPermissionsAndCapacity();
                Location.BlobSite.TotalCapacity = 0;
                Location.BlobSite.BlobPlacedInto -= BlobSite_BlobPlacedInto;
                if(CurrentProject.BuildAction != null) {
                    CurrentProject.BuildAction(Location);
                }
                ParentFactory.DestroyConstructionZone(this);
            }
        }

        #endregion
                
    }

}
