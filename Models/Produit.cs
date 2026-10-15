using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduits.Models;

public class Produit
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,3)")]
    [Range(0, 999999, ErrorMessage = "Prix invalide")]
    public decimal Prix { get; set; }

    [Range(0, 100000, ErrorMessage = "Stock invalide")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Choisissez une catégorie")]
    public int CategorieId { get; set; }

    public Categorie? Categorie { get; set; }
}