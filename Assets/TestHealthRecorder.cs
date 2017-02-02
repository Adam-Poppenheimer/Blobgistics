using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Combat;

namespace Assets {

    public class TestHealthRecorder : HealthRecorder, IPointerClickHandler {

        #region instance methods

        #region from IPointerClickHander

        public void OnPointerClick(PointerEventData eventData) {
            InflictDamage(10);
        }

        #endregion

        #endregion
        
    }

}
