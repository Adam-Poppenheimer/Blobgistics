using System;
using System.Collections.ObjectModel;

using UnityEngine;

namespace Assets.Blobs {

    /// <summary>
    /// The abstract base class for all resource blob factories.
    /// </summary>
    /// <remarks>
    /// Note the presence of an unsubscribe method but the lack of a subscribe method.
    /// Since blobs are not manipulated at design time, there's no need to manually subscribe
    /// them. The class only needs to hedge against blob destruction.
    /// </remarks>
    public abstract class ResourceBlobFactoryBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// All of the extant blobs the factory has created.
        /// </summary>
        public abstract ReadOnlyCollection<ResourceBlobBase> Blobs { get; }

        #endregion

        #region instance methods

        /// <summary>
        /// Create a new ResourceBlobBase with a given BlobType.
        /// </summary>
        /// <param name="typeOfResource">The BlobType of the new blob</param>
        /// <returns>The blob created</returns>
        public abstract ResourceBlobBase BuildBlob(ResourceType typeOfResource);
        /// <summary>
        /// Create a new ResourceBlobBase with a given BlobType at the specified 2D position.
        /// </summary>
        /// <param name="typeOfResource">The BlobType of the new blob</param>
        /// <param name="startingPosition">The starting position of the new blob</param>
        /// <returns>The blob created</returns>
        public abstract ResourceBlobBase BuildBlob(ResourceType typeOfResource, Vector2 startingPosition);

        /// <summary>
        /// Unsubscribes and then destroys a given blob.
        /// </summary>
        /// <param name="blob">The blob to destroy</param>
        public abstract void DestroyBlob(ResourceBlobBase blob);

        /// <summary>
        /// Unsubscribes the blob, removing it from the factory's records.
        /// </summary>
        /// <param name="blob">The blob to unsubscribe</param>
        public abstract void UnsubscribeBlob(ResourceBlobBase blob);

        /// <summary>
        /// Ticks all blobs in the factory's records, causing them to simulate their behavior.
        /// </summary>
        /// <param name="secondsPassed">The number of seconds that've passed since the last tick</param>
        public abstract void TickAllBlobs(float secondsPassed);

        #endregion

    }

}