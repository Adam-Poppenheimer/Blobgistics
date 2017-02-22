using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Societies.Editor {

    public class MockSocietyPrivateData : ISocietyPrivateData {

        #region instance fields and properties

        #region from ISocietyPrivateData

        public IComplexityLadder ActiveComplexityLadder { get; set; }
        public IComplexityDefinition StartingComplexity { get; set; }

        #endregion

        #endregion
        
    }

}
