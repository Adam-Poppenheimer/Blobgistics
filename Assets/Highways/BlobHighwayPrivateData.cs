using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Core;
using Assets.Map;

namespace Assets.Highways {

    public class BlobHighwayPrivateData : BlobHighwayPrivateDataBase {

        #region instance fields and properties

        #region from BlobHighwayPrivateDataBase

        public override UIControlBase UIControl {
            get { return _uiControl; }
        }
        public void SetUIControl(UIControlBase value) {
            _uiControl = value;
        }
        [SerializeField] private UIControlBase _uiControl;

        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public override MapNodeBase FirstEndpoint {
            get { return _firstEndpoint; }
        }
        public void SetFirstEndpoint(MapNodeBase value) {
            _firstEndpoint = value;
        }
        [SerializeField, HideInInspector] private MapNodeBase _firstEndpoint;

        public override MapNodeBase SecondEndpoint {
            get { return _secondEndpoint; }
        }
        public void SetSecondEndpoint(MapNodeBase value) {
            _secondEndpoint = value;
        }
        [SerializeField, HideInInspector] private MapNodeBase _secondEndpoint;
        
        public override BlobTubeBase TubePullingFromFirstEndpoint {
            get { return _tubePullingFromFirstEndpoint; }
        }
        public void SetTubePullingFromFirstEndpoint(BlobTubeBase value) {
            _tubePullingFromFirstEndpoint = value;
        }
        [SerializeField, HideInInspector] private BlobTubeBase _tubePullingFromFirstEndpoint;

        public override BlobTubeBase TubePullingFromSecondEndpoint {
            get { return _tubePullingFromSecondEndpoint; }
        }
        public void SetTubePullingFromSecondEndpoint(BlobTubeBase value) {
            _tubePullingFromSecondEndpoint = value;
        }
        [SerializeField, HideInInspector] private BlobTubeBase _tubePullingFromSecondEndpoint;

        #endregion

        #endregion

    }

}
