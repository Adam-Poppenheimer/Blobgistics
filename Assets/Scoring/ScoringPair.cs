using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Societies;

namespace Assets.Scoring {

    [Serializable]
    public struct ScoringPair {

        #region instance fields and properties

        public ComplexityDefinitionBase Complexity {
            get { return _complexity; }
        }
        [SerializeField] private ComplexityDefinitionBase _complexity;

        public int Score {
            get { return _score; }
        }
        [SerializeField] private int _score;

        #endregion

        #region constructors

        public ScoringPair(ComplexityDefinitionBase complexity, int score) {
            _complexity = complexity;
            _score = score;
        }

        #endregion

    }

}
