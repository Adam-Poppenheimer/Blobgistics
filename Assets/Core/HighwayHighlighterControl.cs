using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;

namespace Assets.Core {

    public class HighwayHighlighterControl : HighwayHighlighterControlBase {

        #region instance fields and properties

        [SerializeField] private BlobHighwayFactoryBase HighwayFactory;

        [SerializeField] private Material BlobTubeHighlightedMaterial;
        [SerializeField] private Material BlobTubeUnhighlightedMaterial;

        private List<BlobHighwayBase> HighlightedHighways = new List<BlobHighwayBase>();

        #endregion

        #region instance methods

        #region from HighwayHighlighterControlBase

        public override void HighlightHighway(int highwayID) {
            var highwayToHighlight = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToHighlight != null && !HighlightedHighways.Contains(highwayToHighlight)) {
                HighlightedHighways.Add(highwayToHighlight);

                var tubesBeneathHighway = highwayToHighlight.GetComponentsInChildren<BlobTubeBase>();
                foreach(var tube in tubesBeneathHighway) {
                    var meshRenderer = tube.GetComponent<MeshRenderer>();
                    if(meshRenderer != null) {
                        meshRenderer.sharedMaterial = BlobTubeHighlightedMaterial;
                    }
                }
            }
        }

        public override void UnhighlightHighway(int highwayID) {
            var highwayToUnhighlight = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToUnhighlight != null) {
                UnhighlightHighway(highwayToUnhighlight);
            }
        }

        public override void UnhighlightAllHighways() {
            foreach(var highway in new List<BlobHighwayBase>(HighlightedHighways)) {
                UnhighlightHighway(highway);
            }
        }

        #endregion

        private void UnhighlightHighway(BlobHighwayBase highway) {
            if(HighlightedHighways.Remove(highway)) {
                var tubesBeneathHighway = highway.GetComponentsInChildren<BlobTubeBase>();
                foreach(var tube in tubesBeneathHighway) {
                    var meshRenderer = tube.GetComponent<MeshRenderer>();
                    if(meshRenderer != null) {
                        meshRenderer.sharedMaterial = BlobTubeUnhighlightedMaterial;
                    }
                }
            }
        }

        #endregion

    }
}
