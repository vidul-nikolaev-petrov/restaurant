using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Restourant
{
    public class Menu
    {
        public static readonly Dictionary<int, string> CategoryName = new Dictionary<int, string>
        {
            { 1, "салата" },
            { 2, "супа" },
            { 3, "основно" },
            { 4, "десерт" },
            { 5, "напитка" }
        };

        public static readonly Dictionary<
            string,
            Func<Tuple<string, int, double>, Product>
        > Category = new Dictionary<string, Func<Tuple<string, int, double>, Product>>
        {
            { Menu.CategoryName[1], args => new Salad(args.Item1, args.Item2, args.Item3) },
            { Menu.CategoryName[2], args => new Soup(args.Item1, args.Item2, args.Item3) },
            { Menu.CategoryName[3], args => new MainCourse(args.Item1, args.Item2, args.Item3) },
            { Menu.CategoryName[4], args => new Dessert(args.Item1, args.Item2, args.Item3) },
            { Menu.CategoryName[5], args => new Drink(args.Item1, args.Item2, args.Item3) }
        };

        public static readonly Dictionary<string, string> CatAnnotation = new Dictionary<
            string,
            string
        >
        {
            { Menu.CategoryName[1], "Салата" },
            { Menu.CategoryName[2], "Супа" },
            { Menu.CategoryName[3], "Основно ястие" },
            { Menu.CategoryName[4], "Десерт" },
            { Menu.CategoryName[5], "Напитка" }
        };
    }

    public class Commands
    {
        public static IEnumerable<string> ShowCommands()
        {
            return new string[]
            {
                "\nКоманди:",
                "  - Добавяне на продукт: <категория>, <име>, <количество>, <цена>",
                "  - Поръчка: <номер на маса>, <продукт1>, <продукт2>, ..., <продуктN>",
                "  - Информация за продажби: продажби",
                "  - Информация за продукт: инфо <име на продукт>",
                "  - Всички категории: категории",
                "  - Всички продукти в менюто: меню",
                "  - Изход от програмата: изход",
                "  - Помощ: ?, помощ, команди",
            };
        }
    }

    public abstract class Product
    {
        private int quantity;
        private double price;
        private string name;
        public string Category { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Невалидно име на продукт!");
                }
                if (value.All(char.IsDigit))
                {
                    throw new ArgumentException("Името не може да е само цифри!");
                }
                name = value;
            }
        }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Количеството не може да е отрицателно число!");
                }
                if (value > 1000)
                {
                    throw new ArgumentException(
                        "Количеството не може да е повече от 1000 единици!"
                    );
                }
                quantity = value;
            }
        }
        public double Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Цената не може да е отрицателно число!");
                }
                if (value > 100)
                {
                    throw new ArgumentException("Цената не може да е повече от 100 единици!");
                }
                price = value;
            }
        }

        public Product(string category, string name, int quantity, double price)
        {
            Category = category;
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public abstract double GetCalories();
        public abstract string QuantityType { get; }
    }

    // create class Salad that inherits class Product
    public class Salad : Product
    {
        public Salad(string name, int quantity, double price)
            : base(Menu.CategoryName[1], name, quantity, price) { }

        public override double GetCalories()
        {
            return 0;
        }

        public override string QuantityType
        {
            get { return "Грама"; }
        }
    }

    // create class Soup that inherits class Product
    public class Soup : Product
    {
        public Soup(string name, int quantity, double price)
            : base(Menu.CategoryName[2], name, quantity, price) { }

        public override double GetCalories()
        {
            return 0;
        }

        public override string QuantityType
        {
            get { return "Грама"; }
        }
    }

    // create class MainCourse that inherits class Product
    public class MainCourse : Product
    {
        public MainCourse(string name, int quantity, double price)
            : base(Menu.CategoryName[3], name, quantity, price) { }

        public override double GetCalories()
        {
            return Quantity * 1;
        }

        public override string QuantityType
        {
            get { return "Грама"; }
        }
    }

    // create class Dessert that inherits class Product
    public class Dessert : Product
    {
        public Dessert(string name, int quantity, double price)
            : base(Menu.CategoryName[4], name, quantity, price) { }

        public override double GetCalories()
        {
            return Quantity * 3;
        }

        public override string QuantityType
        {
            get { return "Грама"; }
        }
    }

    // create class Drink that inherits class Product
    public class Drink : Product
    {
        public Drink(string name, int quantity, double price)
            : base(Menu.CategoryName[5], name, quantity, price) { }

        public override double GetCalories()
        {
            return Quantity * 1.5;
        }

        public override string QuantityType
        {
            get { return "Милилитра"; }
        }
    }

    public class Order
    {
        private int tableNumber;
        public int TableNumber
        {
            get { return tableNumber; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Масата не може да е отрицателно число!");
                }
                if (value > 30)
                {
                    throw new ArgumentException("Броят на масите не може да надвишава 30!");
                }
                tableNumber = value;
            }
        }
        public ImmutableList<Product> Products { get; }
        public double TotalPrice { get; set; }
        public double TotalCalories { get; set; }

        public Order(int tableNumber, ImmutableList<Product> products)
        {
            TableNumber = tableNumber;
            Products = products;
            TotalPrice = 0;
            TotalCalories = 0;
        }

        public void CalculateTotalPrice()
        {
            foreach (var product in Products)
            {
                TotalPrice += product.Price;
            }
        }

        public void CalculateTotalCalories()
        {
            foreach (var product in Products)
            {
                TotalCalories += product.GetCalories();
            }
        }
    }

    public class Program
    {
        static void Main()
        {
            List<Product> products = new List<Product>();
            List<Order> orders = new List<Order>();

            WriteLine("\nЗдравейте! Моля въведете продукти в менюто:\n");

            while (true)
            {
                var input = Console.ReadLine();
                var inputList = Regex.Split(input, @",\s*");

                if (input == "изход")
                {
                    if (orders.Count > 0)
                    {
                        showSales(orders);
                    }
                    else
                    {
                        WriteLine("\nНяма направени поръчки.");
                    }
                    break;
                }

                if (input == null)
                {
                    continue;
                }

                // addition of products to the menu
                if (Menu.Category.ContainsKey(inputList[0]))
                {
                    try
                    {
                        if (inputList.Length != 4)
                        {
                            WriteLine("Невалиден продукт!");
                            continue;
                        }
                        if (products.Any(p => p.Name == inputList[1]))
                        {
                            WriteLine($"Продуктът '{inputList[1]}' вече съществува!");
                            continue;
                        }
                        else
                        {
                            Tuple<string, int, double> newProduct = new Tuple<string, int, double>(
                                inputList[1],
                                int.Parse(inputList[2]),
                                double.Parse(inputList[3], CultureInfo.InvariantCulture)
                            );

                            products.Add(Menu.Category[inputList[0]](newProduct));
                        }
                        continue;
                    }
                    catch (ArgumentException ex)
                    {
                        WriteLine("Грешка в аргументите: " + ex.Message);
                        continue;
                    }
                }

                // orders
                if (int.TryParse(inputList[0], out int tableNumber))
                {
                    try
                    {
                        if (inputList.Length < 2)
                        {
                            WriteLine("Невалидна команда!");
                            continue;
                        }
                        else
                        {
                            List<Product> productsInOrder = new List<Product>();

                            if (!inputList.Skip(1).All(i => products.Any(p => p.Name == i)))
                            {
                                WriteLine("Един или повече невалидни продуктa!");
                                continue;
                            }

                            foreach (var product in products)
                            {
                                for (int i = 1; i < inputList.Length; i++)
                                {
                                    if (product.Name == inputList[i])
                                    {
                                        productsInOrder.Add(product);
                                    }
                                }
                            }

                            Order order = new Order(tableNumber, productsInOrder.ToImmutableList());
                            order.CalculateTotalPrice();
                            order.CalculateTotalCalories();
                            orders.Add(order);
                            continue;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        WriteLine("Грешка в аргументите: " + ex.Message);
                        continue;
                    }
                }

                // sales
                if (input == "продажби")
                {
                    if (orders.Count > 0)
                    {
                        showSales(orders);
                    }
                    else
                    {
                        WriteLine("\nЗасега няма направени поръчки.");
                    }
                    continue;
                }

                if (input.StartsWith("инфо"))
                {
                    var inputInfo = input.Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    if (inputInfo.Length < 2)
                    {
                        WriteLine("Невалидна инфо команда: липсва продукт!");
                        continue;
                    }

                    var productName = string.Join(" ", inputInfo.Skip(1));
                    var product = products.FirstOrDefault(x => x.Name == productName);

                    if (product == null)
                    {
                        WriteLine($"Няма продукт с име '{productName}'!");
                        continue;
                    }

                    WriteLine($"Информация за продукт: {product.Name}");
                    WriteLine($"{product.QuantityType}: {product.Quantity}");
                    WriteLine($"Калории: {product.GetCalories()}\n");
                    continue;
                }

                // всички категории
                if (input == "категории")
                {
                    WriteLine("\nКатегории по индекс и описателно име:");
                    foreach (var cat in Menu.Category.OrderBy(x => x.Key))
                    {
                        WriteLine($"  - {cat.Key}: {Menu.CatAnnotation[cat.Key]}");
                    }

                    WriteLine("");
                    continue;
                }

                // всички продукти в менюто
                if (input == "меню")
                {
                    if (products.Count > 0)
                    {
                        WriteLine("\nПродукти в менюто:");
                        foreach (
                            var product in products.OrderBy(x => x.Category).ThenBy(x => x.Name)
                        )
                        {
                            WriteLine(
                                $"   {Menu.CatAnnotation[product.Category]}: "
                                    + $"{product.Name}, {product.Quantity} "
                                    + $"{product.QuantityType}, {product.Price} лв."
                            );
                        }
                        WriteLine("");
                    }
                    else
                    {
                        WriteLine("\nНяма добавени продукти в менюто.\n");
                    }
                    continue;
                }

                // show commands
                if (input == "?" || input == "помощ" || input == "команди")
                {
                    WriteLine(string.Join("\n", Commands.ShowCommands()));
                    WriteLine("");
                    continue;
                }

                // easteregg
                if (input.StartsWith(":p") || input.StartsWith(":P"))
                {
                    SlowWriteLine($"I like С# {input}", 32);
                    continue;
                }

                if (!string.IsNullOrEmpty(input))
                {
                    WriteLine($"Невалидна команда: '{input}'");
                }
            }
        }

        public static void showSales(List<Order> orders)
        {
            var busyTables = orders.Select(x => x.TableNumber).Distinct().Count();
            var totalOrders = orders.Sum(x => x.Products.Count);
            var totalIncome = orders.Sum(x => x.TotalPrice);
            var totalProductsByCategory = orders
                .SelectMany(x => x.Products)
                .GroupBy(x => x.Category)
                .OrderBy(x => x.Key)
                .Select(
                    x =>
                        new
                        {
                            Category = x.Key,
                            Count = x.Count(),
                            Price = x.Sum(y => y.Price)
                        }
                );

            WriteLine($"Общо заети маси през деня: {busyTables}");
            WriteLine($"Общо продажби: {totalOrders} - {Math.Round(totalIncome, 2)}");
            WriteLine("Продукти по категории:");
            foreach (var p in totalProductsByCategory)
            {
                WriteLine(
                    $"  - {Menu.CatAnnotation[p.Category]}: {p.Count} - {Math.Round(p.Price, 2)}"
                );
            }
            WriteLine("");
        }

        public static void SlowWriteLine(string text, int delayPerCharacter)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayPerCharacter);
            }
            Thread.Sleep(1000);
            for (int i = text.Length - 1; i >= 0; i--)
            {
                Console.Write("\b \b");
                Thread.Sleep(delayPerCharacter);
            }
            WriteLine("Невалидна команда!");
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}

/*

Някои примерни команди:

салата, Шопска салата, 350, 2.50
салата, Овчарска салата, 400, 3.20
супа, Таратор, 300, 1.50
супа, Пилешка супа, 350, 3.00
основно, Винен кебап, 450, 5.00
основно, Мусака, 400, 4.60
десерт, Палачинка, 150, 2.20
десерт, Бисквитена торта, 200, 3.00
напитка, Кафе, 70, 1.00
напитка, Чай, 200, 1.00

11, Шопска салата, Пилешка супа, Винен кебап, Палачинка, Кафе
22, Овчарска салата, Таратор, Мусака, Бисквитена торта, Чай
4, Шопска салата, Таратор, Винен кебап, Палачинка, Кафе
5, Овчарска салата, Пилешка супа, Мусака, Бисквитена торта, Чай

продажби

инфо Шопска салата
инфо Таратор
инфо Винен кебап
инфо Палачинка
инфо Кафе

:P

изход

*/