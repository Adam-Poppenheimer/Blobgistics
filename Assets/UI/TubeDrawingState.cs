using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Extensions;

using Assets.BlobEngine;

namespace Assets.UI {

    public class TubeDrawingState : UIFSMState {

        #region instance fields and properties

        private ITubableObject AnchoredTubable;

        private IBlobSource CandidateSource;
        private IBlobTarget CandidateTarget;

        [SerializeField] private TubeGhost TubeGhost;

        #endregion

        #region instance methods

        #region from UIFSMState

        protected override UIFSMResponse HandlePointerEnter<T>(T obj, PointerEventData eventData) {
            if(eventData.dragging && obj != AnchoredTubable) {
                if(obj is IBlobSource && CandidateSource == null) {
                    CandidateSource = obj as IBlobSource;
                }else if(obj is IBlobTarget && CandidateTarget == null) {
                    CandidateTarget = obj as IBlobTarget;
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandlePointerExit<T>(T obj, PointerEventData eventData) {
            if(eventData.dragging && obj != AnchoredTubable) {
                if(obj is IBlobSource && AnchoredTubable != CandidateSource) {
                    CandidateSource = null;
                }else if(obj is IBlobTarget && AnchoredTubable != CandidateTarget) {
                    CandidateTarget = null;
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleBeginDrag<T>(T obj, PointerEventData eventData) {
            CandidateSource = null;
            CandidateTarget = null;
            if(obj is IBlobSource) {
                CandidateSource = obj as IBlobSource;
                AnchoredTubable = CandidateSource;
                TubeGhost.gameObject.SetActive(true);
            }else if(obj is IBlobTarget) {
                CandidateTarget = obj as IBlobTarget;
                AnchoredTubable = CandidateTarget;
                TubeGhost.gameObject.SetActive(true);
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleDrag<T>(T obj, PointerEventData eventData) {

            var mousePositionInWorld = Camera.main.ScreenToWorldPoint((Vector3)eventData.position + new Vector3(0f, 0f, 10f));
            Vector3 startingPoint, endingPoint;

            if(CandidateSource != null && CandidateTarget != null) {

                var directionFromSource = CandidateSource.transform.GetDominantManhattanDirectionTo(CandidateTarget.transform);
                startingPoint = CandidateSource.GetConnectionPointInDirection(directionFromSource);
                var directionFromTarget = CandidateTarget.transform.GetDominantManhattanDirectionTo(CandidateSource.transform);
                endingPoint = CandidateTarget.GetConnectionPointInDirection(directionFromTarget);

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }else if(CandidateSource != null) {

                var directionFromSource = CandidateSource.transform.position.GetDominantManhattanDirectionTo(mousePositionInWorld);
                startingPoint = CandidateSource.GetConnectionPointInDirection(directionFromSource);
                endingPoint = mousePositionInWorld;

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }else if(CandidateTarget != null) {

                var directionFromTarget = CandidateTarget.transform.position.GetDominantManhattanDirectionTo(mousePositionInWorld);
                endingPoint = CandidateTarget.GetConnectionPointInDirection(directionFromTarget);
                startingPoint = mousePositionInWorld;

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleEndDrag<T>(T obj, PointerEventData eventData) {
            if(CandidateSource != null && CandidateTarget != null) {
                if(BlobTubeFactory.CanBuildTubeBetween(CandidateSource, CandidateTarget)) {
                    BlobTubeFactory.BuildTubeBetween(CandidateSource, CandidateTarget);
                }
            }
            CandidateSource = null;
            CandidateTarget = null;
            AnchoredTubable = null;
            TubeGhost.gameObject.SetActive(false);
            return UIFSMResponse.Bury;
        }

        #endregion

        #endregion

    }

}
