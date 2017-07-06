using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Assets.Blobs;
using Assets.Map;
using Assets.Societies;
using UnityEngine;

namespace Assets.Session.ForTesting {

    public class MockComplexityDefinition : ComplexityDefinitionBase {

        #region instance fields and properties

        #region from ComplexityDefinitionBase

        public override float ComplexityDescentDuration {
            get {
                throw new NotImplementedException();
            }
        }

        public override IntPerResourceDictionary CostToAscendInto {
            get {
                throw new NotImplementedException();
            }
        }

        public override Color ColorForSociety {
            get {
                throw new NotImplementedException();
            }
        }

        public override IntPerResourceDictionary Needs {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint NeedsCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        public override ReadOnlyCollection<TerrainType> PermittedTerrains {
            get {
                throw new NotImplementedException();
            }
        }

        public override IntPerResourceDictionary Production {
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

        public override IEnumerable<IntPerResourceDictionary> Wants {
            get {
                throw new NotImplementedException();
            }
        }

        public override uint WantsCapacityCoefficient {
            get {
                throw new NotImplementedException();
            }
        }

        public override Sprite SpriteForSociety {
            get {
                throw new NotImplementedException();
            }
        }

        public override Color ColorForBackground {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

    }

}
