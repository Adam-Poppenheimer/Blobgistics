using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Highways {

    public abstract class BlobHighwayFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract BlobHighwayBase GetHighwayOfID(int highwayID);
        
        public abstract bool            HasHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint);
        public abstract BlobHighwayBase GetHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint);

        public abstract bool            CanConstructHighwayBetween(MapNode firstEndpoint, MapNode secondEndpoint);
        public abstract BlobHighwayBase ConstructHighwayBetween   (MapNode firstEndpoint, MapNode secondEndpoint);
        

        public abstract void RemoveHighway(BlobHighwayBase highway);

        public abstract void TickHighways(float secondsPassed);

        #endregion

    }

}
