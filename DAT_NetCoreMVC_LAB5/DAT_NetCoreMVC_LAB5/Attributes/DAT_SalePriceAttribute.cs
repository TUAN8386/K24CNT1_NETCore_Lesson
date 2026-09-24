using System;
using System.ComponentModel.DataAnnotations;

namespace DAT_NetCoreMVC_LAB5.Attributes
{
    public class DAT_SalePriceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty("Price");
            if (property != null)
            {
                var priceValue = property.GetValue(validationContext.ObjectInstance, null);

                if (value != null && priceValue != null)
                {
                    float salePrice = Convert.ToSingle(value);
                    float price = Convert.ToSingle(priceValue);

                    if (salePrice < 0)
                    {
                        return new ValidationResult("Giá khuyến mãi không được âm.");
                    }

                    if (salePrice >= price * 0.9f)
                    {
                        return new ValidationResult("Giá khuyến mãi phải nhỏ hơn giá chuẩn ít nhất 10%.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}