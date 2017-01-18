using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public static class BlobTubeFactory {

        #region static fields and properties

        private static DictionaryOfLists<ITubableObject, ITubableObject> ObjectsAttachedToObject =
            new DictionaryOfLists<ITubableObject, ITubableObject>();

        private static GameObject BlobTubePrefab {
            get {
                if(_blobTubePrefab == null) {
                    _blobTubePrefab = Resources.Load<GameObject>("BlobTubePrefab");
                }
                return _blobTubePrefab;
            }
        }
        private static GameObject _blobTubePrefab;

        private static Transform MapAnchor {
            get {
                if(_mapAnchor == null) {
                    _mapAnchor = GameObject.Find("Map").transform;
                }
                return _mapAnchor;
            }
        }
        private static Transform _mapAnchor;

        #endregion

        #region static methods

        public static IEnumerable<ITubableObject> GetObjectsTubedToObject(ITubableObject obj) {
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

        public static bool TubeExistsBetweenObjects(ITubableObject obj1, ITubableObject obj2) {
            if(obj1 == null) {
                throw new ArgumentNullException("obj1");
            }else if(obj2 == null) {
                throw new ArgumentNullException("obj2");
            }
            return ObjectsAttachedToObject.ContainsKey(obj1);
        }

        public static bool CanBuildTubeBetween(IBlobSource source, IBlobTarget target) {
            if(source == null) {
                throw new ArgumentNullException("source");
            }else if(target == null) {
                throw new ArgumentNullException("target");
            }
            return source != target && !TubeExistsBetweenObjects(source, target);
        }

        public static BlobTube BuildTubeBetween(IBlobSource source, IBlobTarget target) {
            if(!CanBuildTubeBetween(source, target)) {
                throw new BlobException("Cannot build a tube between these two objects");
            }

            var newTubeObject = GameObject.Instantiate(BlobTubePrefab);
            var tubeBehaviour = newTubeObject.GetComponent<BlobTube>();
            if(tubeBehaviour == null) {
                throw new BlobException("Prefab failed to produce a BlobTube component");
            }

            newTubeObject.transform.SetParent(MapAnchor, false);
            tubeBehaviour.SetEndpoints(source, target);
            return tubeBehaviour;
        }


        #endregion

    }

}
