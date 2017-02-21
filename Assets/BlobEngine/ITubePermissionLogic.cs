using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public interface ITubePermissionLogic {

        #region methods

        bool HasPermissionForResourcesOfType(ResourceType type);
        void SetPermissionForResourcesOfType(ResourceType type, bool newValue);

        int  GetPriorityForResourcesOfType(ResourceType type);
        void SetPriorityForResourcesOfType(ResourceType type, int newPriority);

        #endregion

    }

}
