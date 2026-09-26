namespace DatNetCoreLAB6_EF.Helpers
{
    // Tiện ích upload / xóa ảnh trong wwwroot
    public static class DatFileHelper
    {
        public const string DAT_PRODUCT_FOLDER = "images/products";
        public const string DAT_BANNER_FOLDER = "images/banners";
        public const string DAT_AVATAR_FOLDER = "images/avatars";
        public const long DAT_MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB

        private static readonly string[] DAT_ALLOWED_EXTENSIONS = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        // Trả về thông báo lỗi, hoặc null nếu file hợp lệ
        public static string? DatValidateImage(IFormFile? datFile)
        {
            if (datFile == null || datFile.Length == 0)
            {
                return null;
            }

            var datExtension = Path.GetExtension(datFile.FileName).ToLowerInvariant();
            if (!DAT_ALLOWED_EXTENSIONS.Contains(datExtension))
            {
                return "Chỉ chấp nhận ảnh .jpg, .jpeg, .png, .gif, .webp";
            }

            if (datFile.Length > DAT_MAX_FILE_SIZE)
            {
                return "Dung lượng ảnh tối đa 5MB";
            }

            return null;
        }

        // Lưu ảnh vào wwwroot/{datFolder}, trả về tên file đã lưu
        public static async Task<string> DatSaveImageAsync(IFormFile datFile, string datWebRootPath, string datFolder)
        {
            var datDirectory = Path.Combine(datWebRootPath, datFolder);
            Directory.CreateDirectory(datDirectory);

            var datExtension = Path.GetExtension(datFile.FileName).ToLowerInvariant();
            var datFileName = $"{Guid.NewGuid():N}{datExtension}";
            var datPath = Path.Combine(datDirectory, datFileName);

            using (var datStream = new FileStream(datPath, FileMode.Create))
            {
                await datFile.CopyToAsync(datStream);
            }

            return datFileName;
        }

        public static void DatDeleteImage(string? datFileName, string datWebRootPath, string datFolder)
        {
            if (string.IsNullOrWhiteSpace(datFileName))
            {
                return;
            }

            var datPath = Path.Combine(datWebRootPath, datFolder, datFileName);
            if (File.Exists(datPath))
            {
                File.Delete(datPath);
            }
        }
    }
}
