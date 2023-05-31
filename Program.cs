using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Restourant.ConsoleCommands;
using Restourant.MenuCategories;
using Restourant.Orders;
using Restourant.Products;

namespace Restourant
{
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

                // exit
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
