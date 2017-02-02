using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Combat;
using UnityCustomUtilities.Misc;

namespace Assets.BlobEngine {

    public class BlobContainerExploder : MonoBehaviour, IInjectionTarget<BlobTubeFactoryBase> {

        #region instance fields and properties

        [SerializeField] private HealthRecorder HealthRecorder;
        [SerializeField] private BlobTubeFactoryBase TubeFactory;

        #endregion

        #region instance methods

        #region Unity event methods

        private void Start() {
            if(HealthRecorder != null) {
                HealthRecorder.HasDied += ExplodeObject;
            }
        }

        #endregion

        #region from IInjectionTarget

        public void InjectDependency(BlobTubeFactoryBase dependency, string tag) {
            TubeFactory = dependency;
        }

        #endregion

        public void ExplodeObject() {
            var attachedBlobTarget = GetComponent<IBlobTarget>();
            if(attachedBlobTarget != null) {
                attachedBlobTarget.ClearAllBlobs(true);
            }
            var attachedTubeObject = GetComponent<ITubableObject>();
            if(attachedTubeObject != null) {
                TubeFactory.DestroyAllTubesConnectingTo(attachedTubeObject);
            }
            Destroy(gameObject);
        }

        private void ExplodeObject(object sender, EventArgs e) {
            ExplodeObject();
        }

        #endregion

    }

}
