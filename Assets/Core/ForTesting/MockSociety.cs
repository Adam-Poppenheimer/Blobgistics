using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Map;
using Assets.Societies;
using UnityCustomUtilities.Extensions;

namespace Assets.Core.ForTesting {

    public class MockSociety : SocietyBase {

        #region instance fields and properties

        #region from SocietyBase

        public override int ID {
            get { return _id; }
        }
        public void SetID(int value) {
            _id = value;
        }
        private int _id;

        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool AscensionIsPermitted {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override ComplexityDefinitionBase CurrentComplexity {
            get {
                throw new NotImplementedException();
            }
        }

        public override MapNodeBase Location {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool NeedsAreSatisfied {
            get {
                throw new NotImplementedException();
            }
        }

        public override float SecondsOfUnsatisfiedNeeds {
            get {
                throw new NotImplementedException();
            }
        }

        public override float SecondsUntilComplexityDescent {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region instance methods

        #region from SocietyBase

        public override IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent() {
            throw new NotImplementedException();
        }

        public override void TickConsumption(float secondsPassed) {
            throw new NotImplementedException();
        }

        public override void TickProduction(float secondsPassed) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

    }

}
