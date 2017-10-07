using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Highways;

namespace Assets.Core {

    /// <summary>
    /// The standard implementation for HighwayHighlighterControlBase. Acts as  a simulation facade
    /// that gives the UI the ability to highlight and unhighlight highways.
    /// </summary>
    /// <remarks>
    /// This class currently functions by swapping out materials, which is a quick but less than stellar
    /// implementation. Ideally, this class would create glowing outlines around affected highways or
    /// apply some other, more visually appealing effect.
    /// </remarks>
    public class HighwayHighlighterControl : HighwayHighlighterControlBase {

        #region instance fields and properties

        [SerializeField] private BlobHighwayFactoryBase HighwayFactory;

        [SerializeField] private Material BlobTubeHighlightedMaterial;
        [SerializeField] private Material BlobTubeUnhighlightedMaterial;

        private List<BlobHighwayBase> HighlightedHighways = new List<BlobHighwayBase>();

        #endregion

        #region instance methods

        #region from HighwayHighlighterControlBase

        /// <inheritdoc/>
        public override void HighlightHighway(int highwayID) {
            var highwayToHighlight = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToHighlight != null && !HighlightedHighways.Contains(highwayToHighlight)) {
                HighlightedHighways.Add(highwayToHighlight);

                var tubesBeneathHighway = highwayToHighlight.GetComponentsInChildren<BlobTubeBase>();
                foreach(var tube in tubesBeneathHighway) {
                    var spriteRenderer = tube.GetComponent<SpriteRenderer>();
                    if(spriteRenderer != null) {
                        spriteRenderer.sharedMaterial = BlobTubeHighlightedMaterial;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void UnhighlightHighway(int highwayID) {
            var highwayToUnhighlight = HighwayFactory.GetHighwayOfID(highwayID);
            if(highwayToUnhighlight != null) {
                UnhighlightHighway(highwayToUnhighlight);
            }
        }

        /// <inheritdoc/>
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
                    var meshRenderer = tube.GetComponent<SpriteRenderer>();
                    if(meshRenderer != null) {
                        meshRenderer.sharedMaterial = BlobTubeUnhighlightedMaterial;
                    }
                }
            }
        }

        #endregion

    }
}
