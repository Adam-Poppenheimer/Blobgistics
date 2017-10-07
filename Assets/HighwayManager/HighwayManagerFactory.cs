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

    /// <summary>
    /// The standard implementation of HighwayManagerFactoryBase. This class builds and destroys
    /// highway managers, ticks them, and deteremines which highways are served by which managers.
    /// </summary>
    /// <remarks>
    /// By policy, every highway is assigned to the nearest highway manager within a certain range,
    /// or null if no such manager exists.
    /// </remarks>
    public class HighwayManagerFactory : HighwayManagerFactoryBase {

        #region instance fields and properties

        #region from HighwayManagerFactoryBase

        /// <inheritdoc/>
        public override ReadOnlyCollection<HighwayManagerBase> Managers {
            get { return managers.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<HighwayManagerBase> managers = new List<HighwayManagerBase>();

        #endregion

        /// <summary>
        /// The maximum range from which a highway can be managed by a highway manager.
        /// </summary>
        public uint ManagementRadius {
            get { return _managementRadius; }
            set { _managementRadius = value; }
        }
        [SerializeField] private uint _managementRadius;

        /// <summary>
        /// The MapGraph this manager uses to find the closest node containing some highway manager.
        /// </summary>
        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        /// <summary>
        /// The highway factory this manager uses in order to update service relations.
        /// </summary>
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

        /// <summary>
        /// The private data that every highway manager subscribed to this factory is configured with.
        /// </summary>
        public HighwayManagerPrivateDataBase ManagerPrivateData {
            get { return _privateData; }
            set { _privateData = value; }
        }
        [SerializeField] private HighwayManagerPrivateDataBase _privateData;

        [SerializeField] private GameObject ManagerPrefab;

        //Caches for service relations, so that they only have to be recalculated when highways or highway managers
        //are created or destroyed.
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

        /// <inheritdoc/>
        public override HighwayManagerBase GetHighwayManagerOfID(int id) {
            return managers.Find(manager => manager.ID == id);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            HighwayManagerBase retval;
            ManagerServingHighway.TryGetValue(highway, out retval);
            return retval;
        }

        /// <inheritdoc/>
        public override HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return managers.Find(manager => manager.Location == location);
        }

        /// <inheritdoc/>
        public override bool CanConstructHighwayManagerAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return GetHighwayManagerAtLocation(location) == null;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override void UnsubscribeHighwayManager(HighwayManagerBase manager) {
            if(manager == null) {
                throw new ArgumentNullException("manager");
            }
            managers.Remove(manager);
            HighwaysServedByManager.RemoveList(manager);
            RefreshServiceDict();
        }

        /// <inheritdoc/>
        public override void TickAllManangers(float secondsPassed) {
            foreach(var manager in managers) {
                manager.Tick(secondsPassed);
            }
        }

        #endregion

        private void HighwayFactory_HighwayConstructed(object sender, BlobHighwayEventArgs e) {
            RefreshServiceDict();
        }

        private void HighwayFactory_HighwayBeingDestroyed(object sender, BlobHighwayEventArgs e) {
            RefreshServiceDict();
        }

        //For performance reasons, the factory calculates the management relations for every highway and manager
        //and then caches the results. These relations are refreshed every time a highway or a highway
        //manager is created or destroyed.
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
