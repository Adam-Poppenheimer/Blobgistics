using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

namespace Assets.Societies {

    public class SocietyPrivateData : SocietyPrivateDataBase {

        #region instance fields and properties

        #region from SocietyPrivateDataBase

        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                if(_activeComplexityLadder == null) {
                    throw new InvalidOperationException("ActiveComplexityLadder is uninitialized");
                } else {
                    return _activeComplexityLadder;
                }
            }
        }
        [SerializeField] private ComplexityLadderBase _activeComplexityLadder;

        public override ResourceBlobFactoryBase BlobFactory {
            get {
                if(_blobFactory == null) {
                    throw new InvalidOperationException("BlobFactory is uninitialized");
                } else {
                    return _blobFactory;
                }
            }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public override MapNodeBase Location {
            get {
                if(_location == null) {
                    throw new InvalidOperationException("Location is uninitialized");
                } else {
                    return _location;
                }
            }
        }
        [SerializeField] private MapNodeBase _location;

        #endregion

        #endregion

    }

}
