using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Map;

namespace Assets.Societies {

    public class SocietyFactory : SocietyFactoryBase {

        #region instance fields and properties



        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override SocietyBase GetSocietyOfID(int id) {
            throw new NotImplementedException();
        }

        public override bool CanConstructSocietyAt(MapNode location) {
            throw new NotImplementedException();
        }

        public override SocietyBase ConstructSocietyAt(MapNode location) {
            throw new NotImplementedException();
        }

        public override void DestroySociety(SocietyBase society) {
            throw new NotImplementedException();
        }

        public override void TickSocieties(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
