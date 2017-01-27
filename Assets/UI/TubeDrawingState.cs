using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Extensions;
using UnityCustomUtilities.Misc;

using Assets.BlobEngine;

namespace Assets.UI {

    public class TubeDrawingState : UIFSMState, IInjectionTarget<BlobTubeFactoryBase> {

        #region instance fields and properties

        private ITubableObject FirstDraggedTubable;
        private ITubableObject SecondDraggedTubable;

        [SerializeField] private TubeGhost TubeGhost;

        public BlobTubeFactoryBase TubeFactory {
            get {
                if(_tubeFactory == null) {
                    throw new InvalidOperationException("TubeFactory is uninitialized");
                } else {
                    return _tubeFactory;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _tubeFactory = value;
                }
            }
        }
        private BlobTubeFactoryBase _tubeFactory;

        #endregion

        #region instance methods

        #region from UIFSMState

        protected override UIFSMResponse HandlePointerEnter<T>(T obj, PointerEventData eventData) {
            if(eventData.dragging && FirstDraggedTubable != null && obj != FirstDraggedTubable) {
                if(obj is ITubableObject) {
                    SecondDraggedTubable = obj as ITubableObject;
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandlePointerExit<T>(T obj, PointerEventData eventData) {
            if(eventData.dragging && FirstDraggedTubable != null && obj != FirstDraggedTubable) {
                if(obj is ITubableObject) {
                    SecondDraggedTubable = null;
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleBeginDrag<T>(T obj, PointerEventData eventData) {
            FirstDraggedTubable = null;
            SecondDraggedTubable = null;
            if(obj is ITubableObject) {
                FirstDraggedTubable = obj as ITubableObject;
                TubeGhost.gameObject.SetActive(true);
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleDrag<T>(T obj, PointerEventData eventData) {

            var mousePositionInWorld = Camera.main.ScreenToWorldPoint((Vector3)eventData.position + new Vector3(0f, 0f, 10f));
            Vector3 startingPoint, endingPoint;

            if(FirstDraggedTubable != null && SecondDraggedTubable != null) {

                var directionFromSource = FirstDraggedTubable.transform.GetDominantManhattanDirectionTo(SecondDraggedTubable.transform);
                startingPoint = FirstDraggedTubable.GetConnectionPointInDirection(directionFromSource);
                var directionFromTarget = SecondDraggedTubable.transform.GetDominantManhattanDirectionTo(FirstDraggedTubable.transform);
                endingPoint = SecondDraggedTubable.GetConnectionPointInDirection(directionFromTarget);

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }else if(FirstDraggedTubable != null) {

                var directionFromSource = FirstDraggedTubable.transform.position.GetDominantManhattanDirectionTo(mousePositionInWorld);
                startingPoint = FirstDraggedTubable.GetConnectionPointInDirection(directionFromSource);
                endingPoint = mousePositionInWorld;

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }else if(SecondDraggedTubable != null) {

                var directionFromTarget = SecondDraggedTubable.transform.position.GetDominantManhattanDirectionTo(mousePositionInWorld);
                endingPoint = SecondDraggedTubable.GetConnectionPointInDirection(directionFromTarget);
                startingPoint = mousePositionInWorld;

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }

            if(FirstDraggedTubable != null && SecondDraggedTubable != null) {
                TubeGhost.SetBuildable(TubeFactory.CanBuildTubeBetween(FirstDraggedTubable, SecondDraggedTubable));
            }else {
                TubeGhost.SetBuildable(false);
            }

            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleEndDrag<T>(T obj, PointerEventData eventData) {
            IBlobSource candidateSource = null;
            IBlobTarget candidateTarget = null;

            if(FirstDraggedTubable is IBlobSource && SecondDraggedTubable is IBlobTarget) {
                candidateSource = FirstDraggedTubable as IBlobSource;
                candidateTarget = SecondDraggedTubable as IBlobTarget;
            }else if(FirstDraggedTubable is IBlobTarget && SecondDraggedTubable is IBlobSource) {
                candidateSource = SecondDraggedTubable as IBlobSource;
                candidateTarget = FirstDraggedTubable as IBlobTarget;
            }

            if( candidateSource != null && candidateTarget != null &&
                TubeFactory.CanBuildTubeBetween(candidateSource, candidateTarget)
            ){
                TubeFactory.BuildTubeBetween(candidateSource, candidateTarget);
            }

            FirstDraggedTubable = null;
            SecondDraggedTubable = null;
            TubeGhost.gameObject.SetActive(false);
            return UIFSMResponse.Bury;
        }

        #endregion

        #region from IInjectionTarget

        public void InjectDependency(BlobTubeFactoryBase dependency, string tag) {
            TubeFactory = dependency;   
        }

        #endregion

        #endregion

    }

}
