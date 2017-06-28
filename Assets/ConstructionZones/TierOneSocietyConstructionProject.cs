using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Societies;
using Assets.Map;
using Assets.BlobSites;

namespace Assets.ConstructionZones {

    public class TierOneSocietyConstructionProject : FlexibleCostConstructionProjectBase {

        #region instance fields and properties

        [SerializeField] private SocietyFactoryBase SocietyFactory;
        [SerializeField] private ComplexityDefinitionBase ComplexityToBuild;
        [SerializeField] private ComplexityLadderBase LadderOfComplexity;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        public override bool IsValidAtLocation(MapNodeBase location) {
            return ComplexityToBuild.PermittedTerrains.Contains(location.Terrain);
        }

        public override void ExecuteBuild(MapNodeBase location) {
            if(SocietyFactory.CanConstructSocietyAt(location, LadderOfComplexity, ComplexityToBuild)) {
                SocietyFactory.ConstructSocietyAt(location, LadderOfComplexity, ComplexityToBuild);
            }
        }

        #endregion

        #endregion

    }

}
