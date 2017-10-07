using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Blobs {

    /// <summary>
    /// A PerResourceDictionary that accepts bools.
    /// </summary>
    /// <remarks>
    /// Since Unity doesn't permit generic MonoBehaviours, it's necessary to create separate
    /// classes for every desirable PerResourceDictionary type. This might not hold true if
    /// PerResourceDictionaryBase was a ScriptableObject.
    /// </remarks>
    public class BoolPerResourceDictionary : PerResourceDictionaryBase<bool> {

        #region instance fields and properties

        #region from PerResourceDictionaryBase<bool>

        /// <inheritdoc/>
        protected override bool DefaultValue {
            get { return false; }
        }

        #endregion

        #endregion
        
    }

}
