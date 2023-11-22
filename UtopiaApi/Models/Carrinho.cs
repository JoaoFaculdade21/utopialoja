using System.ComponentModel.DataAnnotations;

namespace UtopiaAPI.Models;

public class Carrinho{
    [Key]
    public int Id { get; set; }
    public int ItemId { get; set; }
    public List<ItemCarrinho>? Items { get; set; }
}