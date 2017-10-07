using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UI.TitleScreen {

    /// <summary>
    /// Represents all of the title screen pages that can be navigated to or made active.
    /// </summary>
    [Serializable]
    public enum TitleScreenActiveDisplayType {
        /// <summary/>
        NewGame,
        /// <summary/>
        LoadSession,
        /// <summary/>
        ExitGame,
        /// <summary/>
        Options,
        /// <summary/>
        CreditsAndAttribution,
        /// <summary/>
        Controls,
        /// <summary/>
        None
    }

}
