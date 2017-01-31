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

    public class BlobGenerator : BlobSourceBehaviour, IBlobGenerator, IPointerEnterHandler,
        IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

        #region instance fields and properties

        #region from BlobSourceBehaviour

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

        #endregion

        public BlobGeneratorPrivateData PrivateData {
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
                }
            }
        }
        [SerializeField] private BlobGeneratorPrivateData _privateData;

        public ResourceType BlobTypeGenerated {
            get { return _blobTypeGenerated; }
            set { _blobTypeGenerated = value; }
        }
        [SerializeField] private ResourceType _blobTypeGenerated;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            Initialize();
            Capacity = PrivateData.Capacity;
            StartCoroutine(BlobGenerationTick());
        }

        #endregion

        #region Unity EventSystem message implementations

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

        #region from BlobSourceBehaviour

        protected override void DoOnBlobBeingExtracted(ResourceBlob blobExtracted) {
            StartCoroutine(BlobGenerationTick());
        }

        #endregion

        private void GenerateBlob() {
            var newBlob = ResourceBlobBuilder.BuildBlob(BlobTypeGenerated, transform.position);
            BlobsWithin.InsertBlob(newBlob);
            var currentBlobPosition = newBlob.transform.position;
            newBlob.transform.position = new Vector3(currentBlobPosition.x, 
                currentBlobPosition.y, ResourceBlob.DesiredZPositionOfAllBlobs);
            newBlob.transform.SetParent(transform, true);
            RaiseNewBlobAvailable(newBlob);
        }

        private IEnumerator BlobGenerationTick() {
            yield return new WaitForSeconds(PrivateData.SecondsToGenerate);
            if(BlobsWithin.CanInsertBlobOfType(BlobTypeGenerated)) {
                GenerateBlob();
            }
        }

        #endregion

    }

}
