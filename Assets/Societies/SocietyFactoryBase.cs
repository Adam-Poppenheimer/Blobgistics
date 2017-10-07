using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    /// <summary>
    /// The abstract base class for a society factory. Keeps track of the location of societies
    /// and also maintains all the complexity ladders and complexity definitions.
    /// </summary>
    public abstract class SocietyFactoryBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// A complexity ladder that consumers can use to construct societies.
        /// </summary>
        public abstract ComplexityLadderBase StandardComplexityLadder { get; }

        /// <summary>
        /// A complexity definition that consumers can use to construct societies.
        /// </summary>
        public abstract ComplexityDefinitionBase DefaultComplexityDefinition { get; }

        /// <summary>
        /// All the societies currently subscribed to this factory.
        /// </summary>
        public abstract ReadOnlyCollection<SocietyBase> Societies { get; }

        #endregion

        #region events

        /// <summary>
        /// Fires whenever a society becomes subscribed to this factory.
        /// </summary>
        public event EventHandler<SocietyEventArgs> SocietySubscribed;

        /// <summary>
        /// Fires whenever a society becomes unsubscribed from this factory.
        /// </summary>
        public event EventHandler<SocietyEventArgs> SocietyUnsubscribed;

        /// <summary>
        /// Raises a SocietySubscribed event.
        /// </summary>
        /// <param name="newSociety">The society that was subscribed</param>
        protected void RaiseSocietySubscribed  (SocietyBase newSociety) { RaiseEvent(SocietySubscribed,   new SocietyEventArgs(newSociety)); }

        /// <summary>
        /// Raises a SocietyUnsubscribed event.
        /// </summary>
        /// <param name="oldSociety">The society that was unsubscribed</param>
        protected void RaiseSocietyUnsubscribed(SocietyBase oldSociety) { RaiseEvent(SocietyUnsubscribed, new SocietyEventArgs(oldSociety)); }

        /// <summary>
        /// A helper class for raising events.
        /// </summary>
        /// <typeparam name="T">The EventArgs type</typeparam>
        /// <param name="callback">The event to invoke</param>
        /// <param name="e">The EventArgs to pass into the handler</param>
        protected void RaiseEvent<T>(EventHandler<T> callback, T e) where T : EventArgs {
            if(callback != null) {
                callback(this, e);
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Gets the society with the given ID, if one exists.
        /// </summary>
        /// <param name="id">The ID of the society to retrieve</param>
        /// <returns>The society with the Given ID, or null if none exists</returns>
        public abstract SocietyBase GetSocietyOfID(int id);

        /// <summary>
        /// Determines whether there is a society at the given location.
        /// </summary>
        /// <param name="location">The location to consider</param>
        /// <returns>Whether or not a society exists at the given location</returns>
        public abstract bool HasSocietyAtLocation(MapNodeBase location);

        /// <summary>
        /// Gets the society at the given location.
        /// </summary>
        /// <param name="location">The location to consider</param>
        /// <returns>The society at the given location</returns>
        public abstract SocietyBase GetSocietyAtLocation(MapNodeBase location);

        /// <summary>
        /// Determines whether a society can be constructed at the given location using the given ladder
        /// with the given starting complexity.
        /// </summary>
        /// <param name="location">The map node on which to place the society</param>
        /// <param name="ladder">The complexity ladder the society will climb</param>
        /// <param name="startingComplexity">The complexity definition the society will start out with</param>
        /// <returns>Whether such a society is valid</returns>
        public abstract bool        CanConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity);

        /// <summary>
        /// Constructs and subscribes a new society at the given location using the given ladder
        /// with the given starting complexity.
        /// </summary>
        /// <param name="location">The map node on which to place the society</param>
        /// <param name="ladder">The complexity ladder the society will climb</param>
        /// <param name="startingComplexity">The complexity definition the society will start out with</param>
        /// <returns>The newly created and subscribed society</returns>
        public abstract SocietyBase ConstructSocietyAt   (MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity);

        /// <summary>
        /// Unsubscribes and then destroys the given society.
        /// </summary>
        /// <param name="society">The society to destroy</param>
        public abstract void DestroySociety(SocietyBase society);

        /// <summary>
        /// Subscribes a given society, informing the factory of its existence.
        /// </summary>
        /// <param name="society">The society to subscribe</param>
        public abstract void SubscribeSociety  (SocietyBase society);
        
        /// <summary>
        /// Unsubscribes a given society, removing it from the factory's consideration.
        /// </summary>
        /// <param name="society">The society to unsubscribe</param>
        public abstract void UnsubscribeSociety(SocietyBase society);

        /// <summary>
        /// Gets the complexity definition with the given name, if it exists.
        /// </summary>
        /// <param name="name">The name of the complexity to get</param>
        /// <returns>The complexity definition with the given name, or null if non exists</returns>
        public abstract ComplexityDefinitionBase GetComplexityDefinitionOfName(string name);

        /// <summary>
        /// Gets the complexity ladder with the given name, if it exists.
        /// </summary>
        /// <param name="name">The name of the complexity ladder to get</param>
        /// <returns>The complexity ladder with the given name, or null if non exists</returns>
        public abstract ComplexityLadderBase     GetComplexityLadderOfName    (string name);

        /// <summary>
        /// Ticks all the societies currently subscribed to the factory.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds to advance the simulation by</param>
        public abstract void TickSocieties(float secondsPassed);

        #endregion

    }

}
