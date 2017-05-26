using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Map;

namespace Assets.UI.TitleScreen {

    public class NewGameDisplay : MonoBehaviour {

        #region instance fields and properties

        private MapAsset SelectedMap;

        [SerializeField] private Text MapNameField;
        [SerializeField] private Text MapDescriptionField;

        [SerializeField] private List<MapAsset> AvailableMaps;
        [SerializeField] private RectTransform  AvailableMapsSection;
        [SerializeField] private GameObject     MapSummaryPrefab;

        [SerializeField] private Button StartGameButton;
        [SerializeField] private Button BackButton;

        [SerializeField] private MapGraphBase MapGraph;

        #endregion

        #region events

        public event EventHandler<EventArgs> MapLoaded;
        public event EventHandler<EventArgs> DeactivationRequested;

        protected void RaiseMapLoaded() {
            if(MapLoaded != null) {
                MapLoaded(this, EventArgs.Empty);
            }
        }

        protected void RaiseDeactivationRequested() {
            if(DeactivationRequested != null) { }
            DeactivationRequested(this, EventArgs.Empty);
        }

        #endregion

        #region instance methods

        #region Unity message methods

        private void Start() {
            foreach(var map in AvailableMaps) {
                var newSummary = Instantiate(MapSummaryPrefab).GetComponent<MapAssetSummary>();
                newSummary.transform.SetParent(AvailableMapsSection);
                newSummary.LoadMap(map);
                newSummary.gameObject.SetActive(true);
                var cachedCurrentMap = map;
                newSummary.GetComponent<Button>().onClick.AddListener(delegate() { SetSelectedMap(cachedCurrentMap); });
            }

            BackButton.onClick.AddListener(delegate() { RaiseDeactivationRequested(); });
        }

        #endregion

        private void SetSelectedMap(MapAsset newMap) {
            if(newMap != null) {
                SelectedMap = newMap;
                MapNameField.text = SelectedMap.name;
                MapNameField.gameObject.SetActive(true);
                MapDescriptionField.text = "No Description";
                MapDescriptionField.gameObject.SetActive(true);
                StartGameButton.interactable = true;
            }else {
                SelectedMap = null;
                MapNameField.text = "";
                MapNameField.gameObject.SetActive(false);
                MapDescriptionField.text = "";
                MapDescriptionField.gameObject.SetActive(false);
                StartGameButton.interactable = false;
            }
        }

        public void StartGameWithSelectedMap() {
            if(SelectedMap != null) {
                MapGraph.LoadFromMapAsset(SelectedMap);
                RaiseMapLoaded();
            }
        }

        #endregion

    }

}
