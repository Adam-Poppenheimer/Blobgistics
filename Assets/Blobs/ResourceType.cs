using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Blobs {

    public enum ResourceType {
        [Description("Food"  )] Food,
        [Description("Wood"  )] Wood,
        [Description("Ore"   )] Ore,
        [Description("Cotton")] Cotton,

        [Description("Textiles")] Textiles,
        [Description("Steel"   )] Steel,
        [Description("Lumber"  )] Lumber,

        [Description("Service Goods"  )] ServiceGoods,
        [Description("Heavy Machinery")] HeavyMachinery,

        [Description("Hi-Tech Goods")] HiTechGoods
    }

}
