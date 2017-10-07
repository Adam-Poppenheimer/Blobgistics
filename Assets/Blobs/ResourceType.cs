using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Blobs {

    /// <summary>
    /// Defines all of the possible resource types.
    /// </summary>
    /// <remarks>
    /// Be wary when modifying this enum. PerResourceDictionaryBase relies on the precise
    /// order of these values in order to function. You can add values to the end of the
    /// enum, but if you interject new values in the middle or change the order, it's very
    /// likely that you'll break every serialized per-resource dictionary in the project.
    /// </remarks>
    public enum ResourceType {
        /// <summary/>
        [Description("Food"  )] Food,
        /// <summary/>
        [Description("Wood"  )] Wood,
        /// <summary/>
        [Description("Ore"   )] Ore,
        /// <summary/>
        [Description("Cotton")] Cotton,

        /// <summary/>
        [Description("Textiles")] Textiles,
        /// <summary/>
        [Description("Steel"   )] Steel,
        /// <summary/>
        [Description("Lumber"  )] Lumber,

        /// <summary/>
        [Description("Service Goods"  )] ServiceGoods,
        /// <summary/>
        [Description("Heavy Machinery")] HeavyMachinery,

        /// <summary/>
        [Description("Hi-Tech Goods")] HiTechGoods
    }

}
