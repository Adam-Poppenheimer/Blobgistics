using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;

using UnityCustomUtilities.Extensions;

namespace Assets.UI.Blobs {

    /// <summary>
    /// A simple class used by ResourceDisplay to display a single resource/color/count triple.
    /// </summary>
    public class ResourceTypeColoredCountDisplay : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The materials of each resource type.
        /// </summary>
        [SerializeField] public MaterialPerResourceDictionary MaterialsForResourceTypes;

        [SerializeField] private Text  ResourceCountField;
        [SerializeField] private Image ResourceMaterialField;
        [SerializeField] private Text  ResourceNameField;

        /// <summary>
        /// Whether or not to display the count.
        /// </summary>
        [SerializeField] public bool WillDisplayCount;

        /// <summary>
        /// Whether or not to display the material.
        /// </summary>
        [SerializeField] public bool WillDisplayMaterial;

        /// <summary>
        /// Whether or not to display the name.
        /// </summary>
        [SerializeField] public bool WillDisplayName;

        #endregion

        #region instance methods

        /// <summary>
        /// Displays the given ResourceType with the given count.
        /// </summary>
        /// <param name="type">The ResourceType to display</param>
        /// <param name="count">The count of that ResourceType to display</param>
        /// <remarks>
        /// Note that this method does not throw exceptions, instead opting to log errors
        /// and move on. This follows general trend within UI code to not throw exceptions,
        /// as UI errors were perceived to be less severe than simulation ones. It's not
        /// clear that this was an intelligent policy.
        /// </remarks>
        public void DisplayCountOfResourceType(ResourceType type, int count) {
            if(WillDisplayCount) {
                if(ResourceCountField == null) {
                    Debug.LogError("Cannot display count: ResourceCountField is null");
                    return;
                }
                ResourceCountField.text = count.ToString();
                ResourceCountField.gameObject.SetActive(true);
            }else if(ResourceCountField != null) {
                ResourceCountField.gameObject.SetActive(false);
            }

            if(WillDisplayMaterial) {
                if(ResourceMaterialField == null) {
                    Debug.LogError("Cannot display material: ResourceMaterialField is null");
                    return;
                }
                ResourceMaterialField.material = MaterialsForResourceTypes[type];
                ResourceMaterialField.gameObject.SetActive(true);
            }else if(ResourceMaterialField != null) {
                ResourceMaterialField.gameObject.SetActive(false);
            }
            

            if(WillDisplayName) {
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
