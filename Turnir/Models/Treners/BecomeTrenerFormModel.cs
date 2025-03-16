namespace Turnir.Models.Treners
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Trener;

    public class BecomeTrenerFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
