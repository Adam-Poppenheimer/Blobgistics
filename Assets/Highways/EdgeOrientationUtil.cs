using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    public static class EdgeOrientationUtil {

        #region static methods

        public static void AlignTransformWithEndpoints(Transform transform, Vector3 endpoint1, Vector3 endpoint2, bool changeScale = false) {
            transform.position = (endpoint1 + endpoint2) / 2f;
            if(changeScale) {
                var localScale = transform.localScale;
                transform.localScale = new Vector3(Vector3.Distance(endpoint1, endpoint2), localScale.y, localScale.z);
            }
            
            if(!Mathf.Approximately(0f, Vector3.Distance(endpoint1, endpoint2))) {
                transform.rotation = Quaternion.identity;

                var zRotation = Mathf.Atan( (endpoint2.y - endpoint1.y) / (endpoint2.x - endpoint1.x) );
                transform.Rotate(new Vector3(0f, 0f, zRotation * Mathf.Rad2Deg));
            }
        }

        #endregion

    }

}
