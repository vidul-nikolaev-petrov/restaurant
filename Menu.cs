using Restourant.Products;

namespace Restourant.MenuCategories
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
}
