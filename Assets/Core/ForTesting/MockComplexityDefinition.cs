using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Map;
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

        public override IntResourceSummary CostToAscendInto {
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

        public override IntResourceSummary Needs {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint NeedsCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        public override IntResourceSummary Production {
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

        public override IEnumerable<IntResourceSummary> Wants {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint WantsCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<TerrainType> PermittedTerrains {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
