using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Highways;
using Assets.Societies;
using Assets.Depots;
using Assets.HighwayUpgrade;
using Assets.ConstructionZones;

namespace Assets.Core {

    public abstract class UIControlBase : MonoBehaviour {

        #region instance methods

        public abstract void PushHighwayPointerClickEvent(PointerEventData eventData, BlobHighwayBase source);
        public abstract void PushHighwayPointerEnterEvent(PointerEventData eventData, BlobHighwayBase source);
        public abstract void PushHighwayPointerExitEvent(PointerEventData eventData, BlobHighwayBase source);

        public abstract void PushSocietyPointerClickEvent(PointerEventData eventData, SocietyBase source);
        
        public abstract void PushResourceDepotPointerClickEvent(PointerEventData eventData, ResourceDepotBase source);

        public abstract void PushHighwayUpgraderPointerEnterEvent(PointerEventData eventData, HighwayUpgraderBase source);
        public abstract void PushHighwayUpgraderPointerExitEvent(PointerEventData eventData, HighwayUpgraderBase source);

        public abstract void PushConstructionZonePointerEnterEvent(PointerEventData eventData, ConstructionZoneBase source);
        public abstract void PushConstructionZonePointerExitEvent(PointerEventData eventData, ConstructionZoneBase source);

        #endregion

    }

}
