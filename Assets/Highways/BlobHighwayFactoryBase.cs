using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Highways {

    public abstract class BlobHighwayFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ReadOnlyCollection<BlobHighwayBase> Highways { get; }

        #endregion

        #region instance methods

        public abstract BlobHighwayBase GetHighwayOfID(int highwayID);
        
        public abstract bool            HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);
        public abstract BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        public abstract bool            CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);
        public abstract BlobHighwayBase ConstructHighwayBetween   (MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        public abstract void DestroyHighway(BlobHighwayBase highway);

        #endregion

    }

}
