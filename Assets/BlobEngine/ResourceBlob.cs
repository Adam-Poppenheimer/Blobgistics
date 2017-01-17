using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class ResourceBlob : MonoBehaviour {

        #region static fields and properties

        private static float SecondsToPopIn = 0.25f;
        private static Vector3 StartingVelocity = new Vector3(5f, 5f, 5f);

        #endregion

        #region instance fields and properties

        public ResourceType BlobType;

        private Vector3 ScaleToPopTo;
        private Vector3 CurrentVelocity;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            ScaleToPopTo = transform.localScale;
        }

        private void OnEnable() {
            CurrentVelocity = new Vector3(StartingVelocity.x, StartingVelocity.y, StartingVelocity.z);
            StartCoroutine(PopIn());
        }

        #endregion

        private IEnumerator PopIn() {
            transform.localScale = Vector3.zero;
            while(true) {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, ScaleToPopTo,
                    ref CurrentVelocity, SecondsToPopIn);
                if(Mathf.Approximately(transform.localScale.x, ScaleToPopTo.x)) {
                    yield break;
                }else {
                    yield return null;
                }
            }
        }

        #endregion

    }

}
