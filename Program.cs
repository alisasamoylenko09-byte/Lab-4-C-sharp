using System;
using System.Collections.Generic;
using System.IO;

namespace Lab4csharp
{
    // Власне виключення для перевірки даних підприємства (Завдання 1)
    class InvalidEnterpriseDataException : Exception
    {
        public InvalidEnterpriseDataException(string message) : base(message) { }
    }

    // Власне виключення для записної книжки (Завдання 2)
    class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message) { }
    }

    // Клас для Завдання 1 (Варіант 5: Результати роботи підприємства)
    class EnterpriseRecord
    {
        private string month;
        private double plan;
        private double fact;

        public string Month
        {
            get { return month; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidEnterpriseDataException("Назва місяця не може бути порожньою.");
                month = value;
            }
        }

        public double Plan
        {
            get { return plan; }
            set
            {
                if (value <= 0)
                    throw new InvalidEnterpriseDataException("План випуску продукції має бути більшим за 0.");
                plan = value;
            }
        }

        public double Fact
        {
            get { return fact; }
            set
            {
                if (value < 0)
                    throw new InvalidEnterpriseDataException("Фактичний випуск не може бути від'ємним.");
                fact = value;
            }
        }

        public double ExecutionPercentage
        {
            get { return (fact / plan) * 100.0; }
        }

        public EnterpriseRecord(string month, double plan, double fact)
        {
            Month = month;
            Plan = plan;
            Fact = fact;
        }
    }

    // Клас для Завдання 2 (Варіант 5 - Записна книжка)
    class NotebookRecord
    {
        private string lastName;
        private string firstName;
        private string address;
        private string phone;
        private string email;

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value == "") throw new InvalidInputException("Прізвище не може бути порожнім.");
                lastName = value;
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value == "") throw new InvalidInputException("Ім'я не може бути порожнім.");
                firstName = value;
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                if (value == "") throw new InvalidInputException("Адреса не може бути порожньою.");
                address = value;
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (value == "") throw new InvalidInputException("Телефон не може бути порожнім.");
                phone = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (value.IndexOf('@') == -1) throw new InvalidInputException("Електронна пошта має містити символ '@'.");
                email = value;
            }
        }

        public NotebookRecord()
        {
            lastName = "Невідомо";
            firstName = "Невідомо";
            address = "Невідомо";
            phone = "0000000000";
            email = "test@gmail.com";
        }

        public NotebookRecord(string lName, string fName, string addr, string ph, string em)
        {
            LastName = lName;
            FirstName = fName;
            Address = addr;
            Phone = ph;
            Email = em;
        }

        public override string ToString()
        {
            return lastName + ";" + firstName + ";" + address + ";" + phone + ";" + email;
        }

        public static NotebookRecord FromString(string line)
        {
            string[] parts = line.Split(';');
            return new NotebookRecord(parts[0], parts[1], parts[2], parts[3], parts[4]);
        }
    }

    class Program
    {
        static string filePath = "notebook.txt";

        static void EnsureFile()
        {
            if (!File.Exists(filePath))
            {
                StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                sw.WriteLine("Шевченко;Тарас;м. Київ вул. Хрещатик 1;0671112233;shevchenko@gmail.com");
                sw.WriteLine("Коваленко;Олена;м. Львів вул. Городоцька 15;0502223344;kovalenko@ukr.net");
                sw.WriteLine("Петренко;Андрій;м. Одеса вул. Дерибасівська 5;0933334455;petrenko@gmail.com");
                sw.WriteLine("Мельник;Ірина;м. Харків вул. Сумська 10;0684445566;melnyk@meta.ua");
                sw.WriteLine("Іванов;Олексій;м. Дніпро пр. Яворницького 20;0995556677;ivanov@gmail.com");
                sw.WriteLine("Сидоренко;Марія;м. Запоріжжя Соборний пр. 50;0666667788;sydorenko@ukr.net");
                sw.WriteLine("Бондар;Сергій;м. Полтава вул. Соборності 12;0737778899;bondar@gmail.com");
                sw.WriteLine("Гриценко;Наталія;м. Чернігів пр. Миру 30;0508889900;hrytsenko@meta.ua");
                sw.WriteLine("Романюк;Дмитро;м. Чернівці вул. Кобилянської 8;0969990011;romanyuk@gmail.com");
                sw.WriteLine("Антонюк;Ольга;м. Ужгород вул. Корзо 4;0630001122;antonyuk@ukr.net");
                sw.Close();
            }
        }

        static List<NotebookRecord> Load()
        {
            List<NotebookRecord> list = new List<NotebookRecord>();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line.Trim() != "")
                    {
                        list.Add(NotebookRecord.FromString(line));
                    }
                }
            }
            return list;
        }

        static void Save(List<NotebookRecord> list)
        {
            StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            foreach (var r in list)
            {
                sw.WriteLine(r.ToString());
            }
            sw.Close();
        }

        static void PrintNotebookTable(List<NotebookRecord> list)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine("| Прізвище     | Ім'я       | Поштова адреса          | Телефон      | Ел. пошта            |");
            Console.WriteLine("-----------------------------------------------------------------------------------------");

            foreach (var r in list)
            {
                Console.WriteLine("| {0,-12} | {1,-10} | {2,-23} | {3,-12} | {4,-20} |",
                    r.LastName, r.FirstName, r.Address, r.Phone, r.Email);
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------");
        }

        static NotebookRecord InputRecord()
        {
            try
            {
                Console.Write("Введіть прізвище: ");
                string lName = Console.ReadLine();
                Console.Write("Введіть ім'я: ");
                string fName = Console.ReadLine();
                Console.Write("Введіть поштову адресу: ");
                string addr = Console.ReadLine();
                Console.Write("Введіть телефон: ");
                string phone = Console.ReadLine();
                Console.Write("Введіть електронну пошту: ");
                string email = Console.ReadLine();

                return new NotebookRecord(lName, fName, addr, phone, email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка введення: " + ex.Message);
                return null;
            }
        }

        // Завдання 1 
        static void RunTask1()
        {
            Console.WriteLine("\n--- ВИКОНАННЯ ЗАВДАННЯ №1 (Варіант 5: Робота підприємства) ---");
            try
            {
                string[] months = { "Січень", "Лютий", "Березень", "Квітень", "Травень", "Червень",
                                    "Липень", "Серпень", "Вересень", "Жовтень", "Листопад", "Грудень" };

                Random rnd = new Random();
                List<EnterpriseResource> records = new List<EnterpriseResource>();

                // Генеруємо випадкові дані для кожного місяця
                for (int i = 0; i < months.Length; i++)
                {
                    double plan = rnd.Next(1000, 2000); // план від 1000 до 2000
                    double fact = rnd.Next(800, 2200);  // факт від 800 до 2200
                    records.Add(new EnterpriseResource(months[i], plan, fact));
                }

                // Перетворюємо у список EnterpriseRecord
                List<EnterpriseRecord> enterpriseRecords = new List<EnterpriseRecord>();
                foreach (var item in records)
                {
                    enterpriseRecords.Add(new EnterpriseRecord(item.Month, item.Plan, item.Fact));
                }

                // Сортування у порядку зростання відсотка виконання плану
                enterpriseRecords.Sort((x, y) => x.ExecutionPercentage.CompareTo(y.ExecutionPercentage));

                Console.WriteLine("Згенерований та відсортований масив за відсотком виконання плану:");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("| {0,-12} | {1,-10} | {2,-10} | {3,-10} |", "Місяць", "План", "Факт", "Виконання");
                Console.WriteLine("--------------------------------------------------");
                foreach (var r in enterpriseRecords)
                {
                    Console.WriteLine("| {0,-12} | {1,-10} | {2,-10} | {3,8:F2}% |",
                        r.Month, r.Plan, r.Fact, r.ExecutionPercentage);
                }
                Console.WriteLine("--------------------------------------------------");

                var minRec = enterpriseRecords[0];
                var maxRec = enterpriseRecords[enterpriseRecords.Count - 1];

                Console.WriteLine($"\nМісяць з найменшим відсотком виконання плану: {minRec.Month} ({minRec.ExecutionPercentage:F2}%)");
                Console.WriteLine($"Місяць з найбільшим відсотком виконання плану: {maxRec.Month} ({maxRec.ExecutionPercentage:F2}%)");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка у Заданні 1: " + ex.Message);
            }
        }

        // Допоміжний клас для генератора
        private class EnterpriseResource
        {
            public string Month { get; set; }
            public double Plan { get; set; }
            public double Fact { get; set; }
            public EnterpriseResource(string m, double p, double f) { Month = m; Plan = p; Fact = f; }
        }

        // Меню для Завдання 2 
        static void RunTask2Menu()
        {
            EnsureFile();
            bool task2Running = true;

            while (task2Running)
            {
                Console.WriteLine("\n--- МЕНЮ ЗАВДАННЯ №2 (Варіант 5: Записна книжка) ---");
                Console.WriteLine("[к] - Переглянути всі записи з файлу");
                Console.WriteLine("[о] - Додати новий запис");
                Console.WriteLine("[ш] - Редагувати запис");
                Console.WriteLine("[е] - Видалити запис");
                Console.WriteLine("[н] - Пошук за електронною поштою");
                Console.WriteLine("[я] - Сортування за прізвищем");
                Console.WriteLine("[кіт] - Повернутися до головного меню");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine().Trim().ToLower();
                Console.WriteLine();

                if (choice == "кіт")
                {
                    task2Running = false;
                    continue;
                }

                try
                {
                    List<NotebookRecord> list = Load();

                    if (choice == "к")
                    {
                        PrintNotebookTable(list);
                    }
                    else if (choice == "о")
                    {
                        NotebookRecord newRec = InputRecord();
                        if (newRec != null)
                        {
                            list.Add(newRec);
                            Save(list);
                            Console.WriteLine("Запис успішно додано!");
                        }
                    }
                    else if (choice == "ш")
                    {
                        PrintNotebookTable(list);
                        Console.Write("Введіть номер запису для редагування (1 - " + list.Count + "): ");
                        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= list.Count)
                        {
                            NotebookRecord updatedRec = InputRecord();
                            if (updatedRec != null)
                            {
                                list[idx - 1] = updatedRec;
                                Save(list);
                                Console.WriteLine("Запис оновлено!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Невірний номер запису!");
                        }
                    }
                    else if (choice == "е")
                    {
                        PrintNotebookTable(list);
                        Console.Write("Введіть номер запису для видалення (1 - " + list.Count + "): ");
                        if (int.TryParse(Console.ReadLine(), out int delIdx) && delIdx >= 1 && delIdx <= list.Count)
                        {
                            list.RemoveAt(delIdx - 1);
                            Save(list);
                            Console.WriteLine("Запис видалено!");
                        }
                        else
                        {
                            Console.WriteLine("Невірний номер запису!");
                        }
                    }
                    else if (choice == "н")
                    {
                        Console.Write("Введіть частину електронної пошти для пошуку: ");
                        string query = Console.ReadLine().ToLower();

                        List<NotebookRecord> found = new List<NotebookRecord>();
                        foreach (var r in list)
                        {
                            if (r.Email.ToLower().Contains(query))
                            {
                                found.Add(r);
                            }
                        }

                        if (found.Count > 0)
                        {
                            Console.WriteLine("\nРезультати пошуку:");
                            PrintNotebookTable(found);
                        }
                        else
                        {
                            Console.WriteLine("Нічого не знайдено за таким запитом.");
                        }
                    }
                    else if (choice == "я")
                    {
                        list.Sort((x, y) => string.Compare(x.LastName, y.LastName));
                        Console.WriteLine("\nВідсортовано за прізвищем:");
                        PrintNotebookTable(list);
                    }
                    else
                    {
                        Console.WriteLine("Невірний вибір! Використовуйте літери з слова 'кошеня' у правильному порядку або 'кіт' для виходу.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Сталася помилка: " + ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n======================================================");
                Console.WriteLine("         ГОЛОВНЕ МЕНЮ ЛАБОРАТОРНОЇ РОБОТИ №4          ");
                Console.WriteLine("======================================================");
                Console.WriteLine("[п] - Завдання 1 (Підприємство — план, факт, випадкові дані)");
                Console.WriteLine("[р] - Завдання 2 (Робота з файлом та меню для Варіанта 5)");
                Console.WriteLine("[кіт] - Вихід з програми");
                Console.WriteLine("======================================================");
                Console.Write("Ваш вибір: ");

                string mainChoice = Console.ReadLine().Trim().ToLower();
                Console.WriteLine();

                if (mainChoice == "кіт")
                {
                    Console.WriteLine("Дякую за використання програми! До побачення.");
                    isRunning = false;
                }
                else if (mainChoice == "п")
                {
                    RunTask1();
                }
                else if (mainChoice == "р")
                {
                    RunTask2Menu();
                }
                else
                {
                    Console.WriteLine("Невірний вибір! Введіть 'п', 'р' або 'кіт'.");
                }
            }
        }
    }
}