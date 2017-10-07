using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.Highways {

    /// <summary>
    /// The standard implementation for BlobTubeFactoryBase.
    /// </summary>
    /// <remarks>
    /// Note that this factory does not need the subscribe/unsubscribe pattern because it doesn't
    /// manage or keep track of its tubes after they've been constructed.
    /// </remarks>
    [ExecuteInEditMode]
    public class BlobTubeFactory : BlobTubeFactoryBase {

        #region instance fields and properties

        /// <summary>
        /// The private data to configure new tubes with.
        /// </summary>
        public BlobTubePrivateDataBase TubePrivateData {
            get {
                if(_tubePrivateData == null) {
                    throw new InvalidOperationException("TubePrivateData is uninitialized");
                } else {
                    return _tubePrivateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _tubePrivateData = value;
                }
            }
        }
        [SerializeField] private BlobTubePrivateDataBase _tubePrivateData;

        [SerializeField] private GameObject TubePrefab;

        #endregion

        #region instance methods

        #region from BlobTubeFactoryBase

        /// <inheritdoc/>
        public override BlobTubeBase ConstructTube(Vector3 sourceLocation, Vector3 targetLocation) {
            BlobTube newTube = null;

            if(TubePrefab != null) {
                var prefabClone = Instantiate<GameObject>(TubePrefab);
                newTube = prefabClone.GetComponent<BlobTube>();
                if(newTube == null) {
                    throw new BlobTubeException("The TubePrefab lacks a BlobTube component");
                }
            }else {
                var hostingObject = new GameObject();
                newTube = hostingObject.AddComponent<BlobTube>();
            }

            newTube.PrivateData = TubePrivateData;
            newTube.PermissionsForBlobTypes = newTube.gameObject.AddComponent<BoolPerResourceDictionary>();
            newTube.SetEndpoints(sourceLocation, targetLocation);
            newTube.gameObject.SetActive(true);
            return newTube;
        }

        /// <inheritdoc/>
        public override void DestroyTube(BlobTubeBase tube) {
            if(tube == null) {
                throw new ArgumentNullException("tube");
            }
            DestroyImmediate(tube.gameObject);
        }

        #endregion

        #endregion

    }

}
