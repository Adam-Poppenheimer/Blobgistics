using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Map {

    /// <summary>
    /// A simple utility class that's used to help align map edges with their
    /// endpoints.
    /// </summary>
    /// <remarks>
    /// MapEdges are rendered by rotating the edge's transform so that its up vector
    /// is perpendicular to the line between its endpoints, and then scaling its
    /// visual component to stretch it out. This method performs that operation.
    /// </remarks>
    public static class EdgeOrientationUtil {

        #region static methods

        /// <summary>
        /// Rotates the transform so that its Up vector is perpendicular (in 2D space) to the
        /// vector between endpoint1 and endpoint2. The method can be told to change the scale
        /// of the transform to match the distance between the endpoints.
        /// </summary>
        /// <param name="transform">The transform to be modified</param>
        /// <param name="endpoint1">The first endpoint to be aligned with</param>
        /// <param name="endpoint2">The second endpoint to be aligned with</param>
        /// <param name="changeScale">Should the method change the X-scale of the transform to match the distance between the endpoints?</param>
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
