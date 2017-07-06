using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Map;
using Assets.Societies;
using UnityCustomUtilities.Extensions;

namespace Assets.Session.ForTesting {

    public class MockSociety : SocietyBase {

        #region instance fields and properties

        #region from SocietyBase

        public override ComplexityLadderBase ActiveComplexityLadder {
            get { return activeComplexityLadder; }
        }
        public ComplexityLadderBase activeComplexityLadder;

        public override bool AscensionIsPermitted { get; set; }

        public override ComplexityDefinitionBase CurrentComplexity {
            get { return currentComplexity; }
        }
        public ComplexityDefinitionBase currentComplexity;

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override MapNodeBase Location {
            get { return location; }
        }
        public MapNodeBase location;

        public override bool NeedsAreSatisfied {
            get {
                throw new NotImplementedException();
            }
        }

        public override float SecondsOfUnsatisfiedNeeds { get; set; }

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

        #endregion

    }

}
