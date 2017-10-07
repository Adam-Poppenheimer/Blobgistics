using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blobs {

    /// <summary>
    /// A PerResourceDictionary that accepts colors.
    /// </summary>
    /// <remarks>
    /// Since Unity doesn't permit generic MonoBehaviours, it's necessary to create separate
    /// classes for every desirable PerResourceDictionary type. This might not hold true if
    /// PerResourceDictionaryBase was a ScriptableObject.
    /// </remarks>
    public class ColorPerResourceDictionary : PerResourceDictionaryBase<Color> {

        #region instance fields and properties

        #region from PerResourceDictionaryBase<Color>

        /// <inheritdoc/>
        protected override Color DefaultValue {
            get { return Color.white; }
        }

        #endregion

        #endregion

    }

}
