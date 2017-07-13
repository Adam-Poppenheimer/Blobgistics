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

        public event EventHandler<BlobHighwayEventArgs> HighwaySubscribed;
        public event EventHandler<BlobHighwayEventArgs> HighwayUnsubscribed;

        protected void RaiseHighwaySubscribed(BlobHighwayBase newHighway) {
            if(HighwaySubscribed != null) {
                HighwaySubscribed(this, new BlobHighwayEventArgs(newHighway));
            }
        }

        protected void RaiseHighwayUnsubscribed(BlobHighwayBase oldHighway) {
            if(HighwayUnsubscribed != null) {
                HighwayUnsubscribed(this, new BlobHighwayEventArgs(oldHighway));
            }
        }

        #endregion

        #region instance methods

        public abstract BlobHighwayBase GetHighwayOfID(int highwayID);
        
        public abstract bool            HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);
        public abstract BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        public abstract bool            CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);
        public abstract BlobHighwayBase ConstructHighwayBetween   (MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        public abstract void SubscribeHighway(BlobHighwayBase highway);
        public abstract void UnsubscribeHighway(BlobHighwayBase highway);

        public abstract void DestroyHighway(BlobHighwayBase highway);

        public abstract IEnumerable<BlobHighwayBase> GetHighwaysAttachedToNode(MapNodeBase node);

        #endregion

    }

}
