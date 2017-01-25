using System;
using System.Collections.Generic;
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

        public BuildingSchematic Schematic {
            get { return _schematic; }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                }
                _schematic = value;
                Capacity = _schematic.Cost;
            }
        }
        private BuildingSchematic _schematic;

        #endregion

        [SerializeField] private uint Width;
        [SerializeField] private uint Height;

        public UIFSM TopLevelUIFSM {
            get {
                if(_topLevelUIFSM == null) {
                    throw new InvalidOperationException("TopLevelUIFSM is uninitialized");
                } else {
                    return _topLevelUIFSM;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _topLevelUIFSM = value;
                }
            }
        }
        [SerializeField, HideInInspector] private UIFSM _topLevelUIFSM;

        public IBlobTubeFactory TubeFactory {
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
        [SerializeField, HideInInspector] private IBlobTubeFactory _tubeFactory;

        private IBlobAlignmentStrategy AlignmentStrategy;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnValidate() {
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

        #region Unity EventSystem message implementations

        public void OnPointerClick(PointerEventData eventData) {
            Schematic = BuildingSchematicRepository.Instance.GetSchematicOfName("ResourcePool");
            Debug.Log("Changed schematic");
            TopLevelUIFSM.HandlePointerClick(this, eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            TopLevelUIFSM.HandlePointerEnter(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            TopLevelUIFSM.HandlePointerExit(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData) {
            TopLevelUIFSM.HandleBeginDrag(this, eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            TopLevelUIFSM.HandleDrag(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            TopLevelUIFSM.HandleEndDrag(this, eventData);
        }

        #endregion

        #region from BlobTargetBehaviour

        protected override void OnBlobPlacedInto(ResourceBlob blobPlaced) {
            if(Schematic != null && BlobsWithin.IsAtCapacity()) {
                Schematic.PerformConstruction(this);
                TubeFactory.DestroyAllTubesConnectingTo(this);
                Destroy(gameObject);
            }else {
                AlignmentStrategy.RealignBlobs(BlobsWithin.Blobs, (Vector2)transform.position, RealignmentSpeedPerSecond);
            }
        }

        #endregion

        #endregion

    }

}
