using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityCustomUtilities.Extensions;
using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    public class BlobGenerator : MonoBehaviour, IBlobSource, IPointerEnterHandler,
        IPointerExitHandler,  IBeginDragHandler, IDragHandler, IEndDragHandler {

        #region static fields and properties

        private static float SecondsToGenerate = 3f;

        #endregion

        #region instance fields and properties

        #region from ITubableObject

        public Vector3 NorthTubeConnectionPoint {
            get { return LocalNorthConnectionPoint + transform.position; }
        }
        private Vector3 LocalNorthConnectionPoint = new Vector3(0f, 0.5f, ResourceBlob.DesiredZPositionOfAllBlobs);

        public Vector3 SouthTubeConnectionPoint {
            get { return LocalSouthConnectionPoint + transform.position; }
        }
        private Vector3 LocalSouthConnectionPoint = new Vector3(0f, -0.5f, ResourceBlob.DesiredZPositionOfAllBlobs);

        public Vector3 EastTubeConnectionPoint {
            get { return LocalEastConnectionPoint + transform.position; }
        }
        private Vector3 LocalEastConnectionPoint = new Vector3(0.5f, 0, ResourceBlob.DesiredZPositionOfAllBlobs);

        public Vector3 WestTubeConnectionPoint {
            get { return LocalWestConnectionPoint + transform.position; }
        }
        private Vector3 LocalWestConnectionPoint = new Vector3(-0.5f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs);

        #endregion

        [SerializeField] private ResourceType BlobTypeGenerated;
        [SerializeField] private UIFSM TopLevelUIFSM;

        private Coroutine BlobGenerationCoroutine;
        private ResourceBlob BlobInGenerator;

        #endregion

        #region events

        #region from IBlobSource

        public event EventHandler<BlobEventArgs> NewBlobAvailable;

        protected void RaiseNewBlobAvailable() {
            if(NewBlobAvailable != null) {
                NewBlobAvailable(this, new BlobEventArgs(BlobInGenerator));
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            BlobGenerationCoroutine = StartCoroutine(BlobGenerationTick());
        }

        #endregion

        #region Unity EventSystem message implementations

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

        #region from ITubableObject

        public Vector3 GetConnectionPointInDirection(ManhattanDirection direction) {
            switch(direction) {
                case ManhattanDirection.North: return NorthTubeConnectionPoint;
                case ManhattanDirection.South: return SouthTubeConnectionPoint;
                case ManhattanDirection.East:  return EastTubeConnectionPoint;
                case ManhattanDirection.West:  return WestTubeConnectionPoint;
                default: return NorthTubeConnectionPoint;
            }
        }

        #endregion

        #region from IBlobSource

        public bool CanExtractAnyBlob() {
            return BlobInGenerator != null;
        }

        public bool CanExtractBlobOfType(ResourceType type) {
            return BlobInGenerator != null &&
                BlobInGenerator.BlobType == type;
        }

        public ResourceType GetTypeOfNextExtractedBlob() {
            if(BlobInGenerator != null) {
                return BlobInGenerator.BlobType;
            }else {
                throw new BlobException("There is no next blob to extract from this BlobSource");
            }
        }

        public ResourceBlob ExtractAnyBlob() {
            if(CanExtractAnyBlob()) {
                var retval = BlobInGenerator;
                BlobInGenerator.transform.SetParent(null, true);
                BlobInGenerator = null;
                StartCoroutine(BlobGenerationTick());
                return retval;
            }else {
                throw new BlobException("This generator does not have a blob to extract");
            }
        }

        public ResourceBlob ExtractBlobOfType(ResourceType type) {
            if(CanExtractBlobOfType(type)) {
                return ExtractAnyBlob();
            }else {
                throw new BlobException("This generator does not have a blob of that type to extract");
            }
        }

        #endregion

        private void GenerateBlob() {
            if(BlobInGenerator == null) {
                BlobInGenerator = ResourceBlobBuilder.BuildBlob(BlobTypeGenerated, transform.position);
                var currentBlobPosition = BlobInGenerator.transform.position;
                BlobInGenerator.transform.position = new Vector3(currentBlobPosition.x, 
                    currentBlobPosition.y, ResourceBlob.DesiredZPositionOfAllBlobs);
                BlobInGenerator.transform.SetParent(transform, true);
                RaiseNewBlobAvailable();
            }
        }

        private IEnumerator BlobGenerationTick() {
            yield return new WaitForSeconds(SecondsToGenerate);
            if(BlobInGenerator == null) {
                GenerateBlob();
            }
        }

        #endregion

    }

}
