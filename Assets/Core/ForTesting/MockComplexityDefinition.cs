using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Societies;
using UnityEngine;

namespace Assets.Core.ForTesting {

    public class MockComplexityDefinition : ComplexityDefinitionBase {

        #region instance fields and properties

        #region from ComplexityDefinitionBase

        public override float ComplexityDescentDuration {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceSummary CostOfAscent {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool IsPermittedToAscend {
            get {
                throw new NotImplementedException();
            }
        }

        public override Material MaterialForSociety {
            get {
                throw new NotImplementedException();
            }
        }

        public override string Name {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceSummary Needs {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint NeedsCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        public override ResourceSummary Production {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint ProductionCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        public override float SecondsToFullyConsumeNeeds {
            get {
                throw new NotImplementedException();
            }
        }

        public override float SecondsToPerformFullProduction {
            get {
                throw new NotImplementedException();
            }
        }

        public override IEnumerable<ResourceSummary> Wants {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint WantsCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion
        
    }

}
