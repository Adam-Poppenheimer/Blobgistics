using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Highways;

namespace Assets.HighwayManager {

    public abstract class HighwayManagerFactoryBase : MonoBehaviour {

        #region instance methods

        public abstract HighwayManagerBase GetHighwayManagerOfID(int id);

        public abstract HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location);

        public abstract HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway);

        public abstract IEnumerable<BlobHighwayBase> GetHighwaysServedByManager(HighwayManagerBase manager);

        public abstract bool               CanConstructHighwayManagerAtLocation(MapNodeBase location);
        public abstract HighwayManagerBase ConstructHighwayManagerAtLocation   (MapNodeBase location);

        public abstract void DestroyHighwayManager    (HighwayManagerBase manager);
        public abstract void UnsubscribeHighwayManager(HighwayManagerBase manager);

        public abstract void SubscribeHighway  (BlobHighwayBase highway);
        public abstract void UnsubscribeHighway(BlobHighwayBase highway);

        public abstract void TickAllManangers(float secondsPassed);

        #endregion

    }

}
