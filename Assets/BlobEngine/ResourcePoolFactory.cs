using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class ResourcePoolFactory : ResourcePoolFactoryBase {

        #region instance fields and properties

        [SerializeField] private ResourcePoolPrivateData PoolPrivateData;

        [SerializeField] private GameObject PoolPrefab;

        #endregion

        #region constructors

        public ResourcePoolFactory() { }

        #endregion

        #region instance methods

        #region from ResourcePoolFactoryBase

        public override IResourcePool BuildResourcePool(Vector3 localPosition, Transform parent) {
            var poolObject = GameObject.Instantiate(PoolPrefab);
            var poolBehaviour = poolObject.GetComponent<ResourcePool>();
            if(poolBehaviour != null) {
                poolBehaviour.PrivateData = PoolPrivateData;
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
