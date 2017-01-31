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

    public class BuildingPlot : BlobTargetBehaviour, IBuildingPlot, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler  {

        #region static fields and properties

        private static readonly uint Depth = 1;
        private static readonly float RealignmentSpeedPerSecond = 2f;

        #endregion

        #region instance fields and properties

        #region from IBuildingPlot

        public override Vector3 NorthTubeConnectionPoint {
            get { return new Vector3(0f, Height / 2f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public override Vector3 SouthTubeConnectionPoint {
            get { return new Vector3(0f, -Height / 2f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public override Vector3 EastTubeConnectionPoint {
            get { return new Vector3(Width / 2f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public override Vector3 WestTubeConnectionPoint {
            get { return new Vector3(-Width / 2f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs) + transform.position; }
        }

        public Schematic ActiveSchematic {
            get { return _activeSchematic; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                }
                _activeSchematic = value;
                Capacity = _activeSchematic.Cost;
            }
        }
        private Schematic _activeSchematic;

        public ReadOnlyCollection<Schematic> AvailableSchematics {
            get { return PrivateData.AvailableSchematics; }
        }

        #endregion

        [SerializeField] private uint Width = 2;
        [SerializeField] private uint Height = 2;

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
                    RealignToDimensions();
                }
            }
        }
        [SerializeField] private BuildingPlotPrivateData _privateData;

        private IBlobAlignmentStrategy AlignmentStrategy;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            RealignToDimensions();
            Initialize();
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

        #region from BlobTargetBehaviour

        protected override void OnBlobPlacedInto(ResourceBlob blobPlaced) {
            if(ActiveSchematic != null && BlobsWithin.IsAtCapacity()) {
                ActiveSchematic.PerformConstruction(transform);
                PrivateData.TubeFactory.DestroyAllTubesConnectingTo(this);
                Destroy(gameObject);
            }else {
                AlignmentStrategy.RealignBlobs(BlobsWithin.Blobs, (Vector2)transform.position, RealignmentSpeedPerSecond);
            }
        }

        #endregion

        protected void RealignToDimensions() {
            AlignmentStrategy = new BoxyBlobAlignmentStrategy(Width, Height, 5, 5);
            var attachedMeshFilter = GetComponent<MeshFilter>();
            if(attachedMeshFilter != null) {
                attachedMeshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(
                    new Tuple<uint, uint, uint>(Width, Height, Depth));
            }
            var boxCollider = GetComponent<BoxCollider2D>();
            if(boxCollider != null) {
                boxCollider.size = new Vector2(Width, Height);
            }
        }

        #endregion

    }

}
