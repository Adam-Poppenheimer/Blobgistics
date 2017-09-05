using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UI.TitleScreen {

    [Serializable]
    public enum TitleScreenActiveDisplayType {
        NewGame,
        LoadSession,
        ExitGame,
        Options,
        CreditsAndAttribution,
        Controls,
        None
    }

}
