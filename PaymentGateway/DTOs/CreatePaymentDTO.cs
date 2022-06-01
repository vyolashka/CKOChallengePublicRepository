using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class CreatePaymentDTO
    {
        // For simplicity I will assume we only allow Visa cards. I found a regex for the RegularExpression annotation
        // but I do not have time to research and test if this is the correct one.
        [Required(ErrorMessage = "Card number is required.")]
        [MaxLength(16, ErrorMessage = "Card number must contain maximum 16 characters.")]
        [MinLength(13, ErrorMessage = "Card number must contain minimum 16 characters.")]
        [RegularExpression(@"^4[0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Invalid card number format.")]
        public string CardNumber { get; set; }

        // TODO: If there is time: find Visa's requirements for card holder names, create a regex based on them and use it in a data annotation.
        [Required(ErrorMessage = "Card holder names are required.")]
        [MaxLength(300, ErrorMessage = "Card holder names cannot exceed 300 characters.")]
        public string CardHolderNames { get; set; }

        [Required(ErrorMessage = "Card expiration month is required.")]
        [Range(1, 12, ErrorMessage = "Card expiration month must be a number between 1 and 12.")]
        public int CardExpirationMonth { get; set; }

        [Required(ErrorMessage = "Card expiration year is required.")]
        public int CardExpirationYear { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [MaxLength(4, ErrorMessage = "Currency cannot exceed 4 characters.")]
        [MinLength(3, ErrorMessage = "Currency must be 3 or more characters long.")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "Payment amount is required.")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Payment amount should be a positive decimal number smaller than 79228162514264337593543950335.")]
        public decimal PaymentAmount { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [MaxLength(3, ErrorMessage = "CVV must contain 3 characters.")]
        [MinLength(3, ErrorMessage = "CVV must contain 3 characters.")]
        public string CVV { get; set; }
    }
}
