namespace DAT_LESSON_14_LAYOUT.Helpers
{
    // Lưu ảnh upload vào wwwroot/uploads/{folder} và trả về đường dẫn (<= 100 ký tự)
    public static class DatFileHelper
    {
        private static readonly string[] AllowedExt = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxSize = 2 * 1024 * 1024; // 2MB

        public static string? Validate(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExt.Contains(ext)) return "Chỉ chấp nhận ảnh .jpg, .jpeg, .png, .gif, .webp";
            if (file.Length > MaxSize) return "Ảnh tối đa 2MB";
            return null;
        }

        public static async Task<string?> SaveAsync(IFormFile? file, IWebHostEnvironment env, string folder)
        {
            if (file == null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var dir = Path.Combine(env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(dir);

            await using var stream = new FileStream(Path.Combine(dir, fileName), FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }

        public static void Delete(string? path, IWebHostEnvironment env)
        {
            if (string.IsNullOrWhiteSpace(path) || !path.StartsWith("/uploads/")) return;
            var full = Path.Combine(env.WebRootPath, path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(full)) File.Delete(full);
        }
    }
}
