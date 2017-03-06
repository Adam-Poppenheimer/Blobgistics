using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Core;
using Assets.BlobSites;

namespace Assets.Highways {

    public class BlobHighwayPrivateData : BlobHighwayPrivateDataBase {

        #region instance fields and properties

        #region from BlobHighwayPrivateDataBase

        public override BlobTubeFactoryBase TubeFactory {
            get {
                if(tubeFactory == null) {
                    throw new InvalidOperationException("TubeFactory is uninitialized");
                } else {
                    return tubeFactory;
                }
            }
        }
        [SerializeField] private BlobTubeFactoryBase tubeFactory;

        public override UIControl UIControl {
            get {
                if(uiControl == null) {
                    throw new InvalidOperationException("TopLevelUIFSM is uninitialized");
                } else {
                    return uiControl;
                }
            }
        }

        public int ID { get; internal set; }
        public BlobSiteBase FirstEndpoint { get; internal set; }
        public BlobSiteBase SecondEndpoint { get; internal set; }

        [SerializeField] private UIControl uiControl;

        #endregion

        #endregion

        #region instance methods

        public BlobHighwayPrivateData Clone(GameObject hostingObject) {
            var newData = hostingObject.AddComponent<BlobHighwayPrivateData>();
            newData.tubeFactory = TubeFactory;
            newData.uiControl = UIControl;
            return newData;
        }

        #endregion

    }

}
