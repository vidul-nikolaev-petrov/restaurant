using System.Collections.Immutable;
using Restourant.Products;

namespace Restourant.Orders
{
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
}
