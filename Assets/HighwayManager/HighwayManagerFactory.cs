using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Highways;
using Assets.Map;
using Assets.Core;

using UnityCustomUtilities.Extensions;
using System.Collections.ObjectModel;

namespace Assets.HighwayManager {

    public class HighwayManagerFactory : HighwayManagerFactoryBase {

        #region instance fields and properties

        #region from HighwayManagerFactoryBase

        public override ReadOnlyCollection<HighwayManagerBase> Managers {
            get { return managers.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<HighwayManagerBase> managers = new List<HighwayManagerBase>();

        #endregion

        public uint ManagementRadius {
            get { return _managementRadius; }
            set { _managementRadius = value; }
        }
        [SerializeField] private uint _managementRadius;

        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        public BlobHighwayFactoryBase HighwayFactory {
            get { return _highwayFactory; }
            set {
                if(_highwayFactory != null) {
                    _highwayFactory.HighwaySubscribed    -= HighwayFactory_HighwayConstructed;
                    _highwayFactory.HighwayUnsubscribed -= HighwayFactory_HighwayBeingDestroyed;
                }
                _highwayFactory = value;
                if(_highwayFactory != null) {
                    _highwayFactory.HighwaySubscribed    += HighwayFactory_HighwayConstructed;
                    _highwayFactory.HighwayUnsubscribed += HighwayFactory_HighwayBeingDestroyed;
                }
            }
        }
        [SerializeField] private BlobHighwayFactoryBase _highwayFactory;

        public HighwayManagerPrivateDataBase ManagerPrivateData {
            get { return _privateData; }
            set { _privateData = value; }
        }
        [SerializeField] private HighwayManagerPrivateDataBase _privateData;

        [SerializeField] private GameObject ManagerPrefab;

        private DictionaryOfLists<HighwayManagerBase, BlobHighwayBase> HighwaysServedByManager =
            new DictionaryOfLists<HighwayManagerBase, BlobHighwayBase>();

        private Dictionary<BlobHighwayBase, HighwayManagerBase> ManagerServingHighway = 
            new Dictionary<BlobHighwayBase, HighwayManagerBase>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            HighwayFactory.HighwaySubscribed    -= HighwayFactory_HighwayConstructed;
            HighwayFactory.HighwayUnsubscribed -= HighwayFactory_HighwayBeingDestroyed;

            HighwayFactory.HighwaySubscribed    += HighwayFactory_HighwayConstructed;
            HighwayFactory.HighwayUnsubscribed += HighwayFactory_HighwayBeingDestroyed;
        }

        #endregion

        #region from HighwayManagerFactoryBase

        public override HighwayManagerBase GetHighwayManagerOfID(int id) {
            return managers.Find(manager => manager.ID == id);
        }

        public override IEnumerable<BlobHighwayBase> GetHighwaysServedByManager(HighwayManagerBase manager) {
            if(manager == null) {
                throw new ArgumentNullException("manager");
            }
            List<BlobHighwayBase> highwaysServed;
            HighwaysServedByManager.TryGetValue(manager, out highwaysServed);
            if(highwaysServed != null) {
                return highwaysServed;
            }else {
                return new List<BlobHighwayBase>();
            }
        }

        public override HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            HighwayManagerBase retval;
            ManagerServingHighway.TryGetValue(highway, out retval);
            return retval;
        }

        public override HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return managers.Find(manager => manager.Location == location);
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

            newManager.SetLocation(location);
            newManager.PrivateData = ManagerPrivateData;

            managers.Add(newManager);

            RefreshServiceDict();
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
            managers.Remove(manager);
            HighwaysServedByManager.RemoveList(manager);
            RefreshServiceDict();
        }

        public override void TickAllManangers(float secondsPassed) {
            foreach(var manager in managers) {
                manager.TickConsumption(secondsPassed);
            }
        }

        #endregion

        private void HighwayFactory_HighwayConstructed(object sender, BlobHighwayEventArgs e) {
            RefreshServiceDict();
        }

        private void HighwayFactory_HighwayBeingDestroyed(object sender, BlobHighwayEventArgs e) {
            RefreshServiceDict();
        }

        private void RefreshServiceDict() {
            HighwaysServedByManager.Clear();

            foreach(var highway in HighwayFactory.Highways) {
                var edgeOfHighway = MapGraph.GetEdge(highway.FirstEndpoint, highway.SecondEndpoint);

                var summaryOfClosestNodeWithManager = MapGraph.GetNearestNodeToEdgeWhere(edgeOfHighway, delegate(MapNodeBase node) {
                    return GetHighwayManagerAtLocation(node) != null;
                }, (int)ManagementRadius);

                var managerAtLocation = summaryOfClosestNodeWithManager != null ? GetHighwayManagerAtLocation(summaryOfClosestNodeWithManager.Node) : null;

                ManagerServingHighway[highway] = managerAtLocation;

                if(managerAtLocation != null) { 
                    HighwaysServedByManager.AddElementToList(managerAtLocation, highway);
                }
            }
        }

        #endregion

    }

}
