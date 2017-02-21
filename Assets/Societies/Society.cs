using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;
using UnityCustomUtilities.Extensions;
using UnityEngine;

namespace Assets.Societies {

    public class Society : BlobSiteBase, ISociety {

        #region instance fields and properties

        #region from BlobSiteBase

        public override bool AcceptsExtraction {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool AcceptsPlacement {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 EastTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 NorthTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 SouthTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 WestTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        protected override BlobPileBase BlobsWithin {
            get {
                throw new NotImplementedException();
            }
        }

        protected override BlobPileBase BlobsWithReservedPositions {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region from ISociety

        public IComplexityLadder ActiveComplexityLadder {
            get {
                throw new NotImplementedException();
            }
        }

        public IComplexityDefinition CurrentComplexity {
            get {
                throw new NotImplementedException();
            }
        }

        public bool NeedsAreSatisfied {
            get {
                throw new NotImplementedException();
            }
        }

        public float SecondsUntilComplexityDescent {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        public ISocietyPrivateData PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                }
            }
        }
        private ISocietyPrivateData _privateData;

        #endregion

        #region instance methods

        #region from ISociety

        public void AscendComplexityLadder() {
            throw new NotImplementedException();
        }

        public void DescendComplexityLadder() {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent() {
            throw new NotImplementedException();
        }

        public void TickConsumption(float timeElapsed) {
            throw new NotImplementedException();
        }

        public void TickProduction(float timeElapsed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
