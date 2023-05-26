using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;


public class Menu
{
    public static readonly Dictionary<string, Func<Tuple<string, int, double>, Product>> Category =
        new Dictionary<string, Func<Tuple<string, int, double>, Product>>
    {
        { "салата", args => new Salad(args.Item1, args.Item2, args.Item3) },
        { "супа", args => new Soup(args.Item1, args.Item2, args.Item3) },
        { "основно", args => new MainCourse(args.Item1, args.Item2, args.Item3) },
        { "десерт", args => new Dessert(args.Item1, args.Item2, args.Item3) },
        { "напитка", args => new Drink(args.Item1, args.Item2, args.Item3) }
    };

    public static readonly Dictionary<string, string> CatAnnotation = new Dictionary<string, string>
    {
        { "салата", "Салата" },
        { "супа", "Супа" },
        { "основно", "Основно ястие" },
        { "десерт", "Десерт" },
        { "напитка", "Напитка" }
    };
}

public abstract class Product
{
    private int quantity;
    private double price;
    private string name;
    public string Category { get; set; }
    public string Name {
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
    public int Quantity { 
        get { return quantity; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Количеството не може да е отрицателно число!");
            }
            if (value > 1000)
            {
                throw new ArgumentException("Количеството не може да е повече от 1000 единици!");
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
    public Salad(string name, int quantity, double price) : base("салата", name, quantity, price)
    {
    }
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
    public Soup(string name, int quantity, double price) : base("супа", name, quantity, price)
    {
    }
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
    public MainCourse(string name, int quantity, double price) : base("основно", name, quantity, price)
    {
    }
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
    public Dessert(string name, int quantity, double price) : base("десерт", name, quantity, price)
    {
    }
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
    public Drink(string name, int quantity, double price) : base("напитка", name, quantity, price)
    {
    }
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
    public int TableNumber {
        get { return tableNumber; }
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Table number cannot be less than 1");
            }
            if (value > 30)
            {
                throw new ArgumentException("Table number cannot be more than 30");
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

        Console.WriteLine("\nЗдравейте! Моля въведете продукти в менюто:\n");

        while (true) {
            var input = Console.ReadLine();

            if (input == "изход") {
                if (orders.Count > 0)
                {
                    showSales(orders);
                }
                else
                {
                    Console.WriteLine("\nНяма направени поръчки.");
                }
                break;
            }

            if (input == null) {
                continue;
            }

            var inputList = Regex.Split(input, @",\s*");

            // addition of products to the menu
            if (Menu.Category.ContainsKey(inputList[0]))
            {
                try {
                    if (inputList.Length != 4)
                    {
                        Console.WriteLine("Невалиден продукт!");
                        continue;
                    }
                    if (products.Any(p => p.Name == inputList[1]))
                    {
                        Console.WriteLine($"Продуктът '{inputList[1]}' вече съществува!");
                        continue;
                    }
                    else
                    {
                        Tuple<string, int, double>  newProduct = new Tuple<string, int, double>
                            (inputList[1], int.Parse(inputList[2]), double.Parse(inputList[3], CultureInfo.InvariantCulture));

                        products.Add(Menu.Category[inputList[0]](newProduct));
                    }
                    Console.WriteLine($"Продуктът '{inputList[1]}' беше добавен успешно в категорията '{inputList[0]}'!");
                    continue;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("ArgumentException occurred: " + ex.Message);
                }
            }

            // orders
            if (int.TryParse(inputList[0], out int tableNumber))
            {
                try {
                    if (tableNumber < 1 || tableNumber > 30)
                    {
                        Console.WriteLine("Невалиден номер на маса");
                        continue;
                    }
                    else if (inputList.Length < 2)
                    {
                        Console.WriteLine("Невалидна команда!");
                        continue;
                    }
                    else
                    {
                        List<Product> productsInOrder = new List<Product>();
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
                        if (productsInOrder.Count == 0)
                        {
                            Console.WriteLine($"Няма продукт с име '{inputList[1]}'!");
                            continue;
                        }
                        else
                        {
                            Order order = new Order(tableNumber, productsInOrder.ToImmutableList());
                            order.CalculateTotalPrice();
                            order.CalculateTotalCalories();
                            orders.Add(order);

                            continue;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("ArgumentException occurred: " + ex.Message);
                }
            }

            // sales
            if (input == "продажби")
            {
                showSales(orders);
                continue;
            }
            
            if (input.StartsWith("инфо"))
            {
                var inputInfo = input.Split(" ")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();
                
                if (inputInfo.Length < 2) {
                    Console.WriteLine("Невалидна инфо команда!");
                    continue;
                }

                var productName = string.Join(" ", inputInfo.Skip(1));
                var product = products.FirstOrDefault(x => x.Name == productName);

                if (product == null) {
                    Console.WriteLine($"Няма продукт с име '{productName}'!");
                    continue;
                }

                Console.WriteLine($"Информация за продукт: {product.Name}");
                Console.WriteLine($"{product.QuantityType}: {product.Quantity}");
                Console.WriteLine($"Калории: {product.GetCalories()}");
                continue;
            }

            // easteregg
            if (input == ":p" || input == ":P")
            {
                SlowWriteLine("\nI like С#\n", 100);
                continue;
            }

            if (!string.IsNullOrEmpty(input)) {
                Console.WriteLine("Невалидна команда!");
            }
        }
    }

    public static void showSales(List<Order> orders) 
    {
        var busyTables = orders.Select(x => x.TableNumber).Distinct().Count();
        var totalOrders = orders.Count();
        var totalIncome = orders.Sum(x => x.TotalPrice);
        var totalProducts = orders.SelectMany(x => x.Products).Count();
        var totalProductsByCategory = orders.SelectMany(x => x.Products)
            .GroupBy(x => x.Category)
            .OrderBy(x => x.Key)
            .Select(x => new { Category = x.Key, Count = x.Count(), Price = x.Sum(y => y.Price) });

        Console.WriteLine($"Общо заети маси през деня: {busyTables}");
        Console.WriteLine($"Общо продажби: {totalOrders} - {Math.Round(totalIncome, 2)}");
        Console.WriteLine("Продукти по категории:");
        foreach (var product in totalProductsByCategory)
        {
            Console.WriteLine($"  -   {Menu.CatAnnotation[product.Category]}: {product.Count} - {Math.Round(product.Price, 2)}");
        }
    }

    public static void SlowWriteLine(string text, int delayPerCharacter)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delayPerCharacter);
        }
        
        Console.WriteLine();
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