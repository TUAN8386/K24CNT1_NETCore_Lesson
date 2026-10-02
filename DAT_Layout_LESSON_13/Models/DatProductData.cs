namespace DAT_Layout_LESSON_13.Models
{
    // Dữ liệu mẫu dùng chung cho trang Customer và Area Admin
    public static class DatProductData
    {
        public static readonly List<DatProduct> DatProducts = new()
        {
            new DatProduct { DatId = 1, DatName = "Laptop văn phòng",  DatPrice = 15990000, DatImage = "/images/DatLaptop.svg",    DatIsHot = true  },
            new DatProduct { DatId = 2, DatName = "Điện thoại thông minh", DatPrice = 8490000, DatImage = "/images/DatPhone.svg",  DatIsHot = false },
            new DatProduct { DatId = 3, DatName = "Tai nghe không dây", DatPrice = 1290000, DatImage = "/images/DatHeadphone.svg", DatIsHot = true  },
            new DatProduct { DatId = 4, DatName = "Đồng hồ thời trang", DatPrice = 2350000, DatImage = "/images/DatWatch.svg",     DatIsHot = false },
        };
    }
}
