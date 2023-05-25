using System;
using System.Collections.Immutable;
using System.Globalization;


public enum Category
{
    салата,
    супа,
    основно,
    десерт,
    напитка
}

// define abstract class Product with properties "Category", "Name", "Quantity", "Price",
// setters and getters, and constructor with parameters
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
    // add method "GetCalories" that returns the calories of the product
    public abstract double GetCalories();
    // add method "QuantytyType" that returns the quantity type of the product
    public abstract string QuantityType { get; }
}

// create class Salad that inherits class Product
public class Salad : Product
{
    public Salad(string name, int quantity, double price) : base("салата", name, quantity, price)
    {
    }
    // override method "GetCalories" that returns the calories of the product
    public override double GetCalories()
    {
        return 0;
    }
    // override property "QuantityType" that returns the quantity type of the product
    public override string QuantityType
    {
        get { return "грама"; }
    }

}

// create class Soup that inherits class Product
public class Soup : Product
{
    public Soup(string name, int quantity, double price) : base("супа", name, quantity, price)
    {
    }
    // override method "GetCalories" that returns the calories of the product
    public override double GetCalories()
    {
        return 0;
    }
    // override property "QuantityType" that returns the quantity type of the product
    public override string QuantityType
    {
        get { return "грама"; }
    }
}

// create class MainCourse that inherits class Product
public class MainCourse : Product
{
    public MainCourse(string name, int quantity, double price) : base("основно", name, quantity, price)
    {
    }
    // override method "GetCalories" that returns the calories of the product
    public override double GetCalories()
    {
        // return the quantity of the product multiplied by 1
        return Quantity * 1;
    }
    // override property "QuantityType" that returns the quantity type of the product
    public override string QuantityType
    {
        get { return "грама"; }
    }
}

// create class Dessert that inherits class Product
public class Dessert : Product
{
    public Dessert(string name, int quantity, double price) : base("десерт", name, quantity, price)
    {
    }
    // override method "GetCalories" that returns the calories of the product
    public override double GetCalories()
    {
        return Quantity * 3;
    }
    // override property "QuantityType" that returns the quantity type of the product
    public override string QuantityType
    {
        get { return "грама"; }
    }
}

// create class Drink that inherits class Product
public class Drink : Product
{
    public Drink(string name, int quantity, double price) : base("напитка", name, quantity, price)
    {
    }
    // override method "GetCalories" that returns the calories of the product
    public override double GetCalories()
    {
        return Quantity * 1.5;
    }
    // override property "QuantityType" that returns the quantity type of the product
    public override string QuantityType
    {
        get { return "милилитра"; }
    }
}

// create class Order with properties "Table Number", "Products", "TotalPrice", "TotalCalories",
// setters and getters, and constructor with parameters
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
    public ImmutableList<Product> Products { get; set; }
    public double TotalPrice { get; set; }
    public double TotalCalories { get; set; }

    public Order(int tableNumber, ImmutableList<Product> products)
    {
        TableNumber = tableNumber;
        Products = products;
        TotalPrice = 0;
        TotalCalories = 0;
    }
    // add method "CalculateTotalPrice" that calculates the total price of the order
    public void CalculateTotalPrice()
    {
        foreach (var product in Products)
        {
            TotalPrice += product.Price;
        }
    }
    // add method "CalculateTotalCalories" that calculates the total calories of the order
    public void CalculateTotalCalories()
    {
        foreach (var product in Products)
        {
            TotalCalories += product.GetCalories();
        }
    }
}

partial class Program
{
    static void Main()
    {
        List<Product> products = new List<Product>();
        List<Order> orders = new List<Order>();

        Console.WriteLine("\nЗдравейте! Моля въведете продукти в менюто:\n");

        while (true) {
            var input = Console.ReadLine();

            if (input == "изход") {
                break;
            }

            if (input == null) {
                continue;
            }

            var inputList = input.Split(", ");

            // if input starts with category
            if (Enum.IsDefined(typeof(Category), inputList[0]) &&
                Enum.TryParse<Category>(inputList[0], out Category category))
            {
                try {
                    if (inputList.Length != 4)
                    {
                        Console.WriteLine("Невалиден продукт!");
                        continue;
                    }
                    else if (category == Category.салата)
                    {
                        products.Add(new Salad(inputList[1], int.Parse(inputList[2]),
                                        double.Parse(inputList[3], CultureInfo.InvariantCulture)));
                    }
                    else if (category == Category.супа)
                    {
                        products.Add(new Soup(inputList[1], int.Parse(inputList[2]),
                                        double.Parse(inputList[3], CultureInfo.InvariantCulture)));
                    }
                    else if (category == Category.основно)
                    {
                        products.Add(new MainCourse(inputList[1], int.Parse(inputList[2]),
                                        double.Parse(inputList[3], CultureInfo.InvariantCulture)));
                    }
                    else if (category == Category.десерт)
                    {
                        products.Add(new Dessert(inputList[1], int.Parse(inputList[2]), 
                                        double.Parse(inputList[3], CultureInfo.InvariantCulture)));
                    }
                    else if (category == Category.напитка)
                    {
                        products.Add(new Drink(inputList[1], int.Parse(inputList[2]), 
                                        double.Parse(inputList[3], CultureInfo.InvariantCulture)));
                    }
                    Console.WriteLine($"Продуктът '{inputList[1]}' беше добавен успешно в категорията '{inputList[0]}'!");
                    continue;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("ArgumentException occurred: " + ex.Message);
                }
            }

            // input first element is digit in range 1-30
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
                            string productsNames = "";                            
                            Order order = new Order(tableNumber, productsInOrder.ToImmutableList());
                            order.CalculateTotalPrice();
                            order.CalculateTotalCalories();
                            orders.Add(order);
                            Console.WriteLine($"Номер на маса: {tableNumber}");
                            Console.WriteLine($"Цена: {order.TotalPrice}");
                            Console.WriteLine($"Калории: {order.TotalCalories}");
                            for (int i = 1; i < inputList.Length; i++)
                            {
                                productsNames += inputList[i];
                                if (i != inputList.Length - 1)
                                {
                                    productsNames += ", ";
                                }
                            }
                            Console.WriteLine($"Продукти: {productsNames}");

                            continue;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("ArgumentException occurred: " + ex.Message);
                }
            }

            // if input starts with "продажби" show sum of busy tables, sum of all orders and total income, 
            // the sum of all orders' products and the sum of their prices grouped by category ascending
            if (input == "продажби")
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
                Console.WriteLine($"Общо продажби: {totalOrders} - {totalIncome}");
                Console.WriteLine("Продукти по категории:");
                foreach (var product in totalProductsByCategory)
                {
                    Console.WriteLine($"  -   {product.Category}: {product.Count} - {product.Price}");
                }
                continue;
            }
            

            // if input starts with "info" show the product which follows it
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

            if (!string.IsNullOrEmpty(input)) {
                Console.WriteLine("Невалидна команда!");
            }
        }
    }
}

