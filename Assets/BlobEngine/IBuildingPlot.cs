using System;
using System.Collections.ObjectModel;

namespace Assets.BlobEngine {

    public interface IBuildingPlot : IBlobTarget {

        #region properties

        Schematic                     ActiveSchematic     { get; set; }
        ReadOnlyCollection<Schematic> AvailableSchematics { get; }

        #endregion

    }

}