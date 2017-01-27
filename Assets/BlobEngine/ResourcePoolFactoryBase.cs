using UnityEngine;

namespace Assets.BlobEngine {

    public abstract class ResourcePoolFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract IResourcePool BuildResourcePool(Vector3 localPosition, Transform parent);

        #endregion

    }

}
