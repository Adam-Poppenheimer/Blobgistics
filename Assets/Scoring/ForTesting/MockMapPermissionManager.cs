using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Session;

namespace Assets.Scoring.ForTesting {

    public class MockMapPermissionManager : MapPermissionManagerBase {

        #region instance fields and properties

        public string LastMapFlaggedAsHavingBeenWon;

        #endregion

        #region instance methods

        #region MapPermissionManagerBase

        public override void ClearAllVictoryInformation() {
            throw new NotImplementedException();
        }

        public override void FlagMapAsHavingBeenWon(string mapName) {
            LastMapFlaggedAsHavingBeenWon = mapName;
        }

        public override ReadOnlyCollection<string> GetAllMapsRequiredToPlayMap(string mapName) {
            throw new NotImplementedException();
        }

        public override bool GetMapHasBeenWon(string mapName) {
            throw new NotImplementedException();
        }

        public override bool GetMapIsPermittedToBePlayed(string mapName) {
            throw new NotImplementedException();
        }

        public override ReadOnlyCollection<string> GetMapsLeftToWinRequiredToPlayMap(string mapName) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
