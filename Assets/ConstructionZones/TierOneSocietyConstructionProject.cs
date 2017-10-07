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

    /// <summary>
    /// Defines and executes the construction of a particular tier 1 society.
    /// </summary>
    public class TierOneSocietyConstructionProject : FlexibleCostConstructionProjectBase {

        #region instance fields and properties

        /// <summary>
        /// The factory that should be used to construct the society.
        /// </summary>
        [SerializeField] private SocietyFactoryBase SocietyFactory;

        /// <summary>
        /// The complexity to construct when ExecuteBuild is called.
        /// </summary>
        [SerializeField] private ComplexityDefinitionBase ComplexityToBuild;

        /// <summary>
        /// The complexity ladder that the constructed society should be placed upon.
        /// </summary>
        [SerializeField] private ComplexityLadderBase LadderOfComplexity;

        #endregion

        #region instance methods

        #region from ConstructionProjectBase

        /// <inheritdoc/>
        public override bool IsValidAtLocation(MapNodeBase location) {
            return ComplexityToBuild.PermittedTerrains.Contains(location.Terrain);
        }

        /// <inheritdoc/>
        public override void ExecuteBuild(MapNodeBase location) {
            if(SocietyFactory.CanConstructSocietyAt(location, LadderOfComplexity, ComplexityToBuild)) {
                SocietyFactory.ConstructSocietyAt(location, LadderOfComplexity, ComplexityToBuild);
            }
        }

        #endregion

        #endregion

    }

}
