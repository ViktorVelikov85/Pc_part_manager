using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pc_part_manager.Models.PcParts
{
    public class Ssd : ComputerPart
    {
        public int CapacityGb { get; set; } // напр. 1000 (за 1TB)
        public string FormFactor { get; set; } // напр. M.2 NVMe, 2.5" SATA
        public int ReadSpeedMb { get; set; } // Скорост на четене

        public override string GetSpecifications()
        {
            return $"[SSD] {Manufacturer} {Name} | Capacity: {CapacityGb}GB | Form: {FormFactor} | Read: {ReadSpeedMb}MB/s | Price: {Price} lv. | Qty: {Quantity}";
        }
    }
}
