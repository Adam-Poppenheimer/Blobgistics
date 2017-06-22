using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Societies;

namespace Assets.Session {

    [Serializable, DataContract]
    public class SerializableSocietyData {

        #region instance fields and properties

        [DataMember()] public int LocationID;

        [DataMember()] public string ActiveComplexityLadderName;

        [DataMember()] public string CurrentComplexityName;

        [DataMember()] public float SecondsOfUnsatisfiedNeeds;

        [DataMember()] public bool AscensionIsPermitted;

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
