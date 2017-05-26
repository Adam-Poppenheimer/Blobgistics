using System;

using UnityEngine;
using UnityEngine.UI;

using Assets.Map;

namespace Assets.UI.TitleScreen {

    public class MapAssetSummary : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private Text MapNameField;

        #endregion

        #region instance methods

        public void LoadMap(MapAsset map) {
            MapNameField.text = map.name;
        }

        #endregion

    }

}