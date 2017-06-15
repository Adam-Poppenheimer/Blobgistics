using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    public class MapPermissionManager : MonoBehaviour {

        #region internal types

        [Serializable]
        private class MapPermissionData {

            #region instance fields and properties

            public string MapName {
                get { return _mapName; }
            }
            [SerializeField] private string _mapName;

            public ReadOnlyCollection<string> MapNamesRequiredToPlay {
                get { return _mapNamesRequiredToPlay.AsReadOnly(); }
            }
            [SerializeField] private List<string> _mapNamesRequiredToPlay;

            #endregion

        }

        #endregion

        #region instance fields and properties
        
        [SerializeField] private FileSystemLiaison FileSystemLiaison;

        [SerializeField] private List<MapPermissionData> MapPermissions;

        private List<string> LastCalculatedVictoryData;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            LastCalculatedVictoryData = FileSystemLiaison.ReadVictoryDataFromFile();
        }

        #endregion

        public void FlagMapAsHavingBeenWon(string mapName) {
            if(!LastCalculatedVictoryData.Contains(mapName)) {
                LastCalculatedVictoryData.Add(mapName);
                FileSystemLiaison.WriteVictoryDataToFile(LastCalculatedVictoryData);
            }
        }

        public bool GetMapHasBeenWon(string mapName) {
            return LastCalculatedVictoryData.Contains(mapName);
        }

        public void ClearAllVictoryInformation() {
            LastCalculatedVictoryData.Clear();
            FileSystemLiaison.WriteVictoryDataToFile(LastCalculatedVictoryData);
        }

        public bool GetMapIsPermittedToBePlayed(string mapName) {
            var permissionsForMap = MapPermissions.Where(permissions => permissions.MapName.Equals(mapName)).FirstOrDefault();
            if(permissionsForMap != null) {
                foreach(var mapNameRequired in permissionsForMap.MapNamesRequiredToPlay) {
                    if(!LastCalculatedVictoryData.Contains(mapNameRequired)) {
                        return false;
                    }
                }
                return true;
            }else {
                return true;
            }
        }

        public ReadOnlyCollection<string> GetAllMapsRequiredToPlayMap(string mapName) {
            var permissionsForMap = MapPermissions.Where(permissions => permissions.MapName.Equals(mapName)).FirstOrDefault();
            if(permissionsForMap != null) {
                return permissionsForMap.MapNamesRequiredToPlay;
            }else {
                return new List<string>().AsReadOnly();
            }
        }

        public ReadOnlyCollection<string> GetMapsLeftToWinRequiredToPlayMap(string mapName) {
            var retval = new List<string>();

            var permissionsForMap = MapPermissions.Where(permissions => permissions.MapName.Equals(mapName)).FirstOrDefault();
            if(permissionsForMap != null) {
                retval.AddRange(permissionsForMap.MapNamesRequiredToPlay.Where(requiredMap => !GetMapHasBeenWon(mapName)));
            }
            return retval.AsReadOnly();
        }

        #endregion

    }

}
