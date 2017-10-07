using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.UI;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about an orthographic camera.
    /// </summary>
    /// <remarks>
    /// This class is used for session serialization via <see cref="SessionManagerBase"/>.
    /// </remarks>
    [Serializable, DataContract]
    public class SerializableCameraData {

        #region instance fields and properties

        /// <summary>
        /// The camera's size.
        /// </summary>
        [DataMember()] public float Size;

        /// <summary>
        /// The camera's position.
        /// </summary>
        [DataMember()] public SerializableVector3 Position;

        #endregion

        #region constructors

        /// <summary>
        /// Constructs data from a given camera.
        /// </summary>
        /// <param name="camera">The camera to pull data from</param>
        public SerializableCameraData(Camera camera) {
            Size = camera.orthographicSize;
            Position = camera.transform.position;
        }

        #endregion

    }

}
