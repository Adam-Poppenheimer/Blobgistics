using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    public abstract class MapPermissionManagerBase : MonoBehaviour {

        #region instance methods

        public abstract void FlagMapAsHavingBeenWon(string mapName);

        public abstract bool GetMapHasBeenWon(string mapName);

        public abstract void ClearAllVictoryInformation();

        public abstract bool GetMapIsPermittedToBePlayed(string mapName);

        public abstract ReadOnlyCollection<string> GetAllMapsRequiredToPlayMap(string mapName);

        public abstract ReadOnlyCollection<string> GetMapsLeftToWinRequiredToPlayMap(string mapName);

        #endregion

    }

}
