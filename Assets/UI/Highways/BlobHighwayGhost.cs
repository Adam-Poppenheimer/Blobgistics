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

        public override bool GhostIsBuildable {
            get { return _ghostIsBuildable; }
            set {
                _ghostIsBuildable = value;
                var meshRenderer = GetComponent<MeshRenderer>();
                if(meshRenderer != null) {
                    meshRenderer.material = _ghostIsBuildable ? BuildableMaterial : UnbuildableMaterial;
                }
            }
        }
        private bool _ghostIsBuildable;

        private PointerEventData lastEventData;

        [SerializeField] private Material BuildableMaterial;
        [SerializeField] private Material UnbuildableMaterial;

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
                endpoint1 = FirstEndpoint.BlobSite.GetPointOfConnectionFacingPoint(SecondEndpoint.Transform.position);
                endpoint2 = SecondEndpoint.BlobSite.GetPointOfConnectionFacingPoint(FirstEndpoint.Transform.position);
            }else {
                endpoint2 = Camera.main.ScreenToWorldPoint((Vector3)lastEventData.position - new Vector3(0f, 0f, Camera.main.transform.position.z));

                var directionToEndpoint2 = FirstEndpoint.BlobSite.Transform.position.GetDominantManhattanDirectionTo(endpoint2);
                
                endpoint1 = FirstEndpoint.BlobSite.GetPointOfConnectionFacingPoint(endpoint2);
            }

            EdgeOrientationUtil.AlignTransformWithEndpoints(transform, endpoint1, endpoint2, true);
        }

        #endregion

    }

}