using System;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

namespace Assets.Societies {

    public abstract class SocietyPrivateDataBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ComplexityLadderBase ActiveComplexityLadder { get; }

        public abstract ResourceBlobFactoryBase BlobFactory { get; }

        public abstract MapNodeBase Location { get; }

        public abstract UIControlBase UIControl { get; }

        #endregion

    }

}