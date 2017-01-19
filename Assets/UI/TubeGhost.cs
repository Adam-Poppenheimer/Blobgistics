using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;

namespace Assets.UI {

    public class TubeGhost : MonoBehaviour {

        #region static fields and properties

        private static float TubeWidth = 0.5f;
        private static float TubeDepth = 0.5f;

        #endregion

        #region instance methods

        public void SetEndpoints(Vector3 start, Vector3 end) {
            transform.rotation = Quaternion.identity;
            var meshFilter = GetComponent<MeshFilter>();
            if(meshFilter != null) {
                meshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(new Tuple<uint, uint, uint>(1, 1, 1));
            }
            
            transform.position = (start + end ) / 2f;
            transform.localScale = new Vector3(Vector3.Distance(start, end), TubeWidth, TubeDepth);
            
            var zAngleToRotate = Mathf.Rad2Deg * Mathf.Atan(
                (end.y - start.y) /
                (end.x - start.x)
            );
            transform.Rotate(new Vector3(0f, 0f, zAngleToRotate));
        }

        #endregion

    }

}
