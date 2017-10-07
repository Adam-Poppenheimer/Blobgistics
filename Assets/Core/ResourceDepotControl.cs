using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.ResourceDepots;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation of ResourceDepotControlBase. It acts as a facade
    /// by which the UI can acccess parts of the simulation relating to resource depots.
    /// </summary>
    public class ResourceDepotControl : ResourceDepotControlBase {

        #region static fields and properties

        private static string DepotIDErrorMessage = "There exists no ResourceDepot with ID {0}";

        #endregion

        #region instance fields and properties

        /// <summary>
        /// The resource depot factory used to destroy resource depots.
        /// </summary>
        public ResourceDepotFactoryBase ResourceDepotFactory {
            get { return _resourceDepotFactory; }
            set { _resourceDepotFactory = value; }
        }
        [SerializeField] private ResourceDepotFactoryBase _resourceDepotFactory;

        #endregion

        #region instance methods

        #region from ResourceDepotControlBase

        /// <inheritdoc/>
        public override void DestroyResourceDepotOfID(int depotID) {
            var depotToDestroy = ResourceDepotFactory.GetDepotOfID(depotID);
            if(depotToDestroy != null) {
                ResourceDepotFactory.DestroyDepot(depotToDestroy);
            }else {
                Debug.LogErrorFormat(DepotIDErrorMessage, depotID);
            }
        }

        #endregion

        #endregion
        
    }

}
