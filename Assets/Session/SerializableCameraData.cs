using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.UI;

using UnityCustomUtilities.Extensions;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableCameraData {

        #region instance fields and properties

        [DataMember()] public float Size;

        [DataMember()] public SerializableVector3 Position;

        #endregion

        #region constructors

        public SerializableCameraData(Camera camera) {
            Size = camera.orthographicSize;
            Position = camera.transform.position;
        }

        #endregion

    }

}
