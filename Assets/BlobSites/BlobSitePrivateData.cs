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
        [SerializeField] private Vector3 _northConnectionOffset;

        public override Vector3 SouthConnectionOffset {
            get { return _southConnectionOffset; }
        }
        [SerializeField] private Vector3 _southConnectionOffset;

        public override Vector3 EastConnectionOffset {
            get { return _eastConnectionOffset; }
        }
        [SerializeField] private Vector3 _eastConnectionOffset;

        public override Vector3 WestConnectionOffset {
            get { return _westConnectionOffset; }
        }
        [SerializeField] private Vector3 _westConnectionOffset;

        #endregion

        #endregion
        
    }

}
