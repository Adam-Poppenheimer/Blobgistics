using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.BlobEngine;

using UnityCustomUtilities.AI;

namespace Assets.Mobs {

    public class BlobletFactory : BlobletFactoryBase {

        #region instance fields and properties

        [SerializeField] private BlobletPrivateData BlobletPrivateData;
        [SerializeField] private GameObject BlobletPrefab;
        [SerializeField] private Transform MobMapRoot;

        #endregion

        #region instance methods

        #region from BlobletFactoryBase

        public override BlobletBase ConstructBloblet(Vector3 positionInMap) {
            var blobletObject = Instantiate(BlobletPrefab);
            var blobletBehaviour = blobletObject.GetComponent<Bloblet>();
            if(blobletBehaviour != null) {
                blobletBehaviour.transform.SetParent(MobMapRoot, false);
                blobletBehaviour.transform.localPosition = positionInMap;
                blobletBehaviour.PrivateData = BlobletPrivateData;

                var steeringLogic = blobletBehaviour.SteeringLogic;
                
                steeringLogic.TurnOn(SteeringLogic2D.BehaviourTypeFlags.Separation);
                return blobletBehaviour;
            }else {
                throw new BlobException("The Bloblet prefab did not contain a Bloblet component");
            }
        }

        #endregion

        #endregion
        
    }

}
