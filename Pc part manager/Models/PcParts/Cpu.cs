using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pc_part_manager.Models.PcParts
{
    public class Cpu : ComputerPart
    {
        public int Cores { get; set; }
        public string Socket { get; set; }

        public override string GetSpecifications()
        {
            return $"[CPU] {Manufacturer} {Name} | Cores: {Cores} | Socket: {Socket} | Price: {Price} lv. | Qty: {Quantity}";
        }
    }
}
