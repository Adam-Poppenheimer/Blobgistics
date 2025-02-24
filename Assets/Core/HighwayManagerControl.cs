﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.HighwayManager;
using Assets.Highways;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation of HighwayManagerControlBase. It acts as a facade
    /// by which the UI can access parts of the simulation relating to highway managers.
    /// </summary>
    public class HighwayManagerControl : HighwayManagerControlBase {

        #region static fields and properties

        private static string HighwayManagerIDErrorMessage = "There exists no HighwayManager with ID {0}";

        #endregion

        #region instance fields and properties

        /// <summary>
        /// A dependency necessary for the class to function.
        /// </summary>
        public HighwayManagerFactoryBase HighwayManagerFactory {
            get { return _highwayManagerFactory; }
            set { _highwayManagerFactory = value; }
        }
        [SerializeField] private HighwayManagerFactoryBase _highwayManagerFactory;

        #endregion

        #region instance methods

        #region from HighwayManagerControlBase

        /// <inheritdoc/>
        public override void DestroyHighwayManagerOfID(int managerID) {
            var managerToDestroy = HighwayManagerFactory.GetHighwayManagerOfID(managerID);
            if(managerToDestroy != null) {
                HighwayManagerFactory.DestroyHighwayManager(managerToDestroy);
            }else {
                Debug.LogErrorFormat(HighwayManagerIDErrorMessage, managerID);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<BlobHighwayUISummary> GetHighwaysManagedByManagerOfID(int managerID) {
            var managerToConsider = HighwayManagerFactory.GetHighwayManagerOfID(managerID);
            if(managerToConsider != null) {
                return HighwayManagerFactory.GetHighwaysServedByManager(managerToConsider).Select(highway => new BlobHighwayUISummary(highway));
            }else {
                return new List<BlobHighwayUISummary>();
            }
        }

        #endregion

        #endregion

    }

}
