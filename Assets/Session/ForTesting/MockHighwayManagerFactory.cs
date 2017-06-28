using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.HighwayManager;
using Assets.Highways;
using Assets.Map;

namespace Assets.Session.ForTesting {

    public class MockHighwayManagerFactory : HighwayManagerFactoryBase {

        #region instance fields and properties

        #region from HighwayManagerFactoryBase

        public override ReadOnlyCollection<HighwayManagerBase> Managers {
            get { return managers.AsReadOnly(); }
        }
        private List<HighwayManagerBase> managers = new List<HighwayManagerBase>();

        #endregion

        #endregion

        #region instance methods

        #region from HighwayManagerFactoryBase

        public override bool CanConstructHighwayManagerAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase ConstructHighwayManagerAtLocation(MapNodeBase location) {
            var newManager = (new GameObject()).AddComponent<MockHighwayManager>();
            newManager.location = location;
            managers.Add(newManager);
            return newManager;
        }

        public override void DestroyHighwayManager(HighwayManagerBase manager) {
            managers.Remove(manager);
            DestroyImmediate(manager.gameObject);
        }

        public override HighwayManagerBase GetHighwayManagerAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase GetHighwayManagerOfID(int id) {
            throw new NotImplementedException();
        }

        public override IEnumerable<BlobHighwayBase> GetHighwaysServedByManager(HighwayManagerBase manager) {
            throw new NotImplementedException();
        }

        public override HighwayManagerBase GetManagerServingHighway(BlobHighwayBase highway) {
            throw new NotImplementedException();
        }

        public override void TickAllManangers(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeHighwayManager(HighwayManagerBase manager) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
