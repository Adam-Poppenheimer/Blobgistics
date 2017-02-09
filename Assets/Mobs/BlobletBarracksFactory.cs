using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.BlobEngine;

namespace Assets.Mobs {

    public class BlobletBarracksFactory : BlobletBarracksFactoryBase {

        #region instance fields and properties

        [SerializeField] private BlobletBarracksPrivateData BarracksPrivateData;
        [SerializeField] private GameObject BarracksPrefab;

        #endregion

        #region instance methods

        #region from BlobletBarracksFactoryBase

        public override BlobletBarracksBase ConstructBlobletBarracks(MapNode location) {
            var barracksObject = Instantiate(BarracksPrefab);
            var barracksBehaviour = barracksObject.GetComponent<BlobletBarracks>();
            if(barracksBehaviour != null) {
                barracksBehaviour.PrivateData = BarracksPrivateData;
                barracksBehaviour.Location = location;
                return barracksBehaviour;
            }else {
                throw new BlobException("The BlobletBarracks prefab did not contain a BlobletBarracks component");
            }
        }

        public override Schematic BuildSchematic() {
            var cost = BarracksPrivateData.Cost;
            Action<MapNode> constructionAction = delegate(MapNode locationToConstruct) {
                ConstructBlobletBarracks(locationToConstruct);
            };
            return new Schematic(SchematicName, cost, constructionAction);
        }

        #endregion

        #endregion
        
    }

}
