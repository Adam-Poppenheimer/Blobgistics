using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class ResourcePoolFactory : IResourcePoolFactory {

        #region instance fields and properties

        public UIFSM TopLevelUIFSM {
            get {
                if(_topLevelUIFSM == null) {
                    throw new InvalidOperationException("TopLevelUIFSM is uninitialized");
                } else {
                    return _topLevelUIFSM;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _topLevelUIFSM = value;
                }
            }
        }
        private UIFSM _topLevelUIFSM;

        public GameObject PoolPrefab {
            get {
                if(_poolPrefab == null) {
                    throw new InvalidOperationException("PoolPrefab is uninitialized");
                } else {
                    return _poolPrefab;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _poolPrefab = value;
                }
            }
        }
        private GameObject _poolPrefab;

        #endregion

        #region constructors

        public ResourcePoolFactory() { }

        #endregion

        #region instance methods

        #region from IResourcePoolFactory

        public IResourcePool BuildResourcePool(Vector3 localPosition, Transform parent) {
            var poolObject = GameObject.Instantiate(PoolPrefab);
            var poolBehaviour = poolObject.GetComponent<ResourcePool>();
            if(poolBehaviour != null) {
                poolBehaviour.TopLevelUIFSM = TopLevelUIFSM;
                poolBehaviour.transform.SetParent(parent);
                poolBehaviour.transform.localPosition = localPosition;
            }else {
                throw new BlobException("The ResourcePool prefab did not contain a ResourcePool component");
            }
            return poolBehaviour;
        }

        #endregion

        #endregion

    }

}
