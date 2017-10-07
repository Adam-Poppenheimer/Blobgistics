using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.BlobSites;
using Assets.Map;

using Assets.UI.Blobs;

namespace Assets.ConstructionZones {

    /// <summary>
    /// The abstract base class for all construction projects, which do the main work of
    /// determining what gets built, what it costs, and where it can go.
    /// </summary>
    public abstract class ConstructionProjectBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Determines whether this construction project is valid at the given location.
        /// </summary>
        /// <param name="location">The location under consideration</param>
        /// <returns>Whether or not the construction project is valid at the location</returns>
        public abstract bool IsValidAtLocation(MapNodeBase location);

        /// <summary>
        /// Executes whatever actions follow from completing the construction project.
        /// </summary>
        /// <param name="location">The map node where the build will be executed</param>
        public abstract void ExecuteBuild(MapNodeBase location);

        /// <summary>
        /// Modifies the permissions and capacities of the given blob site to fit the
        /// needs of this construction project, and also clears its contents.
        /// </summary>
        /// <param name="site">The site to be modified</param>
        public abstract void SetSiteForProject(BlobSiteBase site);

        /// <summary>
        /// Determines whether the given blob site has the right collection of resources
        /// for the construction project to get considered fulfilled.
        /// </summary>
        /// <param name="site">The site under consideration</param>
        /// <returns>Whether ExecuteBuild is mechanically valid on the location containing this blob site</returns>
        public abstract bool BlobSiteContainsNecessaryResources(BlobSiteBase site);

        /// <summary>
        /// Gets a representation of the cost of this construction project.
        /// </summary>
        /// <returns>A ResourceDisplayInfo object representing the cost of the construction project</returns>
        public abstract ResourceDisplayInfo GetCostInfo();

        #endregion

    }

}
