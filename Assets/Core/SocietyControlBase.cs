using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

namespace Assets.Core {

    public abstract class SocietyControlBase : MonoBehaviour {

        #region instance methods

        public abstract void SetAscensionPermissionForSociety(int societyID, bool ascensionPermitted);

        public abstract void DestroySociety   (int societyID);

        #endregion

    }

}
