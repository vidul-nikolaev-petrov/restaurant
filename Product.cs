
using Restourant.MenuCategories;

namespace Restourant.Products
{
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
}
