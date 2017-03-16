using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Highways {

    public class BlobHighwayGhost : BlobHighwayGhostBase {

        #region instance fields and properties

        #region from BlobHighwayGhostBase

        public override bool IsActivated {
            get { return gameObject.activeInHierarchy; }
        }

        public override MapNodeUISummary FirstEndpoint  {
            get { return _firstEndpoint; }
            set {
                _firstEndpoint = value;
                UpdateAppearance();
            }
        }
        private MapNodeUISummary _firstEndpoint;

        public override MapNodeUISummary SecondEndpoint  {
            get { return _secondEndpoint; }
            set {
                _secondEndpoint = value;
                UpdateAppearance();
            }
        }
        private MapNodeUISummary _secondEndpoint;

        public override bool GhostIsBuildable { get; set; }

        private PointerEventData lastEventData;

        #endregion

        #endregion

        #region instance methods

        #region from BlobHighwayGhostBase

        public override void Activate() {
            gameObject.SetActive(true);
        }

        public override void Clear() {
            FirstEndpoint = null;
            SecondEndpoint = null;
            lastEventData = null;
            GhostIsBuildable = false;
        }

        public override void Deactivate() {
            gameObject.SetActive(false);
        }

        public override void UpdateWithEventData(PointerEventData eventData) {
            lastEventData = eventData;
            UpdateAppearance();
        }

        #endregion

        private void UpdateAppearance() {
            if(FirstEndpoint == null) {
                return;
            }else if(lastEventData == null) {
                return;
            }

            Vector3 endpoint1;
            Vector3 endpoint2;

            if(SecondEndpoint != null) {
                var directionToEndpoint2 = FirstEndpoint.BlobSite.Transform.GetDominantManhattanDirectionTo(SecondEndpoint.BlobSite.Transform);
                var directionToEndpoint1 = SecondEndpoint.BlobSite.Transform.GetDominantManhattanDirectionTo(FirstEndpoint.BlobSite.Transform);

                endpoint1 = FirstEndpoint.BlobSite.GetConnectionPointInDirection(directionToEndpoint2);
                endpoint2 = SecondEndpoint.BlobSite.GetConnectionPointInDirection(directionToEndpoint1);
            }else {
                var directionToEndpoint2 = FirstEndpoint.BlobSite.Transform.position.GetDominantManhattanDirectionTo(lastEventData.position);
                
                endpoint1 = FirstEndpoint.BlobSite.GetConnectionPointInDirection(directionToEndpoint2);
                endpoint2 = lastEventData.position;
            }

            transform.position = (endpoint1 + endpoint2) / 2;
            transform.localScale = new Vector3(Vector3.Distance(endpoint1, endpoint2), 1f, 1f);

            var zRotation = Mathf.Atan( (endpoint2.y - endpoint1.y) / (endpoint2.x - endpoint1.x) );
            transform.Rotate(new Vector3(0f, 0f, zRotation));
        }

        #endregion

    }

}