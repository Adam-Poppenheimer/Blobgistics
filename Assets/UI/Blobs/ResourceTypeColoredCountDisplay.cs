using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Blobs {

    public class ResourceTypeColoredCountDisplay : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] public MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private Text  ResourceCountField;
        [SerializeField] private Image ResourceMaterialField;
        [SerializeField] private Text  ResourceNameField;

        [SerializeField] public bool DisplayCount;
        [SerializeField] public bool DisplayMaterial;
        [SerializeField] public bool DisplayName;

        #endregion

        #region instance methods

        public void DisplayCountOfResourceType(ResourceType type, int count) {
            if(DisplayCount) {
                if(ResourceCountField == null) {
                    Debug.LogError("Cannot display count: ResourceCountField is null");
                    return;
                }
                ResourceCountField.text = count.ToString();
                ResourceCountField.gameObject.SetActive(true);
            }else if(ResourceCountField != null) {
                ResourceCountField.gameObject.SetActive(false);
            }

            if(DisplayMaterial) {
                if(ResourceMaterialField == null) {
                    Debug.LogError("Cannot display material: ResourceMaterialField is null");
                    return;
                }
                ResourceMaterialField.material = MaterialsForResourceTypes[type];
                ResourceMaterialField.gameObject.SetActive(true);
            }else if(ResourceMaterialField != null) {
                ResourceMaterialField.gameObject.SetActive(false);
            }
            

            if(DisplayName) {
                if(ResourceNameField == null) {
                    Debug.LogError("Cannot display name: ResourceNameField is null");
                    return;
                }
                ResourceNameField.text = type.GetDescription();
                ResourceNameField.gameObject.SetActive(true);
            }else if(ResourceNameField != null) {
                ResourceNameField.gameObject.SetActive(false);
            }
        }

        #endregion

    }

}
