using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Blobs {

    public class BoolPerResourceDictionary : PerResourceDictionaryBase<bool> {

        #region instance fields and properties

        #region from PerResourceDictionaryBase<bool>

        protected override bool DefaultValue {
            get { return false; }
        }

        #endregion

        #endregion
        
    }

}
