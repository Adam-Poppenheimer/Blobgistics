using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

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
        public void SetActiveComplexityLadder(ComplexityLadderBase value) {
            if(value == null) {
                throw new ArgumentNullException("value");
            }else {
                _activeComplexityLadder = value;
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
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            if(value == null) {
                throw new ArgumentNullException("value");
            }else {
                _blobFactory = value;
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
        public void SetLocation(MapNodeBase value) {
            if(value == null) {
                throw new ArgumentNullException("value");
            }else {
                _location = value;
            }
        }
        [SerializeField] private MapNodeBase _location;

        public override UIControlBase UIControl {
            get { return _uiControl; }
        }
        public void SetUIControl(UIControlBase value) {
            _uiControl = value;
        }
        [SerializeField] private UIControlBase _uiControl;

        public override SocietyFactoryBase ParentFactory {
            get { return _parentFactory; }
        }
        public void SetParentFactory(SocietyFactoryBase value) {
            _parentFactory = value;
        }
        [SerializeField] private SocietyFactoryBase _parentFactory;

        #endregion

        #endregion

    }

}
