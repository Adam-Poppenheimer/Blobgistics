using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ConstructionZones;
using Assets.ResourceDepots;
using Assets.Societies;
using Assets.Map;
using Assets.HighwayManager;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation of ConstructionZoneControlBase. It acts as a facade
    /// by which the UI can access parts of the simulation relating to construction zones.
    /// </summary>
    /// <remarks>
    /// This class establishes certain gameplay policies regarding construction zones that go beyond
    /// the restrictions of the factories with which it interacts. For instance, ConstructionZoneFactory
    /// doesn't know or care about SocietyFactory or societies (as well it shouldn't). As far as
    /// ConstructionZoneFactory is concerned, a ConstructionZone can be built on a node with a society.
    /// It's here, in ConstructionZoneControl, where I define the policy that nodes with societies
    /// cannot have ConstructionZones placed upon them. This module and others like it give us high-level control
    /// of how the game uses its components and what the game considers a valid action.
    /// </remarks>
    public class ConstructionZoneControl : ConstructionZoneControlBase {

        #region static fields and properties

        private static string MapNodeIDErrorMessage = "There exists no MapNode with ID {0}";
        private static string ConstructionZoneIDErrorMessage = "There exists no ConstructionZone with ID {0}";

        #endregion

        #region instance fields and properties

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public ConstructionZoneFactoryBase ConstructionZoneFactory {
            get { return _constructionZoneFactory; }
            set { _constructionZoneFactory = value; }
        }
        [SerializeField] private ConstructionZoneFactoryBase _constructionZoneFactory;

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public ResourceDepotFactoryBase ResourceDepotFactory {
            get { return _resourceDepotFactory; }
            set { _resourceDepotFactory = value; }
        }
        [SerializeField] private ResourceDepotFactoryBase _resourceDepotFactory;

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public HighwayManagerFactoryBase HighwayManagerFactory {
            get { return _highwayManagerFactory; }
            set { _highwayManagerFactory = value; }
        }
        [SerializeField] private HighwayManagerFactoryBase _highwayManagerFactory;

        #endregion

        #region instance methods

        #region from ConstructionZoneControlBase

        /// <inheritdoc/>
        /// <remarks>
        /// ConstructionZoneControl forbids the creation of a construction node on any node that
        /// has a construction zone, society, resource depot, or highway manager on it.
        /// </remarks>
        public override bool CanCreateConstructionZoneOnNode(int nodeID, string projectName) {
            var node = MapGraph.GetNodeOfID(nodeID);
            if(node != null) {
                ConstructionProjectBase project;
                return (
                    ConstructionZoneFactory.TryGetProjectOfName(projectName, out project) &&
                    !SocietyFactory.HasSocietyAtLocation(node) &&
                    !ResourceDepotFactory.HasDepotAtLocation(node) &&
                    HighwayManagerFactory.GetHighwayManagerAtLocation(node) == null &&
                    ConstructionZoneFactory.CanBuildConstructionZone(node, project)
                );
            }else {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, nodeID);
                return false;
            }
        }

        /// <inheritdoc/>
        public override void CreateConstructionZoneOnNode(int nodeID, string projectName) {
            var node = MapGraph.GetNodeOfID(nodeID);

            if(node == null) {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, nodeID);
            }else if(!CanCreateConstructionZoneOnNode(nodeID, projectName)) {
                Debug.LogErrorFormat("A ConstructionZone for a building of name {0} cannot be built on node {1}",
                    projectName, node);
            }else {
                ConstructionProjectBase project;
                ConstructionZoneFactory.TryGetProjectOfName(projectName, out project);
                ConstructionZoneFactory.BuildConstructionZone(node, project);
            }
        }

        /// <inheritdoc/>
        public override void DestroyConstructionZone(int zoneID) {
            var zoneToDestroy = ConstructionZoneFactory.GetConstructionZoneOfID(zoneID);
            if(zoneToDestroy != null) {
                ConstructionZoneFactory.DestroyConstructionZone(zoneToDestroy);
            }else {
                Debug.LogErrorFormat(ConstructionZoneIDErrorMessage, zoneID);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<ConstructionProjectUISummary> GetAllConstructionZoneProjects() {
            var retval = new List<ConstructionProjectUISummary>();
            foreach(var project in ConstructionZoneFactory.GetAvailableProjects()) {
                retval.Add(new ConstructionProjectUISummary(project));
            }
            return retval;
        }

        /// <inheritdoc/>
        public override IEnumerable<ConstructionProjectUISummary> GetAllPermittedConstructionZoneProjectsOnNode(int nodeID) {
            var retval = new List<ConstructionProjectUISummary>();

            var node = MapGraph.GetNodeOfID(nodeID);
            if(node != null) {
                if(
                    ConstructionZoneFactory.HasConstructionZoneAtLocation(node) ||
                    ResourceDepotFactory.HasDepotAtLocation(node) ||
                    SocietyFactory.HasSocietyAtLocation(node) ||
                    HighwayManagerFactory.GetHighwayManagerAtLocation(node) != null
                ){
                    return new List<ConstructionProjectUISummary>();
                }

                foreach(var project in ConstructionZoneFactory.GetAvailableProjects()) {
                    if(ConstructionZoneFactory.CanBuildConstructionZone(node, project)) {
                        retval.Add(new ConstructionProjectUISummary(project));
                    }
                }
            }else {
                Debug.LogErrorFormat(MapNodeIDErrorMessage, nodeID);
            }
            return retval;
        }

        #endregion

        #endregion
        
    }

}
