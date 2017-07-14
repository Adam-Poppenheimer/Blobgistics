using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using Assets.Map;
using Assets.Societies;

using UnityCustomUtilities.Extensions;

namespace Assets.Scoring.ForTesting {

    public class MockSociety : SocietyBase {

        #region instance fields and properties

        #region from SocietyBase

        public override ComplexityLadderBase ActiveComplexityLadder {
            get { return activeComplexityLadder; }
        }
        public ComplexityLadderBase activeComplexityLadder;

        public override bool AscensionIsPermitted {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override ComplexityDefinitionBase CurrentComplexity {
            get { return currentComplexity; }
        }
        public ComplexityDefinitionBase currentComplexity;

        public override int ID {
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
            get { return needsAreSatisfied; }
            protected set {
                throw new NotImplementedException();
            }
        }
        public bool needsAreSatisfied;

        public override float SecondsOfUnsatisfiedNeeds {
            get {
                throw new NotImplementedException();
            }
            set {
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

        public override bool GetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity) {
            throw new NotImplementedException();
        }

        public override void SetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity, bool isPermitted) {
            throw new NotImplementedException();
        }

        #endregion

        public void SetCurrentComplexity(ComplexityDefinitionBase newComplexity) {
            currentComplexity = newComplexity;
            RaiseCurrentComplexityChanged(newComplexity);
        }

        #endregion

    }

}
