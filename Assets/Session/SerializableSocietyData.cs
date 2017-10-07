using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

using Assets.Societies;

namespace Assets.Session {

    /// <summary>
    /// A POD class for serializing information about a society.
    /// </summary>
    /// <remarks>
    /// Societies serialized in this way do not record their per-complexity ascension
    /// permissions, which is likely a bug. They are also vulnerable to changes to
    /// complexities and complexity ladders.
    /// </remarks>
    [Serializable, DataContract]
    public class SerializableSocietyData {

        #region instance fields and properties

        /// <summary>
        /// The ID of the location of the society.
        /// </summary>
        [DataMember()] public int LocationID;

        /// <summary>
        /// The name of the active complexity ladder of the society.
        /// </summary>
        [DataMember()] public string ActiveComplexityLadderName;

        /// <summary>
        /// The name of the current complexity of the society.
        /// </summary>
        [DataMember()] public string CurrentComplexityName;

        /// <summary>
        /// The seconds of unsatisfied needs in the society.
        /// </summary>
        [DataMember()] public float SecondsOfUnsatisfiedNeeds;

        /// <summary>
        /// Whether or not ascension is permitted in the society.
        /// </summary>
        [DataMember()] public bool AscensionIsPermitted;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the data from the given society.
        /// </summary>
        /// <param name="society">The society to pull data from</param>
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
