using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Societies {

    public interface ISocietyTicker {

        #region properties

        IEnumerable<ISociety> SocietiesTicked { get; }

        #endregion

        #region methods

        void SubscribeSociety  (ISociety society);
        void UnsubscribeSociety(ISociety society);

        void TickProduction();
        void TickConsumption();

        #endregion

    }

}
