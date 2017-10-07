using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Assets.Blobs;

namespace Assets.Blobs {

    /// <summary>
    /// A PerResourceDictionary that accepts Toggles.
    /// </summary>
    /// <remarks>
    /// Since Unity doesn't permit generic MonoBehaviours, it's necessary to create separate
    /// classes for every desirable PerResourceDictionary type. This might not hold true if
    /// PerResourceDictionaryBase was a ScriptableObject.
    /// </remarks>
    public class TogglePerResourceDictionary : PerResourceDictionaryBase<Toggle> {

        #region instance fields and properties

        #region from PerResourceDictionaryBase<Toggle>

        /// <inheritdoc/>
        protected override Toggle DefaultValue {
            get { return null; }
        }

        #endregion

        #endregion
        
    }

}
