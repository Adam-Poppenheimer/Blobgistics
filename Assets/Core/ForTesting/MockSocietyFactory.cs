using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;
using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockSocietyFactory : SocietyFactoryBase {

        #region instance fields and properties

        #region from SocietyFactoryBase

        public override ComplexityLadderBase StandardComplexityLadder {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region events

        public event EventHandler<FloatEventArgs> FactoryTicked;

        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override SocietyBase GetSocietyAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override bool HasSocietyAtLocation(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override bool CanConstructSocietyAt(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder) {
            throw new NotImplementedException();
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity) {
            throw new NotImplementedException();
        }

        public override void DestroySociety(SocietyBase society) {
            throw new NotImplementedException();
        }

        public override void UnsubscribeSocietyBeingDestroyed(SocietyBase societyBeingDestroyed) {
            throw new NotImplementedException();
        }

        public override SocietyBase GetSocietyOfID(int id) {
            throw new NotImplementedException();
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
