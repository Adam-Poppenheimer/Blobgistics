using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

using Assets.Map;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class BlobTubeFactory : BlobTubeFactoryBase {

        #region instance fields and properties

        private DictionaryOfLists<IBlobSite, IBlobSite> ObjectsAttachedToObject =
            new DictionaryOfLists<IBlobSite, IBlobSite>();

        private DictionaryOfLists<IBlobSite, BlobTube> TubesAttachedToObject =
            new DictionaryOfLists<IBlobSite, BlobTube>();

        [SerializeField] private GameObject TubePrefab;

        [SerializeField] private MapGraph Map;

        #endregion

        #region constructors

        public BlobTubeFactory() { }

        #endregion

        #region instance methods

        #region from BlobTubeFactoryBase

        public override IEnumerable<IBlobSite> GetSitesTubedToSite(IBlobSite obj) {
            if(obj == null) {
                throw new ArgumentNullException("obj");
            }
            List<IBlobSite> objectsTubedTo;
            ObjectsAttachedToObject.TryGetValue(obj, out objectsTubedTo);
            if(objectsTubedTo == null) {
                return new List<IBlobSite>();
            }else {
                return objectsTubedTo;
            }
        }

        public override bool TubeExistsBetweenSites(IBlobSite site1, IBlobSite site2) {
            if(site1 == null) {
                throw new ArgumentNullException("site1");
            }else if(site2 == null) {
                throw new ArgumentNullException("site2");
            }
            return ObjectsAttachedToObject.Contains(new KeyValuePair<IBlobSite, IBlobSite>(site1, site2));
        }

        public override bool CanBuildTubeBetween(IBlobSite site1, IBlobSite site2) {
            if(site1 == null) {
                throw new ArgumentNullException("source");
            }else if(site2 == null) {
                throw new ArgumentNullException("target");
            }

            return (
                site1 != site2
                && !TubeExistsBetweenSites(site1, site2)
                && Map.HasEdge(site1.Location, site2.Location)
                && (
                    (site1.AcceptsExtraction && site2.AcceptsPlacement)
                    || (site2.AcceptsPlacement && site1.AcceptsExtraction)
                )
            );
        }

        public override BlobTube BuildTubeBetween(IBlobSite site1, IBlobSite site2) {
            if(!CanBuildTubeBetween(site1, site2)) {
                throw new BlobException("Cannot build a tube between these two objects");
            }

            var source = site1.AcceptsExtraction ? site1 : site2;
            var target = site2.AcceptsPlacement  ? site2 : site1;

            var newTubeObject = GameObject.Instantiate(TubePrefab);
            var tubeBehaviour = newTubeObject.GetComponent<BlobTube>();
            if(tubeBehaviour == null) {
                throw new BlobException("Prefab failed to produce a BlobTube component");
            }

            newTubeObject.transform.SetParent(Map.transform, false);
            tubeBehaviour.SetEndpoints(source, target);

            TubesAttachedToObject.AddElementToList(source, tubeBehaviour);
            TubesAttachedToObject.AddElementToList(target, tubeBehaviour);
            ObjectsAttachedToObject.AddElementToList(source, target);
            ObjectsAttachedToObject.AddElementToList(target, source);

            return tubeBehaviour;
        }

        public override void DestroyAllTubesConnectingTo(IBlobSite site) {
            List<BlobTube> tubesToDestroy;
            TubesAttachedToObject.TryGetValue(site, out tubesToDestroy);
            for(int i = tubesToDestroy.Count - 1; i >= 0; --i) {
                var tubeToDestroy = tubesToDestroy[i];
                ObjectsAttachedToObject[tubeToDestroy.SourceToPullFrom].Remove(tubeToDestroy.TargetToPushTo  );
                ObjectsAttachedToObject[tubeToDestroy.TargetToPushTo  ].Remove(tubeToDestroy.SourceToPullFrom);

                GameObject.Destroy(tubeToDestroy.gameObject);
            }
            TubesAttachedToObject.RemoveList(site);
        }

        #endregion

        #endregion

    }

}
