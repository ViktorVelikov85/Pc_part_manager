using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pc_part_manager.Models
{
    public abstract class ComputerPart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public Category PartCategory { get; set; } // Композиция

        // Абстрактен метод за Полиморфизъм
        public abstract string GetSpecifications();
    }
}
