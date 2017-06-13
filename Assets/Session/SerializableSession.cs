using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Session {

    [Serializable]
    public class SerializableSession {

        #region instance fields and properties

        public string Name { get; set; }

        public string Description { get; set; }

        public int ScoreToWin { get; set; }

        public List<SerializableMapNodeData>          MapNodes;
        public List<SerializableMapEdgeData>          MapEdges;
        public List<SerializableNeighborhoodData>     Neighborhoods;
        public List<SerializableHighwayData>          Highways;
        public List<SerializableConstructionZoneData> ConstructionZones;
        public List<SerializableHighwayManagerData>   HighwayManagers;
        public List<SerializableResourceDepotData>    ResourceDepots;
        public List<SerializableSocietyData>          Societies;

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

    }

}
