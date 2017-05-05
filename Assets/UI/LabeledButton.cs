using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI {

    public class LabeledButton : Button {

        #region instance fields and properties

        public Text Label {
            get { return _label; }
        }
        [SerializeField] private Text _label;

        #endregion

    }

}
