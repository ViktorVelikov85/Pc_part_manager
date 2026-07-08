using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pc_part_manager.Models.PcParts
{
    public class Motherboard : ComputerPart
    {
        public string Chipset { get; set; } // напр. B650, Z790
        public string FormFactor { get; set; } // напр. ATX, Micro-ATX
        public string Socket { get; set; } // Трябва да съвпада с процесора

        public override string GetSpecifications()
        {
            return $"[Motherboard] {Manufacturer} {Name} | Chipset: {Chipset} | Form: {FormFactor} | Socket: {Socket} | Price: {Price} lv. | Qty: {Quantity}";
        }
    }
}
