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

    public class BuildingState : UIFSMState, IInjectionTarget<BlobTubeFactoryBase> {

        #region instance fields and properties

        private IBlobSite FirstDraggedBlobSite;
        private IBlobSite SecondDraggedBlobSite;

        [SerializeField] private TubeGhost TubeGhost;
        [SerializeField] private SchematicSelector BuildingSchematicSelector;

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
            if(eventData.dragging && FirstDraggedBlobSite != null && obj != FirstDraggedBlobSite) {
                if(obj is IBlobSite) {
                    SecondDraggedBlobSite = obj as IBlobSite;
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandlePointerExit<T>(T obj, PointerEventData eventData) {
            if(eventData.dragging && FirstDraggedBlobSite != null && obj != FirstDraggedBlobSite) {
                if(obj is IBlobSite) {
                    SecondDraggedBlobSite = null;
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandlePointerClick<T>(T obj, PointerEventData eventData) {
            if(obj is IBuildingPlot) {
                return HandleBuildingPlotClicked(obj as IBuildingPlot, eventData);
            }else {
                BuildingSchematicSelector.Deactivate();
                return UIFSMResponse.Bury;
            }
        }

        protected override UIFSMResponse HandleBeginDrag<T>(T obj, PointerEventData eventData) {
            if(Input.GetMouseButton(0)) {
                FirstDraggedBlobSite = null;
                SecondDraggedBlobSite = null;
                if(obj is IBlobSite) {
                    FirstDraggedBlobSite = obj as IBlobSite;
                    TubeGhost.gameObject.SetActive(true);
                }
            }
            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleDrag<T>(T obj, PointerEventData eventData) {

            var mousePositionInWorld = Camera.main.ScreenToWorldPoint((Vector3)eventData.position + new Vector3(0f, 0f, 10f));
            Vector3 startingPoint, endingPoint;

            if(FirstDraggedBlobSite != null && SecondDraggedBlobSite != null) {

                var directionFromSource = FirstDraggedBlobSite.transform.GetDominantManhattanDirectionTo(SecondDraggedBlobSite.transform);
                startingPoint = FirstDraggedBlobSite.GetConnectionPointInDirection(directionFromSource);
                var directionFromTarget = SecondDraggedBlobSite.transform.GetDominantManhattanDirectionTo(FirstDraggedBlobSite.transform);
                endingPoint = SecondDraggedBlobSite.GetConnectionPointInDirection(directionFromTarget);

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }else if(FirstDraggedBlobSite != null) {

                var directionFromSource = FirstDraggedBlobSite.transform.position.GetDominantManhattanDirectionTo(mousePositionInWorld);
                startingPoint = FirstDraggedBlobSite.GetConnectionPointInDirection(directionFromSource);
                endingPoint = mousePositionInWorld;

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }else if(SecondDraggedBlobSite != null) {

                var directionFromTarget = SecondDraggedBlobSite.transform.position.GetDominantManhattanDirectionTo(mousePositionInWorld);
                endingPoint = SecondDraggedBlobSite.GetConnectionPointInDirection(directionFromTarget);
                startingPoint = mousePositionInWorld;

                TubeGhost.SetEndpoints(startingPoint, endingPoint);

            }

            if(FirstDraggedBlobSite != null && SecondDraggedBlobSite != null) {
                TubeGhost.SetBuildable(TubeFactory.CanBuildTubeBetween(FirstDraggedBlobSite, SecondDraggedBlobSite));
            }else {
                TubeGhost.SetBuildable(false);
            }

            return UIFSMResponse.Bury;
        }

        protected override UIFSMResponse HandleEndDrag<T>(T obj, PointerEventData eventData) {

            if( TubeFactory.CanBuildTubeBetween(FirstDraggedBlobSite, SecondDraggedBlobSite) ){
                TubeFactory.BuildTubeBetween(FirstDraggedBlobSite, SecondDraggedBlobSite);
            }

            FirstDraggedBlobSite = null;
            SecondDraggedBlobSite = null;
            TubeGhost.gameObject.SetActive(false);
            return UIFSMResponse.Bury;
        }

        #endregion

        #region from IInjectionTarget

        public void InjectDependency(BlobTubeFactoryBase dependency, string tag) {
            TubeFactory = dependency;   
        }

        #endregion

        private UIFSMResponse HandleBuildingPlotClicked(IBuildingPlot plot, PointerEventData eventData) {
            BuildingSchematicSelector.Activate(plot);
            BuildingSchematicSelector.transform.position = eventData.position;
            return UIFSMResponse.Bury;
        }

        #endregion

    }

}
