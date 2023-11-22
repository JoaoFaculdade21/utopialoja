using System.ComponentModel.DataAnnotations;

namespace UtopiaAPI.Models;

public class Pagamento
{
    [Key]
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
}