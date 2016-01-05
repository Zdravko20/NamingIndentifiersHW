namespace Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using IO;
    using Models;

    internal class OrdersMain
    {
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var dataMapper = new DataMapper();
            var consoleWriter = new ConsoleWriter();
            var categories = dataMapper.GetAllCategories();
            var products = dataMapper.GetAllProducts();
            var orders = dataMapper.getAllOrders();

            // Names of the 5 most expensive products
            consoleWriter.Write(MostExpensiveProducts(products, 5));

            // Number of products in each category
            consoleWriter.Write(NumberOfProductsInCategory(products, categories));

            // The 5 top products (by order quantity)
            consoleWriter.Write(TopProducts(products, orders, 5));

            // The most profitable category
            consoleWriter.Write(MostProfitableCategory(products, orders, categories));
        }

        private static string MostProfitableCategory(IEnumerable<Product> products, IEnumerable<Order> orders, 
            IEnumerable<Category> categories)
        {
            var mostProfitableCategory = orders
                .GroupBy(o => o.ProductId)
                .Select(g => new
                {
                    CatId = products.First(p => p.Id == g.Key).CategoryId, 
                    Price = products.First(p => p.Id == g.Key).UnitPrice, 
                    Quantity = g.Sum(p => p.Quantity)
                })
                .GroupBy(gg => gg.CatId)
                .Select(grp => new
                {
                    CategoryName = categories.First(c => c.Id == grp.Key).Name, 
                    TotalQuantity = grp.Sum(g => g.Quantity*g.Price)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .First();

            return $"{mostProfitableCategory.CategoryName}: {mostProfitableCategory.TotalQuantity}";
        }

        private static string TopProducts(IEnumerable<Product> products, IEnumerable<Order> orders, int count)
        {
            StringBuilder output = new StringBuilder();
            var topProducts = orders
                .GroupBy(o => o.ProductId)
                .Select(grp => new
                {
                    Product = products.First(p => p.Id == grp.Key).Name, 
                    Quantities = grp.Sum(grpgrp => grpgrp.Quantity)
                })
                .OrderByDescending(q => q.Quantities)
                .Take(count);

            foreach (var item in topProducts)
            {
                output.AppendFormat("{0}: {1}{2}", item.Product, item.Quantities, Environment.NewLine);
            }

            output.Append(new string('-', 10));
            return output.ToString();
        }

        private static string NumberOfProductsInCategory(IEnumerable<Product> products, IEnumerable<Category> categories)
        {
            StringBuilder output = new StringBuilder();
            var numOfProductsInCategory = products
                .GroupBy(p => p.CategoryId)
                .Select(grp => new {Category = categories.First(c => c.Id == grp.Key).Name, Count = grp.Count()})
                .ToList();

            foreach (var item in numOfProductsInCategory)
            {
                output.AppendFormat("{0}: {1}{2}", item.Category, item.Count, Environment.NewLine);
            }

            output.Append(new string('-', 10));

            return output.ToString();
        }

        private static string MostExpensiveProducts(IEnumerable<Product> products, int count)
        {
            StringBuilder output = new StringBuilder();
            var fiveMostExpensiveProducts = products
                .OrderByDescending(p => p.UnitPrice)
                .Take(count)
                .Select(p => p.Name);

            output.AppendLine(string.Join(Environment.NewLine, fiveMostExpensiveProducts));
            output.Append(new string('-', 10));

            return output.ToString();
        }
    }
}