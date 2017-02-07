using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    public class NodeOccupyingObject : MonoBehaviour {

        #region instance fields and properties

        public MapNode Location {
            get {
                if(_location == null) {
                    throw new InvalidOperationException("Location is uninitialized");
                } else {
                    return _location;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _location = value;
                    transform.SetParent(_location.transform, true);
                    transform.localPosition = Vector3.zero;
                }
            }
        }
        [SerializeField] private MapNode _location;

        #endregion

    }
}
