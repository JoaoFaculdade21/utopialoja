using System.ComponentModel.DataAnnotations;
namespace UtopiaAPI.Models;

public class ItemCarrinho 
{
    [Key]
    public int Id { get; set; }
    public int? Quantidade { get; set; }
    public Produto? Produto { get; set; }
    public decimal? Subtotal {get { return Quantidade * Produto?.Preco; } }

}