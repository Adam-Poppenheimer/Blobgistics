namespace Assets.BlobEngine {

    public interface IBuildingPlot : IBlobTarget {

        BuildingSchematic Schematic { get; set; }

    }

}