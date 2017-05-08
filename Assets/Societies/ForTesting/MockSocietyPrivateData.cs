using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

namespace Assets.Societies.ForTesting {

    public class MockSocietyPrivateData : SocietyPrivateDataBase {

        #region instance fields and properties

        #region from ISocietyPrivateData

        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                if(_activeComplexityLadder == null) {
                    var hostingObject = new GameObject();
                    _activeComplexityLadder = hostingObject.AddComponent<MockComplexityLadder>();
                }
                return _activeComplexityLadder;
            }
        }
        public void SetActiveComplexityLadder(ComplexityLadderBase value) {
            _activeComplexityLadder = value;
        }
        private ComplexityLadderBase _activeComplexityLadder;

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        private ResourceBlobFactoryBase _blobFactory;

        public override MapNodeBase Location {
            get { return _location; }
        }
        public void SetLocation(MapNodeBase value) {
            _location = value;
        }
        private MapNodeBase _location;

        public override UIControlBase UIControl {
            get { return null; }
        }

        public override SocietyFactoryBase ParentFactory {
            get { return _parentFactory; }
        }
        public void SetParentFactory(SocietyFactoryBase value) {
            _parentFactory = value;
        }
        private SocietyFactoryBase _parentFactory;

        #endregion

        #endregion

    }

}
