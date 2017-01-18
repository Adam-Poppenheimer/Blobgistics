using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.UI;

using Assets.BlobEngine;

namespace Assets.UI {

    public class TubeDrawingState : UIFSMState {

        #region instance fields and properties

        private ITubableObject AnchoredTubable;

        private IBlobSource CandidateSource;
        private IBlobTarget CandidateTarget;

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
            }else if(obj is IBlobTarget) {
                CandidateTarget = obj as IBlobTarget;
                AnchoredTubable = CandidateTarget;
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
            return UIFSMResponse.Bury;
        }

        #endregion

        #endregion

    }

}
