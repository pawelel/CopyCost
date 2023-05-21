using System.ComponentModel.DataAnnotations;

namespace CopyCost.Entities;

public class Customer
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Pole \"{0}\" jest wymagane.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć od 3 do 50 znaków")]

    [Display(Name = "Nazwa klienta")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Opis musi mieć od 3 do 100 znaków")]

    [Display(Name = "Opis klienta")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Płatności")]
    public List<Payment> Payments { get; set; } = new();

    public override bool Equals(object? o)
    {
        var other = o as Customer;
        return other?.Id == Id;
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Id.GetHashCode();
}