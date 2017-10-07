using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    /// <summary>
    /// A data structure for serializing the entire state of a given map or runtime.
    /// Underpins the persistence of both saved games and maps.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The information in this class is sufficient to recreate pretty much any game 
    /// state, though it does not keep track of resources in highways or tubes.
    /// </para>
    /// <para>
    /// This class is mostly just a container for data. The real logic of serializing 
    /// the game's state takes place in <see cref="SessionManager"/>
    /// </para>
    /// </remarks>
    [Serializable, DataContract]
    public class SerializableSession {

        #region instance fields and properties

        /// <summary>
        /// The name of the session.
        /// </summary>
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        [SerializeField, DataMember()] private string _name;

        /// <summary>
        /// A description for the session, usually used when the session is a map.
        /// </summary>
        public string Description {
            get { return _description; }
            set { _description = value; }
        }
        [SerializeField, DataMember()] private string _description;

        /// <summary>
        /// The number of Tier 1 societies needed to win in the session.
        /// </summary>
        [SerializeField, DataMember()] public int TierOneSocietiesToWin;

        /// <summary>
        /// The number of Tier 2 societies needed to win in the session.
        /// </summary>
        [SerializeField, DataMember()] public int TierTwoSocietiesToWin;

        /// <summary>
        /// The number of Tier 3 societies needed to win in the session.
        /// </summary>
        [SerializeField, DataMember()] public int TierThreeSocietiesToWin;

        /// <summary>
        /// The number of Tier 4 societies needed to win in the session.
        /// </summary>
        [SerializeField, DataMember()] public int TierFourSocietiesToWin;

        /// <summary>
        /// Information about all of the map nodes. 
        /// </summary>
        [DataMember()] public List<SerializableMapNodeData> MapNodes;

        /// <summary>
        /// Information about all of the map edges.
        /// </summary>
        [DataMember()] public List<SerializableMapEdgeData> MapEdges;

        /// <summary>
        /// Information about all of the neighborhoods.
        /// </summary>
        [DataMember()] public List<SerializableNeighborhoodData> Neighborhoods;

        /// <summary>
        /// Information about all of the highways.
        /// </summary>
        [DataMember()] public List<SerializableHighwayData> Highways;

        /// <summary>
        /// Information about all of the construction zones.
        /// </summary>
        [DataMember()] public List<SerializableConstructionZoneData> ConstructionZones;

        /// <summary>
        /// Information about all of the highway managers.
        /// </summary>
        [DataMember()] public List<SerializableHighwayManagerData> HighwayManagers;

        /// <summary>
        /// Information about all of the resource depots.
        /// </summary>
        [DataMember()] public List<SerializableResourceDepotData> ResourceDepots;

        /// <summary>
        /// Information about all of the societies.
        /// </summary>
        [DataMember()] public List<SerializableSocietyData> Societies;

        /// <summary>
        /// Information about the terrain grid used to display the map graph.
        /// </summary>
        [DataMember()] public SerializableTerrainData TerrainData;

        /// <summary>
        /// Information about the camera at the moment of serialization.
        /// </summary>
        [DataMember()] public SerializableCameraData CameraData;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new session with a given name and description.
        /// </summary>
        /// <param name="name">The name of the new session</param>
        /// <param name="description">The description of the new session</param>
        public SerializableSession(string name, string description) {
            Name = name;
            Description = description;

            MapNodes          = new List<SerializableMapNodeData>();
            MapEdges          = new List<SerializableMapEdgeData>();
            Neighborhoods     = new List<SerializableNeighborhoodData>();
            Highways          = new List<SerializableHighwayData>();
            ConstructionZones = new List<SerializableConstructionZoneData>();
            HighwayManagers   = new List<SerializableHighwayManagerData>();
            ResourceDepots    = new List<SerializableResourceDepotData>();
            Societies         = new List<SerializableSocietyData>();
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Clears all the data within the session.
        /// </summary>
        public void Clear() {
            MapNodes.Clear();
            MapEdges.Clear();
            Neighborhoods.Clear();
            Highways.Clear();
            ConstructionZones.Clear();
            HighwayManagers.Clear();
            ResourceDepots.Clear();
            Societies.Clear();

            TerrainData = null;
            CameraData = null;
        }

        #endregion

    }

}
