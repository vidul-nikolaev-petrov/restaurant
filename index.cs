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
}

// create class Order with properties "Table Number", "Products", "TotalPrice", "TotalCalories",
// setters and getters, and constructor with parameters
public class Order
{
    public int TableNumber {
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
        // create an empty list of products
        List<Product> products = new List<Product>();
        List<Order> orders = new List<Order>();
        
        // read user input while it is not "изход"
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
                            Order order = new Order(tableNumber, productsInOrder.ToImmutableList());
                            order.CalculateTotalPrice();
                            order.CalculateTotalCalories();
                            orders.Add(order);
                            Console.WriteLine($"Номер на маса: {tableNumber}");
                            Console.WriteLine($"Име на продукт: {inputList[1]}");
                            Console.WriteLine($"Цена: {order.TotalPrice}");
                            Console.WriteLine($"Калории: {order.TotalCalories}");
                            continue;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("ArgumentException occurred: " + ex.Message);
                }
            }

            // if input starts with "info" show the product which follows it
            if (input.StartsWith("инфо"))
            {
                var inputInfo = input.Split(" ")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();
                
                if (inputInfo.Length != 2) {
                    Console.WriteLine("Невалидна инфо команда!");
                    continue;
                }

                var productName = inputInfo[1];
                var product = products.FirstOrDefault(x => x.Name == productName);

                if (product == null) {
                    Console.WriteLine($"Няма продукт с име '{productName}'!");
                    continue;
                }

                Console.WriteLine($"Информация на продукт: {product.Name}");
                Console.WriteLine($"Количество: {product.Quantity}");
                Console.WriteLine($"Калории: {product.GetCalories()}");
                continue;
            }

            

            // if input is something else and not empty, print "Невалидна команда!"
            if (!string.IsNullOrEmpty(input)) {
                Console.WriteLine("Невалидна команда!");
            }
        }
    }
}

