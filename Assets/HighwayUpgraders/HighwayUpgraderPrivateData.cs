using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Highways;
using Assets.Core;

namespace Assets.HighwayUpgraders {

    public class HighwayUpgraderPrivateData : HighwayUpgraderPrivateDataBase {

        #region instance fields and properties

        #region from HighwayUpgraderPrivateDataBase

        public override BlobHighwayProfile ProfileToInsert {
            get { return _profileToInsert; }
        }
        public void SetProfileToInsert(BlobHighwayProfile value) {
            _profileToInsert = value;
        }
        [SerializeField] private BlobHighwayProfile _profileToInsert;

        public override BlobHighwayBase TargetedHighway {
            get { return _targetedHighway; }
        }
        public void SetTargetedHighway(BlobHighwayBase value) {
            _targetedHighway = value;
        }
        [SerializeField] private BlobHighwayBase _targetedHighway;

        public override BlobSiteBase UnderlyingSite {
            get { return _underlyingSite; }
        }
        public void SetUnderlyingSite(BlobSiteBase value) {
            _underlyingSite = value;
        }
        [SerializeField] private BlobSiteBase _underlyingSite;

        public override HighwayUpgraderFactoryBase SourceFactory {
            get { return _sourceFactory; }
        }
        public void SetSourceFactory(HighwayUpgraderFactoryBase value) {
            _sourceFactory = value;
        }
        [SerializeField] private HighwayUpgraderFactoryBase _sourceFactory;

        public override UIControlBase UIControl {
            get { return _uiControl; }
        }
        public void SetUIControl(UIControlBase value) {
            _uiControl = value;
        }
        [SerializeField] private UIControlBase _uiControl;

        #endregion

        #endregion

    }

}
