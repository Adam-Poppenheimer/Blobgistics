using UnityEngine;

namespace Assets.BlobEngine {

    public interface IResourcePoolFactory {

        #region methods

        IResourcePool BuildResourcePool(Vector3 localPosition, Transform parent);

        #endregion

    }

}