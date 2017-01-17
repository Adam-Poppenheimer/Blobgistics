using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class BlobGenerator : MonoBehaviour, IBlobSource {

        #region static fields and properties

        private static float TimeToGenerateBlob = 3f;
        private static Vector3 StoredBlobOffset = new Vector3(0f, 0f, -1f);

        #endregion

        #region instance fields and properties

        #region from ITubableObject

        public Vector3 NorthTubeConnectionPoint {
            get { return LocalNorthConnectionPoint + transform.position; }
        }
        private Vector3 LocalNorthConnectionPoint = new Vector3(0f, 0.5f, 0f);

        public Vector3 SouthTubeConnectionPoint {
            get { return LocalSouthConnectionPoint + transform.position; }
        }
        private Vector3 LocalSouthConnectionPoint = new Vector3(0f, -0.5f, 0f);

        public Vector3 EastTubeConnectionPoint {
            get { return LocalEastConnectionPoint + transform.position; }
        }
        private Vector3 LocalEastConnectionPoint = new Vector3(0.5f, 0, 0f);

        public Vector3 WestTubeConnectionPoint {
            get { return LocalWestConnectionPoint + transform.position; }
        }
        private Vector3 LocalWestConnectionPoint = new Vector3(-0.5f, 0f, 0f);

        #endregion

        [SerializeField] private ResourceType BlobTypeGenerated;

        private ResourceBlob BlobInGenerator;

        #endregion

        #region events

        public event EventHandler<EventArgs> BlobGenerated;

        protected void RaiseBlobGenerated() {
            if(BlobGenerated != null) {
                BlobGenerated(this, EventArgs.Empty);
            }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            InvokeRepeating("GenerateBlob", TimeToGenerateBlob, TimeToGenerateBlob);
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
                BlobInGenerator = null;
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
                BlobInGenerator = ResourceBlobBuilder.BuildBlob(BlobTypeGenerated);
                BlobInGenerator.transform.SetParent(transform, false);
                BlobInGenerator.transform.localPosition = StoredBlobOffset;
                RaiseBlobGenerated();
            }
        }

        #endregion

    }

}
