using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Map;
using Assets.Blobs;

namespace Assets.Highways {

    [ExecuteInEditMode]
    public class BlobTubeFactory : BlobTubeFactoryBase {

        #region instance fields and properties

        public ResourceBlobFactoryBase BlobFactory {
            get {
                if(_blobFactory == null) {
                    throw new InvalidOperationException("BlobFactory is uninitialized");
                } else {
                    return _blobFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _blobFactory = value;
                }
            }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        [SerializeField] private GameObject TubePrefab;

        #endregion

        #region instance methods

        #region from BlobTubeFactoryBase

        public override BlobTubeBase ConstructTube(Vector3 sourceLocation, Vector3 targetLocation) {
            BlobTube newTube = null;
            BlobTubePrivateData newPrivateData = null;

            if(TubePrefab != null) {
                var prefabClone = Instantiate<GameObject>(TubePrefab);
                newTube = prefabClone.GetComponent<BlobTube>();
                newPrivateData = prefabClone.AddComponent<BlobTubePrivateData>();
                if(newTube == null) {
                    throw new BlobTubeException("The TubePrefab lacks a BlobTube component");
                }
            }else {
                var hostingObject = new GameObject();
                newTube = hostingObject.AddComponent<BlobTube>();
                newPrivateData = hostingObject.AddComponent<BlobTubePrivateData>();
            }
            
            newPrivateData.SetBlobFactory(BlobFactory);

            newTube.PrivateData = newPrivateData;
            newTube.SetEndpoints(sourceLocation, targetLocation);
            return newTube;
        }

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
