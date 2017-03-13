using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Map;
using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockSocietyFactory : SocietyFactoryBase {

        #region events

        public event EventHandler<FloatEventArgs> FactoryTicked;

        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override bool CanConstructSocietyAt(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location) {
            throw new NotImplementedException();
        }

        public override void DestroySociety(SocietyBase society) {
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
