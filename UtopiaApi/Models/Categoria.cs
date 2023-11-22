using System.ComponentModel.DataAnnotations;
namespace UtopiaAPI.Models;

public class Categoria{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
}