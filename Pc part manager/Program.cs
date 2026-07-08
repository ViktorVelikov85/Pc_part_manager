using System;
using System.Collections.Generic;
using Pc_part_manager.DAL;
using Pc_part_manager.Models;
using Pc_part_manager.Models.PcParts;

namespace Pc_part_manager
{
    class Program
    {
        private static readonly PartRepository _repo = new PartRepository();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Настройка на конзолата за по-красив вид
            Console.Title = "Personal Data Manager - Computer Parts";

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==================================================");
                Console.WriteLine("        PERSONAL DATA MANAGER: PC PARTS           ");
                Console.WriteLine("==================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" 1. Извеждане на всички налични части");
                Console.WriteLine(" 2. Добавяне на нова компютърна част");
                Console.WriteLine(" 3. Редактиране на съществуваща част");
                Console.WriteLine(" 4. Изтриване на част (по ID)");
                Console.WriteLine(" 5. Търсене по име / производител");
                Console.WriteLine(" 6. Филтриране по категория");
                Console.WriteLine(" 0. Изход");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==================================================");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" Изберете опция: ");
                Console.ForegroundColor = ConsoleColor.White;

                string choice = Console.ReadLine();

                // Изискване: Обработка на изключения на най-високо ниво (UI Exception Handling)
                try
                {
                    switch (choice)
                    {
                        case "1": ShowAllParts(); break;
                        case "2": AddNewPart(); break;
                        case "3": UpdateExistingPart(); break;
                        case "4": DeletePart(); break;
                        case "5": SearchParts(); break;
                        case "6": FilterParts(); break;
                        case "0":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nБлагодарим ви, че използвахте мениджъра! Приятен ден!");
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[Грешка] Невалидна опция! Моля, опитайте отново.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[Критична Грешка] {ex.Message}");
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nНатиснете [Enter] за връщане към менюто...");
                Console.ReadLine();
            }
        }

        // ==========================================
        // 1. ИЗВЕЖДАНЕ НА ВСИЧКИ (ПОЛИМОРФИЗЪМ)
        // ==========================================
        private static void ShowAllParts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- ВСИЧКИ НАЛИЧНИ КОМПЮТЪРНИ ЧАСТИ ---");
            Console.ForegroundColor = ConsoleColor.White;

            List<ComputerPart> parts = _repo.GetAll(); // Нашата колекция от обекти

            if (parts.Count == 0)
            {
                Console.WriteLine("Няма намерени записи в базата данни.");
                return;
            }

            foreach (var part in parts)
            {
                // ПОЛИМОРФИЗЪМ: C# автоматично знае кой 'GetSpecifications()' метод да извика (на CPU, на GPU и т.н.)
                Console.WriteLine($"ID: {part.Id} | {part.GetSpecifications()}");
            }
        }

        // ==========================================
        // 2. ДОБАВЯНЕ НА ЗАПИС
        // ==========================================
        private static void AddNewPart()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- ДОБАВЯНЕ НА НОВА ЧАСТ ---");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Изберете тип на частта: 1. CPU | 2. GPU | 3. RAM | 4. Motherboard | 5. SSD");
            string typeChoice = Console.ReadLine();

            ComputerPart part = null;
            Category category = new Category();

            switch (typeChoice)
            {
                case "1":
                    var cpu = new Cpu();
                    Console.Write("Брой ядра: "); cpu.Cores = int.Parse(Console.ReadLine());
                    Console.Write("Сокет (напр. AM5): "); cpu.Socket = Console.ReadLine();
                    part = cpu; category.Id = 1; category.Name = "CPU";
                    break;
                case "2":
                    var gpu = new Gpu();
                    Console.Write("Размер на VRAM (GB): "); gpu.VramSizeGb = int.Parse(Console.ReadLine());
                    Console.Write("Тип памет (напр. GDDR6): "); gpu.MemoryType = Console.ReadLine();
                    part = gpu; category.Id = 2; category.Name = "GPU";
                    break;
                case "3":
                    var ram = new Ram();
                    Console.Write("Капацитет (GB): "); ram.CapacityGb = int.Parse(Console.ReadLine());
                    Console.Write("Честота (MHz): "); ram.SpeedMhz = int.Parse(Console.ReadLine());
                    Console.Write("Генерация (напр. DDR5): "); ram.Generation = Console.ReadLine();
                    part = ram; category.Id = 3; category.Name = "RAM";
                    break;
                case "4":
                    var mb = new Motherboard();
                    Console.Write("Чипсет (напр. B650): "); mb.Chipset = Console.ReadLine();
                    Console.Write("Форм-фактор (напр. ATX): "); mb.FormFactor = Console.ReadLine();
                    Console.Write("Сокет (напр. AM5): "); mb.Socket = Console.ReadLine();
                    part = mb; category.Id = 4; category.Name = "Motherboard";
                    break;
                case "5":
                    var ssd = new Ssd();
                    Console.Write("Капацитет (GB): "); ssd.CapacityGb = int.Parse(Console.ReadLine());
                    Console.Write("Форм-фактор (напр. M.2): "); ssd.FormFactor = Console.ReadLine();
                    Console.Write("Скорост на четене (MB/s): "); ssd.ReadSpeedMb = int.Parse(Console.ReadLine());
                    part = ssd; category.Id = 5; category.Name = "SSD";
                    break;
                default:
                    throw new ArgumentException("Невалиден избор на тип!");
            }

