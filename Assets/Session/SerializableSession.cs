using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    [Serializable]
    public class SerializableSession {

        #region instance fields and properties

        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        [SerializeField] private string _name;

        public string Description {
            get { return _description; }
            set { _description = value; }
        }
        [SerializeField] private string _description;

        public int ScoreToWin {
            get { return _scoreToWin; }
            set { _scoreToWin = value; }
        }
        [SerializeField] private int _scoreToWin;

        public List<SerializableMapNodeData>          MapNodes;
        public List<SerializableMapEdgeData>          MapEdges;
        public List<SerializableNeighborhoodData>     Neighborhoods;
        public List<SerializableHighwayData>          Highways;
        public List<SerializableConstructionZoneData> ConstructionZones;
        public List<SerializableHighwayManagerData>   HighwayManagers;
        public List<SerializableResourceDepotData>    ResourceDepots;
        public List<SerializableSocietyData>          Societies;

        public SerializableTerrainData TerrainData;
        public SerializableCameraData CameraData;

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
