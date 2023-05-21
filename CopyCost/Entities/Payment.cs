using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CopyCost.Entities;

public class Payment
{
    public int Id { get; set; }
    [Required (ErrorMessage = "Data jest wymagana")]
    [Display(Name = "Data")]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Liczba jest wymagana")]
    [Range(1, int.MaxValue, ErrorMessage = "Liczba musi być większa od 0")]
    [Display(Name = "Liczba")]
    public int Amount { get; set; } = 0;

    [Required (ErrorMessage = "Cena za 1000 znaków jest wymagana")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena za 1000 znaków musi być większa od 0")]
    [Display(Name = "Cena za 1000 znaków")]
    public decimal Per1000 { get; set; } = 0.00m;

    [NotMapped]
    [Display(Name = "Razem")]
    public decimal Total => Math.Round(Amount * (Per1000 / 1000m), 2);

    [Required (ErrorMessage = "Kategoria jest wymagana")]
    [Display(Name = "Kategoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Kategoria jest wymagana")]
    public int CategoryId { get; set; }

    public Category Category { get; set; } = new();

    [Required (ErrorMessage = "Klient jest wymagany")]
    [Display(Name = "Klient")]
    [Range(1, int.MaxValue, ErrorMessage = "Klient jest wymagany")]
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = new();

}