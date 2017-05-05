using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockSocietyFactory : SocietyFactoryBase {

        #region instance fields and properties

        #region from SocietyFactoryBase

        public override ComplexityLadderBase StandardComplexityLadder {
            get {
                if(_standardComplexityLadder == null) {
                    _standardComplexityLadder = gameObject.AddComponent<MockComplexityLadder>();
                }
                return _standardComplexityLadder;
            }
        }
        private ComplexityLadderBase _standardComplexityLadder;

        public override ComplexityDefinitionBase DefaultComplexityDefinition {
            get {
                if(_defaultComplexityDefinition == null) {
                    _defaultComplexityDefinition = gameObject.AddComponent<MockComplexityDefinition>();
                }
                return _defaultComplexityDefinition;
            }
        }
        private ComplexityDefinitionBase _defaultComplexityDefinition;

        #endregion

        private List<SocietyBase> Societies = new List<SocietyBase>();

        #endregion

        #region events

        public event EventHandler<FloatEventArgs> FactoryTicked;

        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override SocietyBase GetSocietyAtLocation(MapNodeBase location) {
            return Societies.Where(society => society.Location == location).FirstOrDefault();
        }

        public override bool HasSocietyAtLocation(MapNodeBase location) {
            return GetSocietyAtLocation(location) != null;
        }

        public override bool CanConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity) {
            return true;
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity) {
            var newSociety = (new GameObject()).AddComponent<MockSociety>();

            newSociety.location = location;
            newSociety.activeComplexityLadder = ladder;
            newSociety.currentComplexity = startingComplexity;

            Societies.Add(newSociety);
            return newSociety;
        }

        public override void DestroySociety(SocietyBase society) {
            DestroyImmediate(society.gameObject);
        }

        public override void UnsubscribeSociety(SocietyBase societyBeingDestroyed) {
            throw new NotImplementedException();
        }

        public override SocietyBase GetSocietyOfID(int id) {
            return Societies.Where(society => society.ID == id).FirstOrDefault();
        }

        public override void TickSocieties(float secondsPassed) {
            if(FactoryTicked != null) {
                FactoryTicked(this, new FloatEventArgs(secondsPassed));
            }
        }

        #endregion

        #endregion

    }

}
