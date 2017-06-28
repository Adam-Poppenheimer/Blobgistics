using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;

namespace Assets.UI.Blobs {

    public abstract class ResourceDisplayBase : MonoBehaviour {

        #region instance methods

        public abstract void PushAndDisplaySummary(IntPerResourceDictionary summary);
        public abstract void PushAndDisplaySummary(IDictionary<ResourceType, int> summaryDictionary);
        public abstract void PushAndDisplaySummary(IEnumerable<ResourceType> typesAccepted, int countNeeded);
        public abstract void PushAndDisplayInfo(ResourceDisplayInfo infoToDisplay);

        #endregion

    }

}
