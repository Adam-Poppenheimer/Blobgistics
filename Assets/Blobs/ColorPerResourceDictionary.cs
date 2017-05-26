using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Blobs {

    public class ColorPerResourceDictionary : PerResourceDictionaryBase<Color> {

        #region instance fields and properties

        #region from PerResourceDictionaryBase<Color>

        protected override Color DefaultValue {
            get { return Color.white; }
        }

        #endregion

        #endregion

    }

}
