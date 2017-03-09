using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Core;
using Assets.Map;

namespace Assets.Highways {

    public class BlobHighwayPrivateData : BlobHighwayPrivateDataBase {

        #region instance fields and properties

        #region from BlobHighwayPrivateDataBase

        public override UIControl UIControl {
            get {
                if(uiControl == null) {
                    throw new InvalidOperationException("TopLevelUIFSM is uninitialized");
                } else {
                    return uiControl;
                }
            }
        }
        [SerializeField] private UIControl uiControl;

        public override int ID {
            get { return _id; }
        }
        public void SetID(int value) {
            _id = value;
        }
        [SerializeField] private int _id;

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

        #region instance methods

        public BlobHighwayPrivateData Clone(GameObject hostingObject) {
            var newData = hostingObject.AddComponent<BlobHighwayPrivateData>();
            newData.uiControl = UIControl;
            return newData;
        }

        #endregion

    }

}
