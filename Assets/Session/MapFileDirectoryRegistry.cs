using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Session {

    [CreateAssetMenu(fileName = "MapRegistry", menuName = "Map Registry")]
    public class MapFileDirectoryRegistry : ScriptableObject {

        #region instance fields and properties

        public ReadOnlyCollection<string> MapNamesWithExtensions {
            get { return _mapNamesWithExtensions.AsReadOnly(); }
        }
        [SerializeField] private List<string> _mapNamesWithExtensions;

        #endregion

    }

}
