using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pc_part_manager.Models.PcParts
{
    public class Ram : ComputerPart
    {
        public int CapacityGb { get; set; }
        public int SpeedMhz { get; set; } // напр. 3200, 5200
        public string Generation { get; set; } // напр. DDR4, DDR5

        public override string GetSpecifications()
        {
            return $"[RAM] {Manufacturer} {Name} | {CapacityGb}GB {Generation} @ {SpeedMhz}MHz | Price: {Price} lv. | Qty: {Quantity}";
        }
    }
}
