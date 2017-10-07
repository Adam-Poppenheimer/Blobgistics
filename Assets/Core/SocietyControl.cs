using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;
using Assets.Map;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation of SocietyControlBase. It acts as a facade
    /// by which the UI can access parts of the simulation relating to societies.
    /// </summary>
    public class SocietyControl : SocietyControlBase {

        #region static fields and properties

        private static string SocietyIDErrorMessage = "There exists no Society with ID {0}";

        #endregion

        #region instance fields and properties

        /// <summary>
        /// The society factory this class uses to destroy societies.
        /// </summary>
        public SocietyFactoryBase SocietyFactory {
            get { return _societyFactory; }
            set { _societyFactory = value; }
        }
        [SerializeField] private SocietyFactoryBase _societyFactory;

        /// <summary>
        /// Currently a redundant field that should be removed in the next refactor.
        /// </summary>
        public MapGraphBase MapGraph {
            get { return _mapGraph; }
            set { _mapGraph = value; }
        }
        [SerializeField] private MapGraphBase _mapGraph;

        #endregion

        #region instance methods

        #region from SocietyControlBase

        /// <inheritdoc/>
        public override void DestroySociety(int societyID) {
            var societyToDestroy = SocietyFactory.GetSocietyOfID(societyID);
            if(societyToDestroy != null) {
                SocietyFactory.DestroySociety(societyToDestroy);
            }else {
                Debug.LogErrorFormat(SocietyIDErrorMessage, societyID);
            }
        }

        /// <inheritdoc/>
        public override void SetGeneralAscensionPermissionForSociety(int societyID, bool ascensionPermitted) {
            var societyToChange = SocietyFactory.GetSocietyOfID(societyID);
            if(societyToChange != null) {
                societyToChange.AscensionIsPermitted = ascensionPermitted;
            }else {
                Debug.LogErrorFormat(SocietyIDErrorMessage, societyID);
            }
        }

        /// <inheritdoc/>
        public override void SetSpecificAscensionPermissionForSociety(int societyID, ComplexityDefinitionBase complexity, bool ascensionPermitted) {
            var societyToChange = SocietyFactory.GetSocietyOfID(societyID);
            if(societyToChange != null) {
                societyToChange.SetAscensionPermissionForComplexity(complexity, ascensionPermitted);
            }else {
                Debug.LogErrorFormat(SocietyIDErrorMessage, societyID);
            }
        }

        #endregion

        #endregion

    }

}
