using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    /// <summary>
    /// The abstract base class for all map permission managers, which determine
    /// what order maps must be cleared in and whether or not a given map can
    /// be played.
    /// </summary>
    public abstract class MapPermissionManagerBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Record the given map name as having been won on this system.
        /// </summary>
        /// <param name="mapName">The map name being flagged</param>
        public abstract void FlagMapAsHavingBeenWon(string mapName);

        /// <summary>
        /// Gets whether the map of the given name has been won on this system.
        /// </summary>
        /// <param name="mapName">The name to consider</param>
        /// <returns></returns>
        public abstract bool GetMapHasBeenWon(string mapName);

        /// <summary>
        /// Clears all information regarding which maps have been won.
        /// </summary>
        public abstract void ClearAllVictoryInformation();

        /// <summary>
        /// Gets whether or not the map of the given name is allowed to be played.
        /// </summary>
        /// <param name="mapName">The name to consider</param>
        /// <returns>Whether a map of this name can be played</returns>
        public abstract bool GetMapIsPermittedToBePlayed(string mapName);

        /// <summary>
        /// Returns the names of all maps that must be beaten in order to play
        /// the map of the given name.
        /// </summary>
        /// <param name="mapName">The name to consider</param>
        /// <returns>The name of every map that must be beaten before the map of the given name can be played</returns>
        public abstract ReadOnlyCollection<string> GetAllMapsRequiredToPlayMap(string mapName);

        /// <summary>
        /// Returns the names of all maps that must be beaten and have not yet
        /// been beaten in order to play the map of the given name.
        /// </summary>
        /// <param name="mapName">The name to consider</param>
        /// <returns>All maps the player still needs to beat to play the map of the given name</returns>
        public abstract ReadOnlyCollection<string> GetMapsLeftToWinRequiredToPlayMap(string mapName);

        #endregion

    }

}
