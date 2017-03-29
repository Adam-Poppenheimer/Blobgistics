using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;

using UnityCustomUtilities.Meshes;
using UnityCustomUtilities.Extensions;

namespace Assets.Util {

    public static class MeshResizerUtility {

        #region static methods

        public static void RealignToDimensions(GameObject resizedObject, Tuple<uint, uint, uint> newDimensions) {
            var attachedMeshFilter = resizedObject.GetComponent<MeshFilter>();
            if(attachedMeshFilter != null) {
                attachedMeshFilter.sharedMesh = BoxMeshBuilder.GetAppropriateMesh(newDimensions);
            }
            var boxCollider = resizedObject.GetComponent<BoxCollider2D>();
            if(boxCollider != null) {
                boxCollider.size = new Vector2(newDimensions.Item1, newDimensions.Item2);
            }
        }

        public static void RealignToDimensions(GameObject resizedObject, Tuple<uint, uint, uint> newDimensions,
            out BlobAlignmentStrategyBase alignmentStrategy) {
            RealignToDimensions(resizedObject, newDimensions);
            alignmentStrategy = new BoxyBlobAlignmentStrategy(newDimensions.Item1, newDimensions.Item2, 5, 5);
        }

        #endregion

    }

}
