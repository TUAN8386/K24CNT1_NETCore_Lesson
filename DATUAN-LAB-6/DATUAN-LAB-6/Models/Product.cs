using System.Collections.Generic;

namespace DATUAN_LAB_6.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsHot { get; set; } // Thuộc tính lọc sản phẩm nổi bật

        // Phương thức mock danh sách sản phẩm
        public List<Product> GetProductList()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Nồi cơm điện cao tần Nagakawa NAG0102", Image = "/images/noicom.jpg", Price = 2500000, IsHot = true },
                new Product { Id = 2, Name = "Nồi cơm điện cao tần Nagakawa NAG0102", Image = "/images/noicom.jpg", Price = 2500000, IsHot = false },
                new Product { Id = 3, Name = "Nồi cơm điện cao tần Nagakawa NAG0102", Image = "/images/noicom.jpg", Price = 2500000, IsHot = true },
                new Product { Id = 4, Name = "Nồi cơm điện cao tần Nagakawa NAG0102", Image = "/images/noicom.jpg", Price = 2500000, IsHot = false },
                new Product { Id = 5, Name = "Nồi cơm điện cao tần Nagakawa NAG0102", Image = "/images/noicom.jpg", Price = 2500000, IsHot = true },
                new Product { Id = 6, Name = "Nồi cơm điện cao tần Nagakawa NAG0102", Image = "/images/noicom.jpg", Price = 2500000, IsHot = true }
            };
        }
    }
}