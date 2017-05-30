using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Societies;

namespace Assets.Session.ForTesting {

    public class MockSocietyFactory : SocietyFactoryBase {

        #region instance fields and properties

        #region from SocietyFactoryBase

        public override ComplexityDefinitionBase DefaultComplexityDefinition {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<SocietyBase> Societies {
            get { return societies.AsReadOnly(); }
        }
        private List<SocietyBase> societies = new List<SocietyBase>();

        public override ComplexityLadderBase StandardComplexityLadder {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override bool CanConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity) {
            throw new NotImplementedException();
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity) {
            var newSociety = (new GameObject()).AddComponent<MockSociety>();
            newSociety.location = location;
            newSociety.activeComplexityLadder = ladder;
            newSociety.currentComplexity = startingComplexity;
            societies.Add(newSociety);
            return newSociety;
        }

        public override void DestroySociety(SocietyBase society) {
            societies.Remove(society);
            DestroyImmediate(society.gameObject);
        }

        public override SocietyBase GetSocietyAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override SocietyBase GetSocietyOfID(int id) {
            throw new NotImplementedException();
        }

        public override bool HasSocietyAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override void SubscribeSociety(SocietyBase society) {
            throw new NotImplementedException();
        }

        public override void TickSocieties(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeSociety(SocietyBase society) {
            throw new NotImplementedException();
        }

        public override ComplexityDefinitionBase GetComplexityDefinitionOfName(string name) {
            throw new NotImplementedException();
        }

        public override ComplexityLadderBase GetComplexityLadderOfName(string name) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
