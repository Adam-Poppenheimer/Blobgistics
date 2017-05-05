using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.HighwayManager;

namespace Assets.UI.HighwayManager {

    public class HighwayManagerSummaryDisplay : HighwayManagerSummaryDisplayBase {

        #region instance fields and properties

        #region from HighwayManagerSummaryDisplayBase

        public override HighwayManagerUISummary CurrentSummary { get; set; }

        #endregion
        
        [SerializeField] private Text UpkeepFoodField;
        [SerializeField] private Text UpkeepYellowField;
        [SerializeField] private Text UpkeepWhiteField;
        [SerializeField] private Text UpkeepBlueField;

        [SerializeField] private Button DestroyButton;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            MovePanelWithCamera = true;
        }

        #endregion

        #region from IntelligentPanel

        protected override void DoOnActivate() {
            DestroyButton.onClick.AddListener(delegate() {
                RaiseDestructionRequested();
            });
            DesiredWorldPosition = CurrentSummary.Transform.position;
        }

        protected override void DoOnDeactivate() {
            DestroyButton.onClick.RemoveAllListeners();
        }

        public override void ClearDisplay() {
            UpkeepFoodField.text   = "0";
            UpkeepYellowField.text = "0";
            UpkeepWhiteField.text  = "0";
            UpkeepBlueField.text   = "0";
        }

        public override void UpdateDisplay() {
            if(CurrentSummary != null) {
                var upkeep = CurrentSummary.LastUpkeep;

                int foodValue = 0;
                int yellowValue = 0;
                int whiteValue = 0;
                int blueValue = 0;

                upkeep.TryGetValue(ResourceType.Food,   out foodValue  );
                upkeep.TryGetValue(ResourceType.Textiles, out yellowValue);
                upkeep.TryGetValue(ResourceType.ServiceGoods,  out whiteValue );
                upkeep.TryGetValue(ResourceType.HiTechGoods,   out blueValue  );

                UpkeepFoodField.text   = foodValue.ToString();
                UpkeepYellowField.text = yellowValue.ToString();
                UpkeepWhiteField.text  = whiteValue.ToString();
                UpkeepBlueField.text   = blueValue.ToString();
            }
        }

        #endregion

        #endregion

    }

}
