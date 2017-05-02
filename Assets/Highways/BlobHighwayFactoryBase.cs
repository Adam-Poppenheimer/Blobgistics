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

        #region events

        public event EventHandler<BlobHighwayEventArgs> HighwayConstructed;
        public event EventHandler<BlobHighwayEventArgs> HighwayBeingDestroyed;

        protected void RaiseHighwayConstructed(BlobHighwayBase newHighway) {
            if(HighwayConstructed != null) {
                HighwayConstructed(this, new BlobHighwayEventArgs(newHighway));
            }
        }

        protected void RaiseHighwayBeingDestroyed(BlobHighwayBase oldHighway) {
            if(HighwayBeingDestroyed != null) {
                HighwayBeingDestroyed(this, new BlobHighwayEventArgs(oldHighway));
            }
        }

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
