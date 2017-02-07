using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;

using Assets.Map;

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

        public override IResourcePool ConstructResourcePool(MapNode location) {
            var poolObject = GameObject.Instantiate(PoolPrefab);
            var poolBehaviour = poolObject.GetComponent<ResourcePool>();
            if(poolBehaviour != null) {
                poolBehaviour.PrivateData = PoolPrivateData;
                poolBehaviour.Location = location;
                poolBehaviour.Initialize();
            }else {
                throw new BlobException("The ResourcePool prefab did not contain a ResourcePool component");
            }
            return poolBehaviour;
        }

        public override Schematic BuildSchematic() {
            var cost = PoolPrivateData.Cost;
            Action<MapNode> constructionAction = delegate(MapNode locationToConstruct) {
                ConstructResourcePool(locationToConstruct);
            };
            return new Schematic(SchematicName, cost, constructionAction);
        }

        #endregion

        #endregion

    }

}