            // Общи свойства
            Console.Write("Име на модела: "); part.Name = Console.ReadLine();
            Console.Write("Производител: "); part.Manufacturer = Console.ReadLine();
            Console.Write("Цена (лв): "); part.Price = decimal.Parse(Console.ReadLine());
            Console.Write("Количество: "); part.Quantity = int.Parse(Console.ReadLine());
            part.PartCategory = category;

            // Извикване на DAL за запис + автоматична бизнес валидация вътре
            _repo.Add(part);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Успех] Частта беше добавена успешно към MariaDB!");
        }

        // ==========================================
        // 3. РЕДАКТИРАНЕ НА ЗАПИС
        // ==========================================
        private static void UpdateExistingPart()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- РЕДАКТИРАНЕ НА ЧАСТ ---");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Въведете ID на частта, която искате да редактирате: ");
            int id = int.Parse(Console.ReadLine());

            // Първо извличаме всички, за да намерим типа на обекта (в реално приложение се прави метод GetById)
            var parts = _repo.GetAll();
            ComputerPart partToUpdate = parts.Find(p => p.Id == id);

            if (partToUpdate == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Грешка] Не е намерена част с такова ID!");
                return;
            }

            Console.WriteLine($"Редактирате в момента: {partToUpdate.GetSpecifications()}");
            Console.WriteLine("(Оставете празно/0, ако не искате да променяте текстово свойство)");

            Console.Write($"Ново име ({partToUpdate.Name}): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) partToUpdate.Name = newName;

            Console.Write($"Нова цена ({partToUpdate.Price} лв.): ");
            string newPriceStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPriceStr)) partToUpdate.Price = decimal.Parse(newPriceStr);

            Console.Write($"Ново количество ({partToUpdate.Quantity}): ");
            string newQtyStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newQtyStr)) partToUpdate.Quantity = int.Parse(newQtyStr);

            // Специфични свойства спрямо типа
            if (partToUpdate is Cpu cpu)
            {
                Console.Write($"Нов брой ядра ({cpu.Cores}): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) cpu.Cores = int.Parse(input);
            }
            else if (partToUpdate is Gpu gpu)
            {
                Console.Write($"Нов VRAM капацитет ({gpu.VramSizeGb} GB): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) gpu.VramSizeGb = int.Parse(input);
            }
            // *Забележка: По същия начин могат да се допишат и останалите типове при нужда*

            _repo.Update(partToUpdate);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Успех] Записът беше редактиран успешно!");
        }

        // ==========================================
        // 4. ИЗТРИВАНЕ НА ЗАПИС
        // ==========================================
        private static void DeletePart()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- ИЗТРИВАНЕ НА ЧАСТ ---");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Въведете ID на частта за изтриване: ");
            int id = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"Сигурни ли сте, че искате да изтриете част с ID {id}? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                _repo.Delete(id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[Успех] Частта е изтрита.");
            }
        }

        // ==========================================
        // 5. ТЪРСЕНЕ
        // ==========================================
        private static void SearchParts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- ТЪРСЕНЕ НА ЧАСТИ ---");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Въведете ключова дума (име или производител): ");
            string keyword = Console.ReadLine();

            List<ComputerPart> results = _repo.Search(keyword);

            Console.WriteLine($"\nНамерени резултати ({results.Count}):");
            foreach (var part in results)
            {
                Console.WriteLine(part.GetSpecifications());
            }
        }

        // ==========================================
        // 6. ФИЛТРИРАНЕ
        // ==========================================
        private static void FilterParts()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- ФИЛТРИРАНЕ ПО КАТЕГОРИЯ ---");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Изберете категория: 1. CPU | 2. GPU | 3. RAM | 4. Motherboard | 5. SSD");
            int catId = int.Parse(Console.ReadLine());

            List<ComputerPart> results = _repo.FilterByCategory(catId);

            Console.WriteLine($"\nКомпоненти в избраната категория ({results.Count}):");
            foreach (var part in results)
            {
                Console.WriteLine(part.GetSpecifications());
            }
        }
    }
}