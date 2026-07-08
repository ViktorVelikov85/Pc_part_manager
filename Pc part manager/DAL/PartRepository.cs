using MySql.Data.MySqlClient;
using Pc_part_manager.Models;
using Pc_part_manager.Models.PcParts;
using Pc_part_manager.Models;
using Pc_part_manager.Models.PcParts;
using System;
using System.Collections.Generic;
using System.Data;

namespace Pc_part_manager.DAL
{
    public class PartRepository
    {
        // ==========================================
        // 1. CREATE (ДОБАВЯНЕ) + ВАЛИДАЦИЯ
        // ==========================================
        public void Add(ComputerPart part)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));
            ValidatePart(part); // Извикване на валидацията

            string sql = @"INSERT INTO computer_parts 
                (name, manufacturer, price, quantity, category_id, part_type, socket, cores, vram_size_gb, memory_type, capacity_gb, speed_mhz, generation, chipset, form_factor, read_speed_mb) 
                VALUES (@name, @manufacturer, @price, @quantity, @categoryId, @partType, @socket, @cores, @vramSize, @memoryType, @capacity, @speed, @generation, @chipset, @formFactor, @readSpeed)";

            DbConfig.Execute(sql, GetParams(part));
        }

        // ==========================================
        // 2. READ (ИЗВЕЖДАНЕ НА ВСИЧКИ)
        // ==========================================
        public List<ComputerPart> GetAll()
        {
            string sql = @"SELECT cp.*, c.name AS category_name 
                           FROM computer_parts cp 
                           JOIN categories c ON cp.category_id = c.id";

            DataTable table = DbConfig.GetTable(sql);
            return MapTableToList(table); // Превръща таблицата в List<ComputerPart>
        }

        // ==========================================
        // 3. UPDATE (РЕДАКТИРАНЕ)
        // ==========================================
        public void Update(ComputerPart part)
        {
            if (part.Id <= 0) throw new ArgumentException("Невалидно ID за редакция!");
            ValidatePart(part);

            string sql = @"UPDATE computer_parts SET 
                name=@name, manufacturer=@manufacturer, price=@price, quantity=@quantity, category_id=@categoryId, 
                socket=@socket, cores=@cores, vram_size_gb=@vramSize, memory_type=@memoryType, 
                capacity_gb=@capacity, speed_mhz=@speed, generation=@generation, 
                chipset=@chipset, form_factor=@formFactor, read_speed_mb=@readSpeed 
                WHERE id = @id";

            var parameters = new List<MySqlParameter>(GetParams(part));
            parameters.Add(new MySqlParameter("@id", part.Id));

            DbConfig.Execute(sql, parameters.ToArray());
        }

        // ==========================================
        // 4. DELETE (ИЗТРИВАНЕ)
        // ==========================================
        public void Delete(int id)
        {
            string sql = "DELETE FROM computer_parts WHERE id = @id";
            DbConfig.Execute(sql, new[] { new MySqlParameter("@id", id) });
        }

        // ==========================================
        // 5. SEARCH (ТЪРСЕНЕ ПО ИМЕ / ПРОИЗВОДИТЕЛ)
        // ==========================================
        public List<ComputerPart> Search(string searchTerm)
        {
            string sql = @"SELECT cp.*, c.name AS category_name 
                           FROM computer_parts cp 
                           JOIN categories c ON cp.category_id = c.id
                           WHERE cp.name LIKE @search OR cp.manufacturer LIKE @search";

            var parameters = new[] { new MySqlParameter("@search", "%" + searchTerm + "%") };
            DataTable table = DbConfig.GetTable(sql, parameters);
            return MapTableToList(table);
        }

        // ==========================================
        // 6. FILTER (ФИЛТРИРАНЕ ПО КАТЕГОРИЯ)
        // ==========================================
        public List<ComputerPart> FilterByCategory(int categoryId)
        {
            string sql = @"SELECT cp.*, c.name AS category_name 
                           FROM computer_parts cp 
                           JOIN categories c ON cp.category_id = c.id
                           WHERE cp.category_id = @catId";

            var parameters = new[] { new MySqlParameter("@catId", categoryId) };
            DataTable table = DbConfig.GetTable(sql, parameters);
            return MapTableToList(table);
        }

        // ==========================================
        //         ПОМОЩНИ МЕТОДИ (МАПВАНЕ И ПАРАМЕТРИ)
        // ==========================================

        // Превръща DataTable в Колекция от ООП обекти (Полиморфизъм)
        private List<ComputerPart> MapTableToList(DataTable table)
        {
            var list = new List<ComputerPart>();

            foreach (DataRow row in table.Rows)
            {
                string type = row["part_type"].ToString();
                ComputerPart part = null;

                switch (type)
                {
                    case "CPU":
                        part = new Cpu { Cores = Convert.ToInt32(row["cores"]), Socket = row["socket"].ToString() };
                        break;
                    case "GPU":
                        part = new Gpu { VramSizeGb = Convert.ToInt32(row["vram_size_gb"]), MemoryType = row["memory_type"].ToString() };
                        break;
                    case "RAM":
                        part = new Ram { CapacityGb = Convert.ToInt32(row["capacity_gb"]), SpeedMhz = Convert.ToInt32(row["speed_mhz"]), Generation = row["generation"].ToString() };
                        break;
                    case "Motherboard":
                        part = new Motherboard { Chipset = row["chipset"].ToString(), FormFactor = row["form_factor"].ToString(), Socket = row["socket"].ToString() };
                        break;
                    case "SSD":
                        part = new Ssd { CapacityGb = Convert.ToInt32(row["capacity_gb"]), FormFactor = row["form_factor"].ToString(), ReadSpeedMb = Convert.ToInt32(row["read_speed_mb"]) };
                        break;
                }

                if (part != null)
                {
                    part.Id = Convert.ToInt32(row["id"]);
                    part.Name = row["name"].ToString();
                    part.Manufacturer = row["manufacturer"].ToString();
                    part.Price = Convert.ToDecimal(row["price"]);
                    part.Quantity = Convert.ToInt32(row["quantity"]);
                    part.PartCategory = new Category { Id = Convert.ToInt32(row["category_id"]), Name = row["category_name"].ToString() };

                    list.Add(part);
                }
            }
            return list;
        }

        // Генерира масив от SQL параметри спрямо обекта
        private MySqlParameter[] GetParams(ComputerPart part)
        {
            var list = new List<MySqlParameter>
            {
                new MySqlParameter("@name", part.Name),
                new MySqlParameter("@manufacturer", part.Manufacturer),
                new MySqlParameter("@price", part.Price),
                new MySqlParameter("@quantity", part.Quantity),
                new MySqlParameter("@categoryId", part.PartCategory.Id)
            };

            // Инициализираме всички специфични параметри с DBNull.Value по подразбиране
            object socket = DBNull.Value, cores = DBNull.Value, vram = DBNull.Value, memType = DBNull.Value;
            object capacity = DBNull.Value, speed = DBNull.Value, gen = DBNull.Value, chipset = DBNull.Value;
            object form = DBNull.Value, readSpeed = DBNull.Value;
            string partType = "";

            if (part is Cpu cpu) { partType = "CPU"; cores = cpu.Cores; socket = cpu.Socket; }
            else if (part is Gpu gpu) { partType = "GPU"; vram = gpu.VramSizeGb; memType = gpu.MemoryType; }
            else if (part is Ram ram) { partType = "RAM"; capacity = ram.CapacityGb; speed = ram.SpeedMhz; gen = ram.Generation; }
            else if (part is Motherboard mb) { partType = "Motherboard"; chipset = mb.Chipset; form = mb.FormFactor; socket = mb.Socket; }
            else if (part is Ssd ssd) { partType = "SSD"; capacity = ssd.CapacityGb; form = ssd.FormFactor; readSpeed = ssd.ReadSpeedMb; }

            list.Add(new MySqlParameter("@partType", partType));
            list.Add(new MySqlParameter("@socket", socket));
            list.Add(new MySqlParameter("@cores", cores));
            list.Add(new MySqlParameter("@vramSize", vram));
            list.Add(new MySqlParameter("@memoryType", memType));
            list.Add(new MySqlParameter("@capacity", capacity));
            list.Add(new MySqlParameter("@speed", speed));
            list.Add(new MySqlParameter("@generation", gen));
            list.Add(new MySqlParameter("@chipset", chipset));
            list.Add(new MySqlParameter("@formFactor", form));
            list.Add(new MySqlParameter("@readSpeed", readSpeed));

            return list.ToArray();
        }

        // Обща валидация на данните
        private void ValidatePart(ComputerPart part)
        {
            if (string.IsNullOrWhiteSpace(part.Name)) throw new ArgumentException("Името не може да е празно!");
            if (string.IsNullOrWhiteSpace(part.Manufacturer)) throw new ArgumentException("Производителят не може да е празен!");
            if (part.Price <= 0) throw new ArgumentException("Цената трябва да е над 0 лв.!");
            if (part.Quantity < 0) throw new ArgumentException("Количеството не може да бъде отрицателно!");
        }
    }
}