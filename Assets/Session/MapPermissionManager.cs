using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    /// <summary>
    /// The standard implementation of MapPermissionManagerBase. This class determines
    /// what order maps must be cleared in and whether or not a given map can be played.
    /// </summary>
    /// <remarks>
    /// This class uses FileSystemLiaison to persist victory data across sessions. There
    /// is not currently a player-accessible way of clearing such data, though there exist
    /// methods for doing so.
    /// </remarks>
    public class MapPermissionManager : MapPermissionManagerBase {

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

        [SerializeField] private bool IgnorePermissions;

        [SerializeField] private List<MapPermissionData> MapPermissions;

        private List<string> LastCalculatedVictoryData;

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            LastCalculatedVictoryData = FileSystemLiaison.ReadVictoryDataFromFile();
        }

        #endregion

        /// <inheritdoc/>
        public override void FlagMapAsHavingBeenWon(string mapName) {
            if(!LastCalculatedVictoryData.Contains(mapName)) {
                LastCalculatedVictoryData.Add(mapName);
                FileSystemLiaison.WriteVictoryDataToFile(LastCalculatedVictoryData);
            }
        }

        /// <inheritdoc/>
        public override bool GetMapHasBeenWon(string mapName) {
            return LastCalculatedVictoryData.Contains(mapName);
        }

        /// <inheritdoc/>
        public override void ClearAllVictoryInformation() {
            LastCalculatedVictoryData.Clear();
            FileSystemLiaison.WriteVictoryDataToFile(LastCalculatedVictoryData);
        }

        /// <inheritdoc/>
        public override bool GetMapIsPermittedToBePlayed(string mapName) {
            if(IgnorePermissions) {
                return true;
            }
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

        /// <inheritdoc/>
        public override ReadOnlyCollection<string> GetAllMapsRequiredToPlayMap(string mapName) {
            var permissionsForMap = MapPermissions.Where(permissions => permissions.MapName.Equals(mapName)).FirstOrDefault();
            if(permissionsForMap != null) {
                return permissionsForMap.MapNamesRequiredToPlay;
            }else {
                return new List<string>().AsReadOnly();
            }
        }

        /// <inheritdoc/>
        public override ReadOnlyCollection<string> GetMapsLeftToWinRequiredToPlayMap(string mapName) {
            var retval = new List<string>();

            if(IgnorePermissions) {
                return retval.AsReadOnly();
            }
            var permissionsForMap = MapPermissions.Where(permissions => permissions.MapName.Equals(mapName)).FirstOrDefault();
            if(permissionsForMap != null) {
                retval.AddRange(permissionsForMap.MapNamesRequiredToPlay.Where(requiredMap => !GetMapHasBeenWon(mapName)));
            }
            return retval.AsReadOnly();
        }

        #endregion

    }

}
