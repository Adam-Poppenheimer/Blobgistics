using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Highways {

    /// <summary>
    /// The abstract base class for all highway factories.
    /// </summary>
    public abstract class BlobHighwayFactoryBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// All highway subscribed to this factory.
        /// </summary>
        public abstract ReadOnlyCollection<BlobHighwayBase> Highways { get; }

        #endregion

        #region events

        /// <summary>
        /// Fired whenever a highway becomes subscribed to this factory.
        /// </summary>
        public event EventHandler<BlobHighwayEventArgs> HighwaySubscribed;

        /// <summary>
        /// Fired whenever a highway becomes unsubscribed from this factory.
        /// </summary>
        public event EventHandler<BlobHighwayEventArgs> HighwayUnsubscribed;

        /// <summary>
        /// Fires the HighwaySubscribed event.
        /// </summary>
        /// <param name="newHighway">The highway triggering the event</param>
        protected void RaiseHighwaySubscribed(BlobHighwayBase newHighway) {
            if(HighwaySubscribed != null) {
                HighwaySubscribed(this, new BlobHighwayEventArgs(newHighway));
            }
        }

        /// <summary>
        /// Fires the HighwayUnsubscribed event.
        /// </summary>
        /// <param name="oldHighway">The highway triggering the event</param>
        protected void RaiseHighwayUnsubscribed(BlobHighwayBase oldHighway) {
            if(HighwayUnsubscribed != null) {
                HighwayUnsubscribed(this, new BlobHighwayEventArgs(oldHighway));
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets the highway subscribed to this factory with the given ID, if one exists.
        /// </summary>
        /// <param name="highwayID">The ID of the desired highway</param>
        /// <returns>A highway with the given ID, or null if none exists</returns>
        public abstract BlobHighwayBase GetHighwayOfID(int highwayID);
        
        /// <summary>
        /// Determines whether a highway exists between the given endpoints.
        /// </summary>
        /// <param name="firstEndpoint">The first endpoint to consider</param>
        /// <param name="secondEndpoint">The second endpoint to consider</param>
        /// <returns>Whether a highway exists btween the two endpoints</returns>
        public abstract bool            HasHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        /// <summary>
        /// Gets the highway between the given endpoints.
        /// </summary>
        /// <param name="firstEndpoint">The first endpoint to consider</param>
        /// <param name="secondEndpoint">The second endpoint to consider</param>
        /// <returns>The highway between the two endpoints</returns>
        public abstract BlobHighwayBase GetHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        /// <summary>
        /// Determines whether a highway can be constructed between the given endpoints.
        /// </summary>
        /// <param name="firstEndpoint">The first endpoint to consider</param>
        /// <param name="secondEndpoint">The second endpoint to consider</param>
        /// <returns>Whether a highway can be constructed between the given endpoints</returns>
        public abstract bool            CanConstructHighwayBetween(MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        /// <summary>
        /// Constructs a highway between the given endpoints and subscribes it.
        /// </summary>
        /// <param name="firstEndpoint">The first endpoint of the new highway</param>
        /// <param name="secondEndpoint">The second endpoint of the new highway</param>
        /// <returns>The highway created between the given endpoints</returns>
        public abstract BlobHighwayBase ConstructHighwayBetween   (MapNodeBase firstEndpoint, MapNodeBase secondEndpoint);

        /// <summary>
        /// Subscribes a given highway to this factory.
        /// </summary>
        /// <param name="highway">The highway to subscribe</param>
        public abstract void SubscribeHighway(BlobHighwayBase highway);

        /// <summary>
        /// Unsubscribes a given highway from this factory, removing it from the factory's knowledge.
        /// </summary>
        /// <param name="highway">The highway to unsubscribe</param>
        public abstract void UnsubscribeHighway(BlobHighwayBase highway);

        /// <summary>
        /// Unsubscribes and then destroys a given factory.
        /// </summary>
        /// <param name="highway">The highway to destroy</param>
        public abstract void DestroyHighway(BlobHighwayBase highway);

        /// <summary>
        /// Returns all highways that have the given node as one of their endpoints.
        /// </summary>
        /// <param name="node">The node to consider</param>
        /// <returns>The highways attached to the given node</returns>
        public abstract IEnumerable<BlobHighwayBase> GetHighwaysAttachedToNode(MapNodeBase node);

        #endregion

    }

}
