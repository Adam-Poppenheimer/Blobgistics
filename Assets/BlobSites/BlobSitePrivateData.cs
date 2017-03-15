using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobSites {

    public class BlobSitePrivateData : BlobSitePrivateDataBase {

        #region instance fields and properties

        #region from BlobSitePrivateDataBase

        public override Vector3 NorthConnectionOffset {
            get { return _northConnectionOffset; }
        }
        public void SetNorthConnectionOffset(Vector3 value) {
            _northConnectionOffset = value;
        }
        [SerializeField] private Vector3 _northConnectionOffset = Vector3.zero;

        public override Vector3 SouthConnectionOffset {
            get { return _southConnectionOffset; }
        }
        public void SetSouthConnectionOffset(Vector3 value) {
            _southConnectionOffset = value;
        }
        [SerializeField] private Vector3 _southConnectionOffset = Vector3.zero;

        public override Vector3 EastConnectionOffset {
            get { return _eastConnectionOffset; }
        }
        public void SetEastConnectionOffset(Vector3 value) {
            _eastConnectionOffset = value;
        }
        [SerializeField] private Vector3 _eastConnectionOffset = Vector3.zero;

        public override Vector3 WestConnectionOffset {
            get { return _westConnectionOffset; }
        }
        public void SetWestConnectionOffset(Vector3 value) {
            _westConnectionOffset = value;
        }
        [SerializeField] private Vector3 _westConnectionOffset = Vector3.zero;

        #endregion

        #endregion
        
    }

}
