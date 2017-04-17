using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Core;
using Assets.BlobSites;

namespace Assets.Map {

    [ExecuteInEditMode]
    public class MapNode : MapNodeBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        #region from MapNodeBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapGraphBase ManagingGraph {
            get { return _managingGraph; }
        }
        public void SetManagingGraph(MapGraphBase value) {
            _managingGraph = value;
        }
        [SerializeField, HideInInspector] private MapGraphBase _managingGraph;

        public override BlobSiteBase BlobSite {
            get { return _blobSite; }
        }
        public void SetBlobSite(BlobSiteBase value) {
            _blobSite = value;
        }
        [SerializeField, HideInInspector] private BlobSiteBase _blobSite;

        public override IEnumerable<MapNodeBase> Neighbors {
            get { return ManagingGraph.GetNeighborsOfNode(this); }
        }

        #endregion

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(ManagingGraph != null && ManagingGraph.GetNodeOfID(ID) == null) {
                ManagingGraph.SubscribeNode(this);
            }
        }

        private void OnDestroy() {
            if(ManagingGraph != null) {
                ManagingGraph.RemoveNode(this);
            }
        }

        #endregion

        #region EventSystem interface implementations

        public void OnBeginDrag(PointerEventData eventData) {
            UIControl.PushBeginDragEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            UIControl.PushDragEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            UIControl.PushEndDragEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            UIControl.PushPointerClickEvent(new MapNodeUISummary(this), eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIControl.PushPointerEnterEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIControl.PushPointerExitEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnSelect(BaseEventData eventData) {
            UIControl.PushSelectEvent(new MapNodeUISummary(this), eventData);
        }

        public void OnDeselect(BaseEventData eventData) {
            UIControl.PushDeselectEvent(new MapNodeUISummary(this), eventData);
        }

        #endregion

        #endregion

    }

}
