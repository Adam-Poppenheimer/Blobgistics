using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobDistributors;

using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockBlobDistributor : BlobDistributorBase {

        #region instance fields and properties

        #region from BlobDistributorBase

        public override float EdgePullCooldownInSeconds {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region events

        public event EventHandler<FloatEventArgs> Ticked;

        #endregion

        #region instance methods

        #region from BlobDistributorBase

        public override void Tick(float secondsPassed) {
            if(Ticked != null) {
                Ticked(this, new FloatEventArgs(secondsPassed));
            }
        }

        #endregion

        #endregion

    }
}
