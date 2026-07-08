using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pc_part_manager.Models.PcParts
{
    public class Gpu : ComputerPart
    {
        public int VramSizeGb { get; set; }
        public string MemoryType { get; set; } // напр. GDDR6

        public override string GetSpecifications()
        {
            return $"[GPU] {Manufacturer} {Name} | VRAM: {VramSizeGb}GB {MemoryType} | Price: {Price} lv. | Qty: {Quantity}";
        }
    }
}
