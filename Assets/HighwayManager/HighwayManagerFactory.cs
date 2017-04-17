using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;
using Assets.Map;

namespace Assets.HighwayManager {

    public class HighwayManagerFactory : HighwayManagerFactoryBase {

        #region instance fields and properties

        public uint ManagementRadius {
            get { return _managementRadius; }
            set { _managementRadius = value; }
        }
        [SerializeField] private uint _managementRadius;

        public int NeedStockpileCoefficient {
            get { return _needStockpileCoefficient; }
            set { _needStockpileCoefficient = value; }
        }
        [SerializeField] private int _needStockpileCoefficient;

        public float SecondsForManagerToPerformConsumption {
            get { return _secondsForManagerToPerformConsumption; }
            set { _secondsForManagerToPerformConsumption = value; }
        }
        [SerializeField] private float _secondsForManagerToPerformConsumption;

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set {
                if(_highwayFactory != null) {
                    _highwayFactory.HighwayConstructed    -= HighwayFactory_HighwayConstructed;
                    _highwayFactory.HighwayBeingDestroyed -= HighwayFactory_HighwayBeingDestroyed;
                }
                _highwayFactory = value;
                if(_highwayFactory != null) {
                    _highwayFactory.HighwayConstructed    += HighwayFactory_HighwayConstructed;
                    _highwayFactory.HighwayBeingDestroyed += HighwayFactory_HighwayBeingDestroyed;
                }
            }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        [SerializeField] private GameObject ManagerPrefab;

        [SerializeField, HideInInspector] private List<HighwayManagerBase> InstantiatedManagers = new List<HighwayManagerBase>();

        private Dictionary<HighwayManagerBase, List<BlobHighwayBase>> HighwaysServedByManager =
            new Dictionary<HighwayManagerBase, List<BlobHighwayBase>>();

        private List<BlobHighwayBase> UnservedHighways = 
            new List<BlobHighwayBase>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            HighwayFactory.HighwayConstructed    -= HighwayFactory_HighwayConstructed;
            HighwayFactory.HighwayBeingDestroyed -= HighwayFactory_HighwayBeingDestroyed;

            HighwayFactory.HighwayConstructed    += HighwayFactory_HighwayConstructed;
            HighwayFactory.HighwayBeingDestroyed += HighwayFactory_HighwayBeingDestroyed;
        }

        #endregion

        #region from HighwayManagerFactoryBase

        public override IEnumerable<BlobHighwayBase> GetHighwaysServedByManager(HighwayManagerBase manager) {
            if(manager == null) {
                throw new ArgumentNullException("manager");
            }
            return HighwaysServedByManager[manager];
        }

        public override HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }

            var edgeWithHighway = MapGraph.GetEdge(highway.FirstEndpoint, highway.SecondEndpoint);
            var distanceResults = MapGraph.GetNodesWithinDistanceOfEdge(edgeWithHighway, ManagementRadius);
            distanceResults.Sort((x, y) => x.Distance - y.Distance);
            foreach(var distanceResult in distanceResults) {
                var managerAtLocation = GetHighwayManagerAtLocation(distanceResult.Node);
                if(managerAtLocation != null) {
                    return managerAtLocation;
                }
            }
            return null;
        }

        public override HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return InstantiatedManagers.Find(manager => manager.Location == location);
        }

        public override bool CanConstructHighwayManagerAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return GetHighwayManagerAtLocation(location) == null;
        }

        public override HighwayManagerBase ConstructHighwayManagerAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(!CanConstructHighwayManagerAtLocation(location)) {
                throw new HighwayManagerException("Cannot construct a HighwayManager at this location");
            }
            HighwayManager newManager;
            if(ManagerPrefab != null) {
                var clonedManager = Instantiate(ManagerPrefab);
                newManager = clonedManager.GetComponent<HighwayManager>();
                if(newManager == null) {
                    throw new HighwayManagerException("The HighwayManagerPrefab lacks a HighwayManager component");
                }
            }else {
                var hostingObject = new GameObject();
                newManager = hostingObject.AddComponent<HighwayManager>();
            }
            newManager.transform.SetParent(location.transform, false);

            newManager.SetManagementRadius(ManagementRadius);
            newManager.SetLocation(location);
            newManager.SetNeedStockpileCoefficient(NeedStockpileCoefficient);
            newManager.SetSecondsToPerformConsumption(SecondsForManagerToPerformConsumption);
            newManager.ParentFactory = this;

            InstantiatedManagers.Add(newManager);

            RefreshServiceDict(newManager);
            return newManager;
        }

        public override void DestroyHighwayManager(HighwayManagerBase manager) {
            if(manager == null) {
                throw new ArgumentNullException("manager");
            }
            UnsubscribeHighwayManager(manager);
            if(Application.isPlaying) {
                Destroy(manager.gameObject);
            }else {
                DestroyImmediate(manager.gameObject);
            }
        }

        public override void UnsubscribeHighwayManager(HighwayManagerBase manager) {
            if(manager == null) {
                throw new ArgumentNullException("manager");
            }
            InstantiatedManagers.Remove(manager);
            HighwaysServedByManager.Remove(manager);
        }

        public override void SubscribeHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            var servingManager = GetManagerServingHighway(highway);
            if(servingManager == null) {
                UnservedHighways.Add(highway);
            }else if(!HighwaysServedByManager[servingManager].Contains(highway)) {
                HighwaysServedByManager[servingManager].Add(highway);
            }
        }

        public override void UnsubscribeHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            foreach(var listOfServedHighways in HighwaysServedByManager.Values) {
                listOfServedHighways.Remove(highway);
            }
            UnservedHighways.Remove(highway);
        }

        public override void TickAllManangers(float secondsPassed) {
            foreach(var manager in InstantiatedManagers) {
                manager.TickConsumption(secondsPassed);
            }
        }

        #endregion

        private void HighwayFactory_HighwayConstructed(object sender, BlobHighwayEventArgs e) {
            SubscribeHighway(e.Highway);
        }

        private void HighwayFactory_HighwayBeingDestroyed(object sender, BlobHighwayEventArgs e) {
            UnsubscribeHighway(e.Highway);
        }

        private void RefreshServiceDict(HighwayManager manager) {
            HighwaysServedByManager[manager] = new List<BlobHighwayBase>();

            foreach(var highway in new List<BlobHighwayBase>(UnservedHighways)) {
                if(GetManagerServingHighway(highway) == manager) {
                    HighwaysServedByManager[manager].Add(highway);
                    UnservedHighways.Remove(highway);
                }
            }
        }

        #endregion

    }

}
