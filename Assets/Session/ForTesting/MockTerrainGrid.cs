using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Assets.Map;

using UnityCustomUtilities.Grids;
using UnityEngine;

namespace Assets.Session.ForTesting {

    public class MockTerrainGrid : TerrainGridBase {

        #region instance fields and properties

        #region from TerrainGridBase

        public override float MaxAcquisitionDistance { get; set; }

        public override int Radius { get; set; }

        public override Rect Bounds {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from TerrainGridBase

        public override void ClearMap() {
            
        }

        public override void CreateMap() {
            
        }

        public override void RefreshMapTerrains() {
            
        }

        #endregion

        #endregion
        
    }

}
