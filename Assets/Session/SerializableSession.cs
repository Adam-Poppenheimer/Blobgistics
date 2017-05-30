using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Session {

    [Serializable]
    public class SerializableSession {

        #region instance fields and properties

        public string Name {
            get { return _name; }
        }
        private string _name;

        public List<SerializableMapNodeData> MapNodes;
        public List<SerializableMapEdgeData> MapEdges;
        public List<SerializableHighwayData> Highways;

        #endregion

        #region constructors

        public SerializableSession(string name) {
            _name = name;
            MapNodes = new List<SerializableMapNodeData>();
            MapEdges = new List<SerializableMapEdgeData>();
            Highways = new List<SerializableHighwayData>();
        }

        #endregion

    }

}
