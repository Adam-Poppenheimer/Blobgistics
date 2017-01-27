using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    [ExecuteInEditMode]
    public class BlobTubeFactory : BlobTubeFactoryBase {

        #region instance fields and properties

        private DictionaryOfLists<ITubableObject, ITubableObject> ObjectsAttachedToObject =
            new DictionaryOfLists<ITubableObject, ITubableObject>();

        private DictionaryOfLists<ITubableObject, BlobTube> TubesAttachedToObject =
            new DictionaryOfLists<ITubableObject, BlobTube>();

        [SerializeField] private GameObject TubePrefab;

        [SerializeField] private Transform MapRoot;

        #endregion

        #region constructors

        public BlobTubeFactory() { }

        #endregion

        #region instance methods

        #region from BlobTubeFactoryBase

        public override IEnumerable<ITubableObject> GetObjectsTubedToObject(ITubableObject obj) {
            if(obj == null) {
                throw new ArgumentNullException("obj");
            }
            List<ITubableObject> objectsTubedTo;
            ObjectsAttachedToObject.TryGetValue(obj, out objectsTubedTo);
            if(objectsTubedTo == null) {
                return new List<ITubableObject>();
            }else {
                return objectsTubedTo;
            }
        }

        public override bool TubeExistsBetweenObjects(ITubableObject obj1, ITubableObject obj2) {
            if(obj1 == null) {
                throw new ArgumentNullException("obj1");
            }else if(obj2 == null) {
                throw new ArgumentNullException("obj2");
            }
            return ObjectsAttachedToObject.Contains(new KeyValuePair<ITubableObject, ITubableObject>(obj1, obj2));
        }

        public override bool CanBuildTubeBetween(ITubableObject obj1, ITubableObject obj2) {
            if(obj1 == null) {
                throw new ArgumentNullException("source");
            }else if(obj2 == null) {
                throw new ArgumentNullException("target");
            }else if(obj1 is IBlobSource && obj2 is IBlobTarget) {
                return CanBuildTubeBetween(obj1 as IBlobSource, obj2 as IBlobTarget);
            }else if(obj1 is IBlobTarget && obj2 is IBlobSource) {
                return CanBuildTubeBetween(obj2 as IBlobSource, obj1 as IBlobTarget);
            }else {
                return false;
            }
        }

        public override bool CanBuildTubeBetween(IBlobSource source, IBlobTarget target) {
            if(source == null) {
                throw new ArgumentNullException("source");
            }else if(target == null) {
                throw new ArgumentNullException("target");
            }
            return source != target && !TubeExistsBetweenObjects(source, target);
        }

        public override BlobTube BuildTubeBetween(IBlobSource source, IBlobTarget target) {
            if(!CanBuildTubeBetween(source, target)) {
                throw new BlobException("Cannot build a tube between these two objects");
            }

            var newTubeObject = GameObject.Instantiate(TubePrefab);
            var tubeBehaviour = newTubeObject.GetComponent<BlobTube>();
            if(tubeBehaviour == null) {
                throw new BlobException("Prefab failed to produce a BlobTube component");
            }

            newTubeObject.transform.SetParent(MapRoot, false);
            tubeBehaviour.SetEndpoints(source, target);

            TubesAttachedToObject.AddElementToList(source, tubeBehaviour);
            TubesAttachedToObject.AddElementToList(target, tubeBehaviour);
            ObjectsAttachedToObject.AddElementToList(source, target);
            ObjectsAttachedToObject.AddElementToList(target, source);

            return tubeBehaviour;
        }

        public override void DestroyAllTubesConnectingTo(ITubableObject obj) {
            List<BlobTube> tubesToDestroy;
            TubesAttachedToObject.TryGetValue(obj, out tubesToDestroy);
            for(int i = tubesToDestroy.Count - 1; i >= 0; --i) {
                var tubeToDestroy = tubesToDestroy[i];
                ObjectsAttachedToObject[tubeToDestroy.SourceToPullFrom].Remove(tubeToDestroy.TargetToPushTo  );
                ObjectsAttachedToObject[tubeToDestroy.TargetToPushTo  ].Remove(tubeToDestroy.SourceToPullFrom);

                GameObject.Destroy(tubeToDestroy.gameObject);
            }
            TubesAttachedToObject.RemoveList(obj);
        }

        #endregion

        #endregion

    }

}
