using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Extensions;
using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    public class BuildingPlot : BlobSiteBase, IBuildingPlot, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler  {

        #region instance fields and properties

        #region from BlobSiteBase

        public override Vector3 NorthTubeConnectionPoint {
            get { return PrivateData.LocalNorthConnectionPoint + transform.position; }
        }

        public override Vector3 SouthTubeConnectionPoint {
            get { return PrivateData.LocalSouthConnectionPoint + transform.position; }
        }

        public override Vector3 EastTubeConnectionPoint {
            get { return PrivateData.LocalEastConnectionPoint + transform.position; }
        }

        public override Vector3 WestTubeConnectionPoint {
            get { return PrivateData.LocalWestConnectionPoint + transform.position; }
        }

        public override bool AcceptsExtraction {
            get { return false; }
        }

        public override bool AcceptsPlacement {
            get { return true; }
        }

        protected override BlobPileBase BlobsWithin {
            get { return blobsWithin; }
        }
        private BlobPileBase blobsWithin = new AmbivalentBlobPile(0);

        protected override BlobPileBase BlobsWithReservedPositions {
            get { return blobsWithReservedPositions; }
        }
        private BlobPileBase blobsWithReservedPositions = new AmbivalentBlobPile(0);

        #endregion

        #region from IBuildingPlot

        public Schematic ActiveSchematic {
            get { return _activeSchematic; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                }
                _activeSchematic = value;
                blobsWithin = new TypeConstrainedBlobPile(_activeSchematic.Cost);
                blobsWithReservedPositions = new TypeConstrainedBlobPile(_activeSchematic.Cost);
            }
        }
        private Schematic _activeSchematic;

        public ReadOnlyCollection<Schematic> AvailableSchematics {
            get { return PrivateData.AvailableSchematics; }
        }

        #endregion

        public BuildingPlotPrivateData PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                    Util.MeshResizerUtility.RealignToDimensions(gameObject, PrivateData.Dimensions);
                }
            }
        }
        [SerializeField] private BuildingPlotPrivateData _privateData;

        private IBlobAlignmentStrategy AlignmentStrategy;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            Util.MeshResizerUtility.RealignToDimensions(gameObject, PrivateData.Dimensions, out AlignmentStrategy);
        }

        #endregion

        #region Unity EventSystem message implementations

        public void OnPointerClick(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandlePointerClick(this, eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandlePointerEnter(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandlePointerExit(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandleBeginDrag(this, eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandleDrag(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            PrivateData.TopLevelUIFSM.HandleEndDrag(this, eventData);
        }

        #endregion

        #region from BlobSiteBase

        protected override void OnBlobPlacedInto(ResourceBlob blobPlaced) {
            if(ActiveSchematic != null && BlobsWithin.IsAtCapacity()) {
                ActiveSchematic.PerformConstruction(Location);
                PrivateData.TubeFactory.DestroyAllTubesConnectingTo(this);
                Destroy(gameObject);
            }else {
                AlignmentStrategy.RealignBlobs(BlobsWithin.Contents,
                    (Vector2)transform.position, PrivateData.RealignmentSpeedPerSecond);
            }
        }

        #endregion

        #endregion

    }

}
