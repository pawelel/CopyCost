using System.ComponentModel.DataAnnotations;

namespace CopyCost.Entities;

public class Category
{
    public int Id { get; set; }
    [Required (ErrorMessage = "Nazwa jest wymagana")]
    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "Nazwa kategorii")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Opis musi mieć od 3 do 100 znaków")]
    [Display(Name = "Opis kategorii")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Płatności")]
    public List<Payment> Payments { get; set; } = new();

    public override bool Equals(object? o)
    {
        var other = o as Category;
        return other?.Id == Id;
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Id.GetHashCode();
}