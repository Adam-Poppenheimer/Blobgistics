using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Societies;

namespace Assets.Session {

    [Serializable]
    public class SerializableSocietyData {

        #region instance fields and properties

        public int LocationID;

        public string ActiveComplexityLadderName;

        public string CurrentComplexityName;

        public float SecondsOfUnsatisfiedNeeds;

        public bool AscensionIsPermitted;

        #endregion

        #region constructors

        public SerializableSocietyData(SocietyBase society) {
            LocationID = society.Location.ID;
            ActiveComplexityLadderName = society.ActiveComplexityLadder.name;
            CurrentComplexityName = society.CurrentComplexity.name;
            SecondsOfUnsatisfiedNeeds = society.SecondsOfUnsatisfiedNeeds;
            AscensionIsPermitted = society.AscensionIsPermitted;
        }

        #endregion

    }

}
