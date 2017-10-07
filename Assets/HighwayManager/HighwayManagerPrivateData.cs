using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Core;

namespace Assets.HighwayManager {

    /// <summary>
    /// The standard implementation of HighwayManagerPrivateDataBase. Contains dependency
    /// and configuration data for highway manager.
    /// </summary>
    public class HighwayManagerPrivateData : HighwayManagerPrivateDataBase {

        #region instance fields and properties

        #region from HighwayManagerPrivateDataBase

        /// <inheritdoc/>
        public override ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
        }
        /// <summary>
        /// The externalized Set method for BlobFactory.
        /// </summary>
        /// <param name="value">The new value of BlobFactory</param>
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            _blobFactory = value;
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        /// <inheritdoc/>
        public override int NeedStockpileCoefficient {
            get { return _needStockpileCoefficient; }
        }
        /// <summary>
        /// The externalized Set method for NeedStockpileCoefficient.
        /// </summary>
        /// <param name="value">The new value of NeedStockpileCoefficient</param>
        public void SetNeedStockpileCoefficient(int value) {
            _needStockpileCoefficient = value;
        }
        [SerializeField] private int _needStockpileCoefficient = 2;

        /// <inheritdoc/>
        public override HighwayManagerFactoryBase ParentFactory {
            get { return _parentFactory; }
        }
        /// <summary>
        /// The externalized Set method for ParentFactory.
        /// </summary>
        /// <param name="value">The new value of ParentFactory</param>
        public void SetParentFactory(HighwayManagerFactoryBase value) {
            _parentFactory = value;
        }
        [SerializeField] private HighwayManagerFactoryBase _parentFactory;

        /// <inheritdoc/>
        public override float SecondsToPerformConsumption {
            get { return _secondsToPerformConsumption; }
        }
        /// <summary>
        /// The externalized Set method for SecondsToPerformConsumption.
        /// </summary>
        /// <param name="value">The new value of SecondsToPerformConsumption</param>
        public void SetSecondsToPerformConsumption(float value) {
            _secondsToPerformConsumption = value;
        }
        [SerializeField] private float _secondsToPerformConsumption = 1f;

        /// <inheritdoc/>
        public override UIControlBase UIControl {
            get { return _uiControl; }
        }
        /// <summary>
        /// The externalized Set method for UIControl.
        /// </summary>
        /// <param name="value">The new value of UIControl</param>
        public void SetUIControl(UIControlBase value) {
            _uiControl = value;
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <inheritdoc/>
        public override IntPerResourceDictionary EfficiencyGainFromResource {
            get { return _efficiencyGainFromResource; }
        }
        /// <summary>
        /// The externalized Set method for SetEfficiencyGainFromResource.
        /// </summary>
        /// <param name="value">The new value of SetEfficiencyGainFromResource</param>
        public void SetEfficiencyGainFromResource(IntPerResourceDictionary value) {
            _efficiencyGainFromResource = value;
        }
        [SerializeField] private IntPerResourceDictionary _efficiencyGainFromResource;

        #endregion

        #endregion

    }

}
