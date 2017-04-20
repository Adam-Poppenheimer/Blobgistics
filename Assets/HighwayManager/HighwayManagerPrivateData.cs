using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Core;

namespace Assets.HighwayManager {

    public class HighwayManagerPrivateData : HighwayManagerPrivateDataBase {

        #region instance fields and properties

        #region from HighwayManagerPrivateDataBase



        #endregion

        #endregion
        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public override float EfficiencyGainFromFood {
            get { return _efficiencyGainFromFood; }
        }
        public void SetEfficiencyGainFromFood(float value) {
            _efficiencyGainFromFood = value;
        }
        [SerializeField] private float _efficiencyGainFromFood = 1f;

        public override float EfficiencyGainFromYellow {
            get { return _efficiencyGainFromYellow; }
        }
        public void SetEfficiencyGainFromYellow(float value) {
            _efficiencyGainFromYellow = value;
        }
        [SerializeField] private float _efficiencyGainFromYellow;

        public override float EfficiencyGainFromWhite {
            get { return _efficiencyGainFromWhite; }
        }
        public void SetEfficiencyGainFromWhite(float value) {
            _efficiencyGainFromWhite = value;
        }
        [SerializeField] private float _efficiencyGainFromWhite = 1f;

        public override float EfficiencyGainFromBlue {
            get { return _efficiencyGainFromBlue; }
        }
        public void SetEfficiencyGainFromBlue(float value) {
            _efficiencyGainFromBlue = value;
        }
        [SerializeField] private float _efficiencyGainFromBlue = 1f;

        public override int NeedStockpileCoefficient {
            get { return _needStockpileCoefficient; }
        }
        public void SetNeedStockpileCoefficient(int value) {
            _needStockpileCoefficient = value;
        }
        [SerializeField] private int _needStockpileCoefficient = 2;

        public override HighwayManagerFactoryBase ParentFactory {
            get { return _parentFactory; }
        }
        public void SetParentFactory(HighwayManagerFactoryBase value) {
            _parentFactory = value;
        }
        [SerializeField] private HighwayManagerFactoryBase _parentFactory;

        public override float SecondsToPerformConsumption {
            get { return _secondsToPerformConsumption; }
        }
        public void SetSecondsToPerformConsumption(float value) {
            _secondsToPerformConsumption = value;
        }
        [SerializeField] private float _secondsToPerformConsumption = 1f;

        public override UIControlBase UIControl {
            get { return _uiControl; }
        }
        public void SetUIControl(UIControlBase value) {
            _uiControl = value;
        }
        [SerializeField] private UIControlBase _uiControl;
    }

}
