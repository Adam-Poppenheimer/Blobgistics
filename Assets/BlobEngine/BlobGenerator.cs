using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BlobGenerator : MonoBehaviour, IBlobSource {

        #region static fields and properties

        private static float TimeToGenerateBlob = 3f;
        private static Vector3 StoredBlobOffset = new Vector3(0f, 0f, -1f);

        #endregion

        #region instance fields and properties

        [SerializeField] private ResourceType BlobTypeGenerated;

        private ResourceBlob BlobInGenerator {
            get { return _blobInGenerator; }
            set {
                if(value != null) {
                    _blobInGenerator = value;
                    _blobInGenerator.transform.SetParent(transform, false);
                    _blobInGenerator.transform.localPosition = StoredBlobOffset;
                    RaiseBlobGenerated();
                }
            }
        }
        private ResourceBlob _blobInGenerator = null;

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

        private void OnEnable() {
            StartCoroutine(GenerateBlob());
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
                StartCoroutine(GenerateBlob());
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

        private IEnumerator GenerateBlob() {
            yield return new WaitForSeconds(TimeToGenerateBlob);
            BlobInGenerator = ResourceBlobBuilder.BuildBlob(BlobTypeGenerated);
        }

        #endregion

    }

}
