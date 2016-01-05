namespace Orders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;

    public class DataMapper
    {
        private readonly string categoriesFileName;
        private readonly string ordersFileName;
        private readonly string productsFileName;

        public DataMapper(string categoriesFileName, string productsFileName, string ordersFileName)
        {
            this.categoriesFileName = categoriesFileName;
            this.productsFileName = productsFileName;
            this.ordersFileName = ordersFileName;
        }

        public DataMapper()
            : this("../../Data/categories.txt", "../../Data/products.txt", "../../Data/orders.txt")
        {
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = this.readFileLines(this.categoriesFileName, true);
            return categories
                .Select(c => c.Split(','))
                .Select(c => new Category
                {
                    Id = int.Parse(c[0]), 
                    Name = c[1], 
                    Description = c[2]
                });
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = this.readFileLines(this.productsFileName, true);
            return products
                .Select(p => p.Split(','))
                .Select(p => new Product
                {
                    Id = int.Parse(p[0]), 
                    Name = p[1], 
                    CategoryId = int.Parse(p[2]), 
                    UnitPrice = decimal.Parse(p[3]), 
                    UnitsInStock = int.Parse(p[4])
                });
        }

        public IEnumerable<Order> getAllOrders()
        {
            var orders = this.readFileLines(this.ordersFileName, true);
            return orders
                .Select(p => p.Split(','))
                .Select(p => new Order
                {
                    Id = int.Parse(p[0]), 
                    ProductId = int.Parse(p[1]), 
                    Quantity = int.Parse(p[2]), 
                    Discount = decimal.Parse(p[3])
                });
        }

        private List<string> readFileLines(string filename, bool hasHeader)
        {
            var allLines = new List<string>();
            using (var reader = new StreamReader(filename))
            {
                string currentLine;
                if (hasHeader)
                {
                    reader.ReadLine();
                }

                while ((currentLine = reader.ReadLine()) != null)
                {
                    allLines.Add(currentLine);
                }
            }

            return allLines;
        }
    }
}