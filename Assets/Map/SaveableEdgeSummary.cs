using System;

namespace Assets.Map {

    [Serializable]
    public class SaveableEdgeSummary {

        #region instance fields and properties

        public int ID;

        public int FirstNodeID;
        public int SecondNodeID;

        #endregion

        #region constructors

        public SaveableEdgeSummary(int id, int firstNodeID, int secondNodeID) {
            ID = id;
            FirstNodeID = firstNodeID;
            SecondNodeID = secondNodeID;
        }

        #endregion

    }

}