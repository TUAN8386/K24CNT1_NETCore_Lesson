using System.ComponentModel.DataAnnotations;

namespace DAT_NetCoreMVC_LAB5.Attributes
{
    public class DAT_BadWordAttribute : ValidationAttribute
    {
        private readonly string[] _badWords = new[] { "die", "admin", "fuck", "hack" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string strValue = value.ToString().ToLower();
                foreach (var word in _badWords)
                {
                    if (strValue.Contains(word))
                    {
                        return new ValidationResult($"Mô tả chứa từ ngữ không hợp lệ ('{word}').");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}