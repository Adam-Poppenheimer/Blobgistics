using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.ResourceDepots;

namespace Assets.UI.ResourceDepots {

    public class ResourceDepotSummaryDisplay : ResourceDepotSummaryDisplayBase {

        #region instance fields and properties

        #region from ResourceDepotSummaryDisplayBase

        public override ResourceDepotUISummary CurrentSummary { get; set; }

        #endregion

        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            DestroyButton.onClick.AddListener(delegate(){
                RaiseDestructionRequested();
            });
        }

        protected override void DoOnDeactivate() {
            DestroyButton.onClick.RemoveAllListeners();
        }

        #endregion

        #endregion

    }

}
