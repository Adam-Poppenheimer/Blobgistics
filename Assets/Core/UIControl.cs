using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Highways;
using Assets.UI;

using UnityCustomUtilities.UI;

namespace Assets.Core {

    public class UIControl : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private BlobHighwaySummaryDisplay HighwayDisplay;

        #endregion

        #region instance methods

        public void PushHighwayClickEvent(PointerEventData eventData, BlobHighwayBase source) {
            HighwayDisplay.UpdateDisplay(new BlobHighwayUISummary(source));
        }

        #endregion

    }

}
