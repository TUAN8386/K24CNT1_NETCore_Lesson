using Microsoft.AspNetCore.Mvc;
using DATUAN_LESSON_10.Models;

namespace DATUAN_LESSON_10.Controllers
{
    public class BookController : Controller
    {
        // Action hiển thị danh sách Sách từ dữ liệu C# cứng (Không kết nối Database)
        public IActionResult Index()
        {
            var books = new List<Book>
            {
                new Book
                {
                    BookId = 1,
                    Title = "Lập trình C# cơ bản",
                    Author = "Nguyễn Văn A",
                    Price = 150000,
                    PublishedYear = 2023,
                    Category = new Category { CategoryName = "Lập trình" },
                    Publisher = new Publisher { PublisherName = "NXB Giáo Dục" }
                },
                new Book
                {
                    BookId = 2,
                    Title = "Giáo trình ASP.NET Core MVC",
                    Author = "Trịnh Văn B",
                    Price = 200000,
                    PublishedYear = 2024,
                    Category = new Category { CategoryName = "Lập trình" },
                    Publisher = new Publisher { PublisherName = "NXB Thông tin & Truyền thông" }
                },
                new Book
                {
                    BookId = 3,
                    Title = "Khởi nghiệp Thực chiến",
                    Author = "Lê Văn C",
                    Price = 180000,
                    PublishedYear = 2025,
                    Category = new Category { CategoryName = "Kinh tế" },
                    Publisher = new Publisher { PublisherName = "NXB Trẻ" }
                }
            };

            // Truyền danh sách dữ liệu cứng sang View
            return View(books);
        }
    }
}