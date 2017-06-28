using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableSession {

        #region instance fields and properties

        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        [SerializeField, DataMember()] private string _name;

        public string Description {
            get { return _description; }
            set { _description = value; }
        }
        [SerializeField, DataMember()] private string _description;

        public int ScoreToWin {
            get { return _scoreToWin; }
            set { _scoreToWin = value; }
        }
        [SerializeField, DataMember()] private int _scoreToWin;

        [DataMember()] public List<SerializableMapNodeData>          MapNodes;
        [DataMember()] public List<SerializableMapEdgeData>          MapEdges;
        [DataMember()] public List<SerializableNeighborhoodData>     Neighborhoods;
        [DataMember()] public List<SerializableHighwayData>          Highways;
        [DataMember()] public List<SerializableConstructionZoneData> ConstructionZones;
        [DataMember()] public List<SerializableHighwayManagerData>   HighwayManagers;
        [DataMember()] public List<SerializableResourceDepotData>    ResourceDepots;
        [DataMember()] public List<SerializableSocietyData>          Societies;

        [DataMember()] public SerializableTerrainData TerrainData;
        [DataMember()] public SerializableCameraData CameraData;

        #endregion

        #region constructors

        public SerializableSession(string name, string description, int scoreToWin) {
            Name = name;
            Description = description;
            ScoreToWin = scoreToWin;

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

        public void Clear() {
            MapNodes.Clear();
            MapEdges.Clear();
            Neighborhoods.Clear();
            Highways.Clear();
            ConstructionZones.Clear();
            HighwayManagers.Clear();
            ResourceDepots.Clear();
            Societies.Clear();
        }

        #endregion

    }

}
